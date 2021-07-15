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
    partial class ModelForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelForm));
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdNotepad = new System.Windows.Forms.Button();
            this.txtFitts1D = new System.Windows.Forms.TextBox();
            this.tabModel = new System.Windows.Forms.TabControl();
            this.tabpgFitts1D = new System.Windows.Forms.TabPage();
            this.grfFitts1D = new WobbrockLib.Controls.Plot();
            this.tabpgFitts2D = new System.Windows.Forms.TabPage();
            this.grfFitts2D = new WobbrockLib.Controls.Plot();
            this.txtFitts2D = new System.Windows.Forms.TextBox();
            this.tabpgError1D = new System.Windows.Forms.TabPage();
            this.grfErrors1D = new WobbrockLib.Controls.Plot();
            this.txtErrors1D = new System.Windows.Forms.TextBox();
            this.tabpgError2D = new System.Windows.Forms.TabPage();
            this.grfErrors2D = new WobbrockLib.Controls.Plot();
            this.txtErrors2D = new System.Windows.Forms.TextBox();
            this.tabModel.SuspendLayout();
            this.tabpgFitts1D.SuspendLayout();
            this.tabpgFitts2D.SuspendLayout();
            this.tabpgError1D.SuspendLayout();
            this.tabpgError2D.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(626, 653);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(68, 24);
            this.cmdOK.TabIndex = 2;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            // 
            // cmdNotepad
            // 
            this.cmdNotepad.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdNotepad.Location = new System.Drawing.Point(530, 653);
            this.cmdNotepad.Name = "cmdNotepad";
            this.cmdNotepad.Size = new System.Drawing.Size(90, 24);
            this.cmdNotepad.TabIndex = 1;
            this.cmdNotepad.Text = "Save Model";
            this.cmdNotepad.UseVisualStyleBackColor = true;
            this.cmdNotepad.Click += new System.EventHandler(this.cmdNotepad_Click);
            // 
            // txtFitts1D
            // 
            this.txtFitts1D.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFitts1D.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtFitts1D.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.txtFitts1D.Location = new System.Drawing.Point(3, 356);
            this.txtFitts1D.Multiline = true;
            this.txtFitts1D.Name = "txtFitts1D";
            this.txtFitts1D.ReadOnly = true;
            this.txtFitts1D.Size = new System.Drawing.Size(668, 250);
            this.txtFitts1D.TabIndex = 1;
            // 
            // tabModel
            // 
            this.tabModel.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabModel.Controls.Add(this.tabpgFitts1D);
            this.tabModel.Controls.Add(this.tabpgFitts2D);
            this.tabModel.Controls.Add(this.tabpgError1D);
            this.tabModel.Controls.Add(this.tabpgError2D);
            this.tabModel.Location = new System.Drawing.Point(12, 12);
            this.tabModel.Name = "tabModel";
            this.tabModel.SelectedIndex = 0;
            this.tabModel.Size = new System.Drawing.Size(682, 635);
            this.tabModel.TabIndex = 0;
            // 
            // tabpgFitts1D
            // 
            this.tabpgFitts1D.Controls.Add(this.grfFitts1D);
            this.tabpgFitts1D.Controls.Add(this.txtFitts1D);
            this.tabpgFitts1D.Location = new System.Drawing.Point(4, 22);
            this.tabpgFitts1D.Name = "tabpgFitts1D";
            this.tabpgFitts1D.Padding = new System.Windows.Forms.Padding(3);
            this.tabpgFitts1D.Size = new System.Drawing.Size(674, 609);
            this.tabpgFitts1D.TabIndex = 0;
            this.tabpgFitts1D.Text = "Fitts\' - Univariate";
            this.tabpgFitts1D.UseVisualStyleBackColor = true;
            // 
            // grfFitts1D
            // 
            this.grfFitts1D.BackColor = System.Drawing.Color.White;
            this.grfFitts1D.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grfFitts1D.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.grfFitts1D.Location = new System.Drawing.Point(3, 3);
            this.grfFitts1D.Name = "grfFitts1D";
            this.grfFitts1D.Size = new System.Drawing.Size(668, 353);
            this.grfFitts1D.TabIndex = 0;
            this.grfFitts1D.Text = "Fitts\' Law - Univariate";
            this.grfFitts1D.Title = "Fitts\' Law - Univariate";
            this.grfFitts1D.XAxisName = "IDe (bits)";
            this.grfFitts1D.YAxisName = "MT (ms)";
            // 
            // tabpgFitts2D
            // 
            this.tabpgFitts2D.Controls.Add(this.grfFitts2D);
            this.tabpgFitts2D.Controls.Add(this.txtFitts2D);
            this.tabpgFitts2D.Location = new System.Drawing.Point(4, 22);
            this.tabpgFitts2D.Name = "tabpgFitts2D";
            this.tabpgFitts2D.Padding = new System.Windows.Forms.Padding(3);
            this.tabpgFitts2D.Size = new System.Drawing.Size(674, 609);
            this.tabpgFitts2D.TabIndex = 2;
            this.tabpgFitts2D.Text = "Fitts\' - Bivariate";
            this.tabpgFitts2D.UseVisualStyleBackColor = true;
            // 
            // grfFitts2D
            // 
            this.grfFitts2D.BackColor = System.Drawing.Color.White;
            this.grfFitts2D.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grfFitts2D.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.grfFitts2D.Location = new System.Drawing.Point(3, 3);
            this.grfFitts2D.Name = "grfFitts2D";
            this.grfFitts2D.Size = new System.Drawing.Size(668, 353);
            this.grfFitts2D.TabIndex = 0;
            this.grfFitts2D.Text = "Fitts\' Law Model - Bivariate";
            this.grfFitts2D.Title = "Fitts\' Law Model - Bivariate";
            this.grfFitts2D.XAxisName = "IDe (bits)";
            this.grfFitts2D.YAxisName = "MT (ms)";
            // 
            // txtFitts2D
            // 
            this.txtFitts2D.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFitts2D.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtFitts2D.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.txtFitts2D.Location = new System.Drawing.Point(3, 356);
            this.txtFitts2D.Multiline = true;
            this.txtFitts2D.Name = "txtFitts2D";
            this.txtFitts2D.ReadOnly = true;
            this.txtFitts2D.Size = new System.Drawing.Size(668, 250);
            this.txtFitts2D.TabIndex = 1;
            // 
            // tabpgError1D
            // 
            this.tabpgError1D.Controls.Add(this.grfErrors1D);
            this.tabpgError1D.Controls.Add(this.txtErrors1D);
            this.tabpgError1D.Location = new System.Drawing.Point(4, 22);
            this.tabpgError1D.Name = "tabpgError1D";
            this.tabpgError1D.Padding = new System.Windows.Forms.Padding(3);
            this.tabpgError1D.Size = new System.Drawing.Size(674, 609);
            this.tabpgError1D.TabIndex = 1;
            this.tabpgError1D.Text = "Errors - Univariate";
            this.tabpgError1D.UseVisualStyleBackColor = true;
            // 
            // grfErrors1D
            // 
            this.grfErrors1D.BackColor = System.Drawing.Color.White;
            this.grfErrors1D.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grfErrors1D.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.grfErrors1D.Location = new System.Drawing.Point(3, 3);
            this.grfErrors1D.Name = "grfErrors1D";
            this.grfErrors1D.Size = new System.Drawing.Size(668, 353);
            this.grfErrors1D.TabIndex = 0;
            this.grfErrors1D.Text = "Pointing Error Model - Univariate";
            this.grfErrors1D.Title = "Pointing Error Model - Univariate";
            this.grfErrors1D.XAxisName = "Predicted Error Rate";
            this.grfErrors1D.YAxisName = "Observed Error Rate";
            // 
            // txtErrors1D
            // 
            this.txtErrors1D.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtErrors1D.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtErrors1D.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.txtErrors1D.Location = new System.Drawing.Point(3, 356);
            this.txtErrors1D.Multiline = true;
            this.txtErrors1D.Name = "txtErrors1D";
            this.txtErrors1D.ReadOnly = true;
            this.txtErrors1D.Size = new System.Drawing.Size(668, 250);
            this.txtErrors1D.TabIndex = 1;
            // 
            // tabpgError2D
            // 
            this.tabpgError2D.Controls.Add(this.grfErrors2D);
            this.tabpgError2D.Controls.Add(this.txtErrors2D);
            this.tabpgError2D.Location = new System.Drawing.Point(4, 22);
            this.tabpgError2D.Name = "tabpgError2D";
            this.tabpgError2D.Padding = new System.Windows.Forms.Padding(3);
            this.tabpgError2D.Size = new System.Drawing.Size(674, 609);
            this.tabpgError2D.TabIndex = 3;
            this.tabpgError2D.Text = "Errors - Bivariate";
            this.tabpgError2D.UseVisualStyleBackColor = true;
            // 
            // grfErrors2D
            // 
            this.grfErrors2D.BackColor = System.Drawing.Color.White;
            this.grfErrors2D.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grfErrors2D.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.grfErrors2D.Location = new System.Drawing.Point(3, 3);
            this.grfErrors2D.Name = "grfErrors2D";
            this.grfErrors2D.Size = new System.Drawing.Size(668, 353);
            this.grfErrors2D.TabIndex = 3;
            this.grfErrors2D.Text = "Pointing Error Model - Bivariate";
            this.grfErrors2D.Title = "Pointing Error Model - Bivariate";
            this.grfErrors2D.XAxisName = "Predicted Error Rate";
            this.grfErrors2D.YAxisName = "Observed Error Rate";
            // 
            // txtErrors2D
            // 
            this.txtErrors2D.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtErrors2D.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtErrors2D.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.txtErrors2D.Location = new System.Drawing.Point(3, 356);
            this.txtErrors2D.Multiline = true;
            this.txtErrors2D.Name = "txtErrors2D";
            this.txtErrors2D.ReadOnly = true;
            this.txtErrors2D.Size = new System.Drawing.Size(668, 250);
            this.txtErrors2D.TabIndex = 2;
            // 
            // ModelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 684);
            this.Controls.Add(this.tabModel);
            this.Controls.Add(this.cmdNotepad);
            this.Controls.Add(this.cmdOK);
            this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "ModelForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Model";
            this.Load += new System.EventHandler(this.ModelForm_Load);
            this.tabModel.ResumeLayout(false);
            this.tabpgFitts1D.ResumeLayout(false);
            this.tabpgFitts1D.PerformLayout();
            this.tabpgFitts2D.ResumeLayout(false);
            this.tabpgFitts2D.PerformLayout();
            this.tabpgError1D.ResumeLayout(false);
            this.tabpgError1D.PerformLayout();
            this.tabpgError2D.ResumeLayout(false);
            this.tabpgError2D.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdNotepad;
        private WobbrockLib.Controls.Plot grfFitts1D;
        private System.Windows.Forms.TextBox txtFitts1D;
        private System.Windows.Forms.TabControl tabModel;
        private System.Windows.Forms.TabPage tabpgFitts1D;
        private System.Windows.Forms.TabPage tabpgError1D;
        private WobbrockLib.Controls.Plot grfErrors1D;
        private System.Windows.Forms.TextBox txtErrors1D;
        private System.Windows.Forms.TabPage tabpgFitts2D;
        private System.Windows.Forms.TabPage tabpgError2D;
        private WobbrockLib.Controls.Plot grfFitts2D;
        private System.Windows.Forms.TextBox txtFitts2D;
        private WobbrockLib.Controls.Plot grfErrors2D;
        private System.Windows.Forms.TextBox txtErrors2D;

    }
}