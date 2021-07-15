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
using System.Xml;
using WobbrockLib.Types;
using WobbrockLib.Extensions;
using WobbrockLib.Controls;

namespace FittsStudy
{
    public partial class GraphForm : Form
    {
        // icon indices
        private const int SessionIcon = 0;
        private const int BlockIcon = 1;
        private const int ConditionIcon = 2;
        private const int TrialPracticeIcon = 3;
        private const int TrialGoodIcon = 4;
        private const int TrialErrorIcon = 5;
        private const int TrialOutlierIcon = 6;

        // member fields
        private SessionData _sdata;
        private ConditionData _cdata;
        private TrialData _tdata;
        private List<string> _tmpXml;

        /// <summary>
        /// Constructs a new GraphForm instance.
        /// </summary>
        /// <param name="sdata">The session instance that holds all data for this study session.</param>
        public GraphForm(SessionData sdata)
        {
            InitializeComponent();
            _sdata = sdata;
            _tmpXml = new List<string>(); // temporary files to delete upon closing
            txtFilename.Text = _sdata.FilenameBase + ".xml";
        }

        /// <summary>
        /// When the close box is clicked in the top-right, close the form.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mnuClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Add a handler to clicking on the menu bar that closes the form when the 
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

        /// <summary>
        /// Delete the temporary XML files that were created for the browser control when the form closes.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void GraphForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (string tmp in _tmpXml)
            {
                File.Delete(tmp);
            }
        }

        /// <summary>
        /// Build the tree view of the session, blocks, conditions, and trials on the left. Use icons
        /// to depict each, and show valuable information in each string label.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void GraphForm_Load(object sender, EventArgs e)
        {
            string s;
            trvTrials.SuspendLayout();

            s = String.Format("Subject {0} - {1}-D, {2}", _sdata.Subject, _sdata.Is2D ? 2 : 1, _sdata.UsedMetronome ? "with metronome" : "no metronome");
            TreeNode sNode = new TreeNode(s);
            sNode.Tag = _sdata;
            sNode.ImageIndex = SessionIcon;
            sNode.SelectedImageIndex = SessionIcon;
            trvTrials.Nodes.Add(sNode);

            for (int b = 0; b < _sdata.NumBlocks; b++)
            {
                s = String.Format("Block {0}", b);
                TreeNode bNode = new TreeNode(s);
                bNode.ImageIndex = BlockIcon;
                bNode.SelectedImageIndex = BlockIcon;
                sNode.Nodes.Add(bNode);

                for (int i = 0; i < _sdata.NumConditionsPerBlock; i++)
                {
                    ConditionData cdata = _sdata[b, i];
                    if (cdata.MTPct != -1.0)
                        s = String.Format("Condition {0} - {1}×{2}×{3}", cdata.Index, cdata.MTPct, cdata.A, cdata.W);
                    else
                        s = String.Format("Condition {0} - {1}×{2}", cdata.Index, cdata.A, cdata.W);
                    TreeNode cNode = new TreeNode(s);
                    cNode.Tag = cdata;
                    cNode.ImageIndex = ConditionIcon;
                    cNode.SelectedImageIndex = ConditionIcon;
                    bNode.Nodes.Add(cNode);

                    for (int j = 1; j <= cdata.NumTrials; j++)
                    {
                        TrialData tdata = cdata[j];
                        s = String.Format("Trial {0}", tdata.Number);
                        TreeNode tNode = new TreeNode(s);
                        tNode.Tag = tdata;
                        
                        if (tdata.IsPractice)
                        {
                            tNode.ImageIndex = TrialPracticeIcon;
                            tNode.SelectedImageIndex = TrialPracticeIcon;
                            tNode.ToolTipText = "Practice";
                        }
                        else if (tdata.IsSpatialOutlier || tdata.IsTemporalOutlier)
                        {
                            tNode.ImageIndex = TrialOutlierIcon;
                            tNode.SelectedImageIndex = TrialOutlierIcon;
                            tNode.ToolTipText = "Outlier";
                        }
                        else if (tdata.IsError)
                        {
                            tNode.ImageIndex = TrialErrorIcon;
                            tNode.SelectedImageIndex = TrialErrorIcon;
                            tNode.ToolTipText = "Miss";
                        }
                        else
                        {
                            tNode.ImageIndex = TrialGoodIcon;
                            tNode.SelectedImageIndex = TrialGoodIcon;
                        }
                        
                        cNode.Nodes.Add(tNode);
                    }
                }
            }

            sNode.Nodes[0].Nodes[0].Nodes[0].EnsureVisible(); // ensure first trial node is visible
            trvTrials.SelectedNode = sNode.Nodes[0].Nodes[0].Nodes[0]; // select first trial
            trvTrials.ResumeLayout(true);
        }

        /// <summary>
        /// When a new node has been selected, graph the information for that node and the trial portrait.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void trvTrials_AfterSelect(object sender, TreeViewEventArgs e)
        {
            switch (e.Node.Level)
            {
                case 0: // session node
                    ShowSessionInfo(e.Node);
                    break;
                case 1: // block node
                    ShowBlockInfo(e.Node);
                    break;
                case 2: // condition node
                    ShowConditionInfo(e.Node);
                    break;
                case 3: // trial node
                    ShowTrialInfo(e.Node);
                    break;
            }
        }

        /// <summary>
        /// Shows all the information for a session-level node highlighted in the treeview.
        /// </summary>
        /// <param name="node">The highlighted node in the left treeview, assumed to represent a single session.</param>
        private void ShowSessionInfo(TreeNode node)
        {
            //
            // Get the session data. Note that sdata should be the same instance as _sdata.
            // For consistency, we pull it from the node's tag.
            //
            SessionData sdata = (SessionData) node.Tag;

            //
            // Set up a reusable XML writer.
            //
            XmlTextWriter writer = null;
            string tmpXml = String.Format("{0}{1}.xml", Path.GetTempPath(), Path.GetRandomFileName());
            try
            {
                writer = new XmlTextWriter(tmpXml, Encoding.UTF8);
                writer.Formatting = Formatting.Indented;
                sdata.WriteXmlHeader(writer);

                //
                // Iterate through the conditions within the session.
                //
                foreach (TreeNode bnode in node.Nodes)
                {
                    foreach (TreeNode cnode in bnode.Nodes)
                    {
                        ConditionData cd = (ConditionData) cnode.Tag;
                        cd.WriteXmlHeader(writer); // writes out trials as well
                    }
                }
                sdata.WriteXmlFooter(writer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                tmpXml = String.Empty;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
                if (tmpXml != String.Empty)
                {
                    webXml.Navigate(tmpXml);
                    rtxXml.LoadFile(tmpXml, RichTextBoxStreamType.PlainText);
                    _tmpXml.Add(tmpXml);
                }
            }
        }

        /// <summary>
        /// Shows all the information for a block-level node highlighted in the treeview.
        /// </summary>
        /// <param name="node">The highlighted node in the left treeview, assumed to represent a single block.</param>
        private void ShowBlockInfo(TreeNode node)
        {
            //
            // Set up a reusable XML writer.
            //
            XmlTextWriter writer = null;
            string tmpXml = String.Format("{0}{1}.xml", Path.GetTempPath(), Path.GetRandomFileName());
            try
            {
                writer = new XmlTextWriter(tmpXml, Encoding.UTF8);
                writer.Formatting = Formatting.Indented;
                writer.WriteStartElement("Block");
                writer.WriteAttributeString("index", XmlConvert.ToString(((ConditionData) node.Nodes[0].Tag).Block));

                //
                // Blocks themselves are not data objects, they're just identifiers on conditions.
                // So iterate through the block's children nodes and write them out, to be later merged.
                //
                foreach (TreeNode cnode in node.Nodes)
                {
                    ConditionData cd = (ConditionData) cnode.Tag;
                    cd.WriteXmlHeader(writer);
                }

                writer.WriteEndElement(); // </Block>
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                tmpXml = String.Empty;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
                if (tmpXml != String.Empty)
                {
                    webXml.Navigate(tmpXml);
                    rtxXml.LoadFile(tmpXml, RichTextBoxStreamType.PlainText);
                    _tmpXml.Add(tmpXml);
                }
            }
        }

        /// <summary>
        /// Shows all the information for a condition-level node highlighted in the treeview.
        /// </summary>
        /// <param name="node">The highlighted node in the left treeview, assumed to represent a single condition.</param>
        private void ShowConditionInfo(TreeNode node)
        {
            //
            // Get the condition from the 'Tag' field from the selected node. We don't
            // set the member variable because we're leaving the most recent trial depicted
            // on the graphs, etc., and just loading the XML for the condition.
            //
            ConditionData cd = (ConditionData) node.Tag;

            //
            // Set up a reusable XML writer.
            //
            XmlTextWriter writer = null;
            string tmpXml = String.Format("{0}{1}.xml", Path.GetTempPath(), Path.GetRandomFileName());
            try
            {
                writer = new XmlTextWriter(tmpXml, Encoding.UTF8);
                writer.Formatting = Formatting.Indented;
                cd.WriteXmlHeader(writer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                tmpXml = String.Empty;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
                if (tmpXml != String.Empty)
                {
                    webXml.Navigate(tmpXml);
                    rtxXml.LoadFile(tmpXml, RichTextBoxStreamType.PlainText);
                    _tmpXml.Add(tmpXml);
                }
            }
        }

        /// <summary>
        /// Shows all the information for a trial-level node highlighted in the treeview.
        /// </summary>
        /// <param name="node">The highlighted node in the left treeview, assumed to represent an individual trial.</param>
        private void ShowTrialInfo(TreeNode node)
        {
            //
            // Get the trial and its condition from the 'Tag' field from the selected node.
            //
            _tdata = (TrialData) node.Tag;
            _cdata = (ConditionData) node.Parent.Tag;

            //
            // Get the resampled and smoothed velocity, acceleration, and jerk profiles.
            //
            MovementData.Profiles resampled = _tdata.Movement.CreateResampledProfiles();
            MovementData.Profiles smoothed = _tdata.Movement.CreateSmoothedProfiles();

            //
            // Clear any series in the graphs.
            //
            grfDistance.ClearSeries();
            grfVelocity.ClearSeries();
            grfAcceleration.ClearSeries();
            grfJerk.ClearSeries();

            //
            // Add the origin point to all graphs.
            //
            Plot.Series origin = new Plot.Series("origin", Color.Black, Color.Black, false, false); // origin point
            origin.AddPoint(0.0, 0.0);
            grfDistance.AddSeries(origin);
            grfVelocity.AddSeries(origin);
            grfAcceleration.AddSeries(origin);
            grfJerk.AddSeries(origin);

            //
            // Graph the resampled distance to the target over time.
            //
            List<PointR> rdist = new List<PointR>(resampled.Position.Count);
            for (int i = 0; i < resampled.Position.Count; i++)
            {
                long t = resampled.Position[i].Time - resampled.Position[0].Time;
                double dx = GeotrigEx.Distance((PointR) resampled.Position[i], _tdata.TargetCenterFromStart);
                rdist.Add(new PointR(t, dx));
            }
            Plot.Series rd = new Plot.Series("resampled distance", Color.Salmon, Color.Salmon, false, true);
            rd.AddPoints(rdist);

            //
            // Graph the smoothed distance from the target over time.
            //
            List<PointR> sdist = new List<PointR>(smoothed.Position.Count);
            for (int i = 0; i < smoothed.Position.Count; i++)
            {
                long t = smoothed.Position[i].Time - smoothed.Position[0].Time;
                double dx = GeotrigEx.Distance((PointR) smoothed.Position[i], _tdata.TargetCenterFromStart);
                sdist.Add(new PointR(t, dx));
            }
            Plot.Series sd = new Plot.Series("smoothed distance", Color.MediumBlue, Color.MediumBlue, false, true);
            sd.AddPoints(sdist);

            grfDistance.AddSeries(rd);
            grfDistance.AddSeries(sd);

            //
            // Graph the velocity.
            //
            Plot.Series rv = new Plot.Series("resampled velocity", Color.Salmon, Color.Salmon, false, true);
            rv.AddPoints(resampled.Velocity);

            Plot.Series sv = new Plot.Series("smoothed velocity", Color.MediumBlue, Color.MediumBlue, false, true);
            sv.AddPoints(smoothed.Velocity);

            grfVelocity.AddSeries(rv);
            grfVelocity.AddSeries(sv);

            //
            // Graph the acceleration.
            //
            Plot.Series aaxis = new Plot.Series("zero line", Color.Gray, Color.Gray, false, true);
            aaxis.AddPoint(0.0, 0.0);
            aaxis.AddPoint(resampled.Acceleration[resampled.Acceleration.Count - 1].X, 0.0);

            Plot.Series ra = new Plot.Series("resampled acceleration", Color.Salmon, Color.Salmon, false, true);
            ra.AddPoints(resampled.Acceleration);

            Plot.Series sa = new Plot.Series("smoothed acceleration", Color.MediumBlue, Color.MediumBlue, false, true);
            sa.AddPoints(smoothed.Acceleration);

            grfAcceleration.AddSeries(aaxis);
            grfAcceleration.AddSeries(ra);
            grfAcceleration.AddSeries(sa);

            //
            // Graph the jerk.
            //
            Plot.Series jaxis = new Plot.Series("zero line", Color.Gray, Color.Gray, false, true);
            jaxis.AddPoint(0.0, 0.0);
            jaxis.AddPoint(resampled.Jerk[resampled.Jerk.Count - 1].X, 0.0);

            Plot.Series rj = new Plot.Series("resampled jerk", Color.Salmon, Color.Salmon, false, true);
            rj.AddPoints(resampled.Jerk);

            Plot.Series sj = new Plot.Series("smoothed jerk", Color.MediumBlue, Color.MediumBlue, false, true);
            sj.AddPoints(smoothed.Jerk);

            grfJerk.AddSeries(jaxis);
            grfJerk.AddSeries(rj);
            grfJerk.AddSeries(sj);

            //
            // Now show the trial-level XML in the web control.
            //
            XmlTextWriter writer = null;
            string tmpXml = String.Format("{0}{1}.xml", Path.GetTempPath(), Path.GetRandomFileName());
            try
            {
                writer = new XmlTextWriter(tmpXml, Encoding.UTF8);
                writer.Formatting = Formatting.Indented;
                _tdata.WriteXmlHeader(writer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                tmpXml = String.Empty;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
                if (tmpXml != String.Empty)
                {
                    webXml.Navigate(tmpXml);
                    rtxXml.LoadFile(tmpXml, RichTextBoxStreamType.PlainText);
                    _tmpXml.Add(tmpXml);
                }
            }

            //
            // Invalidate everything that needs to be repainted based on the node just selected.
            //
            pnlTrial.Invalidate();
            grfDistance.Invalidate();
            grfVelocity.Invalidate();
            grfAcceleration.Invalidate();
            grfJerk.Invalidate();
        }

        /// <summary>
        /// Paint the panel in which we depict the trial among its targets and show the path the mouse took.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void pnlTrial_Paint(object sender, PaintEventArgs e)
        {
            if (_tdata != null)
            {
                //
                // We'll have to map from the original trial area to where we're drawing here.
                //
                RectangleR rTo = pnlTrial.ClientRectangle; // this panel's client area
                RectangleR rFrom = _sdata.ScreenBounds; // the screen's boundaries used for these trials
                //(Note: Owner.ClientRectangle would work only if the trials were run on this same machine.)

                //
                // Our zoom slider allows zooming in or out (10%-190%). We have to carefully
                // adjust our percent: when rFrom shrinks, stuff drawn in rTo will enlarge,
                // and when rFrom enlarges, stuff drawn in rTo will shrink. So we can't
                // naively use the percent we get off the slider. It feels right enough.
                //
                double pct = 1.0 + (1.0 - trkZoom.Value / 100.0); // adjust
                rFrom.ScaleBy(pct * pct);
                
                //
                // If we're zoomed in past 100%, then perform semantic zooming to the endpoint click
                // by moving a percentage of the way from our default origin to having the point in
                // the dead center of the panel.
                //
                if (pct < 1.0)
                {
                    rFrom.Left = GeotrigEx.MapValue(pct * pct, 1.0, 0.0, rFrom.X, _tdata.End.X - rFrom.Width / 2.0);
                    rFrom.Top = GeotrigEx.MapValue(pct * pct, 1.0, 0.0, rFrom.Y, _tdata.End.Y - rFrom.Height / 2.0);
                }

                //
                // Draw all the targets for this condition.
                //
                foreach (Region rgn in _cdata.TargetRegions)
                {
                    Region grgn = GeotrigEx.MapRegion(rgn, rFrom, rTo);
                    e.Graphics.FillRegion(Brushes.LightGray, grgn);
                    rgn.Dispose();
                    grgn.Dispose();
                }

                //
                // Draw the active target for this condition.
                //
                Region trgn = GeotrigEx.MapRegion(_tdata.TargetRegion, rFrom, rTo);
                e.Graphics.FillRegion(Brushes.DeepSkyBlue, trgn);
                trgn.Dispose();

                //
                // Draw the movement path.
                //
                Pen pen;
                if (_tdata.IsError)
                    pen = new Pen(Color.Red);
                else
                    pen = new Pen(Color.Green);

                for (int i = 1; i < _tdata.Movement.NumMoves; i++)
                {
                    PointF p0 = (PointF) GeotrigEx.MapPoint((PointR) _tdata.Movement[i - 1], rFrom, rTo);
                    PointF p1 = (PointF) GeotrigEx.MapPoint((PointR) _tdata.Movement[i], rFrom, rTo);
                    e.Graphics.DrawLine(pen, p0, p1);
                }

                //
                // Draw the start point as a hollow circle, and the end as an X.
                //
                PointF start = (PointF) GeotrigEx.MapPoint((PointR) _tdata.Start, rFrom, rTo);
                PointF end = (PointF) GeotrigEx.MapPoint((PointR) _tdata.End, rFrom, rTo);

                pen.Width = 2f;
                e.Graphics.DrawEllipse(pen, start.X - 3f, start.Y - 3f, 6f, 6f);
                e.Graphics.DrawLine(pen, end.X - 5f, end.Y - 5f, end.X + 5f, end.Y + 5f);
                e.Graphics.DrawLine(pen, end.X - 5f, end.Y + 5f, end.X + 5f, end.Y - 5f);
                pen.Dispose();
            }
        }

        /// <summary>
        /// Copies the image of the trial drawing to the clipboard.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mniCopy_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pnlTrial.Width, pnlTrial.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);

            // don't blt directly from the screen, but draw directly on this bitmap we've made.
            PaintEventArgs pe = new PaintEventArgs(g, pnlTrial.ClientRectangle);
            pnlTrial_Paint(this, pe);
            Clipboard.SetImage(bmp);

            bmp.Dispose();
            g.Dispose();
            pe.Dispose();
        }

        /// <summary>
        /// Handler for when the value changes on the trackbar used to control the zoom level of the trial drawing.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void trkZoom_ValueChanged(object sender, EventArgs e)
        {
            Point cursor = trkZoom.PointToClient(Cursor.Position);
            ttZoom.Show(trkZoom.Value.ToString() + "%", trkZoom, trkZoom.Width - 2, cursor.Y - 5, 1000); // ToolTip
            pnlTrial.Invalidate();
            pnlTrial.Update();
        }

        /// <summary>
        /// Allows the user to inspect the current zoom level without changing it by clicking down on the thumb.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void trkZoom_MouseDown(object sender, MouseEventArgs e)
        {
            ttZoom.Show(trkZoom.Value.ToString() + "%", trkZoom, trkZoom.Width - 2, e.Y - 5, 1000); // ToolTip
        }

    }
}
