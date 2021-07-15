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
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;
using System.IO;
using System.Xml;
using System.Reflection;
using WobbrockLib;
using WobbrockLib.Extensions;
using WobbrockLib.Types;
using WobbrockLib.Devices;
using WobbrockLib.Windows;

namespace FittsStudy
{
    /// <summary>
    /// The main form that presents the UI and owns the data for the FittsStudy.exe application.
    /// </summary>
    public partial class MainForm : System.Windows.Forms.Form
    {
        #region Constants

        private const int MnomeAnimationTime = 30; // millisecond updates for border animation 
        private const double MinDblClickDist = 4.0; // minimum distance two clicks must be apart (filters double-clicks)
        private const int WaitBeforeClickMs = 2500; // milliseconds to wait before first click in metronome

        #endregion

        #region Fields

        OptionsForm.Options _o; // the options we obtain from the options dialog 

        private long _tLastMnomeTick; // timestamp of last metronome tick in milliseconds
        private long _tMnomeInterval; // duration between metronome ticks in milliseconds

        private SessionData _sdata; // the whole session (one test); holds conditions in order
        private ConditionData _cdata; // the current condition; retrieved from the session
        private TrialData _tdata; // the current trial; retrieved from the condition

        private byte[] _sndMnome; // metronome tick sound
        private uint _timerID; // timer identifier

        private string _fileNoExt; // full path and filename without extension
        private XmlTextWriter _writer; // XML writer -- uses _fileNoExt.xml

        #endregion

        #region Constructor, Loading, and Closing

        /// <summary>
        /// Constructs the main form for this application. The main form is a full-screen window in which
        /// the Fitts' law study trials take place.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint | 
                ControlStyles.OptimizedDoubleBuffer | 
                ControlStyles.UserPaint | 
                ControlStyles.UserMouse |
                ControlStyles.StandardClick, true);
            this.SetStyle(ControlStyles.StandardDoubleClick, false);
            
            _tLastMnomeTick = -1L;
            _tMnomeInterval = -1L;

            _o = new OptionsForm.Options(); // defaults
        }

        /// <summary>
        /// The form load event is used to load *.WAV file sounds from disk into memory for playback during trials.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            lblVersion.Text = "v" + Assembly.GetExecutingAssembly().GetName().Version.ToString(3); // major.minor.build.revision
            _sndMnome = SoundEx.LoadSoundResource(typeof(MainForm), "rsc.tick.wav");
        }

        /// <summary>
        /// Handler for when the form is about to close. If there is an active condition, the closing is
        /// canceled. If not, the form is allowed to close.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (_sdata != null); // don't close the app if there is an active session
            if (e.Cancel)
            {
                MessageBox.Show(this, "A test session is underway. First stop the test, then exit the application.", "Test Underway", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Win32.KillTimer(this.Handle, _timerID);
            }
        }

        #endregion

        #region Paint Handler

        /// <summary>
        /// The main form's paint event handler. It paints the current target on the screen.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            if (_sdata == null) // no session is yet underway
            {
                string msg = "Type Ctrl+N to begin a new test session.";
                SizeF sz = e.Graphics.MeasureString(msg, mnsMain.Font);
                e.Graphics.DrawString(msg, mnsMain.Font, Brushes.Black, Width / 2f - sz.Width / 2f, Height / 2f);
            }
            else // a session is underway
            {
                // Draw all of the targets in the condition light gray.
                foreach (Region rgn in _cdata.TargetRegions)
                {
                    e.Graphics.FillRegion(Brushes.LightGray, rgn);
                    rgn.Dispose();
                }
                
                // Draw the current target blue.
                Region trgn = _tdata.TargetRegion;
                e.Graphics.FillRegion(Brushes.DeepSkyBlue, trgn);
                trgn.Dispose();

                // Draw the animated fill for metronome trials.
                if (_cdata.MT != -1.0)
                {
                    Region[] rgns = _tdata.GetAnimatedRegions(TimeEx.NowMs - _tLastMnomeTick);
                    foreach (Region rgn in rgns)
                    {
                        if (_tdata.IsStartAreaTrial && TimeEx.NowMs - _cdata.TAppeared < WaitBeforeClickMs)
                            e.Graphics.FillRegion(Brushes.LightGray, rgn);
                        else
                            e.Graphics.FillRegion(Brushes.Blue, rgn);
                        rgn.Dispose();
                    }
                }

                // Draw the start label beside the first target to indicate the start of each condition.
                if (_tdata.IsStartAreaTrial)
                {   
                    SizeF sz = e.Graphics.MeasureString("Start Here >", mnsMain.Font);
                    RectangleF r = (RectangleF) _tdata.TargetBounds;
                    e.Graphics.FillRectangle(Brushes.White, r.X - sz.Width, r.Y + r.Height / 2 - sz.Height / 2, sz.Width, sz.Height);
                    e.Graphics.DrawRectangle(Pens.Black, r.X - sz.Width, r.Y + r.Height / 2 - sz.Height / 2, sz.Width, sz.Height);
                    e.Graphics.DrawString("Start Here", mnsMain.Font, Brushes.Black, r.X - sz.Width, r.Y + r.Height / 2 - sz.Height / 2 + 1);
                    Font wdings = new Font("Wingdings", mnsMain.Font.Size + 2f); // for arrow symbol
                    SizeF szw = e.Graphics.MeasureString("à", wdings); // == (char) 224
                    e.Graphics.DrawString("à", wdings, Brushes.Black, r.X - szw.Width + 2, r.Y + r.Height / 2 - sz.Height / 2 + 1); // arrow symbol
                    wdings.Dispose();
                }
            }
        }

        #endregion

        #region File Menu

        /// <summary>
        /// The handler for just before the File menu opens. It is used to enable and disable 
        /// menu items.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mnuFile_DropDownOpening(object sender, EventArgs e)
        {
            mniAnalyze.Enabled = (_sdata == null);
            mniViewLog.Enabled = (_sdata == null);
            mniViewModel.Enabled = (_sdata == null);
            mniGraph.Enabled = (_sdata == null);
            mniExit.Enabled = (_sdata == null);
        }

        /// <summary>
        /// Handler for the File > Exit menu item. Closes the form.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mniExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handler for the File > View Log menu item. This allows the user to view a
        /// FittsStudy log within the application itself in both Web and plain text
        /// formats.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mniViewLog_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Log Files (*.xml)|*.xml";
            ofd.DefaultExt = "xml";
            ofd.AddExtension = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.InitialDirectory = _o.Directory;
            ofd.Title = "Open FittsStudy Log";
            ofd.Multiselect = false;

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                LogForm frm = new LogForm();
                frm.Filename = ofd.FileName;
                frm.Show(this);
            }

            ofd.Dispose();
        }

        /// <summary>
        /// Menu handler for the View Model menu item. Opens a log file and reads it in, and
        /// then opens the Model form and displays the proper model for the study.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mniViewModel_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Log Files (*.xml)|*.xml";
            ofd.DefaultExt = "xml";
            ofd.AddExtension = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.InitialDirectory = _o.Directory;
            ofd.Title = "Open FittsStudy Log";
            ofd.Multiselect = false;

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                string msg;
                SessionData fs = new SessionData();
                XmlTextReader reader = new XmlTextReader(new StreamReader(ofd.FileName));
                if (fs.ReadFromXml(reader))
                {
                    ModelForm mform = new ModelForm(fs, Path.GetFileNameWithoutExtension(ofd.FileName) + ".model.txt");
                    mform.ShowDialog(this);
                }
                else
                {
                    msg = String.Format("Unable to read data from {0}.", Path.GetFileName(ofd.FileName));
                    MessageBox.Show(this, msg, "Invalid Log", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            ofd.Dispose();
        }

        /// <summary>
        /// Menu handler for the Analyze menu item. Opens and parses an XML log file written
        /// previously by the FittsStudy application. Then it builds up its internal data
        /// structures and writes them to a comma-separated TXT file that can be pasted into
        /// a spreadsheet (e.g., Excel or JMP).
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mniAnalyze_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Log Files (*.xml)|*.xml";
            ofd.DefaultExt = "xml";
            ofd.AddExtension = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.InitialDirectory = _o.Directory;
            ofd.Title = "Open FittsStudy Log(s)";
            ofd.Multiselect = true;

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                string msg;
                int successes = 0;
                for (int i = 0; i < ofd.FileNames.Length; i++)
                {
                    SessionData fs = new SessionData();
                    XmlTextReader reader = new XmlTextReader(new StreamReader(ofd.FileNames[i]));
                    if (fs.ReadFromXml(reader))
                    {
                        string fileNoExt = Path.GetFileNameWithoutExtension(ofd.FileNames[i]);
                        if (fs.WriteResultsToTxt(new StreamWriter(fileNoExt + ".txt", false, Encoding.UTF8)))
                        {
                            successes++;
                        }
                        else
                        {
                            msg = String.Format("Unable to write results for {0}.", Path.GetFileName(ofd.FileNames[i]));
                            MessageBox.Show(this, msg, "Unwritable Results", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        msg = String.Format("Unable to read data from {0}.", Path.GetFileName(ofd.FileNames[i]));
                        MessageBox.Show(this, msg, "Invalid Log", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                msg = String.Format("{0} of {1} log(s) parsed and analyzed successfully,\r\nand written to the same directory as the log file(s).", successes, ofd.FileNames.Length);
                MessageBox.Show(this, msg, "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            ofd.Dispose();
        }

        /// <summary>
        /// Menu handler to open a log file and graph all the trials that occurred in that log. This
        /// includes a graphical depiction of each trial, and distance, velocity, acceleration, and 
        /// jerk submovement profiles.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mniGraph_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Log Files (*.xml)|*.xml";
            ofd.DefaultExt = "xml";
            ofd.AddExtension = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.InitialDirectory = _o.Directory;
            ofd.Title = "Open FittsStudy Log";
            ofd.Multiselect = false;

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                string msg;
                SessionData fs = new SessionData();
                XmlTextReader reader = new XmlTextReader(new StreamReader(ofd.FileName));
                if (fs.ReadFromXml(reader))
                {
                    GraphForm gform = new GraphForm(fs);
                    gform.ShowDialog(this);
                }
                else
                {
                    msg = String.Format("Unable to read data from {0}.", Path.GetFileName(ofd.FileName));
                    MessageBox.Show(this, msg, "Invalid Log", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            ofd.Dispose();
        }

        #endregion

        #region Test Menu

        /// <summary>
        /// The handler for just before the Test menu opens. It is used to enable
        /// and disable menu items.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mnuTest_DropDownOpening(object sender, EventArgs e)
        {
            mniNew.Enabled = (_sdata == null);
            mniStop.Enabled = (_sdata != null);
        }

        /// <summary>
        /// Menu item handler that starts a new Fitts study session. This handler opens the experiment
        /// options dialog and parameterizes the test with the information obtained.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        /// <remarks>A new Fitts' session <i>_fs</i> is begun in this handler. The first condition from that 
        /// session (index 0) is obtained. The XML log file for the session is also created.</remarks>
        private void mniNew_Click(object sender, EventArgs e)
        {
            OptionsForm dlg = new OptionsForm(_o);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                _o = dlg.Settings; // update our options with those from the dialog

                // create the session instance. from that, we will get the first condition and its trial.
                _sdata = new SessionData(_o);
                _cdata = _sdata[0, 0]; // first overall condition
                _tdata = _cdata[0]; // first trial is special start-area trial at index 0

                // create the XML log for output and write the log header
                _fileNoExt = String.Format("{0}\\{1}__{2}", _o.Directory, _sdata.FilenameBase, Environment.TickCount);
                _writer = new XmlTextWriter(_fileNoExt + ".xml", Encoding.UTF8);
                _writer.Formatting = Formatting.Indented;
                _sdata.WriteXmlHeader(_writer);

                // initialize the test state
                if (_cdata.MT != -1.0) // for metronome studies
                {
                    _tLastMnomeTick = TimeEx.NowMs; // ms
                    _cdata.TAppeared = TimeEx.NowMs; // now
                    _tMnomeInterval = _cdata.MT; // interval in ms for 1 metronome 'tick'
                    _timerID = Win32.SetTimer(this.Handle, 1u, MnomeAnimationTime, IntPtr.Zero);
                }
                UpdateStatusBar(); // update the status bar at the bottom of the test screen
                Invalidate(); // redraw to show the initial targets
            }
        }

        /// <summary>
        /// Menu item handler that stops a Fitts study session in progress. This menu item should not
        /// be used in a normal session, but may be used to abort a session underway and close the
        /// existing XML log file. No TXT analysis file will be written.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        /// <remarks>This handler assumes that a Fitts condition is underway. The current trial is not
        /// added to the current condition before the test is stopped.</remarks>
        private void mniStop_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "This will stop the session, truncating the log at this point. The log may need adjusting by hand before parsing.", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                Win32.KillTimer(this.Handle, _timerID);
                _cdata.WriteXmlHeader(_writer); // write out the current condition
                _sdata.WriteXmlFooter(_writer); // write the end of the XML log and closes the log
                ResetState();
                UpdateStatusBar();
                Invalidate();
            }
        }

        #endregion

        #region Help Menu

        /// <summary>
        /// The handler for just before the Help menu opens. It is used to enable
        /// and disable menu items.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mnuHelp_DropDownOpening(object sender, EventArgs e)
        {
            mniMouse.Enabled = (_sdata == null);
            mniAbout.Enabled = (_sdata == null);
        }

        /// <summary>
        /// Menu item handler to open the mouse control panel, for convenience.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mniMouse_Click(object sender, EventArgs e)
        {
            ControlPanelEx.OpenMouseControlPanel(2);
        }
        /// <summary>
        /// Displays the About box for this application.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mniAbout_Click(object sender, EventArgs e)
        {
            AboutBox dlg = new AboutBox();
            dlg.ShowDialog(this);
        }

        #endregion

        #region Close Box "Menu"

        /// <summary>
        /// No titlebar or menu items are shown for the main form window so that the
        /// entire form is maximized client space. But we have a menu that shows the
        /// close box image and can be clicked as a button.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mnuClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Add a handler to clicking on the main menu bar that closes the form when the 
        /// close box is just barely missed in the top-right corner of the form. A "real"
        /// close box on a Microsoft window has this functionality already, but we have
        /// to provide it ourselves because of our full-screen style where the close box
        /// is actually a menu.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mnsMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (mnuClose.Bounds.Left <= e.X && e.Y <= mnuClose.Bounds.Bottom && !mnuClose.Bounds.Contains(e.X, e.Y))
            {
                Close();
            }
        }

        #endregion

        #region Keyboard, Mouse, and WndProc

        /// <summary>
        /// When 'Esc' is pressed (ASCII 27), the current condition is reset and begun again.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27) // 'Esc'
            {
                if (_cdata != null) // condition in progress
                {
                    Win32.KillTimer(this.Handle, _timerID);
                    _cdata.ClearTrialData(); // clear the performance data for the trials in this condition
                    _tdata = _cdata[0]; // get the special start-area trial
                    if (_cdata.MT != -1.0) // metronome trials
                    {
                        _tLastMnomeTick = TimeEx.NowMs; // ms
                        _cdata.TAppeared = TimeEx.NowMs; // now
                        _timerID = Win32.SetTimer(this.Handle, 1u, MnomeAnimationTime, IntPtr.Zero);
                    }
                    UpdateStatusBar();
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// When the mouse goes down, if there is an active condition, it represents the
        /// end of one trial and the beginning of the next. A time-stamped click-point is
        /// passed to the function that ends the previous trial and begins the next one.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        /// <remarks>
        /// Double-clicks are filtered by ensuring that the distance between two clicks
        /// is greater than a minimum.
        /// </remarks>
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (_cdata != null)
            {
                if (_cdata.MT == -1.0 || TimeEx.NowMs - _cdata.TAppeared > WaitBeforeClickMs)
                {
                    TimePointR pt = new TimePointR(e.X, e.Y, TimeEx.NowMs);
                    if (_tdata.IsStartAreaTrial) // first click to begin condition
                    {
                        NextTrial(pt);
                    }
                    else // 2nd+ click ends trial, starts next
                    {
                        if (GeotrigEx.Distance((PointR) _tdata.Start, (PointR) pt) > MinDblClickDist) // filter double clicks
                        {
                            NextTrial(pt);
                        }
                    }
                }
                else DoError(); // clicking before start is enabled
            }
        }

        /// <summary>
        /// When there is an active trial and the mouse is moved, the movements are added
        /// to the trial for later logging. The movements are time-stamped points.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        /// <remarks>
        /// Mouse movements matter only when there is a current trial. This is not the same as
        /// when there is a current condition, since the first click to start a the first trial
        /// in a condition exists after an active condition but before the first active trial.
        /// We don't want to log mouse movements before that first trial is begun with that
        /// initial click.
        /// </remarks>
        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (_tdata != null && !_tdata.IsStartAreaTrial) // only record moves when we are within a trial
            {
                _tdata.Movement.AddMove(new TimePointR(e.X, e.Y, TimeEx.NowMs));
            }
        }

        /// <summary>
        /// When using a metronome, the metronome is portrayed graphically around the targets. This
        /// graphical representation animates according to this repeating animation timer. When this 
        /// timer function fires, the animations are invalidated, causing Paint to trigger. The actual 
        /// animation dimensions are determined in Paint based on the amount of time that has elapsed 
        /// since the last metronome tick. That value is set here when the required amount of time has passed.
        /// </summary>
        /// <param name="m">The Win32 MSG structure provided to this event dispatch.</param>
        protected override void WndProc(ref Message m)
        {
            // call the base window dispatch
            base.WndProc(ref m);

            // handle the timer message from our animation timer
            if (m.Msg == (int) Win32.WM.TIMER)
            {
                if (TimeEx.NowMs - _tLastMnomeTick >= _tMnomeInterval)
                {
                    // time interval has passed
                    _tLastMnomeTick = TimeEx.NowMs; // update
                    SoundEx.PlaySound(_sndMnome);
                }
                // invalidate the current target so that the metronome animation is portrayed.
                Rectangle r0 = (Rectangle) _tdata.TargetBounds;
                Invalidate(r0);
            }
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// This is the main function that begins a new trial. The beginning of a new trial may
        /// mark the beginning of a new condition, a new trial in a condition already underway,
        /// or the last trial of the current condition, which may lead to a new condition or to the 
        /// end of the entire test session.
        /// </summary>
        /// <param name="click">The time-stamped mouse click that marks the transition from the 
        /// last trial (if there is one) to the next trial.</param>
        private void NextTrial(TimePointR click)
        {
            if (_tdata.IsStartAreaTrial) // click was to begin the first actual trial in the current condition
            {
                if (!_tdata.TargetContains((PointR) click)) // click missed start target
                {
                    DoError();
                }
                else // start first actual trial
                {
                    _tdata = _cdata[1]; // trial number 1
                    _tdata.Start = click;
                }
                Invalidate(); // paint whole form because of START label 
            }
            else if (_tdata.Number < _cdata.NumTrials) // go to next trial in condition
            {
                _tdata.End = click;
                _tdata.NormalizeTimes();
                if (_tdata.IsError)
                    DoError();
                
                Rectangle r0 = (Rectangle) _tdata.TargetBounds;
                Invalidate(r0);
                _tdata = _cdata[_tdata.Number + 1];
                _tdata.Start = click;
                Rectangle r1 = (Rectangle) _tdata.TargetBounds;
                Invalidate(r1);
            }
            else // end the condition and go to the next, or end the session if done
            {
                Win32.KillTimer(this.Handle, _timerID);
                _tdata.End = click; // time point end of previous trial
                _tdata.NormalizeTimes();
                if (_tdata.IsError)
                    DoError();
                
                // format rich-text message
                string rtf = String.Format("{{\\rtf1 {{\\colortbl;\\red215\\green0\\blue0;}} {{\\par\\par\\qc\\b\\cf0 {0} of {1} conditions in block {2} complete.  Errors on test trials: {3:f1}%. {{\\cf1  Cumulative: {4:f1}%.}} \\par}}}}", 
                    _cdata.Index + 1, 
                    _sdata.NumConditionsPerBlock, 
                    _cdata.Block, 
                    _cdata.GetErrorRate(ExcludeOutliersType.None) * 100.0, 
                    _sdata.GetErrorRate(ExcludeOutliersType.None) * 100.0);

                if (MessageBanner.ShowDialog(this, rtf, true) == DialogResult.OK)
                {
                    _cdata.WriteXmlHeader(_writer); // write out the condition and its trials to XML
                    if (_cdata.Index + 1 == _sdata.NumConditionsPerBlock) // we have run all conditions in this block
                    {
                        if (_cdata.Block + 1 == _sdata.NumBlocks) // we have run all blocks
                        {
                            _sdata.WriteXmlFooter(_writer); // writes the end of the XML log and closes the log
                            MessageBox.Show(this, "All conditions are done. Session complete!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ModelForm m = new ModelForm(_sdata, _fileNoExt + ".model.txt"); // show the modal model :)
                            m.ShowDialog(this);
                            ResetState();
                        }
                        else // still more blocks to run
                        {
                            MessageBox.Show(this, "Starting a new block of all conditions.", "Block Break", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _cdata = _sdata[_cdata.Block + 1, 0]; // first condition in next block
                            _tdata = _cdata[0]; // special start-area trial at index 0
                        }
                    }
                    else // still more conditions in this block to be run
                    {
                        _cdata = _sdata[_cdata.Block, _cdata.Index + 1]; // next condition in same block
                        _tdata = _cdata[0]; // special start-area trial at index 0
                    }
                }
                else // 'Esc' was pressed on the message banner -- redo the condition
                {
                    _cdata.ClearTrialData(); // clear the data from the set of trials
                    _tdata = _cdata[0]; // get special start-area trial at index 0
                }
                if (_cdata != null && _cdata.MT != -1.0) // start metronome if applicable
                {
                    _tLastMnomeTick = TimeEx.NowMs; // ms
                    _cdata.TAppeared = TimeEx.NowMs; // now
                    _tMnomeInterval = _cdata.MT;
                    _timerID = Win32.SetTimer(this.Handle, 1u, MnomeAnimationTime, IntPtr.Zero);
                }
                Invalidate(); // redraw all for end of a condition or session
            }

            // update the status bar for the new trial
            UpdateStatusBar();
        }

        /// <summary>
        /// This method notifies the user of errors through a system beep and drawing the target red.
        /// </summary>
        /// <remarks>This is the only place that drawing is done outside of the Paint handler.</remarks>
        private void DoError()
        {
            SystemSounds.Beep.Play();
            Graphics g = Graphics.FromHwnd(this.Handle);
            Region rgn = _tdata.TargetRegion;
            g.FillRegion(Brushes.Red, rgn);
            rgn.Dispose();
            g.Dispose();
            Thread.Sleep(MnomeAnimationTime);
        }

        /// <summary>
        /// Resets the current test data and state for the Fitts study. Does not involve clearing
        /// an UI rendering, widgets, file names, or logging.
        /// </summary>
        private void ResetState()
        {
            Win32.KillTimer(this.Handle, _timerID);
            _tLastMnomeTick = -1L;
            _tMnomeInterval = -1L;

            _sdata = null;
            _cdata = null;
            _tdata = null;

            if (_writer != null && _writer.BaseStream != null)
            {
                _writer.Close();
                _writer = null;
            }
        }

        /// <summary>
        /// Update the status bar at the bottom of the main form after each trial. All of the fields
        /// are updated to the current values if there is an active condition. If not, then the
        /// fields are set to zeroes.
        /// </summary>
        private void UpdateStatusBar()
        {
            if (_sdata != null)
            {
                lblSubject.Text = String.Format("Subject: {0}", _sdata.Subject);
                lblLayout.Text = String.Format("Layout: {0}-D", _sdata.Is2D ? 2 : 1);
                lblBlock.Text = String.Format("Block: {0}", _cdata.Block);
                lblCondition.Text = String.Format("Condition: {0}", _cdata.Index);
                lblTrial.Text = String.Format("Trial: {0}", _tdata.Number);
                lblA.Text = String.Format("A: {0} px", _cdata.A);
                lblW.Text = String.Format("W: {0} px", _cdata.W);
                lblID.Text = String.Format("ID: {0:f2} bits", _cdata.ID);
                lblMTPct.Text = String.Format("MT%: {0:f2}", _cdata.MTPct);
                lblMTPred.Text = String.Format("MTPred: {0} ms", _cdata.MTPred);
                lblMT.Text = String.Format("MT: {0} ms", _cdata.MT);
                lbl_a.Text = String.Format("a: {0} ms", _sdata.Intercept);
                lbl_b.Text = String.Format("b: {0} ms/bit", _sdata.Slope);
            }
            else
            {
                lblSubject.Text = "Subject: 0";
                lblLayout.Text = "Layout: 0";
                lblBlock.Text = "Block: 0";
                lblCondition.Text = "Condition: 0";
                lblTrial.Text = "Trial: 0";
                lblA.Text = "A: 0 px";
                lblW.Text = "W: 0 px";
                lblID.Text = "ID: 0 bits";
                lblMTPct.Text = "MT%: 0.00";
                lblMTPred.Text = "MTPred: 0 ms";
                lblMT.Text = "MT: 0 ms";
                lbl_a.Text = "a: 0 ms";
                lbl_b.Text = "b: 0 ms/bit";
            }
        }


        #endregion

    }
}