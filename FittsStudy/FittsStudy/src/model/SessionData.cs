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
using System.Xml;
using System.IO;
using System.Reflection;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using WobbrockLib.Extensions;
using WobbrockLib.Types;

namespace FittsStudy
{
    /// <summary>
    /// This class encapsulates a single study session for a given participant in a Fitts' law study. 
    /// Conditions comprise a session, trials comprise a condition, and movement comprises a trial.
    /// </summary>
    public class SessionData : IXmlLoggable
    {
        #region Fields

        private int _subject; // subject ID
        private bool _circular; // circular ISO 9241-9 or vertical ribbons
        private int[] _a; // movement amplitudes in pixels
        private int[] _w; // target widths in pixels
        private double[] _mtpct; // percents of Fitts' law-predicted movement speeds (e.g., 90% is faster, 110% is slower)
        private double _intercept; // the supplied Fitts' 'a' intercept regression coefficient, in milliseconds
        private double _slope; // the supplied Fitts' 'b' slope regression coefficient, in ms/bit
        private RectangleR _screen; // the screen bounds saved for the zoom feature on the GraphForm
        private List<ConditionData> _conditions; // ordered list of (MT% x A x W) or (A x W) conditions

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor used for XML deserialization.
        /// </summary>
        public SessionData()
        {
            // do nothing
        }

        /// <summary>
        /// Constructs a FittsSession instance. A FittsSession contains conditions for a Fitts' law study,
        /// which, in turn, contain a set of trials. A constructed instance contains a list of conditions 
        /// in sequence, which themselves contain a list of trials.
        /// </summary>
        /// <param name="o">The options that configure this session obtained from the OptionsForm dialog.</param>
        public SessionData(OptionsForm.Options o)
        {
            //
            // Set the condition variables that define this test.
            //
            _subject = o.Subject;
            _circular = o.Is2D;
            _a = o.A;
            _w = o.W;
            _mtpct = o.MTPct;
            _intercept = o.Intercept;
            _slope = o.Slope;
            _screen = Screen.PrimaryScreen.Bounds;
            _conditions = new List<ConditionData>();

            //
            // Create the order of conditions. Nesting is mt%[a[w]]].
            //
            int[] a_order, w_order, mt_order;
            if (o.Randomize) // randomize the order of conditions
            {
                a_order = RandomEx.Array(0, o.A.Length - 1, o.A.Length, true);
                w_order = RandomEx.Array(0, o.W.Length - 1, o.W.Length, true);
                if (o.MTPct != null)
                {
                    mt_order = RandomEx.Array(0, o.MTPct.Length - 1, o.MTPct.Length, true);
                    while (o.MTPct[mt_order[0]] < StatsEx.Mean(o.MTPct)) // enforce that the first MT% condition is >avg(MT%)
                    {
                        mt_order = RandomEx.Array(0, o.MTPct.Length - 1, o.MTPct.Length, true);
                    }
                }
                else mt_order = null;
            }
            else // in-order arrays
            {
                a_order = new int[o.A.Length];
                for (int i = 0; i < o.A.Length; i++)
                    a_order[i] = i;
                w_order = new int[o.W.Length];
                for (int i = 0; i < o.W.Length; i++)
                    w_order[i] = i;
                mt_order = (o.MTPct != null) ? new int[o.MTPct.Length] : null;
                for (int i = 0; o.MTPct != null && i < o.MTPct.Length; i++)
                    mt_order[i] = i;
            }

            //
            // Create the ordered condition list for the first block of all conditions (i.e., one time through).
            //
            int n = 0;
            for (int i = 0; o.MTPct == null || i < o.MTPct.Length; i++)
            {
                for (int j = 0; j < o.A.Length; j++)
                {
                    for (int k = 0; k < o.W.Length; k++)
                    {
                        double pct = (o.MTPct != null) ? o.MTPct[mt_order[i]] : -1.0; // MT%
                        double fitts = o.Intercept + o.Slope * Math.Log((double) o.A[a_order[j]] / o.W[w_order[k]] + 1.0, 2.0);
                        long mt = (o.MTPct != null) ? (long) fitts : -1L; // Fitts' law predicted movement time
                        ConditionData cd = new ConditionData(0, n++, o.A[a_order[j]], o.W[w_order[k]], pct, mt, o.Trials, o.Practice, o.Is2D);
                        _conditions.Add(cd);
                    }
                }
                if (o.MTPct == null)
                    break;
            }

            //
            // Now possibly replicate the created order multiple times.
            //
            int nConditions = _conditions.Count; // number of conditions in one block
            for (int b = 1; b < o.Blocks; b++)
            {
                for (int c = 0; c < nConditions; c++)
                {
                    ConditionData fx = _conditions[c];
                    ConditionData fc = new ConditionData(b, c, fx.A, fx.W, fx.MTPct, fx.MTPred, o.Trials, o.Practice, o.Is2D);
                    _conditions.Add(fc);
                }
            }
        }

        #endregion

        #region Conditions

        /// <summary>
        /// Gets the condition at the specified block and index within block.
        /// </summary>
        /// <param name="block">The 0-based block index. A block is a repeat of all conditions in the same order.</param>
        /// <param name="index">The 0-based condition index within the block.</param>
        /// <returns>The condition at the given block and index, or <b>null</b> if it does not exist.</returns>
        public ConditionData this[int block, int index]
        {
            get
            {
                int perBlock = this.NumConditionsPerBlock;
                int globalIndex = block * perBlock + index;
                if (0 <= globalIndex && globalIndex < _conditions.Count)
                {
                    return _conditions[globalIndex];
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the number of conditions for this Fitts' law study session. These conditions
        /// have not necessarily been run yet, but they have been ordered and are retrievable
        /// with the class indexer ([n,m]).
        /// </summary>
        public int NumTotalConditions
        {
            get { return _conditions.Count; }
        }

        /// <summary>
        /// Gets the number of conditions in each block.
        /// </summary>
        public int NumConditionsPerBlock
        {
            get
            {
                if (_conditions.Count > 0)
                {
                    // the index of the last condition, plus one, should be the number of  
                    // conditions in each block, as all blocks have the same number of conditions.
                    return _conditions[_conditions.Count - 1].Index + 1;
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets the number of all-condition blocks present in this session. A "block" in this
        /// sense is one complete run of all MT% x A x W conditions. Blocking allows for the
        /// condition set to be repeated an arbitrary number of times.
        /// </summary>
        public int NumBlocks
        {
            get
            {
                // the block index of the last condition, plus one, should be the number of
                // blocks, as all blocks have the same number of conditions.
                return _conditions[_conditions.Count - 1].Block + 1;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the subject ID for this session.
        /// </summary>
        public int Subject
        {
            get { return _subject; }
        }

        /// <summary>
        /// Gets whether or not the trials in this session are 2D circles in the
        /// ISO 9241-9 standard pattern, or vertical ribbons in the Fitts' traditional
        /// horizontal 1D task.
        /// </summary>
        public bool Is2D
        {
            get { return _circular; }
        }

        /// <summary>
        /// Gets whether or not this session used a metronome to govern movement time as an
        /// independent variable.
        /// </summary>
        public bool UsedMetronome
        {
            get { return _mtpct != null; }
        }

        /// <summary>
        /// Gets the array of movement time percentages that help to comprise
        /// MT% x A x W conditions for this Fitts' law study session. May be
        /// <b>null</b>, in which case, the study was run without a metronome, and
        /// used just traditional A x W conditions.
        /// </summary>
        /// <remarks>The conditions in the array will be ordered according to
        /// how they were passed in. If the randomize option was used, then
        /// this order does not reflect the testing order.</remarks>
        public double[] MTPct
        {
            get { return _mtpct; }
        }

        /// <summary>
        /// Gets the array of movement amplitudes in pixels that help to comprise 
        /// the MT% x A x W or just A x W conditions for this Fitts' law study.
        /// </summary>
        /// <remarks>The conditions in the array will be ordered according to
        /// how they were passed in. If the randomize option was used, then
        /// this order does not reflect the testing order.</remarks>
        public int[] A
        {
            get { return _a; }
        }

        /// <summary>
        /// Gets the array of target widths in pixels that help to comprise the
        /// MT% x A x W or just A x W conditions for this Fitts' law study.
        /// </summary>
        /// <remarks>The conditions in the array will be ordered according to
        /// how they were passed in. If the randomize option was used, then
        /// this order does not reflect the testing order.</remarks>
        public int[] W
        {
            get { return _w; }
        }

        /// <summary>
        /// Gets the supplied 'a' regression coefficient used to compute raw movement times
        /// for conditions based on movement time percentages (MT%). If non-metronome trials
        /// are being run, this value should be -1.0. Otherwise, it should be in milliseconds.
        /// </summary>
        public double Intercept
        {
            get { return _intercept; }
        }

        /// <summary>
        /// Gets the supplied 'b' regression coefficient used to compute raw movement times
        /// for conditions based on movement time percentages (MT%). If non-metronome trials
        /// are being run, this value should be -1.0. Otherwise, it should be in ms/bit.
        /// </summary>
        public double Slope
        {
            get { return _slope; }
        }

        /// <summary>
        /// Gets the screen's boundaries that were used to record this session.
        /// </summary>
        /// <remarks>This is used in the GraphForm to implement the zooming functionality. The zoom
        /// will not be centered if the trials were recorded on a different computer with different
        /// screen dimensions unless we save what those bounds were and access them when zooming.
        /// </remarks>
        public RectangleR ScreenBounds
        {
            get { return _screen; }
        }

        /// <summary>
        /// Gets the average error rate across all conditions for which there were any completed test trials.
        /// </summary>
        /// <param name="ex">Determines which outlier types to exclude, if any.</param>
        /// <returns>The average error rate for executed conditions in this session.</returns>
        public double GetErrorRate(ExcludeOutliersType ex)
        {
            double rate = 0.0;
            int conditions = 0;

            foreach (ConditionData fc in _conditions)
            {
                if (fc.NumCompletedTestTrials > 0)
                {
                    rate += fc.GetErrorRate(ex);
                    conditions++;
                }
            }

            return (conditions > 0) ? rate / conditions : 0.0;
        }

        #endregion

        #region Fitts and Error Model Builder

        /// <summary>
        /// Performs linear regression on the entire session's worth of data to fit a Fitts' law model
        /// of the form MTe = a + b * log2(Ae/We + 1).
        /// </summary>
        /// <returns>Model and fitting parameters.</returns>
        public Model BuildModel()
        {
            Model model = Model.Empty;
            model.FittsPts_1d = new List<PointR>(_conditions.Count);
            model.FittsPts_2d = new List<PointR>(_conditions.Count);

            double[] ide = new double[_conditions.Count]; // observed index of difficulties for each condition
            double[] mte = new double[_conditions.Count]; // observed mean movement times for each condition
            double[] tp = new double[_conditions.Count]; // observed throughputs for each condition

            for (int i = 0; i <= 1; i++) // first loop is univariate, second loop is bivariate
            {
                for (int j = 0; j < _conditions.Count; j++) // each A x W or MT% x A x W condition (blocks kept separate)
                {
                    ConditionData cdata = _conditions[j];
                    ide[j] = cdata.GetIDe(i == 1); // bits
                    mte[j] = cdata.GetMTe(ExcludeOutliersType.Spatial); // ms
                    tp[j] = cdata.GetTP(i == 1); // bits/s
                    if (i == 0)
                        model.FittsPts_1d.Add(new PointR(ide[j], mte[j])); // for graphing later
                    else
                        model.FittsPts_2d.Add(new PointR(ide[j], mte[j])); // for graphing later
                }
                if (i == 0) // univariate
                {
                    model.N = _conditions.Count; // == model.FittsPts.Count
                    model.MTe = StatsEx.Mean(mte); // ms
                    model.Fitts_TP_avg_1d = StatsEx.Mean(tp); // bits/s
                    model.Fitts_a_1d = StatsEx.Intercept(ide, mte); // ms
                    model.Fitts_b_1d = StatsEx.Slope(ide, mte); // ms/bit
                    model.Fitts_TP_inv_1d = 1.0 / model.Fitts_b_1d * 1000.0; // bits/s
                    model.Fitts_r_1d = StatsEx.Pearson(ide, mte); // correlation
                }
                else // bivariate
                {
                    model.Fitts_TP_avg_2d = _circular ? StatsEx.Mean(tp) : 0.0; // bits/s
                    model.Fitts_a_2d = _circular ? StatsEx.Intercept(ide, mte) : 0.0; // ms
                    model.Fitts_b_2d = _circular ? StatsEx.Slope(ide, mte) : 0.0; // ms/bit
                    model.Fitts_TP_inv_2d = _circular ? 1.0 / model.Fitts_b_2d * 1000.0 : 0.0; // bits/s
                    model.Fitts_r_2d = _circular ? StatsEx.Pearson(ide, mte) : 0.0; // correlation
                }
            }

            //
            // Now compute the predicted error rates relevant to metronome experiments.
            //
            model.ErrorPts_1d = new List<PointR>(_conditions.Count);
            model.ErrorPts_2d = new List<PointR>(_conditions.Count);

            double[] errPred = new double[_conditions.Count]; // x, predicted errors based on the error model for pointing
            double[] errObs = new double[_conditions.Count]; // y, observed error rates for each condition (spatial outliers included)

            for (int i = 0; i <= 1; i++) // first of two loops is univariate, second is bivariate
            {
                for (int j = 0; j < _conditions.Count; j++) // each MT% x A x W condition, duplicates left separate
                {
                    ConditionData cdata = _conditions[j];
                    errObs[j] = cdata.GetErrorRate(ExcludeOutliersType.Temporal); // observed error rates -- temporal outliers excluded
                    double mt = cdata.GetMTe(ExcludeOutliersType.Temporal); // observed movement times -- temporal outliers excluded

                    double z = (i == 0)
                        ? 2.066 * ((double) cdata.W / cdata.A) * (Math.Pow(2.0, (mt - model.Fitts_a_1d) / model.Fitts_b_1d) - 1.0)
                        : 2.066 * ((double) cdata.W / cdata.A) * (Math.Pow(2.0, (mt - model.Fitts_a_2d) / model.Fitts_b_2d) - 1.0);

                    errPred[j] = GeotrigEx.PinValue(2.0 * (1.0 - StatsEx.CDF(0.0, 1.0, z)), 0.0, 1.0); // predicted error rates == 1 - erf(z / sqrt(2))
                    //double shah = 1.0 - (2.0 * StatsEx.ShahNormal(z)); // Shah approximation of the standard normal distribution

                    if (i == 0)
                        model.ErrorPts_1d.Add(new PointR(errPred[j], errObs[j])); // for graphing later
                    else
                        model.ErrorPts_2d.Add(new PointR(errPred[j], errObs[j])); // for graphing later
                }
                if (i == 0) // univariate
                {
                    model.ErrorPct = StatsEx.Mean(errObs); // percent
                    model.Error_m_1d = StatsEx.Slope(errPred, errObs);
                    model.Error_b_1d = StatsEx.Intercept(errPred, errObs);
                    model.Error_r_1d = StatsEx.Pearson(errPred, errObs);
                }
                else // bivariate
                {
                    model.Error_m_2d = _circular ? StatsEx.Slope(errPred, errObs) : 0.0;
                    model.Error_b_2d = _circular ? StatsEx.Intercept(errPred, errObs) : 0.0;
                    model.Error_r_2d = _circular ? StatsEx.Pearson(errPred, errObs) : 0.0;
                }
            }
            return model;
        }

        #endregion

        #region Filename

        /// <summary>
        /// Gets the filename base (no tick-count or extension) for this session. The  
        /// filename base shows the subject ID, whether or not a 2D or 1D arrangement
        /// was used, and whether or not a metronome was used, as these are the main
        /// distinguishing features of the studies run with this software.
        /// </summary>
        public string FilenameBase
        {
            get
            {
                return String.Format("s{0}_{1}_{2}",
                    _subject > 9 ? _subject.ToString() : "0" + _subject,
                    _circular ? "2D" : "1D",
                    this.UsedMetronome ? "mnome" : "nomet");
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
        public bool WriteXmlHeader(XmlTextWriter writer)
        {
            bool success = true;
            try
            {
                writer.WriteStartDocument(true);
                writer.WriteStartElement("Fitts_Study");
                writer.WriteAttributeString("subject", XmlConvert.ToString(_subject));
                writer.WriteAttributeString("circular", XmlConvert.ToString(_circular));
                writer.WriteAttributeString("metronome", XmlConvert.ToString(this.UsedMetronome));
                writer.WriteAttributeString("conditions", XmlConvert.ToString(_conditions.Count));
                writer.WriteAttributeString("MTPct", StringEx.Array2String(_mtpct));
                writer.WriteAttributeString("A", StringEx.Array2String(_a));
                writer.WriteAttributeString("W", StringEx.Array2String(_w));
                writer.WriteAttributeString("intercept", XmlConvert.ToString(_intercept));
                writer.WriteAttributeString("slope", XmlConvert.ToString(_slope));
                writer.WriteAttributeString("screen", _screen.ToString());
                writer.WriteAttributeString("version", Assembly.GetExecutingAssembly().GetName().Version.ToString());
                writer.WriteAttributeString("date", DateTime.Now.ToLongDateString());
                writer.WriteAttributeString("timeOfDay", DateTime.Now.ToLongTimeString());
            }
            catch (XmlException xex)
            {
                Console.WriteLine(xex);
                success = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                success = false;
            }
            return success;
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
            bool success = true;
            try
            {
                Model fm = this.BuildModel();
                fm.RoundTerms(4);

                writer.WriteStartElement("Fitts_Model");
                writer.WriteAttributeString("applicable", XmlConvert.ToString(!this.UsedMetronome));
                writer.WriteAttributeString("N", XmlConvert.ToString(fm.N));
                writer.WriteAttributeString("MT", XmlConvert.ToString(fm.MTe));

                writer.WriteStartElement("Univariate_Model");
                writer.WriteAttributeString("applicable", XmlConvert.ToString(!this.UsedMetronome));
                writer.WriteAttributeString("TP_avg", XmlConvert.ToString(fm.Fitts_TP_avg_1d));
                writer.WriteAttributeString("TP_inv", XmlConvert.ToString(fm.Fitts_TP_inv_1d));
                writer.WriteAttributeString("regression", String.Format("MT = {0} + {1} * IDe", fm.Fitts_a_1d, fm.Fitts_b_1d));
                writer.WriteAttributeString("a", XmlConvert.ToString(fm.Fitts_a_1d));
                writer.WriteAttributeString("b", XmlConvert.ToString(fm.Fitts_b_1d));
                writer.WriteAttributeString("r", XmlConvert.ToString(fm.Fitts_r_1d));
                writer.WriteEndElement(); // </Univariate_Model>

                writer.WriteStartElement("Bivariate_Model");
                writer.WriteAttributeString("applicable", XmlConvert.ToString(!this.UsedMetronome && this.Is2D));
                writer.WriteAttributeString("TP_avg", XmlConvert.ToString(fm.Fitts_TP_avg_2d));
                writer.WriteAttributeString("TP_inv", XmlConvert.ToString(fm.Fitts_TP_inv_2d));
                writer.WriteAttributeString("regression", String.Format("MT = {0} + {1} * IDe", fm.Fitts_a_2d, fm.Fitts_b_2d));
                writer.WriteAttributeString("a", XmlConvert.ToString(fm.Fitts_a_2d));
                writer.WriteAttributeString("b", XmlConvert.ToString(fm.Fitts_b_2d));
                writer.WriteAttributeString("r", XmlConvert.ToString(fm.Fitts_r_2d));
                writer.WriteEndElement(); // </Bivariate_Model>

                writer.WriteEndElement(); // </Fitts_Model>

                writer.WriteStartElement("Pointing_Error_Model");
                writer.WriteAttributeString("applicable", XmlConvert.ToString(this.UsedMetronome));
                writer.WriteAttributeString("N", XmlConvert.ToString(fm.N));
                writer.WriteAttributeString("ErrorPct", XmlConvert.ToString(fm.ErrorPct));

                writer.WriteStartElement("Univariate_Model");
                writer.WriteAttributeString("applicable", XmlConvert.ToString(this.UsedMetronome));
                writer.WriteAttributeString("model", String.Format("Predicted = 1 - erf[(2.066 * W/A * (2^((MT-{0})/{1}) - 1)) / sqrt(2)]", fm.Fitts_a_1d, fm.Fitts_b_1d));
                writer.WriteAttributeString("regression", String.Format("Observed = {0} * Predicted + {1}", fm.Error_m_1d, fm.Error_b_1d)); // y=mx+b
                writer.WriteAttributeString("m", XmlConvert.ToString(fm.Error_m_1d));
                writer.WriteAttributeString("b", XmlConvert.ToString(fm.Error_b_1d));                
                writer.WriteAttributeString("r", XmlConvert.ToString(fm.Error_r_1d));
                writer.WriteEndElement(); // </Univariate_Model>

                writer.WriteStartElement("Bivariate_Model");
                writer.WriteAttributeString("applicable", XmlConvert.ToString(this.UsedMetronome && this.Is2D));
                writer.WriteAttributeString("model", String.Format("Predicted = 1 - erf[(2.066 * W/A * (2^((MT-{0})/{1}) - 1)) / sqrt(2)]", fm.Fitts_a_2d, fm.Fitts_b_2d));
                writer.WriteAttributeString("regression", String.Format("Observed = {0} * Predicted + {1}", fm.Error_m_2d, fm.Error_b_2d)); // y=mx+b
                writer.WriteAttributeString("m", XmlConvert.ToString(fm.Error_m_2d));
                writer.WriteAttributeString("b", XmlConvert.ToString(fm.Error_b_2d));
                writer.WriteAttributeString("r", XmlConvert.ToString(fm.Error_r_2d));
                writer.WriteEndElement(); // </Bivariate_Model>

                writer.WriteEndElement(); // </Pointing_Error_Model>
                
                writer.WriteEndDocument(); // </Fitts_Study>
            }
            catch (XmlException xex)
            {
                Console.WriteLine(xex);
                success = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                success = false;
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
            return success;
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
            bool success = true;
            try
            {
                // start up the xml text reader and move to the xml header
                reader.WhitespaceHandling = WhitespaceHandling.None;
                if (!reader.IsStartElement("Fitts_Study")) // moves to content and tests top tag
                    throw new XmlException("XML format error: Expected the <Fitts_Study> tag.");

                // read the session-level information from the header
                _subject = XmlConvert.ToInt32(reader.GetAttribute("subject"));
                _circular = XmlConvert.ToBoolean(reader.GetAttribute("circular"));
                int conditions = XmlConvert.ToInt32(reader.GetAttribute("conditions"));
                _mtpct = StringEx.String2DoubleArray(reader.GetAttribute("MTPct"));
                _a = StringEx.String2IntArray(reader.GetAttribute("A"));
                _w = StringEx.String2IntArray(reader.GetAttribute("W"));
                _intercept = XmlConvert.ToDouble(reader.GetAttribute("intercept"));
                _slope = XmlConvert.ToDouble(reader.GetAttribute("slope"));
                _screen = RectangleR.FromString(reader.GetAttribute("screen")); // saved for zoom feature
                
                // read the conditions and add condition objects to the session. the
                // conditions will be responsible for reading their individual trials.
                _conditions = new List<ConditionData>(conditions);
                for (int i = 0; i < conditions; i++)
                {
                    ConditionData fc = new ConditionData();
                    if (!fc.ReadFromXml(reader))
                        throw new XmlException("Failed to read the ConditionData.");
                    else
                        _conditions.Add(fc);
                }
                // here would be the place to read the Fitts_Model parameters, but the model
                // can be recreated from the session data anyway, so there's really no point.
            }
            catch (XmlException xex)
            {
                Console.WriteLine(xex);
                success = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                success = false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return success;
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
            bool success = true;
            try
            {
                // write an identifying title for this file. 
                writer.WriteLine("FittsStudy log analysis results for '{0}' on {1} at {2}. FittsStudy.exe version: {3}.\r\n",
                    this.FilenameBase + ".xml",     // Note: ((FileStream) writer.BaseStream).Name holds the file path.
                    DateTime.Now.ToLongDateString(),
                    DateTime.Now.ToLongTimeString(),
                    Assembly.GetExecutingAssembly().GetName().Version);

                // write the column headers with comma separation -- see Columns.txt
                writer.WriteLine("Subject,Circular?,Block,Condition,Trial,Practice?,Metronome?,MT%,MTPred,MT,a(given),b(given),A,W,ID,axis,angle,ae(1d),dx(1d),ae(2d),dx(2d),Ae(1d),SD(1d),We(1d),IDe(1d),TP(1d),Ae(2d),SD(2d),We(2d),IDe(2d),TP(2d),MTe,MTRatio,MeanMTe,MeanMTe(sx),MeanMTe(tx),Entries,Overshoots,Error?,Errors,Errors(sx),Errors(tx),Error%,Error%(sx),Error%(tx),SpatialOutlier?,TemporalOutlier?,SpatialOutliers,TemporalOutliers,StartX,StartY,EndX,EndY,TargetX,TargetY,Travel,Duration,Submovements,MaxVelocity,MaxAcceleration,MaxJerk,tMaxVelocity,tMaxAcceleration,tMaxJerk,TAC,MDC,ODC,MV,ME,MO,N,Fitts_TP_avg(1d),Fitts_TP_inv(1d),Fitts_a(1d),Fitts_b(1d),Fitts_r(1d),Fitts_TP_avg(2d),Fitts_TP_inv(2d),Fitts_a(2d),Fitts_b(2d),Fitts_r(2d),Error_m(1d),Error_b(1d),Error_r(1d),Error_m(2d),Error_b(2d),Error_r(2d)");

                // pre-compute session-level values here
                Model fm = this.BuildModel();
                fm.RoundTerms(4);

                // now iterate through all of the conditions
                for (int i = 0; i < _conditions.Count; i++)
                {
                    // get the condition and pre-compute any condition-level values here. we could 
                    // compute them again and again while writing each trial, but that is inefficient.
                    ConditionData cd = _conditions[i];

                    double cAe_1d = Math.Round(cd.GetAe(false), 4);
                    double cSD_1d = Math.Round(cd.GetSD(false), 4);
                    double cWe_1d = Math.Round(cd.GetWe(false), 4);
                    double cIDe_1d = Math.Round(cd.GetIDe(false), 4);
                    double cTP_1d = Math.Round(cd.GetTP(false), 4);

                    double cAe_2d = Math.Round(cd.GetAe(true), 4);
                    double cSD_2d = Math.Round(cd.GetSD(true), 4);
                    double cWe_2d = Math.Round(cd.GetWe(true), 4);
                    double cIDe_2d = Math.Round(cd.GetIDe(true), 4);
                    double cTP_2d = Math.Round(cd.GetTP(true), 4);

                    long meanMTe = cd.GetMTe(ExcludeOutliersType.None);
                    long meanMTe_sx = cd.GetMTe(ExcludeOutliersType.Spatial);
                    long meanMTe_tx = cd.GetMTe(ExcludeOutliersType.Temporal);

                    int errors = cd.GetNumErrors(ExcludeOutliersType.None);
                    int errors_sx = cd.GetNumErrors(ExcludeOutliersType.Spatial);
                    int errors_tx = cd.GetNumErrors(ExcludeOutliersType.Temporal);
                    
                    double errorPct = Math.Round(cd.GetErrorRate(ExcludeOutliersType.None), 4);
                    double errorPct_sx = Math.Round(cd.GetErrorRate(ExcludeOutliersType.Spatial), 4);
                    double errorPct_tx = Math.Round(cd.GetErrorRate(ExcludeOutliersType.Temporal), 4);

                    int nSpatialOutliers = cd.NumSpatialOutliers;
                    int nTemporalOutliers = cd.NumTemporalOutliers;

                    // within each condition, iterate through the trials. start at index 1 because
                    // the trial at index 0 is the special start-area trial, and should be ignored.
                    for (int j = 1; j <= cd.NumTrials; j++)
                    {
                        TrialData td = cd[j];
                        MovementData md = td.Movement; // the movement path itself

                        // calculate the resampled submovement profiles
                        MovementData.Profiles profiles = md.CreateResampledProfiles();
                        int vidx = SeriesEx.Max(profiles.Velocity, 0, -1); // max velocity
                        int aidx = SeriesEx.Max(profiles.Acceleration, 0, -1); // max acceleration
                        int jidx = SeriesEx.Max(profiles.Jerk, 0, -1); // max jerk

                        // calculate the MacKenzie et al. (2001) path analysis measures
                        MovementData.PathAnalyses analyses = md.DoPathAnalyses((PointR) td.Start, td.TargetCenterFromStart);

                        // write the spreadsheet row here
                        writer.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},{39},{40},{41},{42},{43},{44},{45},{46},{47},{48},{49},{50},{51},{52},{53},{54},{55},{56},{57},{58},{59},{60},{61},{62},{63},{64},{65},{66},{67},{68},{69},{70},{71},{72},{73},{74},{75},{76},{77},{78},{79},{80},{81},{82},{83},{84},{85},{86}",
                            _subject,                                                   // Subject,
                            _circular ? 1 : 0,                                          // Circular?
                            cd.Block,                                                   // Block,
                            cd.Index,                                                   // Condition,
                            td.Number,                                                  // Trial, (== j)
                            td.IsPractice ? 1 : 0,                                      // Practice?,

                            cd.UsedMetronome ? 1 : 0,                                   // Metronome?, (== td.UsedMetronome)
                            cd.MTPct,                                                   // MT%,
                            cd.MTPred,                                                  // MTPred, 
                            cd.MT,                                                      // MT, (== td.MT)
                            _intercept,                                                 // a(given),
                            _slope,                                                     // b(given),

                            cd.A,                                                       // A, (== td.A)
                            cd.W,                                                       // W, (== td.W)
                            Math.Round(cd.ID, 4),                                       // ID, (== td.ID)
                            Math.Round(GeotrigEx.Radians2Degrees(td.Axis), 4),          // axis,

                            Math.Round(GeotrigEx.Radians2Degrees(td.Angle), 4),         // angle,
                            Math.Round(td.GetAe(false), 4),                             // ae(1d),
                            Math.Round(td.GetDx(false), 4),                             // dx(1d),
                            
                            Math.Round(td.GetAe(true), 4),                              // ae(2d),
                            Math.Round(td.GetDx(true), 4),                              // dx(2d),

                            cAe_1d,                                                     // Ae(1d),
                            cSD_1d,                                                     // SD(1d),
                            cWe_1d,                                                     // We(1d),
                            cIDe_1d,                                                    // IDe(1d),
                            cTP_1d,                                                     // TP(1d),

                            cAe_2d,                                                     // Ae(2d),
                            cSD_2d,                                                     // SD(2d),
                            cWe_2d,                                                     // We(2d),
                            cIDe_2d,                                                    // IDe(2d),
                            cTP_2d,                                                     // TP(2d),

                            td.MTe,                                                     // MTe,
                            Math.Round(td.MTRatio, 4),                                  // MTRatio,
                            meanMTe,                                                    // MeanMTe,
                            meanMTe_sx,                                                 // MeanMTe(sx),
                            meanMTe_tx,                                                 // MeanMTe(tx),

                            td.TargetEntries,                                           // Entries,
                            td.TargetOvershoots,                                        // Overshoots,
                            td.IsError ? 1 : 0,                                         // Error?,
                            errors,                                                     // Errors,
                            errors_sx,                                                  // Errors(sx),
                            errors_tx,                                                  // Errors(tx),
                            errorPct,                                                   // Error%,
                            errorPct_sx,                                                // Error%(sx),
                            errorPct_tx,                                                // Error%(tx),

                            td.IsSpatialOutlier ? 1 : 0,                                // SpatialOutlier?,
                            td.IsTemporalOutlier ? 1 : 0,                               // TemporalOutlier?,
                            nSpatialOutliers,                                           // SpatialOutliers,
                            nTemporalOutliers,                                          // TemporalOutliers,

                            td.Start.X,                                                 // StartX
                            td.Start.Y,                                                 // StartY
                            td.End.X,                                                   // EndX
                            td.End.Y,                                                   // EndY
                            td.TargetCenterFromStart.X,                                 // TargetX
                            td.TargetCenterFromStart.Y,                                 // TargetY

                            Math.Round(md.Travel, 4),                                   // Travel,
                            md.Duration,                                                // Duration,
                            md.GetNumSubmovements(),                                    // Submovements,
                            
                            Math.Round(profiles.Velocity[vidx].Y, 4),                   // MaxVelocity,
                            Math.Round(profiles.Acceleration[aidx].Y, 4),               // MaxAcceleration,
                            Math.Round(profiles.Jerk[jidx].Y, 4),                       // MaxJerk,
                            Math.Round(profiles.Velocity[vidx].X / md.Duration, 4),     // tMaxVelocity,
                            Math.Round(profiles.Acceleration[aidx].X / md.Duration, 4), // tMaxAcceleration,
                            Math.Round(profiles.Jerk[jidx].X / md.Duration, 4),         // tMaxJerk,

                            analyses.TaskAxisCrossings,                                 // TAC,
                            analyses.MovementDirectionChanges,                          // MDC,
                            analyses.OrthogonalDirectionChanges,                        // ODC,
                            Math.Round(analyses.MovementVariability, 4),                // MV,
                            Math.Round(analyses.MovementError, 4),                      // ME,
                            Math.Round(analyses.MovementOffset, 4),                     // MO,

                            fm.N,                                                       // N,
                            fm.Fitts_TP_avg_1d,                                         // Fitts_TP_avg(1d),
                            fm.Fitts_TP_inv_1d,                                         // Fitts_TP_inv(1d),
                            fm.Fitts_a_1d,                                              // Fitts_a(1d),
                            fm.Fitts_b_1d,                                              // Fitts_b(1d),
                            fm.Fitts_r_1d,                                              // Fitts_r(1d),

                            fm.Fitts_TP_avg_2d,                                         // Fitts_TP_avg(2d),
                            fm.Fitts_TP_inv_2d,                                         // Fitts_TP_inv(2d),
                            fm.Fitts_a_2d,                                              // Fitts_a(2d),
                            fm.Fitts_b_2d,                                              // Fitts_b(2d),
                            fm.Fitts_r_2d,                                              // Fitts_r(2d),

                            fm.Error_m_1d,                                              // Error_m(1d),
                            fm.Error_b_1d,                                              // Error_b(1d),
                            fm.Error_r_1d,                                              // Error_r(1d),

                            fm.Error_m_2d,                                              // Error_m(2d),
                            fm.Error_b_2d,                                              // Error_b(2d),
                            fm.Error_r_2d                                               // Error_r(2d)
                            );
                    }
                }
            }
            catch (IOException ioex)
            {
                Console.WriteLine(ioex);
                success = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                success = false;
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
            return success;
        }

        #endregion
    }
}
