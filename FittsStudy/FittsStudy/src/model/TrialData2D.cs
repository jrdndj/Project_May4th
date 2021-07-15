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
using System.Drawing.Drawing2D;
using System.IO;
using System.Xml;
using WobbrockLib.Extensions;
using WobbrockLib.Types;

namespace FittsStudy
{
    /// <summary>
    /// This class, which inherits from DataTrial, encapsulates all the data associated with a 
    /// two-dimensional (2D) Fitts' law trial. A 1D trial is a click on a circle in the ISO 9241-9
    /// circular arrangment of targets.
    /// </summary>
    public class TrialData2D : TrialData, IXmlLoggable
    {
        #region Fields

        private CircleR _thisCircle; // the circular area for this target
        private CircleR _lastCircle; // the circular area for the last target, or Empty if none
        private PointR _isoCenter; // the center of the whole ISO 9241-9 circular arrangement

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor for a 2-dimensional Fitts' law trial.
        /// </summary>
        public TrialData2D()
            : base()
        {
            // do nothing
        }

        /// <summary>
        /// Constructor for a 2-dimensional Fitts' law trial. A 2D trial moves clockwise around a
        /// circle with circular targets at the outside according to the ISO 9241-9 standard.
        /// </summary>
        /// <param name="index">The 0-based index number of this trial.</param>
        /// <param name="practice">True if this trial is practice; false otherwise. Practice trials aren't included in any calculations.</param>
        /// <param name="lastCircle">The last circular target prior to the current target for this trial, or <b>Circle.Empty</b> if none.</param>
        /// <param name="thisCircle">The current circular target for this trial.</param>
        /// <param name="isoCenter">The center point of the ISO 9241-9 target arrangement to which this target belongs.</param>
        /// <param name="tInterval">The metronome time interval in milliseconds, or -1L if unused.</param>
        public TrialData2D(int index, bool practice, CircleR lastCircle, CircleR thisCircle, PointR isoCenter, long tInterval)
            : base(index, practice, tInterval)
        {
            _lastCircle = lastCircle;
            _thisCircle = thisCircle;
            _isoCenter = isoCenter;
        }

        /// <summary>
        /// This static method is to be used by the ConditionData instance when creating the
        /// special start-area trial after reading trial number 1 in a XML log file. The special
        /// start-area trial is not logged explicitly, but it can be created from the information
        /// in trial number 1.
        /// </summary>
        /// <param name="trialOne">The first trial in the condition whose last target is used to
        /// create the special start-area trial.</param>
        /// <returns>The special start-area trial for a 2D task.</returns>
        internal static TrialData2D CreateStartArea(TrialData2D trialOne)
        {
            TrialData2D sa = new TrialData2D();
            sa._number = 0;
            sa._practice = true;
            sa._thisCircle = trialOne._lastCircle;
            sa._isoCenter = trialOne._isoCenter;
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
            get { return true; }
        }

        /// <summary>
        /// Gets the nominal amplitude of movement for this condition. Note that in the ISO 9241-9 arrangement, 
        /// the nominal A is the same for an entire condition and is based on the diameter of the arrangement, even
        /// though individual movements may be slightly less than the full diameter.
        /// </summary>
        public override int A
        {
            get
            {
                double radius = GeotrigEx.Distance(_isoCenter, _thisCircle.Center); // radius of ISO 9241-9 arrangement
                return (int) Math.Ceiling(radius * 2.0);
            }
        }

        /// <summary>
        /// Gets the nominal target size for this condition. This is the diameter of the circular target.
        /// </summary>
        public override int W
        {
            get { return (int) _thisCircle.Diameter; }
        }

        /// <summary>
        /// Gets the angle of the nominal movement axis for this trial, in radians.
        /// </summary>
        public override double Axis
        {
            get
            {
                return GeotrigEx.Angle(_lastCircle.Center, _thisCircle.Center, true);
            }
        }

        #endregion

        #region The Target

        /// <summary>
        /// Gets the center of the current target.
        /// </summary>
        public override PointR TargetCenter
        {
            get { return _thisCircle.Center; }
        }

        /// <summary>
        /// Gets the center point of this target relative to the start of the trial.
        /// However, for circular targets, the center is the same regardless of approach
        /// angle.
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
                    return _thisCircle.Center;
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
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse((RectangleF) _thisCircle.Bounds);
                Region rgn = new Region(path);
                path.Dispose();
                return rgn;
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

            CircleR circle = new CircleR(_thisCircle.Center, _thisCircle.Radius * pct);

            GraphicsPath path = new GraphicsPath();
            path.AddEllipse((RectangleF) circle.Bounds);
            Region rgn = new Region(path);
            path.Dispose();

            return new Region[1] { rgn };
        }

        /// <summary>
        /// Gets the bounding rectangle for this target.
        /// </summary>
        public override RectangleR TargetBounds
        {
            get { return _thisCircle.Bounds; }
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
            return _thisCircle.Contains(pt);
        }

        /// <summary>
        /// Gets the number of times the mouse passed beyond the target's far edge, whether inside or 
        /// outside the target. For 2D trials, this is conceptually like putting a line tangent to the
        /// far side of the circle perpendicular to the movement direction. At every overshoot occurrence,
        /// the tangent line is re-computed to the new far side of the target, and so on.
        /// </summary>
        public override int TargetOvershoots
        {
            get
            {
                int n = 0;

                double radians = GeotrigEx.Angle((PointR) _start, _thisCircle.Center, true); // angle of the task axis
                
                for (int i = 0; i < _movement.NumMoves; i++)
                {
                    TimePointR pt = GeotrigEx.RotatePoint((PointR) _movement[i], _thisCircle.Center, -radians); // rotate for 0-degree task
                    if (pt.X > _thisCircle.Center.X + _thisCircle.Radius) // if we've broken the line tangent to the far side of the circle 
                    {
                        n++; // overshoot
                        radians = GeotrigEx.Angle((PointR) _movement[i], _thisCircle.Center, true); // update for new angle from this point
                    }
                }
                return n;
            }
        }

        #endregion

        #region Other Measures

        /// <summary>
        /// Gets the actual trial amplitude for this trial as the effective amplitude (Ae).
        /// </summary>
        public override double GetAe(bool bivariate)
        {
            if (bivariate) // two-dimensional distance
            {
                return GeotrigEx.Distance((PointR) _start, (PointR) _end); 
            }
            else // only consider x-coordinate
            {
                PointR nstart = this.NormalizedStart;  // relative to a target at (0,0)
                PointR nend = this.NormalizedEnd;
                return Math.Abs(nend.X - nstart.X);
            }
        }

        /// <summary>
        /// Gets the distance from the center of the target. For circle targets, the bivariate outcome is
        /// the Euclidean distance to the target center. The univariate outcome is the x-distance to the
        /// x-coordinate of the target center.
        /// </summary>
        /// <remarks>This is NOT used to compute We as 4.133 * SDx. Instead, we must compute We
        /// more carefully using the standard deviation of normalized distances from the normalized selection
        /// mean.</remarks>
        public override double GetDx(bool bivariate)
        {
            return bivariate ? GeotrigEx.Distance(_thisCircle.Center, (PointR) _end) : this.NormalizedEnd.X; // nend is relative to (0,0)
        }

        /// <summary>
        /// Gets a value indicating whether or not the selection endpoint fell outside this target,
        /// resulting in a selection error.
        /// </summary>
        public override bool IsError
        {
            get
            {
                return !_thisCircle.Contains((PointR) _end);
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
            writer.WriteAttributeString("circular", XmlConvert.ToString(true)); // 2D
            writer.WriteAttributeString("metronome", XmlConvert.ToString(this.UsedMetronome));
            writer.WriteAttributeString("completed", XmlConvert.ToString(this.IsComplete));
            writer.WriteAttributeString("practice", XmlConvert.ToString(_practice));

            writer.WriteAttributeString("lastCircle", _lastCircle.ToString());
            writer.WriteAttributeString("thisCircle", _thisCircle.ToString());
            writer.WriteAttributeString("isoCenter", _isoCenter.ToString());

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
            _lastCircle = CircleR.FromString(reader.GetAttribute("lastCircle"));
            _thisCircle = CircleR.FromString(reader.GetAttribute("thisCircle"));
            _isoCenter = PointR.FromString(reader.GetAttribute("isoCenter"));
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