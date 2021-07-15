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
    partial class LogForm
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
            this.tabXml = new System.Windows.Forms.TabControl();
            this.tabpgWeb = new System.Windows.Forms.TabPage();
            this.webXml = new System.Windows.Forms.WebBrowser();
            this.tabpgPlain = new System.Windows.Forms.TabPage();
            this.rtxXml = new System.Windows.Forms.RichTextBox();
            this.tabXml.SuspendLayout();
            this.tabpgWeb.SuspendLayout();
            this.tabpgPlain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabXml
            // 
            this.tabXml.Controls.Add(this.tabpgWeb);
            this.tabXml.Controls.Add(this.tabpgPlain);
            this.tabXml.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabXml.Location = new System.Drawing.Point(0, 0);
            this.tabXml.Margin = new System.Windows.Forms.Padding(1);
            this.tabXml.Multiline = true;
            this.tabXml.Name = "tabXml";
            this.tabXml.Padding = new System.Drawing.Point(0, 0);
            this.tabXml.SelectedIndex = 0;
            this.tabXml.Size = new System.Drawing.Size(584, 564);
            this.tabXml.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabXml.TabIndex = 8;
            this.tabXml.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabXml_Selecting);
            this.tabXml.SelectedIndexChanged += new System.EventHandler(this.tabXml_SelectedIndexChanged);
            this.tabXml.ClientSizeChanged += new System.EventHandler(this.tabXml_ClientSizeChanged);
            // 
            // tabpgWeb
            // 
            this.tabpgWeb.Controls.Add(this.webXml);
            this.tabpgWeb.Location = new System.Drawing.Point(4, 22);
            this.tabpgWeb.Margin = new System.Windows.Forms.Padding(0);
            this.tabpgWeb.Name = "tabpgWeb";
            this.tabpgWeb.Size = new System.Drawing.Size(576, 538);
            this.tabpgWeb.TabIndex = 0;
            this.tabpgWeb.Text = "Web";
            this.tabpgWeb.UseVisualStyleBackColor = true;
            // 
            // webXml
            // 
            this.webXml.AllowNavigation = false;
            this.webXml.AllowWebBrowserDrop = false;
            this.webXml.CausesValidation = false;
            this.webXml.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webXml.Location = new System.Drawing.Point(0, 0);
            this.webXml.Margin = new System.Windows.Forms.Padding(1);
            this.webXml.MinimumSize = new System.Drawing.Size(20, 20);
            this.webXml.Name = "webXml";
            this.webXml.ScriptErrorsSuppressed = true;
            this.webXml.Size = new System.Drawing.Size(576, 538);
            this.webXml.TabIndex = 3;
            // 
            // tabpgPlain
            // 
            this.tabpgPlain.Controls.Add(this.rtxXml);
            this.tabpgPlain.Location = new System.Drawing.Point(4, 22);
            this.tabpgPlain.Margin = new System.Windows.Forms.Padding(0);
            this.tabpgPlain.Name = "tabpgPlain";
            this.tabpgPlain.Size = new System.Drawing.Size(576, 538);
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
            this.rtxXml.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.rtxXml.HideSelection = false;
            this.rtxXml.Location = new System.Drawing.Point(0, 0);
            this.rtxXml.Margin = new System.Windows.Forms.Padding(1);
            this.rtxXml.Name = "rtxXml";
            this.rtxXml.ReadOnly = true;
            this.rtxXml.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.rtxXml.ShowSelectionMargin = true;
            this.rtxXml.Size = new System.Drawing.Size(576, 538);
            this.rtxXml.TabIndex = 0;
            this.rtxXml.Text = "";
            this.rtxXml.WordWrap = false;
            // 
            // LogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 564);
            this.Controls.Add(this.tabXml);
            this.Name = "LogForm";
            this.Text = "FittsStudy Log";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.LogForm_Load);
            this.ResizeBegin += new System.EventHandler(this.LogForm_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.LogForm_ResizeEnd);
            this.tabXml.ResumeLayout(false);
            this.tabpgWeb.ResumeLayout(false);
            this.tabpgPlain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabXml;
        private System.Windows.Forms.TabPage tabpgWeb;
        private System.Windows.Forms.WebBrowser webXml;
        private System.Windows.Forms.TabPage tabpgPlain;
        private System.Windows.Forms.RichTextBox rtxXml;
    }
}