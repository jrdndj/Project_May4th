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
namespace FittsStudy
{
    partial class OptionsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.cmdCancel = new System.Windows.Forms.Button();
            this.lblA = new System.Windows.Forms.Label();
            this.lblID = new System.Windows.Forms.Label();
            this.lblSubject = new System.Windows.Forms.Label();
            this.lblW = new System.Windows.Forms.Label();
            this.lstID = new System.Windows.Forms.ListBox();
            this.lstW = new System.Windows.Forms.ListBox();
            this.lstA = new System.Windows.Forms.ListBox();
            this.cmdAddA = new System.Windows.Forms.Button();
            this.cmdRemoveA = new System.Windows.Forms.Button();
            this.cmdRemoveW = new System.Windows.Forms.Button();
            this.cmdAddW = new System.Windows.Forms.Button();
            this.lblScreenDimension = new System.Windows.Forms.Label();
            this.cmdOK = new System.Windows.Forms.Button();
            this.lblTotalTrials = new System.Windows.Forms.Label();
            this.lblDirectory = new System.Windows.Forms.Label();
            this.chkRandomize = new System.Windows.Forms.CheckBox();
            this.lblTrials = new System.Windows.Forms.Label();
            this.lblIntercept = new System.Windows.Forms.Label();
            this.lblSlope = new System.Windows.Forms.Label();
            this.lblMT = new System.Windows.Forms.Label();
            this.txtDirectory = new System.Windows.Forms.TextBox();
            this.cmdBrowse = new System.Windows.Forms.Button();
            this.cmdRemoveMT = new System.Windows.Forms.Button();
            this.cmdAddMT = new System.Windows.Forms.Button();
            this.lstMT = new System.Windows.Forms.ListBox();
            this.numTrials = new System.Windows.Forms.NumericUpDown();
            this.grpConditions = new System.Windows.Forms.GroupBox();
            this.cmdPopulate = new System.Windows.Forms.Button();
            this.numBlocks = new System.Windows.Forms.NumericUpDown();
            this.lblBlocks = new System.Windows.Forms.Label();
            this.ntxtSlope = new WobbrockLib.Controls.NumericTextBox(this.components);
            this.ntxtIntercept = new WobbrockLib.Controls.NumericTextBox(this.components);
            this.numW = new System.Windows.Forms.NumericUpDown();
            this.numA = new System.Windows.Forms.NumericUpDown();
            this.numMT = new System.Windows.Forms.NumericUpDown();
            this.numSubject = new System.Windows.Forms.NumericUpDown();
            this.grpTrials = new System.Windows.Forms.GroupBox();
            this.cboLayout = new System.Windows.Forms.ComboBox();
            this.lblLayout = new System.Windows.Forms.Label();
            this.numPracticeTrials = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize) (this.numTrials)).BeginInit();
            this.grpConditions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.numBlocks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.numW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.numA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.numMT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.numSubject)).BeginInit();
            this.grpTrials.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.numPracticeTrials)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cmdCancel.Location = new System.Drawing.Point(399, 370);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(68, 24);
            this.cmdCancel.TabIndex = 10;
            this.cmdCancel.Text = "Cancel";
            // 
            // lblA
            // 
            this.lblA.AutoSize = true;
            this.lblA.Location = new System.Drawing.Point(119, 25);
            this.lblA.Name = "lblA";
            this.lblA.Size = new System.Drawing.Size(61, 13);
            this.lblA.TabIndex = 10;
            this.lblA.Text = "Amplitudes:";
            // 
            // lblID
            // 
            this.lblID.Location = new System.Drawing.Point(339, 25);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(99, 36);
            this.lblID.TabIndex = 20;
            this.lblID.Text = "Indices of Difficulty";
            // 
            // lblSubject
            // 
            this.lblSubject.AutoSize = true;
            this.lblSubject.Location = new System.Drawing.Point(12, 9);
            this.lblSubject.Name = "lblSubject";
            this.lblSubject.Size = new System.Drawing.Size(60, 13);
            this.lblSubject.TabIndex = 0;
            this.lblSubject.Text = "Subject ID:";
            this.lblSubject.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblW
            // 
            this.lblW.AutoSize = true;
            this.lblW.Location = new System.Drawing.Point(229, 25);
            this.lblW.Name = "lblW";
            this.lblW.Size = new System.Drawing.Size(43, 13);
            this.lblW.TabIndex = 15;
            this.lblW.Text = "Widths:";
            // 
            // lstID
            // 
            this.lstID.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lstID.FormattingEnabled = true;
            this.lstID.IntegralHeight = false;
            this.lstID.Location = new System.Drawing.Point(342, 68);
            this.lstID.Name = "lstID";
            this.lstID.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lstID.Size = new System.Drawing.Size(104, 160);
            this.lstID.Sorted = true;
            this.lstID.TabIndex = 21;
            this.lstID.Tag = "";
            // 
            // lstW
            // 
            this.lstW.FormattingEnabled = true;
            this.lstW.IntegralHeight = false;
            this.lstW.Location = new System.Drawing.Point(232, 68);
            this.lstW.Name = "lstW";
            this.lstW.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstW.Size = new System.Drawing.Size(104, 160);
            this.lstW.TabIndex = 19;
            this.lstW.Tag = "";
            this.lstW.SelectedIndexChanged += new System.EventHandler(this.lstW_SelectedIndexChanged);
            // 
            // lstA
            // 
            this.lstA.BackColor = System.Drawing.SystemColors.Window;
            this.lstA.FormattingEnabled = true;
            this.lstA.IntegralHeight = false;
            this.lstA.Location = new System.Drawing.Point(122, 68);
            this.lstA.Name = "lstA";
            this.lstA.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstA.Size = new System.Drawing.Size(104, 160);
            this.lstA.TabIndex = 14;
            this.lstA.Tag = "";
            this.lstA.SelectedIndexChanged += new System.EventHandler(this.lstA_SelectedIndexChanged);
            // 
            // cmdAddA
            // 
            this.cmdAddA.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.cmdAddA.Location = new System.Drawing.Point(185, 44);
            this.cmdAddA.Name = "cmdAddA";
            this.cmdAddA.Size = new System.Drawing.Size(16, 20);
            this.cmdAddA.TabIndex = 12;
            this.cmdAddA.Tag = "";
            this.cmdAddA.Text = "+";
            this.cmdAddA.UseVisualStyleBackColor = true;
            this.cmdAddA.Click += new System.EventHandler(this.cmdAddA_Click);
            // 
            // cmdRemoveA
            // 
            this.cmdRemoveA.Enabled = false;
            this.cmdRemoveA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.cmdRemoveA.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.cmdRemoveA.Location = new System.Drawing.Point(203, 44);
            this.cmdRemoveA.Name = "cmdRemoveA";
            this.cmdRemoveA.Size = new System.Drawing.Size(16, 20);
            this.cmdRemoveA.TabIndex = 13;
            this.cmdRemoveA.Tag = "";
            this.cmdRemoveA.Text = "-";
            this.cmdRemoveA.UseVisualStyleBackColor = true;
            this.cmdRemoveA.Click += new System.EventHandler(this.cmdRemoveA_Click);
            // 
            // cmdRemoveW
            // 
            this.cmdRemoveW.Enabled = false;
            this.cmdRemoveW.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.cmdRemoveW.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.cmdRemoveW.Location = new System.Drawing.Point(313, 44);
            this.cmdRemoveW.Name = "cmdRemoveW";
            this.cmdRemoveW.Size = new System.Drawing.Size(16, 20);
            this.cmdRemoveW.TabIndex = 18;
            this.cmdRemoveW.Tag = "";
            this.cmdRemoveW.Text = "-";
            this.cmdRemoveW.UseVisualStyleBackColor = true;
            this.cmdRemoveW.Click += new System.EventHandler(this.cmdRemoveW_Click);
            // 
            // cmdAddW
            // 
            this.cmdAddW.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.cmdAddW.Location = new System.Drawing.Point(295, 44);
            this.cmdAddW.Name = "cmdAddW";
            this.cmdAddW.Size = new System.Drawing.Size(16, 20);
            this.cmdAddW.TabIndex = 17;
            this.cmdAddW.Tag = "";
            this.cmdAddW.Text = "+";
            this.cmdAddW.UseVisualStyleBackColor = true;
            this.cmdAddW.Click += new System.EventHandler(this.cmdAddW_Click);
            // 
            // lblScreenDimension
            // 
            this.lblScreenDimension.AutoSize = true;
            this.lblScreenDimension.Location = new System.Drawing.Point(12, 370);
            this.lblScreenDimension.Name = "lblScreenDimension";
            this.lblScreenDimension.Size = new System.Drawing.Size(102, 13);
            this.lblScreenDimension.TabIndex = 7;
            this.lblScreenDimension.Text = "Screen dimensions: ";
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cmdOK.Location = new System.Drawing.Point(325, 370);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(68, 24);
            this.cmdOK.TabIndex = 9;
            this.cmdOK.Text = "OK";
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // lblTotalTrials
            // 
            this.lblTotalTrials.AutoSize = true;
            this.lblTotalTrials.Location = new System.Drawing.Point(181, 370);
            this.lblTotalTrials.Name = "lblTotalTrials";
            this.lblTotalTrials.Size = new System.Drawing.Size(58, 13);
            this.lblTotalTrials.TabIndex = 8;
            this.lblTotalTrials.Text = "Total trials:";
            // 
            // lblDirectory
            // 
            this.lblDirectory.AutoSize = true;
            this.lblDirectory.Location = new System.Drawing.Point(142, 9);
            this.lblDirectory.Name = "lblDirectory";
            this.lblDirectory.Size = new System.Drawing.Size(88, 13);
            this.lblDirectory.TabIndex = 2;
            this.lblDirectory.Text = "Results directory:";
            this.lblDirectory.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkRandomize
            // 
            this.chkRandomize.AutoSize = true;
            this.chkRandomize.Checked = true;
            this.chkRandomize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRandomize.Location = new System.Drawing.Point(326, 238);
            this.chkRandomize.Name = "chkRandomize";
            this.chkRandomize.Size = new System.Drawing.Size(106, 17);
            this.chkRandomize.TabIndex = 24;
            this.chkRandomize.Text = "Randomize order";
            this.chkRandomize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkRandomize.UseVisualStyleBackColor = true;
            // 
            // lblTrials
            // 
            this.lblTrials.AutoSize = true;
            this.lblTrials.Location = new System.Drawing.Point(9, 25);
            this.lblTrials.Name = "lblTrials";
            this.lblTrials.Size = new System.Drawing.Size(99, 13);
            this.lblTrials.TabIndex = 0;
            this.lblTrials.Text = "Trials per condition:";
            // 
            // lblIntercept
            // 
            this.lblIntercept.AutoSize = true;
            this.lblIntercept.Location = new System.Drawing.Point(9, 185);
            this.lblIntercept.Name = "lblIntercept";
            this.lblIntercept.Size = new System.Drawing.Size(16, 13);
            this.lblIntercept.TabIndex = 5;
            this.lblIntercept.Text = "a:";
            // 
            // lblSlope
            // 
            this.lblSlope.AutoSize = true;
            this.lblSlope.Location = new System.Drawing.Point(9, 211);
            this.lblSlope.Name = "lblSlope";
            this.lblSlope.Size = new System.Drawing.Size(16, 13);
            this.lblSlope.TabIndex = 7;
            this.lblSlope.Text = "b:";
            // 
            // lblMT
            // 
            this.lblMT.Location = new System.Drawing.Point(9, 25);
            this.lblMT.Name = "lblMT";
            this.lblMT.Size = new System.Drawing.Size(108, 13);
            this.lblMT.TabIndex = 0;
            this.lblMT.Text = "Normative Times (%):";
            // 
            // txtDirectory
            // 
            this.txtDirectory.Location = new System.Drawing.Point(236, 6);
            this.txtDirectory.Name = "txtDirectory";
            this.txtDirectory.Size = new System.Drawing.Size(201, 20);
            this.txtDirectory.TabIndex = 3;
            this.txtDirectory.WordWrap = false;
            // 
            // cmdBrowse
            // 
            this.cmdBrowse.Location = new System.Drawing.Point(443, 6);
            this.cmdBrowse.Name = "cmdBrowse";
            this.cmdBrowse.Size = new System.Drawing.Size(24, 22);
            this.cmdBrowse.TabIndex = 4;
            this.cmdBrowse.Text = "...";
            this.cmdBrowse.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cmdBrowse.UseVisualStyleBackColor = true;
            this.cmdBrowse.Click += new System.EventHandler(this.cmdBrowse_Click);
            // 
            // cmdRemoveMT
            // 
            this.cmdRemoveMT.Enabled = false;
            this.cmdRemoveMT.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.cmdRemoveMT.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.cmdRemoveMT.Location = new System.Drawing.Point(100, 44);
            this.cmdRemoveMT.Name = "cmdRemoveMT";
            this.cmdRemoveMT.Size = new System.Drawing.Size(16, 20);
            this.cmdRemoveMT.TabIndex = 3;
            this.cmdRemoveMT.Tag = "timeWindow";
            this.cmdRemoveMT.Text = "-";
            this.cmdRemoveMT.UseVisualStyleBackColor = true;
            this.cmdRemoveMT.Click += new System.EventHandler(this.cmdRemoveMT_Click);
            // 
            // cmdAddMT
            // 
            this.cmdAddMT.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.cmdAddMT.Location = new System.Drawing.Point(82, 44);
            this.cmdAddMT.Name = "cmdAddMT";
            this.cmdAddMT.Size = new System.Drawing.Size(16, 20);
            this.cmdAddMT.TabIndex = 2;
            this.cmdAddMT.Tag = "timeWindow";
            this.cmdAddMT.Text = "+";
            this.cmdAddMT.UseVisualStyleBackColor = true;
            this.cmdAddMT.Click += new System.EventHandler(this.cmdAddMT_Click);
            // 
            // lstMT
            // 
            this.lstMT.FormatString = "N2";
            this.lstMT.FormattingEnabled = true;
            this.lstMT.Location = new System.Drawing.Point(12, 68);
            this.lstMT.Name = "lstMT";
            this.lstMT.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstMT.Size = new System.Drawing.Size(104, 108);
            this.lstMT.TabIndex = 4;
            this.lstMT.Tag = "";
            this.lstMT.SelectedIndexChanged += new System.EventHandler(this.lstMT_SelectedIndexChanged);
            // 
            // numTrials
            // 
            this.numTrials.Location = new System.Drawing.Point(114, 23);
            this.numTrials.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numTrials.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numTrials.Name = "numTrials";
            this.numTrials.Size = new System.Drawing.Size(49, 20);
            this.numTrials.TabIndex = 1;
            this.numTrials.Value = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.numTrials.ValueChanged += new System.EventHandler(this.numTrials_ValueChanged);
            // 
            // grpConditions
            // 
            this.grpConditions.Controls.Add(this.cmdPopulate);
            this.grpConditions.Controls.Add(this.numBlocks);
            this.grpConditions.Controls.Add(this.lblBlocks);
            this.grpConditions.Controls.Add(this.ntxtSlope);
            this.grpConditions.Controls.Add(this.ntxtIntercept);
            this.grpConditions.Controls.Add(this.lblSlope);
            this.grpConditions.Controls.Add(this.chkRandomize);
            this.grpConditions.Controls.Add(this.lblIntercept);
            this.grpConditions.Controls.Add(this.numW);
            this.grpConditions.Controls.Add(this.numA);
            this.grpConditions.Controls.Add(this.numMT);
            this.grpConditions.Controls.Add(this.lblA);
            this.grpConditions.Controls.Add(this.lblID);
            this.grpConditions.Controls.Add(this.cmdRemoveMT);
            this.grpConditions.Controls.Add(this.lblW);
            this.grpConditions.Controls.Add(this.cmdAddMT);
            this.grpConditions.Controls.Add(this.lstID);
            this.grpConditions.Controls.Add(this.lstW);
            this.grpConditions.Controls.Add(this.lstMT);
            this.grpConditions.Controls.Add(this.lstA);
            this.grpConditions.Controls.Add(this.lblMT);
            this.grpConditions.Controls.Add(this.cmdAddA);
            this.grpConditions.Controls.Add(this.cmdRemoveA);
            this.grpConditions.Controls.Add(this.cmdAddW);
            this.grpConditions.Controls.Add(this.cmdRemoveW);
            this.grpConditions.Location = new System.Drawing.Point(12, 95);
            this.grpConditions.Name = "grpConditions";
            this.grpConditions.Size = new System.Drawing.Size(455, 269);
            this.grpConditions.TabIndex = 6;
            this.grpConditions.TabStop = false;
            this.grpConditions.Text = "Conditions";
            // 
            // cmdPopulate
            // 
            this.cmdPopulate.Location = new System.Drawing.Point(31, 235);
            this.cmdPopulate.Name = "cmdPopulate";
            this.cmdPopulate.Size = new System.Drawing.Size(86, 20);
            this.cmdPopulate.TabIndex = 9;
            this.cmdPopulate.Text = "Populate";
            this.cmdPopulate.UseVisualStyleBackColor = true;
            this.cmdPopulate.Click += new System.EventHandler(this.cmdPopulate_Click);
            // 
            // numBlocks
            // 
            this.numBlocks.Location = new System.Drawing.Point(224, 237);
            this.numBlocks.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numBlocks.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numBlocks.Name = "numBlocks";
            this.numBlocks.Size = new System.Drawing.Size(49, 20);
            this.numBlocks.TabIndex = 23;
            this.numBlocks.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numBlocks.ValueChanged += new System.EventHandler(this.numBlocks_ValueChanged);
            // 
            // lblBlocks
            // 
            this.lblBlocks.AutoSize = true;
            this.lblBlocks.Location = new System.Drawing.Point(131, 239);
            this.lblBlocks.Name = "lblBlocks";
            this.lblBlocks.Size = new System.Drawing.Size(184, 13);
            this.lblBlocks.TabIndex = 22;
            this.lblBlocks.Text = "Run all conditions                    time(s).";
            // 
            // ntxtSlope
            // 
            this.ntxtSlope.AllowDrop = true;
            this.ntxtSlope.Location = new System.Drawing.Point(31, 208);
            this.ntxtSlope.Maximum = 10000;
            this.ntxtSlope.Minimum = 0;
            this.ntxtSlope.Name = "ntxtSlope";
            this.ntxtSlope.Precision = ((uint) (4u));
            this.ntxtSlope.Size = new System.Drawing.Size(85, 20);
            this.ntxtSlope.TabIndex = 8;
            this.ntxtSlope.Text = "200.0000";
            this.ntxtSlope.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ntxtSlope.Value = 200;
            this.ntxtSlope.WordWrap = false;
            this.ntxtSlope.TextChanged += new System.EventHandler(this.ntxtSlope_TextChanged);
            // 
            // ntxtIntercept
            // 
            this.ntxtIntercept.AllowDrop = true;
            this.ntxtIntercept.Location = new System.Drawing.Point(31, 182);
            this.ntxtIntercept.Maximum = 10000;
            this.ntxtIntercept.Minimum = -10000;
            this.ntxtIntercept.Name = "ntxtIntercept";
            this.ntxtIntercept.Precision = ((uint) (4u));
            this.ntxtIntercept.Size = new System.Drawing.Size(85, 20);
            this.ntxtIntercept.TabIndex = 6;
            this.ntxtIntercept.Text = "100.0000";
            this.ntxtIntercept.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ntxtIntercept.Value = 100;
            this.ntxtIntercept.WordWrap = false;
            // 
            // numW
            // 
            this.numW.Location = new System.Drawing.Point(232, 44);
            this.numW.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numW.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numW.Name = "numW";
            this.numW.Size = new System.Drawing.Size(60, 20);
            this.numW.TabIndex = 16;
            this.numW.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numW.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numW_KeyDown);
            // 
            // numA
            // 
            this.numA.Location = new System.Drawing.Point(122, 44);
            this.numA.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numA.Minimum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numA.Name = "numA";
            this.numA.Size = new System.Drawing.Size(60, 20);
            this.numA.TabIndex = 11;
            this.numA.Value = new decimal(new int[] {
            192,
            0,
            0,
            0});
            this.numA.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numA_KeyDown);
            // 
            // numMT
            // 
            this.numMT.Location = new System.Drawing.Point(12, 44);
            this.numMT.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numMT.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numMT.Name = "numMT";
            this.numMT.Size = new System.Drawing.Size(66, 20);
            this.numMT.TabIndex = 1;
            this.numMT.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numMT.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numMT_KeyDown);
            // 
            // numSubject
            // 
            this.numSubject.Location = new System.Drawing.Point(78, 7);
            this.numSubject.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numSubject.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSubject.Name = "numSubject";
            this.numSubject.Size = new System.Drawing.Size(49, 20);
            this.numSubject.TabIndex = 1;
            this.numSubject.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // grpTrials
            // 
            this.grpTrials.Controls.Add(this.cboLayout);
            this.grpTrials.Controls.Add(this.lblLayout);
            this.grpTrials.Controls.Add(this.numPracticeTrials);
            this.grpTrials.Controls.Add(this.label1);
            this.grpTrials.Controls.Add(this.lblTrials);
            this.grpTrials.Controls.Add(this.numTrials);
            this.grpTrials.Location = new System.Drawing.Point(12, 33);
            this.grpTrials.Name = "grpTrials";
            this.grpTrials.Size = new System.Drawing.Size(455, 56);
            this.grpTrials.TabIndex = 5;
            this.grpTrials.TabStop = false;
            this.grpTrials.Text = "Trials";
            // 
            // cboLayout
            // 
            this.cboLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLayout.FormattingEnabled = true;
            this.cboLayout.Items.AddRange(new object[] {
            "Ribbons",
            "Circles"});
            this.cboLayout.Location = new System.Drawing.Point(368, 22);
            this.cboLayout.Name = "cboLayout";
            this.cboLayout.Size = new System.Drawing.Size(81, 21);
            this.cboLayout.TabIndex = 6;
            this.cboLayout.SelectedIndexChanged += new System.EventHandler(this.cboLayout_SelectedIndexChanged);
            // 
            // lblLayout
            // 
            this.lblLayout.AutoSize = true;
            this.lblLayout.Location = new System.Drawing.Point(323, 25);
            this.lblLayout.Name = "lblLayout";
            this.lblLayout.Size = new System.Drawing.Size(42, 13);
            this.lblLayout.TabIndex = 5;
            this.lblLayout.Text = "Layout:";
            // 
            // numPracticeTrials
            // 
            this.numPracticeTrials.Location = new System.Drawing.Point(265, 23);
            this.numPracticeTrials.Maximum = new decimal(new int[] {
            97,
            0,
            0,
            0});
            this.numPracticeTrials.Name = "numPracticeTrials";
            this.numPracticeTrials.Size = new System.Drawing.Size(49, 20);
            this.numPracticeTrials.TabIndex = 4;
            this.numPracticeTrials.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(169, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Treat as practice:";
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 402);
            this.Controls.Add(this.grpTrials);
            this.Controls.Add(this.numSubject);
            this.Controls.Add(this.grpConditions);
            this.Controls.Add(this.cmdBrowse);
            this.Controls.Add(this.lblTotalTrials);
            this.Controls.Add(this.txtDirectory);
            this.Controls.Add(this.lblDirectory);
            this.Controls.Add(this.lblScreenDimension);
            this.Controls.Add(this.lblSubject);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "OptionsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Test Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            ((System.ComponentModel.ISupportInitialize) (this.numTrials)).EndInit();
            this.grpConditions.ResumeLayout(false);
            this.grpConditions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.numBlocks)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.numW)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.numA)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.numMT)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.numSubject)).EndInit();
            this.grpTrials.ResumeLayout(false);
            this.grpTrials.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.numPracticeTrials)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblA;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.Label lblSubject;
        private System.Windows.Forms.Label lblW;
        private System.Windows.Forms.ListBox lstID;
        private System.Windows.Forms.ListBox lstW;
        private System.Windows.Forms.ListBox lstA;
        private System.Windows.Forms.Button cmdAddA;
        private System.Windows.Forms.Button cmdRemoveA;
        private System.Windows.Forms.Button cmdRemoveW;
        private System.Windows.Forms.Button cmdAddW;
        private System.Windows.Forms.Label lblScreenDimension;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Label lblTotalTrials;
        private System.Windows.Forms.TextBox txtDirectory;
        private System.Windows.Forms.Label lblDirectory;
        private System.Windows.Forms.Button cmdBrowse;
        private System.Windows.Forms.Button cmdRemoveMT;
        private System.Windows.Forms.Button cmdAddMT;
        private System.Windows.Forms.ListBox lstMT;
        private System.Windows.Forms.Label lblMT;
        private System.Windows.Forms.CheckBox chkRandomize;
      private System.Windows.Forms.NumericUpDown numTrials;
        private System.Windows.Forms.Label lblTrials;
        private System.Windows.Forms.GroupBox grpConditions;
        private System.Windows.Forms.NumericUpDown numSubject;
        private System.Windows.Forms.NumericUpDown numA;
        private System.Windows.Forms.NumericUpDown numMT;
        private System.Windows.Forms.NumericUpDown numW;
        private System.Windows.Forms.Label lblSlope;
        private System.Windows.Forms.Label lblIntercept;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.GroupBox grpTrials;
        private WobbrockLib.Controls.NumericTextBox ntxtSlope;
        private WobbrockLib.Controls.NumericTextBox ntxtIntercept;
        private System.Windows.Forms.NumericUpDown numBlocks;
        private System.Windows.Forms.Label lblBlocks;
        private System.Windows.Forms.Button cmdPopulate;
        private System.Windows.Forms.NumericUpDown numPracticeTrials;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboLayout;
        private System.Windows.Forms.Label lblLayout;
    }
}