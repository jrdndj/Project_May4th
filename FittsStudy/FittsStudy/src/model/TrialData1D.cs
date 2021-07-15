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
using WobbrockLib.Types;
using WobbrockLib.Extensions;

namespace FittsStudy
{
    /// <summary>
    /// This class, which inherits from DataTrial, encapsulates all the data associated with a 
    /// one-dimensional (1D) Fitts' law trial. A 1D trial is a click on a vertical ribbon in
    /// a horizontal movement task.
    /// </summary>
    public class TrialData1D : TrialData, IXmlLoggable
    {
        #region Fields

        private RectangleR _lastRect;
        private RectangleR _thisRect;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor for a 1-dimensional Fitts' law trial.
        /// </summary>
        public TrialData1D()
            : base()
        {
            // do nothing
        }
        
        /// <summary>
        /// Constructor for a 1-dimensional Fitts' law trial. A 1D trial moves horizontally between
        /// two vertical ribbon targets, as in Fitts' original study from 1954.
        /// </summary>
        /// <param name="index">The 0-based index number of this trial.</param>
        /// <param name="practice">True if this trial is practice; false otherwise. Practice trials aren't included in any calculations.</param>
        /// <param name="lastRect">The last vertical ribbon target prior to the current target for this trial, or <b>RectangleR.Empty</b> if none.</param>
        /// <param name="thisRect">The current vertical ribbon target for this trial.</param>
        /// <param name="tInterval">The metronome time interval in milliseconds, or -1L if unused.</param>
        public TrialData1D(int index, bool practice, RectangleR lastRect, RectangleR thisRect, long tInterval)
            : base(index, practice, tInterval)
        {
            _lastRect = lastRect;
            _thisRect = thisRect;
        }

        /// <summary>
        /// This static method is to be used by the ConditionData instance when creating the
        /// special start-area trial after reading trial number 1 in a XML log file. The special
        /// start-area trial is not logged explicitly, but it can be created from the information
        /// in trial number 1.
        /// </summary>
        /// <param name="trialOne">The first trial in the condition whose last target is used to
        /// create the special start-area trial.</param>
        /// <returns>The special start-area trial for a 1D task.</returns>
        internal static TrialData1D CreateStartArea(TrialData1D trialOne)
        {
            TrialData1D sa = new TrialData1D();
            sa._number = 0;
            sa._practice = true;
            sa._thisRect = trialOne._lastRect;
            sa._tInterval = trialOne._tInterval;
            return sa;
        }

        #endregion

        #region Condition Values

        /// <summary>
        /// Gets whether this trial is a 2-dimensional trial or a 1-dimensional trial.
        /// </summary>
        public override bool Circular
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the nominal amplitude of movement for this condition. This is the horizontal distance between
        /// the two vertical ribbon targets.
        /// </summary>
        public override int A
        {
            get
            {
                double x0 = _lastRect.Left + _lastRect.Width / 2.0 ;
                double x1 = _thisRect.Left + _thisRect.Width / 2.0;
                return (int) Math.Abs(x1 - x0);
            }
        }

        /// <summary>
        /// Gets the nominal target size for this condition. This is the width of the vertical ribbon target.
        /// </summary>
        public override int W
        {
            get { return (int) _thisRect.Width; }
        }

        /// <summary>
        /// Gets the angle of the nominal movement axis for this trial, in radians.
        /// </summary>
        public override double Axis
        {
            get
            {
                PointR c0 = new PointR(_lastRect.Left + _lastRect.Width / 2.0, _lastRect.Top + _lastRect.Height / 2.0);
                PointR c1 = new PointR(_thisRect.Left + _thisRect.Width / 2.0, _thisRect.Top + _thisRect.Height / 2.0);
                return GeotrigEx.Angle(c0, c1, true);
            }
        }

        #endregion

        #region The Target

        /// <summary>
        /// Gets the center of the current target.
        /// </summary>
        public override PointR TargetCenter
        {
            get
            {
                return new PointR(_thisRect.Left + _thisRect.Width / 2.0, _thisRect.Top + _thisRect.Height / 2.0);
            }
        }

        /// <summary>
        /// Gets the center point of this target relative to the start of the trial.
        /// For some target types, the center part to hit may depend on the approach.
        /// </summary>
        /// <remarks>If the trial has not been run yet, there will not be a start point
        /// and this property's value is meaningless. In this case, <b>PointF.Empty</b> is
        /// its value.</remarks>
        public override PointR TargetCenterFromStart
        {
            get
            {
                if (this.IsComplete)
                {
                    return new PointR(_thisRect.Left + _thisRect.Width / 2.0, _start.Y);
                }
                return PointR.Empty;
            }
        }

        /// <summary>
        /// Gets a paintable region object representing this target. Because it is a 
        /// GDI+ resource, the region should be disposed of after being used.
        /// </summary>
        public override Region TargetRegion
        {
            get
            {
                return new Region((RectangleF) _thisRect);
            }
        }

        /// <summary>
        /// Gets one or more target area regions to draw to be used in animating the
        /// metronome visualization for this trial's target. The animations will be
        /// drawn after the targets are drawn, i.e., on top of them. Because they are 
        /// GDI+ resources, the regions should be disposed of after being used.
        /// </summary>
        /// <param name="elapsed">The milliseconds elapsed since the last metronome 'tick'.
        /// This should be less than or equal to the interval passed into the trial's
        /// constructor.</param>
        /// <returns>One or more regions to draw in an animation sequence.</returns>
        public override Region[] GetAnimatedRegions(long elapsed)
        {
            double pct = (double) elapsed / _tInterval;

            RectangleR r0 = new RectangleR(
                _thisRect.X,
                _thisRect.Y,
                _thisRect.Width,
                _thisRect.Height / 2.0 * pct
                );
            RectangleR r1 = new RectangleR(
                _thisRect.X,
                _thisRect.Bottom - _thisRect.Height / 2.0 * pct,
                _thisRect.Width,
                _thisRect.Height / 2.0 * pct
                );

            return new Region[2] { 
                new Region((RectangleF) r0), 
                new Region((RectangleF) r1)
            };
        }

        /// <summary>
        /// Gets the bounding rectangle for this target.
        /// </summary>
        public override RectangleR TargetBounds
        {
            get { return _thisRect; }
        }

        /// <summary>
        /// Tests whether or not the point supplied is contained within the target.
        /// </summary>
        /// <param name="pt">The point to test.</param>
        /// <returns>True if the point is contained; false otherwise.</returns>
        /// <remarks>Note that is is not sufficient to use the TargetBounds property
        /// to hit-test the point, since not all targets are rectangular in shape.</remarks>
        public override bool TargetContains(PointR pt)
        {
            return _thisRect.Contains(pt);
        }

        /// <summary>
        /// Gets the number of times the mouse overshot the target, regardless of whether the mouse was
        /// inside or outside the confines of the target boundaries. For 1D trials, this means overshooting
        /// the vertical ribbon such that one passes out of it on its far side, relative to the mouse
        /// cursor.
        /// </summary>
        public override int TargetOvershoots
        {
            get
            {
                int n = 0; // number of overshoots
                double dx0 = -1.0; // we know we begin short of the target

                double center = _thisRect.Left + _thisRect.Width / 2.0;

                for (int i = 0; i < _movement.NumMoves; i++)
                {
                    TimePointR pt = _movement[i];

                    double dx1;
                    if (_thisRect.Left < _lastRect.Left)    // trial moves left
                        dx1 = center - pt.X;
                    else                                    // trial moves right
                        dx1 = pt.X - center;

                    if ((dx0 * dx1 < 0.0) &&                     // sign has changed, so
                        (Math.Abs(dx1) > _thisRect.Width / 2.0)) // could be overshoot if beyond far edge of ribbon
                    {
                        n++; // overshoot
                        dx0 = dx1; // update
                    }
                }
                return n;
            }
        }

        #endregion

        #region Other Measures

        /// <summary>
        /// Gets the effective amplitude for this trial (Ae).
        /// </summary>
        public override double GetAe(bool bivariate)
        {
            return bivariate ? 0.0 : Math.Abs(_end.X - _start.X);
        }

        /// <summary>
        /// Gets the dimensionally-relevant distance from the center of the target. For 1D trials,
        /// this is the signed x-axis distance. Negative values indicate undershooting, while positive
        /// values indicate overshooting.
        /// </summary>
        /// <remarks>This is NOT used to compute We as 4.133 * SDx. Instead, we must compute We
        /// more carefully using the standard deviation of normalized distances from the normalized selection
        /// mean.</remarks>
        public override double GetDx(bool bivariate)
        {
            if (bivariate)
                return 0.0; // no bivariate dx for a 1D trial

            double center = _thisRect.Left + _thisRect.Width / 2.0;
            return (_thisRect.Left < _lastRect.Left) ? center - _end.X : _end.X - center;
        }

        /// <summary>
        /// Gets a value indicating whether or not the selection endpoint fell outside this target,
        /// resulting in a selection error.
        /// </summary>
        public override bool IsError
        {
            get
            {
                return !_thisRect.Contains((PointR) _end);
            }
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
        public override bool WriteXmlHeader(XmlTextWriter writer)
        {
            writer.WriteStartElement("Trial");
            writer.WriteAttributeString("number", XmlConvert.ToString(_number));
            writer.WriteAttributeString("circular", XmlConvert.ToString(false)); // 1D
            writer.WriteAttributeString("metronome", XmlConvert.ToString(this.UsedMetronome));
            writer.WriteAttributeString("completed", XmlConvert.ToString(this.IsComplete));
            writer.WriteAttributeString("practice", XmlConvert.ToString(_practice));

            writer.WriteAttributeString("lastRect", _lastRect.ToString());
            writer.WriteAttributeString("thisRect", _thisRect.ToString());
            
            writer.WriteAttributeString("MT", XmlConvert.ToString(_tInterval));
            writer.WriteAttributeString("A", XmlConvert.ToString(this.A));
            writer.WriteAttributeString("W", XmlConvert.ToString(this.W));
            writer.WriteAttributeString("axis", XmlConvert.ToString(Math.Round(GeotrigEx.Radians2Degrees(this.Axis), 4)));

            writer.WriteAttributeString("angle", XmlConvert.ToString(Math.Round(GeotrigEx.Radians2Degrees(this.Angle), 4)));
            
            writer.WriteAttributeString("ae_1d", XmlConvert.ToString(Math.Round(this.GetAe(false), 4)));
            writer.WriteAttributeString("dx_1d", XmlConvert.ToString(Math.Round(this.GetDx(false), 4)));

            writer.WriteAttributeString("ae_2d", XmlConvert.ToString(Math.Round(this.GetAe(true), 4)));
            writer.WriteAttributeString("dx_2d", XmlConvert.ToString(Math.Round(this.GetDx(true), 4)));

            writer.WriteAttributeString("MTe", XmlConvert.ToString(this.MTe));
            writer.WriteAttributeString("MTRatio", XmlConvert.ToString(Math.Round(this.MTRatio, 4)));

            writer.WriteAttributeString("entries", XmlConvert.ToString(this.TargetEntries));
            writer.WriteAttributeString("overshoots", XmlConvert.ToString(this.TargetOvershoots));
            writer.WriteAttributeString("error", XmlConvert.ToString(this.IsError));
            writer.WriteAttributeString("spatialOutlier", XmlConvert.ToString(this.IsSpatialOutlier));
            writer.WriteAttributeString("temporalOutlier", XmlConvert.ToString(this.IsTemporalOutlier));

            writer.WriteAttributeString("start", _start.ToString());
            writer.WriteAttributeString("end", _end.ToString());

            // write out the movement for this trial
            _movement.WriteXmlHeader(writer);

            writer.WriteEndElement(); // </Trial>

            return true;
        }

        /// <summary>
        /// Writes any closing XML necessary for this data object. This method can simply
        /// return <b>true</b> if all data was already written in the header.
        /// </summary>
        /// <param name="writer">An open XML writer. The writer will be closed by this method
        /// after writing.</param>
        /// <returns>Returns <b>true</b> if successful; <b>false</b> otherwise.</returns>
        public override bool WriteXmlFooter(XmlTextWriter writer)
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
        public override bool ReadFromXml(XmlTextReader reader)
        {
            reader.Read(); // <Trial>
            if (reader.Name != "Trial")
                throw new XmlException("XML format error: Expected the <Trial> tag.");

            _number = XmlConvert.ToInt32(reader.GetAttribute("number"));
            _practice = XmlConvert.ToBoolean(reader.GetAttribute("practice"));
            _lastRect = RectangleR.FromString(reader.GetAttribute("lastRect"));
            _thisRect = RectangleR.FromString(reader.GetAttribute("thisRect"));
            _tInterval = XmlConvert.ToInt64(reader.GetAttribute("MT"));
            _start = TimePointR.FromString(reader.GetAttribute("start"));
            _end = TimePointR.FromString(reader.GetAttribute("end"));

            // read in the movement and add it to the trial
            _movement = new MovementData(this);
            _movement.ReadFromXml(reader);

            reader.Read(); // </Trial>
            if (reader.Name != "Trial" || reader.NodeType != XmlNodeType.EndElement)
                throw new XmlException("XML format error: Expected the </Trial> tag.");

            return true;
        }

        /// <summary>
        /// Performs any analyses on this data object and writes the results to a space-delimitted
        /// file for subsequent copy-and-pasting into a spreadsheet like Microsoft Excel or SAS JMP.
        /// </summary>
        /// <param name="writer">An open stream writer pointed to a text file. The writer will be closed by
        /// this method after writing.</param>
        /// <returns>True if writing is successful; false otherwise.</returns>
        public override bool WriteResultsToTxt(StreamWriter writer)
        {
            return true; // do nothing
        }

        #endregion
    }
}
