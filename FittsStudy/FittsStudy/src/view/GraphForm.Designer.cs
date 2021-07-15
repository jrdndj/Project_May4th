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
    partial class GraphForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphForm));
            this.imglTreeIcons = new System.Windows.Forms.ImageList(this.components);
            this.tableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.grfJerk = new WobbrockLib.Controls.Plot();
            this.grfVelocity = new WobbrockLib.Controls.Plot();
            this.trvTrials = new System.Windows.Forms.TreeView();
            this.grfAcceleration = new WobbrockLib.Controls.Plot();
            this.grfDistance = new WobbrockLib.Controls.Plot();
            this.pnlTrial = new WobbrockLib.Controls.DoubleBufferedPanel();
            this.mnuCopy = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mniCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.trkZoom = new System.Windows.Forms.TrackBar();
            this.tabXml = new System.Windows.Forms.TabControl();
            this.tabpgWeb = new System.Windows.Forms.TabPage();
            this.webXml = new System.Windows.Forms.WebBrowser();
            this.tabpgPlain = new System.Windows.Forms.TabPage();
            this.rtxXml = new System.Windows.Forms.RichTextBox();
            this.mnsMain = new System.Windows.Forms.MenuStrip();
            this.mnuClose = new System.Windows.Forms.ToolStripMenuItem();
            this.txtFilename = new System.Windows.Forms.ToolStripTextBox();
            this.ttZoom = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayout.SuspendLayout();
            this.pnlTrial.SuspendLayout();
            this.mnuCopy.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkZoom)).BeginInit();
            this.tabXml.SuspendLayout();
            this.tabpgWeb.SuspendLayout();
            this.tabpgPlain.SuspendLayout();
            this.mnsMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // imglTreeIcons
            // 
            this.imglTreeIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglTreeIcons.ImageStream")));
            this.imglTreeIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.imglTreeIcons.Images.SetKeyName(0, "Session.ico");
            this.imglTreeIcons.Images.SetKeyName(1, "Block.ico");
            this.imglTreeIcons.Images.SetKeyName(2, "Condition.ico");
            this.imglTreeIcons.Images.SetKeyName(3, "TrialPractice.ico");
            this.imglTreeIcons.Images.SetKeyName(4, "TrialGood.ico");
            this.imglTreeIcons.Images.SetKeyName(5, "TrialError.ico");
            this.imglTreeIcons.Images.SetKeyName(6, "TrialOutlier.ico");
            // 
            // tableLayout
            // 
            this.tableLayout.ColumnCount = 3;
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 57F));
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43F));
            this.tableLayout.Controls.Add(this.grfJerk, 3, 3);
            this.tableLayout.Controls.Add(this.grfVelocity, 2, 1);
            this.tableLayout.Controls.Add(this.trvTrials, 0, 0);
            this.tableLayout.Controls.Add(this.grfAcceleration, 2, 2);
            this.tableLayout.Controls.Add(this.grfDistance, 2, 0);
            this.tableLayout.Controls.Add(this.pnlTrial, 1, 0);
            this.tableLayout.Controls.Add(this.tabXml, 1, 2);
            this.tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayout.Location = new System.Drawing.Point(0, 27);
            this.tableLayout.Margin = new System.Windows.Forms.Padding(1);
            this.tableLayout.Name = "tableLayout";
            this.tableLayout.RowCount = 4;
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayout.Size = new System.Drawing.Size(1185, 588);
            this.tableLayout.TabIndex = 1;
            // 
            // grfJerk
            // 
            this.grfJerk.BackColor = System.Drawing.Color.White;
            this.grfJerk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grfJerk.PlotMargin = new System.Windows.Forms.Padding(40);
            this.grfJerk.Location = new System.Drawing.Point(784, 442);
            this.grfJerk.Margin = new System.Windows.Forms.Padding(1);
            this.grfJerk.Name = "grfJerk";
            this.grfJerk.Size = new System.Drawing.Size(400, 145);
            this.grfJerk.TabIndex = 6;
            this.grfJerk.TabStop = false;
            this.grfJerk.Text = "Jerk";
            this.grfJerk.Title = "Jerk";
            this.grfJerk.XAxisName = "ms";
            this.grfJerk.YAxisDecimals = 3;
            this.grfJerk.YAxisName = "px/ms^3";
            // 
            // grfVelocity
            // 
            this.grfVelocity.BackColor = System.Drawing.Color.White;
            this.grfVelocity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grfVelocity.PlotMargin = new System.Windows.Forms.Padding(40);
            this.grfVelocity.Location = new System.Drawing.Point(784, 148);
            this.grfVelocity.Margin = new System.Windows.Forms.Padding(1);
            this.grfVelocity.Name = "grfVelocity";
            this.grfVelocity.Size = new System.Drawing.Size(400, 145);
            this.grfVelocity.TabIndex = 4;
            this.grfVelocity.TabStop = false;
            this.grfVelocity.Text = "Velocity";
            this.grfVelocity.Title = "Velocity";
            this.grfVelocity.XAxisName = "ms";
            this.grfVelocity.YAxisDecimals = 2;
            this.grfVelocity.YAxisName = "px/ms";
            // 
            // trvTrials
            // 
            this.trvTrials.Dock = System.Windows.Forms.DockStyle.Left;
            this.trvTrials.HideSelection = false;
            this.trvTrials.ImageIndex = 0;
            this.trvTrials.ImageList = this.imglTreeIcons;
            this.trvTrials.Location = new System.Drawing.Point(1, 1);
            this.trvTrials.Margin = new System.Windows.Forms.Padding(1);
            this.trvTrials.Name = "trvTrials";
            this.tableLayout.SetRowSpan(this.trvTrials, 4);
            this.trvTrials.SelectedImageIndex = 0;
            this.trvTrials.ShowNodeToolTips = true;
            this.trvTrials.Size = new System.Drawing.Size(250, 586);
            this.trvTrials.TabIndex = 0;
            this.trvTrials.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvTrials_AfterSelect);
            // 
            // grfAcceleration
            // 
            this.grfAcceleration.BackColor = System.Drawing.Color.White;
            this.grfAcceleration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grfAcceleration.PlotMargin = new System.Windows.Forms.Padding(40);
            this.grfAcceleration.Location = new System.Drawing.Point(784, 295);
            this.grfAcceleration.Margin = new System.Windows.Forms.Padding(1);
            this.grfAcceleration.Name = "grfAcceleration";
            this.grfAcceleration.Size = new System.Drawing.Size(400, 145);
            this.grfAcceleration.TabIndex = 5;
            this.grfAcceleration.TabStop = false;
            this.grfAcceleration.Text = "Acceleration";
            this.grfAcceleration.Title = "Acceleration";
            this.grfAcceleration.XAxisName = "ms";
            this.grfAcceleration.YAxisDecimals = 3;
            this.grfAcceleration.YAxisName = "px/ms^2";
            // 
            // grfDistance
            // 
            this.grfDistance.BackColor = System.Drawing.Color.White;
            this.grfDistance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grfDistance.PlotMargin = new System.Windows.Forms.Padding(40);
            this.grfDistance.Location = new System.Drawing.Point(784, 1);
            this.grfDistance.Margin = new System.Windows.Forms.Padding(1);
            this.grfDistance.Name = "grfDistance";
            this.grfDistance.Size = new System.Drawing.Size(400, 145);
            this.grfDistance.TabIndex = 3;
            this.grfDistance.TabStop = false;
            this.grfDistance.Text = "Distance to Target";
            this.grfDistance.Title = "Distance to Target";
            this.grfDistance.XAxisName = "ms";
            this.grfDistance.YAxisDecimals = 0;
            this.grfDistance.YAxisName = "px";
            // 
            // pnlTrial
            // 
            this.pnlTrial.BackColor = System.Drawing.Color.White;
            this.pnlTrial.ContextMenuStrip = this.mnuCopy;
            this.pnlTrial.Controls.Add(this.trkZoom);
            this.pnlTrial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTrial.Location = new System.Drawing.Point(255, 3);
            this.pnlTrial.Name = "pnlTrial";
            this.tableLayout.SetRowSpan(this.pnlTrial, 2);
            this.pnlTrial.Size = new System.Drawing.Size(525, 288);
            this.pnlTrial.TabIndex = 1;
            this.pnlTrial.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlTrial_Paint);
            // 
            // mnuCopy
            // 
            this.mnuCopy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniCopy});
            this.mnuCopy.Name = "mnuCopy";
            this.mnuCopy.Size = new System.Drawing.Size(208, 26);
            // 
            // mniCopy
            // 
            this.mniCopy.Name = "mniCopy";
            this.mniCopy.Size = new System.Drawing.Size(207, 22);
            this.mniCopy.Text = "Copy Image to Clipboard";
            this.mniCopy.Click += new System.EventHandler(this.mniCopy_Click);
            // 
            // trkZoom
            // 
            this.trkZoom.AutoSize = false;
            this.trkZoom.Dock = System.Windows.Forms.DockStyle.Right;
            this.trkZoom.LargeChange = 20;
            this.trkZoom.Location = new System.Drawing.Point(504, 0);
            this.trkZoom.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.trkZoom.Maximum = 190;
            this.trkZoom.Minimum = 10;
            this.trkZoom.Name = "trkZoom";
            this.trkZoom.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trkZoom.Size = new System.Drawing.Size(21, 288);
            this.trkZoom.SmallChange = 5;
            this.trkZoom.TabIndex = 0;
            this.trkZoom.TickFrequency = 10;
            this.trkZoom.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trkZoom.Value = 100;
            this.trkZoom.ValueChanged += new System.EventHandler(this.trkZoom_ValueChanged);
            this.trkZoom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trkZoom_MouseDown);
            // 
            // tabXml
            // 
            this.tabXml.Controls.Add(this.tabpgWeb);
            this.tabXml.Controls.Add(this.tabpgPlain);
            this.tabXml.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabXml.Location = new System.Drawing.Point(253, 295);
            this.tabXml.Margin = new System.Windows.Forms.Padding(1);
            this.tabXml.Multiline = true;
            this.tabXml.Name = "tabXml";
            this.tabXml.Padding = new System.Drawing.Point(0, 0);
            this.tableLayout.SetRowSpan(this.tabXml, 2);
            this.tabXml.SelectedIndex = 0;
            this.tabXml.Size = new System.Drawing.Size(529, 292);
            this.tabXml.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabXml.TabIndex = 7;
            // 
            // tabpgWeb
            // 
            this.tabpgWeb.Controls.Add(this.webXml);
            this.tabpgWeb.Location = new System.Drawing.Point(4, 22);
            this.tabpgWeb.Margin = new System.Windows.Forms.Padding(0);
            this.tabpgWeb.Name = "tabpgWeb";
            this.tabpgWeb.Size = new System.Drawing.Size(521, 266);
            this.tabpgWeb.TabIndex = 0;
            this.tabpgWeb.Text = "Web";
            this.tabpgWeb.UseVisualStyleBackColor = true;
            // 
            // webXml
            // 
            this.webXml.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webXml.Location = new System.Drawing.Point(0, 0);
            this.webXml.Margin = new System.Windows.Forms.Padding(1);
            this.webXml.MinimumSize = new System.Drawing.Size(20, 20);
            this.webXml.Name = "webXml";
            this.webXml.ScriptErrorsSuppressed = true;
            this.webXml.Size = new System.Drawing.Size(521, 266);
            this.webXml.TabIndex = 3;
            // 
            // tabpgPlain
            // 
            this.tabpgPlain.Controls.Add(this.rtxXml);
            this.tabpgPlain.Location = new System.Drawing.Point(4, 22);
            this.tabpgPlain.Margin = new System.Windows.Forms.Padding(0);
            this.tabpgPlain.Name = "tabpgPlain";
            this.tabpgPlain.Size = new System.Drawing.Size(521, 266);
            this.tabpgPlain.TabIndex = 1;
            this.tabpgPlain.Text = "Plain";
            this.tabpgPlain.UseVisualStyleBackColor = true;
            // 
            // rtxXml
            // 
            this.rtxXml.BackColor = System.Drawing.Color.White;
            this.rtxXml.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxXml.DetectUrls = false;
            this.rtxXml.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxXml.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtxXml.HideSelection = false;
            this.rtxXml.Location = new System.Drawing.Point(0, 0);
            this.rtxXml.Margin = new System.Windows.Forms.Padding(1);
            this.rtxXml.Name = "rtxXml";
            this.rtxXml.ReadOnly = true;
            this.rtxXml.ShowSelectionMargin = true;
            this.rtxXml.Size = new System.Drawing.Size(521, 266);
            this.rtxXml.TabIndex = 0;
            this.rtxXml.Text = "";
            this.rtxXml.WordWrap = false;
            // 
            // mnsMain
            // 
            this.mnsMain.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.mnsMain.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.mnsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuClose,
            this.txtFilename});
            this.mnsMain.Location = new System.Drawing.Point(0, 0);
            this.mnsMain.Name = "mnsMain";
            this.mnsMain.Size = new System.Drawing.Size(1185, 27);
            this.mnsMain.TabIndex = 0;
            this.mnsMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mnsMain_MouseClick);
            // 
            // mnuClose
            // 
            this.mnuClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.mnuClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuClose.Image = ((System.Drawing.Image)(resources.GetObject("mnuClose.Image")));
            this.mnuClose.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.mnuClose.Name = "mnuClose";
            this.mnuClose.ShowShortcutKeys = false;
            this.mnuClose.Size = new System.Drawing.Size(31, 23);
            this.mnuClose.Click += new System.EventHandler(this.mnuClose_Click);
            // 
            // txtFilename
            // 
            this.txtFilename.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.txtFilename.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.ReadOnly = true;
            this.txtFilename.Size = new System.Drawing.Size(300, 23);
            // 
            // GraphForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1185, 615);
            this.Controls.Add(this.tableLayout);
            this.Controls.Add(this.mnsMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GraphForm";
            this.Text = "Trial Explorer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GraphForm_FormClosing);
            this.Load += new System.EventHandler(this.GraphForm_Load);
            this.tableLayout.ResumeLayout(false);
            this.pnlTrial.ResumeLayout(false);
            this.mnuCopy.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trkZoom)).EndInit();
            this.tabXml.ResumeLayout(false);
            this.tabpgWeb.ResumeLayout(false);
            this.tabpgPlain.ResumeLayout(false);
            this.mnsMain.ResumeLayout(false);
            this.mnsMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayout;
        private WobbrockLib.Controls.DoubleBufferedPanel pnlTrial;
        private WobbrockLib.Controls.Plot grfDistance;
        private WobbrockLib.Controls.Plot grfAcceleration;
        private System.Windows.Forms.ImageList imglTreeIcons;
        private System.Windows.Forms.MenuStrip mnsMain;
        private System.Windows.Forms.ToolStripMenuItem mnuClose;
        private System.Windows.Forms.ToolStripTextBox txtFilename;
        private System.Windows.Forms.TreeView trvTrials;
        private WobbrockLib.Controls.Plot grfVelocity;
        private WobbrockLib.Controls.Plot grfJerk;
        private System.Windows.Forms.TrackBar trkZoom;
        private System.Windows.Forms.ToolTip ttZoom;
        private System.Windows.Forms.TabControl tabXml;
        private System.Windows.Forms.TabPage tabpgWeb;
        private System.Windows.Forms.WebBrowser webXml;
        private System.Windows.Forms.TabPage tabpgPlain;
        private System.Windows.Forms.RichTextBox rtxXml;
        private System.Windows.Forms.ContextMenuStrip mnuCopy;
        private System.Windows.Forms.ToolStripMenuItem mniCopy;
    }
}