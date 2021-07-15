/**
 * FittsStudy
 *
 *		Jacob O. Wobbrock, Ph.D.
 * 		The Information School
 *		University of Washington
 *		Mary Gates Hall, Box 352840
 *		Seattle, WA 98195-2840
 *		wobbrock@uw.edu
 *		
 * This software is distributed under the "New BSD License" agreement:
 * 
 * Copyright (c) 2007-2011, Jacob O. Wobbrock. All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *    * Redistributions of source code must retain the above copyright
 *      notice, this list of conditions and the following disclaimer.
 *    * Redistributions in binary form must reproduce the above copyright
 *      notice, this list of conditions and the following disclaimer in the
 *      documentation and/or other materials provided with the distribution.
 *    * Neither the name of the University of Washington nor the names of its 
 *      contributors may be used to endorse or promote products derived from 
 *      this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS
 * IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
 * THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
 * PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL Jacob O. Wobbrock
 * BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
 * GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
 * LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
 * OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
 * SUCH DAMAGE.
**/
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Xml;
using WobbrockLib.Extensions;
using WobbrockLib.Types;

namespace FittsStudy
{
    /// <summary>
    /// This class encapsulates all the data associated with mouse movement in a single Fitts' law
    /// trial. It contains no trial-specific information, just mouse movement, and is capable of
    /// performing numerous computations on that movement. All trial-specific information is kept
    /// at the <i>TrialData</i> level; this class is purely trial-agnostic.
    /// </summary>
    public class MovementData : IXmlLoggable
    {
        #region Types

        /// <summary>
        /// A structure containing references to a velocity, acceleration, and jerk time series.
        /// </summary>
        public struct Profiles
        {
            public static readonly Profiles Empty;
            public bool IsEmpty { get { return Position == null && Velocity == null && Acceleration == null && Jerk == null; } }
            public List<TimePointR> Position;
            public List<PointR> Velocity;
            public List<PointR> Acceleration;
            public List<PointR> Jerk;
        }

        /// <summary>
        /// A structure containing the six path analysis measures defined by MacKenzie et al. (CHI 2001).
        /// </summary>
        public struct PathAnalyses
        {
            public static readonly PathAnalyses Empty;
            public bool IsEmpty { get { return TaskAxisCrossings == 0 && MovementDirectionChanges == 0 && OrthogonalDirectionChanges == 0 && MovementVariability == 0.0 && MovementError == 0.0 && MovementOffset == 0.0; } }
            public int TaskAxisCrossings;
            public int MovementDirectionChanges;
            public int OrthogonalDirectionChanges;
            public double MovementVariability;
            public double MovementError;
            public double MovementOffset;
        }

        #endregion

        #region Constants

        /// <summary>
        /// The frequency in cycles/second at which to resample. Resampling at 100 Hz, for example, 
        /// would cause the movement points to fall at 1000 / 100 = 10 ms apart.
        /// </summary>
        private const int Hertz = 100;
        
        /// <summary>
        /// The standard deviation of the Gaussian kernel to use. The greater the standard deviation, 
        /// the larger the kernel, and the smoother the result, as more neighboring values are taken into 
        /// account when computing the current value. The two-sided kernel will be of size
        /// 3 * <i>stdev</i> * 2 + 1, which means each smoothed value takes this many resampled values into
        /// account according to the weighting given by the kernel at each position.
        /// </summary>
        private const int GaussianStdDev = 5; // 3*(5)*2+1 = 31 kernel size.

        /// <summary>
        /// The weighting kernel to be used as a 1D convolution filter. The kernel size is based on
        /// the standard deviation of a Gaussian curve, which reaches zero at about 3 times this
        /// value.
        /// </summary>
        private static readonly double[] Kernel;

        #endregion

        #region Fields

        private TrialData _owner;
        private List<TimePointR> _moves;

        #endregion

        #region Constructor

        /// <summary>
        /// Private static constructor for the entire class.
        /// </summary>
        static MovementData()
        {
            Kernel = SeriesEx.GaussianKernel(GaussianStdDev);
        }

        /// <summary>
        /// Constructor for a DataMovement instance. 
        /// </summary>
        /// <param name="owner">The trial to which this movement belongs.</param>
        public MovementData(TrialData owner)
        {
            _moves = new List<TimePointR>(128);
            _owner = owner;
        }

        /// <summary>
        /// Gets the trial that owns this movement.
        /// </summary>
        public TrialData Owner
        {
            get { return _owner; }
        }

        #endregion

        #region Movement Points

        /// <summary>
        /// Gets or sets the TimePointF at the given index.
        /// </summary>
        /// <param name="index">A 0-based index of the movement TimePointF to get or set.</param>
        /// <returns>The TimePointF at the given index.</returns>
        /// <remarks>Throws an exception if <i>index</i> is out of bounds.</remarks>
        public TimePointR this[int index]
        {
            get
            {
                return _moves[index]; // let it throw any exceptions
            }
            set
            {
                _moves[index] = value; // let it throw any exceptions
            }
        }

        /// <summary>
        /// Adds a movement point to this movement path.
        /// </summary>
        /// <param name="pt">The point to add.</param>
        /// <remarks>If the point has identical (x,y) values as the point before it, the point replaces
        /// the point before it so that identical (x,y) values are not added but the time stamp is updated.</remarks>
        public void AddMove(TimePointR pt)
        {
            if (_moves.Count > 0 && _moves[_moves.Count - 1] == pt)
            {
                _moves.RemoveAt(_moves.Count - 1);
            }
            _moves.Add(pt);
        }

        /// <summary>
        /// Gets the number of distinct moves occuring in this movement.
        /// </summary>
        public int NumMoves
        {
            get { return _moves.Count; }
        }

        /// <summary>
        /// Clears all the mouse movement data.
        /// </summary>
        public void ClearMoves()
        {
            _moves.Clear();
        }

        /// <summary>
        /// Gets the total distance traveled during movement.
        /// </summary>
        public double Travel
        {
            get
            {
                double dx = 0.0;
                for (int i = 1; i < _moves.Count; i++)
                {
                    dx += GeotrigEx.Distance((PointR) _moves[i - 1], (PointR) _moves[i]);
                }
                return dx;
            }
        }

        /// <summary>
        /// Gets the duration of this movement, in milliseconds.
        /// </summary>
        public long Duration
        {
            get
            {
                if (_moves.Count > 0)
                {
                    TimePointR first = _moves[0];
                    TimePointR last = _moves[_moves.Count - 1];
                    return (last.Time - first.Time);
                }
                return 0L;
            }
        }

        /// <summary>
        /// Normalizes the timestamps of the movement points relative to a base time.
        /// </summary>
        /// <param name="normTo">The base time against which to normalize the movement points.</param>
        public void NormalizeTimes(long normTo)
        {
            for (int i = 0; i < _moves.Count; i++)
            {
                _moves[i] = new TimePointR(_moves[i].X, _moves[i].Y, _moves[i].Time - normTo);
            }
        }

        #endregion

        #region Submovement Processing

        /// <summary>
        /// Temporally resamples the movement at 100 Hz, and then produces the position, velocity, acceleration, 
        /// and jerk profiles that accompany it. Note that the position profile are TimePoints, and the derivative
        /// profiles are PointF time series.
        /// </summary>
        /// <returns>The velocity, acceleration, and jerk submovement profiles from the resampled movement.</returns>
        public Profiles CreateResampledProfiles()
        {
            if (_moves.Count == 0)
                return Profiles.Empty;

            // Resampled position, velocity, acceleration, jerk
            Profiles resampled;
            resampled.Position = SeriesEx.ResampleInTime(_moves, Hertz);
            resampled.Velocity = SeriesEx.Derivative(resampled.Position);
            resampled.Acceleration = SeriesEx.Derivative(resampled.Velocity);
            resampled.Jerk = SeriesEx.Derivative(resampled.Acceleration);

            return resampled;
        }

        /// <summary>
        /// First, temporally resample the movement at 100 Hz. Second, compute the submovement profiles 
        /// for velocity, acceleration, and jerk. Third, smooth these profiles using a 1D Gaussian convolution 
        /// filter.
        /// </summary>
        /// <returns>This movement smoothed over time.</returns>
        /// <remarks>Smoothing velocity, acceleration, and jerk amounts to smoothing 1D time series. But
        /// position is a 2D time series, and although its x-coords and y-coords can be smoothed independently,
        /// bad artifacts occur at the beginnings and ends. These can be ameliorated by extending the first
        /// and last values position values in the series so that the kernel overlaps them during smoothing.
        /// </remarks>
        public Profiles CreateSmoothedProfiles()
        {
            //
            // Resample to get the submovement profiles with temporally-evenly spaced points. This
            // is a necessary step before applying the Gaussian convolution filter, since intervals
            // should be evenly spaced.
            //
            Profiles resampled = CreateResampledProfiles();
            if (resampled.IsEmpty)
                return Profiles.Empty;

            //
            // Now smooth the resampled submovement profiles.
            //
            Profiles smoothed;
            int halfLen = Kernel.Length / 2;

            //
            // To smooth position with 1D filter, we have to smooth the (x,y) coordinates independently. 
            // This works fine except at either end of the profile, where it causes major departures. So
            // we must extend the profile at the head and tail to reduce this problem.
            //
            List<PointR> posx = new List<PointR>(resampled.Position.Count + Kernel.Length);
            List<PointR> posy = new List<PointR>(resampled.Position.Count + Kernel.Length);

            for (int i = 0; i < halfLen; i++) // extend first value
            {
                posx.Add(new PointR(0, resampled.Position[0].X));
                posy.Add(new PointR(0, resampled.Position[0].Y));
            }
            for (int i = 0; i < resampled.Position.Count; i++) // add actual values
            {
                posx.Add(new PointR(resampled.Position[i].Time, resampled.Position[i].X));
                posy.Add(new PointR(resampled.Position[i].Time, resampled.Position[i].Y));
            }
            for (int i = 0; i < halfLen; i++) // extend last value
            {
                posx.Add(new PointR(0, resampled.Position[resampled.Position.Count - 1].X));
                posy.Add(new PointR(0, resampled.Position[resampled.Position.Count - 1].Y));
            }

            posx = SeriesEx.Filter(posx, Kernel); // smooth x-values
            posy = SeriesEx.Filter(posy, Kernel); // smooth y-values
            
            smoothed.Position = new List<TimePointR>(resampled.Position.Count); // reassemble the (x,y) points
            for (int i = halfLen; i < resampled.Position.Count + halfLen; i++)
            {
                smoothed.Position.Add(new TimePointR(posx[i].Y, posy[i].Y, resampled.Position[i - halfLen].Time));
            }

            //
            // Smooth the derivative resampled time series to create the smoothed velocity, acceleration, and jerk profiles.
            //
            smoothed.Velocity = SeriesEx.Filter(resampled.Velocity, Kernel);
            smoothed.Acceleration = SeriesEx.Filter(resampled.Acceleration, Kernel);
            smoothed.Jerk = SeriesEx.Filter(resampled.Jerk, Kernel);
            
            return smoothed;
        }

        /// <summary>
        /// Calculates the number of submovements in this movement. The number of submovements is defined
        /// by the number of velocity peaks in the smoothed velocity profile. The profile is obtained after
        /// resampling at 100 Hz and smoothing using a Gaussian convolution filter with a standard deviation 
        /// of 3.
        /// </summary>
        /// <returns>The number of submovements defined by peaks in the smoothed velocity profile.</returns>
        public int GetNumSubmovements()
        {
            Profiles smoothed = CreateSmoothedProfiles();
            if (smoothed.IsEmpty)
                return -1;

            int[] maxima = SeriesEx.Maxima(smoothed.Velocity, 0, -1);
            return maxima.Length;
        }

        #endregion

        #region MacKenzie et al. (CHI 2001) Path Analyses

        /// <summary>
        /// Calculates the six path analyses defined by MacKenzie et al. (CHI 2001) using
        /// the task axis given. The smoothed position is used to avoid counting minor
        /// artifacts that occur in the raw or resampled position paths.
        /// </summary>
        /// <param name="axisStart">The starting point of the task axis.</param>
        /// <param name="axisEnd">The ending point of the task axis.</param>
        /// <returns></returns>
        public PathAnalyses DoPathAnalyses(PointR axisStart, PointR axisEnd)
        {
            if (_moves.Count == 0)
                return PathAnalyses.Empty;

            // get the smoothed position profile and convert it to PointRs (we don't need time for path analyses)
            Profiles smoothed = CreateSmoothedProfiles();
            List<PointR> pts = TimePointR.ConvertList(smoothed.Position);

            // rotate the task so that it proceeds at 0 degrees (+x axis, straight right)
            PointR c = GeotrigEx.Centroid(pts);
            double radians = GeotrigEx.Angle(axisStart, axisEnd, true);
            axisStart = GeotrigEx.RotatePoint(axisStart, c, -radians);
            axisEnd = GeotrigEx.RotatePoint(axisEnd, c, -radians);
            List<PointR> rotatedPts = GeotrigEx.RotatePoints(pts, -radians);

            // perform the path analysis computations for each path analysis measure
            PathAnalyses analyses;
            analyses.TaskAxisCrossings = TaskAxisCrossings(rotatedPts, axisEnd.Y);
            analyses.MovementDirectionChanges = MovementDirectionChanges(rotatedPts);
            analyses.OrthogonalDirectionChanges = OrthogonalDirectionChanges(rotatedPts);
            analyses.MovementVariability = MovementVariability(rotatedPts, axisEnd.Y);
            analyses.MovementError = MovementError(rotatedPts, axisEnd.Y);
            analyses.MovementOffset = MovementOffset(rotatedPts, axisEnd.Y);
            return analyses;
        }

        /// <summary>
        /// Calculates the number of task axis crossings occurring in this movement path.
        /// </summary>
        /// <param name="pts">The points to analyze. These must be first rotated such that
        /// the task axis they are following is horizontal.</param>
        /// <param name="horizontal">The y-value coordinate of the horizontal task axis.</param>
        /// <returns>The task axis crossings computation.</returns>
        private int TaskAxisCrossings(List<PointR> pts, double horizontal)
        {
            if (pts.Count < 2)
                return 0;

            int tac = 0;
            double d0 = pts[0].Y - horizontal; // initial side of horizontal
            for (int i = 1; i < pts.Count; i++)
            {
                double d1 = pts[i].Y - horizontal;
                if (d0 * d1 < 0.0) // multiplying will only be less than zero if they are of opposite signs
                    tac++;
                if (d1 != 0.0)
                    d0 = d1; // update
            }
            return tac;
        }

        /// <summary>
        /// Calculates the number of movement direction changes in this movement path. An movement
        /// direction change is, for a horizontal path, when the sign of the difference between successive
        /// Y-values change.
        /// </summary>
        /// <param name="pts">The points to analyze. These must be first rotated such that
        /// the task axis they are following is horizontal.</param>
        /// <returns>The movement direction changes computation.</returns>
        private int MovementDirectionChanges(List<PointR> pts)
        {
            if (pts.Count < 3)
                return 0;

            int mdc = 0;
            double d0 = pts[1].Y - pts[0].Y;
            for (int i = 2; i < pts.Count; i++)
            {
                double d1 = pts[i].Y - pts[i - 1].Y;
                if (d0 * d1 < 0.0) // direction change in Y
                    mdc++;
                if (d1 != 0.0)
                    d0 = d1; // update
            }
            return mdc;
        }

        /// <summary>
        /// Calculates the number of orthogonal direction changes in this movement path. An orthogonal
        /// direction change is, for a horizontal path, when the sign of the difference between successive
        /// X-values change.
        /// </summary>
        /// <param name="pts">The points to analyze. These must be first rotated such that
        /// the task axis they are following is horizontal.</param>
        /// <returns>The orthogonal direction changes computation.</returns>
        private int OrthogonalDirectionChanges(List<PointR> pts)
        {
            if (pts.Count < 3)
                return 0;

            int odc = 0;
            double d0 = pts[1].X - pts[0].X;
            for (int i = 2; i < pts.Count; i++)
            {
                double d1 = pts[i].X - pts[i - 1].X;
                if (d0 * d1 < 0.0) // direction change in X
                    odc++;
                if (d1 != 0.0)
                    d0 = d1; // update
            }
            return odc;
        }

        /// <summary>
        /// Calculates the movement variability of this movement path. This is the wigglyness of
        /// the path and is based on the standard deviation of movement points' distances from the
        /// task axis.
        /// </summary>
        /// <param name="pts">The points to analyze. These must be first rotated such that
        /// the task axis they are following is horizontal.</param>
        /// <param name="horizontal">The y-value coordinate of the horizontal task axis.</param>
        /// <returns>The movement variability computation.</returns>
        private double MovementVariability(List<PointR> pts, double horizontal)
        {
            double[] yvals = new double[pts.Count];
            for (int i = 0; i < pts.Count; i++)
            {
                yvals[i] = pts[i].Y - horizontal;
            }
            return StatsEx.StdDev(yvals);
        }

        /// <summary>
        /// Calculates the movement error, which is the average deviation of the sample points from 
        /// the task axis, irrespective of whether the points are above or below the axis.
        /// </summary>
        /// <param name="pts">The points to analyze. These must be first rotated such that
        /// the task axis they are following is horizontal.</param>
        /// <param name="horizontal">The y-value coordinate of the horizontal task axis.</param>
        /// <returns>The movement error computation.</returns>
        private double MovementError(List<PointR> pts, double horizontal)
        {
            double sum = 0.0;
            for (int i = 0; i < pts.Count; i++)
            {
                sum += Math.Abs(pts[i].Y - horizontal);
            }
            return sum / pts.Count;
        }

        /// <summary>
        /// Calculates the movement offset, which is the mean deviation of sample points from the 
        /// task axis.
        /// </summary>
        /// <param name="pts">The points to analyze. These must be first rotated such that
        /// the task axis they are following is horizontal.</param>
        /// <param name="horizontal">The y-value coordinate of the horizontal task axis.</param>
        /// <returns>The movement offset computation.</returns>
        private double MovementOffset(List<PointR> pts, double horizontal)
        {
            double sum = 0.0;
            for (int i = 0; i < pts.Count; i++)
            {
                sum += pts[i].Y - horizontal;
            }
            return sum / pts.Count;
        }

        #endregion

        #region IXmlLoggable Members

        /// <summary>
        /// Writes all or part of this data object to XML. If this data object owns other
        /// data objects that will also be written, this method may leave some XML elements
        /// open, which will be closed with a later call to <i>WriteXmlFooter</i>.
        /// </summary>
        /// <param name="writer">An open XML writer. The writer will be left open by this method
        /// after writing.</param>
        /// <returns>Returns <b>true</b> if successful; <b>false</b> otherwise.</returns>
        public bool WriteXmlHeader(XmlTextWriter writer)
        {
            writer.WriteStartElement("Movement");
            writer.WriteAttributeString("count", XmlConvert.ToString(_moves.Count));
            writer.WriteAttributeString("travel", XmlConvert.ToString(Math.Round(this.Travel, 4)));
            writer.WriteAttributeString("duration", XmlConvert.ToString(this.Duration));

            // write out the submovement information
            Profiles resampled = CreateResampledProfiles();
            if (resampled.IsEmpty) // this can happen if the trial is terminated prematurely
            {
                writer.WriteAttributeString("submovements", XmlConvert.ToString(0));
                writer.WriteAttributeString("maxVelocity", PointR.Empty.ToString());
                writer.WriteAttributeString("maxAcceleration", PointR.Empty.ToString());
                writer.WriteAttributeString("maxJerk", PointR.Empty.ToString());
            }
            else
            {
                int nsub = GetNumSubmovements(); // from smoothed velocity profile
                int vidx = SeriesEx.Max(resampled.Velocity, 0, -1);
                int aidx = SeriesEx.Max(resampled.Acceleration, 0, -1);
                int jidx = SeriesEx.Max(resampled.Jerk, 0, -1);

                writer.WriteAttributeString("submovements", XmlConvert.ToString(nsub));
                writer.WriteAttributeString("maxVelocity", resampled.Velocity[vidx].ToString());
                writer.WriteAttributeString("maxAcceleration", resampled.Acceleration[aidx].ToString());
                writer.WriteAttributeString("maxJerk", resampled.Jerk[jidx].ToString());
            }

            // write out all of MacKenzie's path analysis measures
            PathAnalyses analyses = DoPathAnalyses((PointR) _owner.Start, _owner.TargetCenterFromStart);
            writer.WriteAttributeString("taskAxisCrossings", XmlConvert.ToString(analyses.TaskAxisCrossings));
            writer.WriteAttributeString("movementDirectionChanges", XmlConvert.ToString(analyses.MovementDirectionChanges));
            writer.WriteAttributeString("orthogonalDirectionChanges", XmlConvert.ToString(analyses.OrthogonalDirectionChanges));
            writer.WriteAttributeString("movementVariability", XmlConvert.ToString(Math.Round(analyses.MovementVariability, 4)));
            writer.WriteAttributeString("movementError", XmlConvert.ToString(Math.Round(analyses.MovementError, 4)));
            writer.WriteAttributeString("movementOffset", XmlConvert.ToString(Math.Round(analyses.MovementOffset, 4)));

            // write out all the mouse move points that make up this trial
            int i = 0;
            foreach (TimePointR pt in _moves)
            {
                writer.WriteStartElement("move");
                writer.WriteAttributeString("index", XmlConvert.ToString(i++));
                writer.WriteAttributeString("point", pt.ToString());
                writer.WriteEndElement(); // </move>
            }

            writer.WriteEndElement(); // </Movement>

            return true;
        }

        /// <summary>
        /// Writes any closing XML necessary for this data object. This method can simply
        /// return <b>true</b> if all data was already written in the header.
        /// </summary>
        /// <param name="writer">An open XML writer. The writer will be closed by this method
        /// after writing.</param>
        /// <returns>Returns <b>true</b> if successful; <b>false</b> otherwise.</returns>
        public bool WriteXmlFooter(XmlTextWriter writer)
        {
            return true; // do nothing
        }

        /// <summary>
        /// Reads a data object from XML and returns an instance of the object.
        /// </summary>
        /// <param name="reader">An open XML reader. The reader will be closed by this
        /// method after reading.</param>
        /// <returns>Returns <b>true</b> if successful; <b>false</b> otherwise.</returns>
        /// <remarks>Clients should first create a new instance using a default constructor, and then
        /// call this method to populate the data fields of the default instance.</remarks>
        public bool ReadFromXml(XmlTextReader reader)
        {
            reader.Read(); // <Movement>
            if (reader.Name != "Movement")
                throw new XmlException("XML format error: Expected the <Movement> tag.");

            int count = XmlConvert.ToInt32(reader.GetAttribute("count"));
            _moves = new List<TimePointR>(count);

            // read in the moves and add it to the movement
            for (int i = 0; i < count; i++)
            {
                reader.Read(); // <move>
                if (reader.Name != "move")
                    throw new XmlException("XML format error: Expected the <move> tag.");
                TimePointR pt = TimePointR.FromString(reader.GetAttribute("point"));
                this.AddMove(pt);
            }

            reader.Read(); // </Movement>
            if (reader.Name != "Movement" || reader.NodeType != XmlNodeType.EndElement)
                throw new XmlException("XML format error: Expected the </Movement> tag.");

            return true;
        }

        /// <summary>
        /// Performs any analyses on this data object and writes the results to a space-delimitted
        /// file for subsequent copy-and-pasting into a spreadsheet like Microsoft Excel or SAS JMP.
        /// </summary>
        /// <param name="writer">An open stream writer pointed to a text file. The writer will be closed by
        /// this method after writing.</param>
        /// <returns>True if writing is successful; false otherwise.</returns>
        public bool WriteResultsToTxt(StreamWriter writer)
        {
            return true; // do nothing
        }

        #endregion

    }
}
