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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using WobbrockLib;
using WobbrockLib.Types;
using WobbrockLib.Controls;

namespace FittsStudy
{
    /// <summary>
    /// Encapsulates a dialog that displays a graph and model terms for a Fitts' law model.
    /// </summary>
    public partial class ModelForm : Form
    {
        private SessionData _sdata;
        private string _filename;

        /// <summary>
        /// Constructs a FittsModelForm instance, which is capable of displaying a Fitts' law model
        /// including a graph and model terms.
        /// </summary>
        /// <param name="sdata">The session instance that contains all data for the session.</param>
        /// <param name="txtFile">The TXT file that the model information should be saved to.</param>
        public ModelForm(SessionData sdata, string txtFile)
        {
            InitializeComponent();
            _sdata = sdata;
            _filename = txtFile;
        }

        /// <summary>
        /// Handles the Load event for the form. Loads the data from the model into the graph control, and
        /// displays the model terms in the read-only textbox.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void ModelForm_Load(object sender, EventArgs e)
        {
            //
            // Ask the session instance to build a Fitts' law model.
            //
            Model fm = _sdata.BuildModel();
            fm.RoundTerms(4); // round for display

            if (_sdata.MTPct == null) // regular Fitts' law experiment (no metronome)
            {
                PointR min = PointR.Empty;
                PointR max = PointR.Empty;

                //
                // Add the (IDe, MTe) points for plotting.
                //
                Plot.Series s1 = new Plot.Series("Fitts model 1D points", Color.Tomato, Color.Tomato, true, false);
                s1.AddPoints(fm.FittsPts_1d); // add the points to the series
                grfFitts1D.AddSeries(s1); // add the series to the graph
                if (s1.NumPoints > 1)
                {
                    RectangleR r = s1.DomainRange; // get the domain and range of the first series
                    Plot.Series s3 = new Plot.Series("Fitts model 1D line", Color.OrangeRed, Color.OrangeRed, false, true);
                    s3.AddPoint(0.0, fm.Fitts_a_1d); // y-intercept
                    s3.AddPoint(r.Right, fm.Fitts_b_1d * r.Right + fm.Fitts_a_1d); // rightmost point
                    grfFitts1D.AddSeries(s3);
                    min = s3[0];
                    max = s3[1];
                }

                if (_sdata.Is2D)
                {
                    Plot.Series s2 = new Plot.Series("Fitts model 2D points", Color.Tomato, Color.Tomato, true, false);
                    s2.AddPoints(fm.FittsPts_2d);
                    grfFitts2D.AddSeries(s2);
                    if (s2.NumPoints > 1)
                    {
                        RectangleR r = s2.DomainRange; // get the domain and range of the first series
                        Plot.Series s4 = new Plot.Series("Fitts model 2D line", Color.OrangeRed, Color.OrangeRed, false, true);
                        s4.AddPoint(0.0, fm.Fitts_a_2d); // y-intercept
                        s4.AddPoint(r.Right, fm.Fitts_b_2d * r.Right + fm.Fitts_a_2d); // rightmost point
                        grfFitts2D.AddSeries(s4);
                        min.X = Math.Min(min.X, s4[0].X);
                        min.Y = Math.Min(min.Y, s4[0].Y);
                        max.X = Math.Max(max.X, s4[1].X);
                        max.Y = Math.Max(max.Y, s4[1].Y);
                    }
                }

                // add a series to ensure both graphs display at the same scale for better comparisons
                Plot.Series s5 = new Plot.Series("Fitts min-max", Color.Black, Color.Black, false, false);
                s5.AddPoint(min);
                s5.AddPoint(max);
                grfFitts1D.AddSeries(s5);
                grfFitts2D.AddSeries(s5);

                //
                // Fill the text box with the Fitts' law model parameters.
                //
                txtFitts1D.Text =
                    string.Format("Fitts' Law - Univariate for Subject {0}\r\n", _sdata.Subject) +
                    string.Format("N = {0}\r\n", fm.N) +
                    string.Format("MTavg = {0} ms\r\n", fm.MTe) +
                    string.Format("Error% = {0}\r\n", fm.ErrorPct) +

                    string.Format("MT(1d) = {0} + {1} * IDe\r\n", fm.Fitts_a_1d, fm.Fitts_b_1d) +
                    string.Format("TP_avg(1d) = {0} bits/s\r\n", fm.Fitts_TP_avg_1d) +
                    string.Format("TP_inv(1d) = {0} bits/s\r\n", fm.Fitts_TP_inv_1d) +

                    string.Format("a(1d) = {0} ms\r\n", fm.Fitts_a_1d) +
                    string.Format("b(1d) = {0} ms/bit\r\n", fm.Fitts_b_1d) +
                    string.Format("r(1d) = {0}\r\n", fm.Fitts_r_1d);

                if (_sdata.Is2D)
                {
                    txtFitts2D.Text =
                        string.Format("Fitts' Law - Bivariate for Subject {0}\r\n", _sdata.Subject) +
                        string.Format("N = {0}\r\n", fm.N) +
                        string.Format("MTavg = {0} ms\r\n", fm.MTe) +
                        string.Format("Error% = {0}\r\n", fm.ErrorPct) +

                        string.Format("MT(2d) = {0} + {1} * IDe\r\n", fm.Fitts_a_2d, fm.Fitts_b_2d) +
                        string.Format("TP_avg(2d) = {0} bits/s\r\n", fm.Fitts_TP_avg_2d) +
                        string.Format("TP_inv(2d) = {0} bits/s\r\n", fm.Fitts_TP_inv_2d) +

                        string.Format("a(2d) = {0} ms\r\n", fm.Fitts_a_2d) +
                        string.Format("b(2d) = {0} ms/bit\r\n", fm.Fitts_b_2d) +
                        string.Format("r(2d) = {0}\r\n", fm.Fitts_r_2d);
                }
                else tabModel.TabPages.RemoveAt(1); // remove the Fitts' law 2D tab
                tabModel.TabPages.RemoveAt(tabModel.TabPages.Count - 1);  // remove the Error model 2D tab
                tabModel.TabPages.RemoveAt(tabModel.TabPages.Count - 1); // remove the Error model 1D tab
            }
            else // metronome experiment
            {
                //
                // Add the diagonal line for easy visual model comparisons.
                //
                Plot.Series s0 = new Plot.Series("Diagonal", Color.LightGray, Color.LightGray, false, true);
                s0.AddPoint(0.0, 0.0);
                s0.AddPoint(1.0, 1.0);
                grfErrors1D.AddSeries(s0);
                grfErrors2D.AddSeries(s0);

                //
                // Add the (predicted error rate, observed error rate) points for plotting.
                //
                Plot.Series s1 = new Plot.Series("Error model 1D points", Color.Tomato, Color.Tomato, true, false);
                s1.AddPoints(fm.ErrorPts_1d);
                grfErrors1D.AddSeries(s1);
                if (s1.NumPoints > 1)
                {
                    RectangleR r = s1.DomainRange;
                    Plot.Series s3 = new Plot.Series("Error model 1D line", Color.OrangeRed, Color.OrangeRed, false, true);
                    s3.AddPoint(0.0, fm.Error_b_1d); // y-intercept
                    s3.AddPoint(r.Right, fm.Error_m_1d * r.Right + fm.Error_b_1d);
                    grfErrors1D.AddSeries(s3);
                }

                if (_sdata.Is2D)
                {
                    Plot.Series s2 = new Plot.Series("Error model 2D points", Color.Tomato, Color.Tomato, true, false);
                    s2.AddPoints(fm.ErrorPts_2d);
                    grfErrors2D.AddSeries(s2);
                    if (s2.NumPoints > 1)
                    {
                        RectangleR r = s2.DomainRange;
                        Plot.Series s4 = new Plot.Series("Error model 2D line", Color.OrangeRed, Color.OrangeRed, false, true);
                        s4.AddPoint(0.0, fm.Error_b_2d); // y-intercept
                        s4.AddPoint(r.Right, fm.Error_m_2d * r.Right + fm.Error_b_2d);
                        grfErrors2D.AddSeries(s4);
                    }
                }

                //
                // Fill the text box with the Error model parameters.
                //
                txtErrors1D.Text =
                    string.Format("Pointing Error Model - Univariate for Subject {0}\r\n", _sdata.Subject) +
                    string.Format("N = {0}\r\n", fm.N) +
                    string.Format("MTavg = {0} ms\r\n", fm.MTe) +
                    string.Format("Error% = {0}\r\n", fm.ErrorPct) +

                    string.Format("Predicted(1d) = 1 - erf[(2.066 * W/A * (2^((MT-{0})/{1}) - 1)) / sqrt(2)]\r\n", fm.Fitts_a_1d, fm.Fitts_b_1d) +
                    string.Format("Observed(1d) = {0} * Predicted + {1}\r\n", fm.Error_m_1d, fm.Error_b_1d) +
                    string.Format("m(1d) = {0}\r\n", fm.Error_m_1d) +
                    string.Format("b(1d) = {0}\r\n", fm.Error_b_1d) +
                    string.Format("r(1d) = {0}\r\n", fm.Error_r_1d);

                if (_sdata.Is2D)
                {
                    txtErrors2D.Text =
                        string.Format("Pointing Error Model - Bivariate for Subject {0}\r\n", _sdata.Subject) +
                        string.Format("N = {0}\r\n", fm.N) +
                        string.Format("MTavg = {0} ms\r\n", fm.MTe) +
                        string.Format("Error% = {0}\r\n", fm.ErrorPct) +

                        string.Format("Predicted(2d) = 1 - erf[(2.066 * W/A * (2^((MT-{0})/{1}) - 1)) / sqrt(2)]\r\n", fm.Fitts_a_2d, fm.Fitts_b_2d) +
                        string.Format("Observed(2d) = {0} * Predicted + {1}\r\n", fm.Error_m_2d, fm.Error_b_2d) +
                        string.Format("m(2d) = {0}\r\n", fm.Error_m_2d) +
                        string.Format("b(2d) = {0}\r\n", fm.Error_b_2d) +
                        string.Format("r(2d) = {0}\r\n", fm.Error_r_2d);
                }
                else tabModel.TabPages.RemoveAt(3); // remove the Error model 2D tab
                tabModel.TabPages.RemoveAt(0); // remove the Fitts' law 1D tab
                tabModel.TabPages.RemoveAt(0); // remove the Fitts' law 2D tab
            }
        }

        /// <summary>
        /// Handles a click on the "Save to Notepad" button, which writes out the model terms to
        /// a text file and then opens Notepad with that file. This saves the model for future use.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void cmdNotepad_Click(object sender, EventArgs e)
        {
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(_filename, false, Encoding.UTF8);
                writer.Write(txtFitts1D.Text);
                writer.WriteLine();
                writer.Write(txtFitts2D.Text);
                writer.WriteLine();
                writer.Write(txtErrors1D.Text);
                writer.WriteLine();
                writer.Write(txtErrors2D.Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
            Process.Start("notepad.exe", _filename);
        }
    }
}