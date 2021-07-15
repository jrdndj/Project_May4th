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
using System.Windows.Forms;
using WobbrockLib.Extensions;
using WobbrockLib.Types;

namespace FittsStudy
{
    /// <summary>
    /// This class encapsulates the data associated with a single trial (single click) within a
    /// condition within a Fitts law study. The class holds all information necessary for defining
    /// a single trial, including its target locations.
    /// </summary>
    public abstract class TrialData : IXmlLoggable
    {
        #region Fields

        protected int _number; // 1-based number of this trial; trial 0 is reserved for the start area for the condition
        protected bool _practice; // true if this is a practice trial; false otherwise
        protected long _tInterval; // the normative movement time interval in milliseconds, or -1L

        protected TimePointR _start; // the click point that started this trial
        protected TimePointR _end; // the click point that ended this trial
        
        protected MovementData _movement; // the movement associated with this trial

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor for an abstract Fitts' law trial.
        /// </summary>
        public TrialData()
        {
            // do nothing
        }

        /// <summary>
        /// Constructor for an abstract Fitts' law trial. Actual trial instances must be created from
        /// subclasses that define the specific trial mechanics.
        /// </summary>
        /// <param name="index">The 0-based index number of this trial.</param>
        /// <param name="practice">True if this trial is practice; false otherwise. Practice trials aren't included in any calculations.</param>
        /// <param name="tInterval">The metronome time interval in milliseconds, or -1L if unused.</param>
        public TrialData(int index, bool practice, long tInterval)
        {
            _number = index;
            _practice = practice;
            _tInterval = tInterval;
            _start = TimePointR.Empty;
            _end = TimePointR.Empty;
            _movement = new MovementData(this);
        }

        #endregion

        #region Movement

        /// <summary>
        /// Gets or sets the movement associated with this trial.
        /// </summary>
        public MovementData Movement
        {
            get { return _movement; }
            set { _movement = value; }
        }

        #endregion

        #region Condition Values

        /// <summary>
        /// Gets the trial number of this trial. This is a 1-based index within the
        /// condition although there <i>is</i> a trial at index 0, which is the special 
        /// start-area trial.
        /// </summary>
        public int Number
        {
            get { return _number; }
        }

        /// <summary>
        /// Gets whether or not this trial is the special start-area trial, and therefore not
        /// really a trial at all, but a location for the initial click to begin a condition.
        /// </summary>
        public bool IsStartAreaTrial
        {
            get { return _number == 0; }
        }

        /// <summary>
        /// Gets a value indicating whether or not this trial is a practice trial. Is used
        /// to disregard marked trials for condition-level calculations.
        /// </summary>
        public bool IsPractice
        {
            get { return _practice; }
        }

        /// <summary>
        /// Gets whether or not this trial used a metronome to govern movement time as an
        /// independent variable.
        /// </summary>
        public bool UsedMetronome
        {
            get { return _tInterval != -1L; }
        }

        /// <summary>
        /// Gets the nominal index of difficulty for this trial.
        /// </summary>
        public double ID
        {
            get
            {
                return Math.Log((double) this.A / this.W + 1.0, 2.0);
            }
        }

        /// <summary>
        /// Gets the normative movement time in milliseconds for this trial. The normative 
        /// movement time is the time of a desired movement "encouraged" by the metronome 
        /// interval tick time. If a normative time isn't used, this value is -1L.
        /// </summary>
        public long MT
        {
            get { return _tInterval; }
        }

        /// <summary>
        /// Gets whether this trial is a 2-dimensional trial or a 1-dimensional trial.
        /// </summary>
        public abstract bool Circular { get; }

        /// <summary>
        /// Gets the nominal target distance for this trial.
        /// </summary>
        public abstract int A { get; }
       
        /// <summary>
        /// Gets the nominal target width for this trial.
        /// </summary>
        public abstract int W { get; }

        /// <summary>
        /// Gets the angle of the nominal movement axis for this trial, in radians.
        /// </summary>
        public abstract double Axis { get; }

        /// <summary>
        /// Gets the absolute center point of this target.
        /// </summary>
        public abstract PointR TargetCenter { get; }

        /// <summary>
        /// Gets the center point of this target relative to the start of the trial.
        /// For some target types, the center part to hit may depend on the approach.
        /// </summary>
        /// <remarks>If the trial has not been run yet, there will not be a start point
        /// and this property's value is meaningless. In this case, <b>PointR.Empty</b> is
        /// its value.</remarks>
        public abstract PointR TargetCenterFromStart { get; }

        /// <summary>
        /// Gets a paintable region object representing this target. Because it is a 
        /// GDI+ resource, the region should be disposed of after being used.
        /// </summary>
        public abstract Region TargetRegion { get; }

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
        public abstract Region[] GetAnimatedRegions(long elapsed);

        /// <summary>
        /// Gets the bounding rectangle for this target.
        /// </summary>
        public abstract RectangleR TargetBounds { get; }

        /// <summary>
        /// Tests whether or not the point supplied is contained within the target.
        /// </summary>
        /// <param name="pt">The point to test.</param>
        /// <returns>True if the point is contained; false otherwise.</returns>
        /// <remarks>Note that is is not sufficient to use the <b>FittsTrial.TargetBounds</b> 
        /// property to hit-test the point, since not all targets are rectangular in shape.</remarks>
        public abstract bool TargetContains(PointR pt);

        #endregion

        #region Measured Values

        /// <summary>
        /// Clears the performance data associated with this trial. Does not clear the
        /// independent variables that define the settings for this trial.
        /// </summary>
        public virtual void ClearData()
        {
            _start = TimePointR.Empty;
            _end = TimePointR.Empty;
            _movement.ClearMoves();
        }

        /// <summary>
        /// Gets whether or not this trial has been completed. A completed trial has been
        /// performed and therefore has a non-zero ending time-stamp.
        /// </summary>
        public bool IsComplete
        {
            get
            {
                return (_end.Time != 0L);
            }
        }

        /// <summary>
        /// Gets or sets the start click point and time that began this trial.
        /// </summary>
        public TimePointR Start
        {
            get { return _start; }
            set { _start = value; }
        }

        /// <summary>
        /// Gets or sets the selection endpoint and time that ended this trial.
        /// </summary>
        public TimePointR End
        {
            get { return _end; }
            set { _end = value; }
        }

        /// <summary>
        /// Gets the actual movement angle for this trial, in radians.
        /// </summary>
        public double Angle
        {
            get
            {
                return GeotrigEx.Angle((PointR) _start, (PointR) _end, true);
            }
        }

        /// <summary>
        /// Gets the trial start point normalized relative to this trial's target as if the target
        /// were centered at (0,0) and movement towards it was at 0 degrees, that is, straight right
        /// along the +x-axis. Thus, normalized start points will always begin at (-x,0).
        /// </summary>
        public PointR NormalizedStart
        {
            get
            {
                PointR center = this.TargetCenterFromStart;
                double radians = GeotrigEx.Angle((PointR) _start, (PointR) center, true);
                PointR newStart = GeotrigEx.RotatePoint((PointR) _start, (PointR) center, -radians);
                newStart.X -= center.X;
                newStart.Y -= center.Y;
                return newStart;
            }
        }

        /// <summary>
        /// Gets the selection endpoint normalized relative to this trial's target as if the target
        /// were centered at (0,0) and movement towards it was at 0 degrees, that is, straight right
        /// along the +x-axis. This allows endpoint distributions of trials within a condition to be
        /// compared despite not any of the actual target locations in each condition being the same.
        /// </summary>
        /// <remarks>This property is used in the calculation of the effective target width for the condition, We.</remarks>
        public PointR NormalizedEnd
        {
            get
            {
                // find the angle of the ideal task axis for this trial
                PointR center = this.TargetCenterFromStart;
                double radians = GeotrigEx.Angle((PointR) _start, center, true);

                // rotate the endpoint around the target center so that it would have come from 
                // a task whose task-axis was at 0 degrees (+x, straight to the right).
                PointR newEnd = GeotrigEx.RotatePoint((PointR) _end, center, -radians);
                
                // translate the endpoint so that it is as if the target was centered at (0,0).
                newEnd.X -= center.X;
                newEnd.Y -= center.Y;

                return newEnd;
            }
        }

        /// <summary>
        /// Normalizes the trial time so that the start time is zero and each move time
        /// and the end time are relative to that. Only works on completed trials.
        /// </summary>
        public void NormalizeTimes()
        {
            if (!IsComplete)
                return;

            _movement.NormalizeTimes(_start.Time);
            _end.Time -= _start.Time;
            _start.Time = 0L;
        }

        /// <summary>
        /// Gets the actual movement time in milliseconds for this trial.
        /// </summary>
        public long MTe
        {
            get
            {
                return _end.Time - _start.Time;
            }
        }

        /// <summary>
        /// Gets a ratio indicating how the actual movement time (MTe) corresponded to the normative
        /// movement time (MT). The ratio is <c>MTe / MT</c>. Thus, values &gt;1 indicate that the actual movement
        /// time was too slow. Values &lt;1 indicate that the actual movement time was too fast. A value
        /// of 1.00 indicates the actual movement time was the same as that prescribed.
        /// </summary>
        /// <remarks>
        /// The maximum possible number of actual metronome ticks that could have been heard is the <b>Math.Ceiling</b> 
        /// of this property. The minimum number of ticks that could have been heard is the <b>Math.Floor</b> of this 
        /// property.
        /// </remarks>
        public double MTRatio
        {
            get
            {
                if (this.UsedMetronome)
                {
                    double diff = _end.Time - _start.Time;
                    return diff / _tInterval;
                }
                return -1.0; // unused
            }
        }

        /// <summary>
        /// For a completed trial, gets the number of target entries. If the trial was an error,
        /// it may be that the target was never entered. The most successful trials will have
        /// a target entered once and only once. If target re-entry occurs, the target was entered 
        /// multiple times.
        /// </summary>
        public int TargetEntries
        {
            get
            {
                int n = 0;
                bool inside = false;

                for (int i = 0; i < _movement.NumMoves; i++)
                {
                    TimePointR pt = _movement[i];
                    if (TargetContains((PointR) pt)) // now inside
                    {
                        if (!inside) // were not yet inside
                        {
                            inside = true;
                            n++; // entry
                        }
                    }
                    else inside = false; // not inside
                }
                return n;
            }
        }

        /// <summary>
        /// Gets the number of times the mouse overshot the target, regardless of whether the mouse was
        /// inside or outside the confines of the target boundaries.
        /// </summary>
        public abstract int TargetOvershoots { get; }

        /// <summary>
        /// Gets the actual distance moved between clicks for this trial.
        /// </summary>
        public abstract double GetAe(bool bivariate);

        /// <summary>
        /// Gets the distance from the center of the target.
        /// </summary>
        public abstract double GetDx(bool bivariate);

        #endregion

        #region Errors and Outliers

        /// <summary>
        /// Gets a value indicating whether or not the selection endpoint fell outside this target,
        /// resulting in a selection error.
        /// </summary>
        public abstract bool IsError { get; }

        /// <summary>
        /// Gets a value indicating whether or not this trial is defined as a 
        /// "spatial outlier," which means that the selection point was outside
        /// the target, and (a) the actual distance moved was less than half the
        /// nominal distance required, and/or (b) the distance from the selection
        /// point to the target center was more than twice the width of the target.
        /// </summary>
        public bool IsSpatialOutlier
        {
            get
            {
                return this.IsError && (
                    (this.GetAe(this.Circular) < this.A / 2.0) || (Math.Abs(this.GetDx(this.Circular)) > 2.0 * this.W)
                    );
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not this trial had a movement time 
        /// that is a temporal outlier. A temporal outlier is considered unduly far
        /// from the normative time interval. It is defined by a movement time that 
        /// is less than 75% of the normative movement time (too fast), or more than 
        /// 125% of the normative movement time (too slow).
        /// </summary>
        /// <example>
        /// If a metronome movement time (MT) is set to 500 ms, a temporal outlier
        /// will occur if the effective movement time (MTe) is either less than 375 ms
        /// or greater than 625 ms. That amounts to 500 +/- 125 ms.
        /// </example>
        public bool IsTemporalOutlier
        {
            get
            {
                if (_tInterval != -1.0)
                {
                    double diff = _end.Time - _start.Time;
                    return (
                        (diff < _tInterval * 0.75) ||
                        (diff > _tInterval * 1.25)
                        );
                }
                return false;
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
        public abstract bool WriteXmlHeader(XmlTextWriter writer);

        /// <summary>
        /// Writes any closing XML necessary for this data object. This method can simply
        /// return <b>true</b> if all data was already written in the header.
        /// </summary>
        /// <param name="writer">An open XML writer. The writer will be closed by this method
        /// after writing.</param>
        /// <returns>Returns <b>true</b> if successful; <b>false</b> otherwise.</returns>
        public abstract bool WriteXmlFooter(XmlTextWriter writer);

        /// <summary>
        /// Reads a data object from XML and returns an instance of the object.
        /// </summary>
        /// <param name="reader">An open XML reader. The reader will be closed by this
        /// method after reading.</param>
        /// <returns>Returns <b>true</b> if successful; <b>false</b> otherwise.</returns>
        /// <remarks>Clients should first create a new instance using a default constructor, and then
        /// call this method to populate the data fields of the default instance.</remarks>
        public abstract bool ReadFromXml(XmlTextReader reader);

        /// <summary>
        /// Performs any analyses on this data object and writes the results to a space-delimitted
        /// file for subsequent copy-and-pasting into a spreadsheet like Microsoft Excel or SAS JMP.
        /// </summary>
        /// <param name="writer">An open stream writer pointed to a text file. The writer will be closed by
        /// this method after writing.</param>
        /// <returns>True if writing is successful; false otherwise.</returns>
        public abstract bool WriteResultsToTxt(StreamWriter writer);

        #endregion

    }
}