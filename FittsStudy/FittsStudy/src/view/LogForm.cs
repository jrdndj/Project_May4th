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
using System.IO;

namespace FittsStudy
{
    public partial class LogForm : Form
    {
        private string _filename;

        public LogForm()
            : this(String.Empty)
        {
        }

        public LogForm(string filename)
        {
            InitializeComponent();
            _filename = filename;
        }

        public string Filename
        {
            set { _filename = value; }
            get { return _filename; }
        }

        private void LogForm_Load(object sender, EventArgs e)
        {
            this.Text = Path.GetFileName(_filename);
            Cursor.Current = Cursors.WaitCursor;

            webXml.SuspendLayout();
            webXml.Navigate(_filename);
            webXml.ResumeLayout();

            rtxXml.SuspendLayout();
            rtxXml.LoadFile(_filename, RichTextBoxStreamType.PlainText);
            rtxXml.ResumeLayout();

            Cursor.Current = Cursors.Default;
        }

        private void tabXml_ClientSizeChanged(object sender, EventArgs e)
        {
            rtxXml.Width = tabpgPlain.Width;
            rtxXml.Height = tabpgPlain.Height;
        }

        private void tabXml_Selecting(object sender, TabControlCancelEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
        }

        private void tabXml_SelectedIndexChanged(object sender, EventArgs e)
        {
            rtxXml.Width = tabpgPlain.Width;
            rtxXml.Height = tabpgPlain.Height;
            Cursor.Current = Cursors.Default;
        }

        private void LogForm_ResizeBegin(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            webXml.SuspendLayout();
            rtxXml.SuspendLayout();
        }

        private void LogForm_ResizeEnd(object sender, EventArgs e)
        {
            webXml.ResumeLayout();
            rtxXml.ResumeLayout();
            Cursor.Current = Cursors.Default;
        }

    }
}
