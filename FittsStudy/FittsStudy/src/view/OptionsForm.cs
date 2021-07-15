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
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace FittsStudy
{
    public partial class OptionsForm : System.Windows.Forms.Form
    {
        #region Options Data

        /// <summary>
        /// 
        /// </summary>
        public class Options
        {
            public int Subject;
            public string Directory;
            public bool Is2D;
            public int Trials;
            public int Practice;
            public int Blocks;
            public bool Randomize;
            public double Intercept;
            public double Slope;
            public double[] MTPct;
            public int[] A;
            public int[] W;

            /// <summary>
            /// 
            /// </summary>
            public Options() // defaults
            {
                this.Subject = 1;
                this.Directory = System.IO.Directory.GetCurrentDirectory();
                this.Is2D = false; // vertical ribbons
                this.Trials = 23;
                this.Practice = 3;
                this.Blocks = 1; // number of times to run all conditions
                this.Randomize = true;
                this.Intercept = 100.0;
                this.Slope = 200.0;
                this.MTPct = null;
                this.A = new int[3] { 256, 384, 512 };
                this.W = new int[6] { 8, 16, 32, 64, 96, 128 };
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="o"></param>
            public Options(Options o) // copy constructor
            {
                this.Subject = o.Subject;
                this.Directory = o.Directory;
                this.Is2D = o.Is2D;
                this.Trials = o.Trials;
                this.Practice = o.Practice;
                this.Blocks = o.Blocks;
                this.Randomize = o.Randomize;
                this.Intercept = o.Intercept;
                this.Slope = o.Slope;

                if (o.MTPct != null)
                {
                    this.MTPct = new double[o.MTPct.Length];
                    Array.Copy(o.MTPct, this.MTPct, this.MTPct.Length);
                }
                else this.MTPct = null;

                if (o.A != null)
                {
                    this.A = new int[o.A.Length];
                    Array.Copy(o.A, this.A, this.A.Length);
                }
                else this.A = null;
                
                if (o.W != null)
                {
                    this.W = new int[o.W.Length];
                    Array.Copy(o.W, this.W, this.W.Length);
                }
                else this.W = null;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public Options Copy() // deep copy
            {
                return new Options(this);
            }
        }

        #endregion

        #region Options Form

        private OptionsForm.Options _o;

        /// <summary>
        /// 
        /// </summary>
        public OptionsForm()
        {
            InitializeComponent();
            _o = new Options(); // defaults
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        public OptionsForm(OptionsForm.Options o)
        {
            InitializeComponent();
            _o = new Options(o); // copy in
        }

        /// <summary>
        /// Users of this dialog can access 'Settings' after a DialogResult.OK result
        /// </summary>
        public OptionsForm.Options Settings
        {
            get
            {
                return _o;
            }
        }

        /// <summary>
        /// The form load handler transfers data from the settings provided to the dialog's 
        /// constructor to the dialog's controls. This is the "transfer in" handler.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void OptionsForm_Load(object sender, EventArgs e)
        {
            numSubject.Value = _o.Subject;
            txtDirectory.Text = _o.Directory;
            txtDirectory.SelectionStart = txtDirectory.TextLength;
            cboLayout.SelectedIndex = _o.Is2D ? 1 : 0;
            numTrials.Value = _o.Trials;
            numPracticeTrials.Value = _o.Practice;
            numBlocks.Value = _o.Blocks;
            ntxtIntercept.Value = _o.Intercept; // ms
            ntxtSlope.Value = _o.Slope; // ms/bit
            chkRandomize.Checked = _o.Randomize;

            if (_o.MTPct != null)
            {
                foreach (double dtp in _o.MTPct)
                    lstMT.Items.Add(dtp);
            }
            if (_o.A != null)
            {
                foreach (int a in _o.A)
                    lstA.Items.Add(a);
            }
            if (_o.W != null)
            {
                foreach (int w in _o.W)
                    lstW.Items.Add(w);
            }

            ComputeIDs();
            ComputeTotalTrials();
            CheckEnableInterceptSlope();
            CheckEnableOK();

            // fill in the screen bounds label
            Rectangle screen = Screen.PrimaryScreen.Bounds;
            lblScreenDimension.Text = String.Format("Screen dimension: {0}{1}{2}", screen.Width, (char) 215, screen.Height);
        }

        /// <summary>
        /// The handler for the dialog's OK button click event. When the OK button is clicked, data
        /// is transfered from the controls to the dialog's <b>Settings</b> property, which a caller
        /// can access. This is the "transfer out" handler.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void cmdOK_Click(object sender, EventArgs e)
        {
            _o.Subject = (int) numSubject.Value;
            _o.Directory = txtDirectory.Text;
            _o.Is2D = (cboLayout.SelectedIndex == 1);
            _o.Trials = (int) numTrials.Value;
            _o.Practice = (int) numPracticeTrials.Value;
            _o.Blocks = (int) numBlocks.Value;
            _o.Intercept = ntxtIntercept.Value; // ms
            _o.Slope = ntxtSlope.Value; // ms/bit
            _o.Randomize = chkRandomize.Checked;

            _o.MTPct = (lstMT.Items.Count > 0) ? new double[lstMT.Items.Count] : null;
            for (int i = 0; i < lstMT.Items.Count; i++)
                _o.MTPct[i] = (double) (decimal) lstMT.Items[i];

            _o.A = (lstA.Items.Count > 0) ? new int[lstA.Items.Count] : null;
            for (int i = 0; i < lstA.Items.Count; i++)
                _o.A[i] = (int) lstA.Items[i];

            _o.W = (lstW.Items.Count > 0) ? new int[lstW.Items.Count] : null;
            for (int i = 0; i < lstW.Items.Count; i++)
                _o.W[i] = (int) lstW.Items[i];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = txtDirectory.Text;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtDirectory.Text = dlg.SelectedPath;
            }
        }

        #endregion

        #region MT%, A, W Event Handlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void cmdAddMT_Click(object sender, EventArgs e)
        {
            decimal mtp = numMT.Value / 100m;
            if (!lstMT.Items.Contains(mtp))
            {
                lstMT.Items.Add(mtp);
                ComputeTotalTrials();
                CheckEnableInterceptSlope();
                CheckEnableOK();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void numMT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                decimal mtp = numMT.Value / 100m;
                if (!lstMT.Items.Contains(mtp))
                {
                    lstMT.Items.Add(mtp);
                    ComputeTotalTrials();
                    CheckEnableInterceptSlope();
                    CheckEnableOK();
                }
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void cmdRemoveMT_Click(object sender, EventArgs e)
        {
            lstMT.SuspendLayout();
            for (int i = lstMT.SelectedIndices.Count - 1; i >= 0; i--)
            {
                lstMT.Items.RemoveAt(lstMT.SelectedIndices[i]);
            }
            lstMT.ResumeLayout();

            ComputeTotalTrials();
            CheckEnableInterceptSlope();
            CheckEnableOK();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void lstMT_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmdRemoveMT.Enabled = (lstMT.SelectedIndices.Count > 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ntxtSlope_TextChanged(object sender, EventArgs e)
        {
            CheckEnableOK();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdPopulate_Click(object sender, EventArgs e)
        {
            lstMT.SuspendLayout();
            lstMT.Items.Clear();
            for (decimal i = 0.40m; i <= 1.30m; i += 0.10m)
            {
                lstMT.Items.Add(i);
            }
            lstMT.ResumeLayout();

            ComputeTotalTrials();
            CheckEnableInterceptSlope();
            CheckEnableOK();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void cmdAddA_Click(object sender, EventArgs e)
        {
            int a = (int) numA.Value;
            if (!lstA.Items.Contains(a))
            {
                lstA.Items.Add(a);
                ComputeIDs();
                ComputeTotalTrials();
                CheckEnableOK();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void numA_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int a = (int) numA.Value;
                if (!lstA.Items.Contains(a))
                {
                    lstA.Items.Add(a);
                    ComputeIDs();
                    ComputeTotalTrials();
                    CheckEnableOK();
                }
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void cmdRemoveA_Click(object sender, EventArgs e)
        {
            lstA.SuspendLayout();
            for (int i = lstA.SelectedIndices.Count - 1; i >= 0; i--)
            {
                lstA.Items.RemoveAt(lstA.SelectedIndices[i]);
            }
            lstA.ResumeLayout();

            ComputeIDs();
            ComputeTotalTrials();
            CheckEnableOK();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void lstA_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmdRemoveA.Enabled = (lstA.SelectedIndices.Count > 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void cmdAddW_Click(object sender, EventArgs e)
        {
            int w = (int) numW.Value;
            if (!lstW.Items.Contains(w))
            {
                lstW.Items.Add(w);
                ComputeIDs();
                ComputeTotalTrials();
                CheckEnableOK();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void numW_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int w = (int) numW.Value;
                if (!lstW.Items.Contains(w))
                {
                    lstW.Items.Add(w);
                    ComputeIDs();
                    ComputeTotalTrials();
                    CheckEnableOK();
                }
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void cmdRemoveW_Click(object sender, EventArgs e)
        {
            lstW.SuspendLayout();
            for (int i = lstW.SelectedIndices.Count - 1; i >= 0; i--)
            {
                lstW.Items.RemoveAt(lstW.SelectedIndices[i]);
            }
            lstW.ResumeLayout();

            ComputeIDs();
            ComputeTotalTrials();
            CheckEnableOK();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void lstW_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmdRemoveW.Enabled = (lstW.SelectedIndices.Count > 0);
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckEnableOK()
        {
            cmdOK.Enabled =
                lstA.Items.Count > 0 &&
                lstW.Items.Count > 0 &&
                (lstMT.Items.Count == 0 || ntxtSlope.Value > 0) &&
                txtDirectory.TextLength > 0 &&
                Directory.Exists(txtDirectory.Text);
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckEnableInterceptSlope()
        {
            lblIntercept.Enabled = lstMT.Items.Count > 0;
            ntxtIntercept.Enabled = lstMT.Items.Count > 0;
            lblSlope.Enabled = lstMT.Items.Count > 0;
            ntxtSlope.Enabled = lstMT.Items.Count > 0; 
        }

        /// <summary>
        /// 
        /// </summary>
        private void ComputeIDs()
        {
            lstID.SuspendLayout();
            lstID.Items.Clear();
            
            for (int i = 0; i < lstA.Items.Count; i++)
            {
                double a = (int) lstA.Items[i];
                for (int j = 0; j < lstW.Items.Count; j++)
                {
                    double w = (int) lstW.Items[j];
                    double id = Math.Round(Math.Log(a / w + 1.0, 2.0), 4);
                    if (!lstID.Items.Contains(id))
                    {
                        lstID.Items.Add(id);
                    }
                }
            }
            lstID.ResumeLayout();

            lblID.Text = "Indices of Difficulty\r\n(" + lstID.Items.Count + " unique):";
        }

        #endregion

        #region Total Trials

        /// <summary>
        /// Enforce an odd number of total trials if the ISO 9241-9 layout is used. This pattern
        /// as prescribed in ISO 9241-9 requires an odd number of targets to be used.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void cboLayout_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLayout.SelectedIndex == 0)
            {
                numTrials.Increment = 1;
            }
            else if (cboLayout.SelectedIndex == 1)
            {
                if (numTrials.Value % 2 == 0)
                {
                    numTrials.Value++;
                }
                numTrials.Increment = 2;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void numTrials_ValueChanged(object sender, EventArgs e)
        {
            if (cboLayout.SelectedIndex == 1) // enforce odd trials for ISO 9241-9
            {
                if (numTrials.Value % 2 == 0)
                {
                    numTrials.Value++;
                }
            }
            numPracticeTrials.Maximum = numTrials.Value - 2;
            ComputeTotalTrials();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void numBlocks_ValueChanged(object sender, EventArgs e)
        {
            ComputeTotalTrials();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ComputeTotalTrials()
        {
            int mt = lstMT.Items.Count > 0 ? lstMT.Items.Count : 1;
            int total = mt * lstA.Items.Count * lstW.Items.Count * (int) numTrials.Value * (int) numBlocks.Value;
            lblTotalTrials.Text = String.Format("Total trials: {0}", total);
        }

        #endregion

    }
}