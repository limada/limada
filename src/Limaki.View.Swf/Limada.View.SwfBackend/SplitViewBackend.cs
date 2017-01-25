/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2014 Lytico
 *
 * http://www.limada.org
 */

using System.Drawing;
using Limada.View.Vidgets;
using Limaki.Usecases.Vidgets;
using Limaki.View;
using Limaki.View.Properties;
using Limaki.View.SwfBackend.VidgetBackends;
using Limaki.View.SwfBackend.Viz;
using Limaki.View.Vidgets;
using Limaki.View.Viz;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Xwt.GdiBackend;
using DialogResult = Limaki.View.Vidgets.DialogResult;
using System.Collections.Generic;

namespace Limada.View.SwfBackend {

    public class SplitViewBackend : VidgetBackend<SplitContainer>, ISplitViewBackend, IDisposable {
        
        public new SplitView0 Frontend { get; protected set; }

        protected SplitContainer SplitContainer { get { return Control; } }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            this.Frontend = (SplitView0) frontend;
            Compose2 ();
        }

        protected override void Compose () {
            base.Compose ();
            
            Control.Dock = DockStyle.Fill;
            Control.Margin = new Padding(2);
            Control.Name = "SplitView";
            Control.Panel2MinSize = 0;
            Control.SplitterWidth = 3;
            Control.TabIndex = 1;


        }

        protected void Compose2 () {
            var displayBackend1 = Frontend.Display1.Backend.ToSwf ();
            displayBackend1.Dock = DockStyle.Fill;
            SplitContainer.Panel1.Controls.Add (displayBackend1);

            var displayBackend2 = Frontend.Display2.Backend.ToSwf ();
            displayBackend2.Dock = DockStyle.Fill;
            SplitContainer.Panel2.Controls.Add (displayBackend2);

            SplitContainer.Panel1.BackColor = displayBackend1.BackColor;
            SplitContainer.Panel2.BackColor = displayBackend1.BackColor;

            // this is for mono on linux:
            Control.ActiveControl = displayBackend1;
        }

        public void InitializeDisplay (IVidgetBackend displayBackend) {
        }


        protected Dictionary<Control, IVidget> _vidgets = new Dictionary<Control, IVidget> ();

        void ControlGotFocus(object sender, EventArgs e) {
            Trace.WriteLine(string.Format("{0} {1}", sender.GetType().Name,sender.GetHashCode()));
            var control = sender as Control;
            var displayBackend = sender as VisualsDisplayBackend;
            if (displayBackend != null) {
                Frontend.DisplayGotFocus(displayBackend.Display);
            } else if(control!=null) {
                IVidget vidget = null;
                if (_vidgets.TryGetValue (control, out vidget)) {
                    Frontend.VidgetGotFocus (vidget);
                }
            }
        }

        public void SetFocusCatcher (IVidgetBackend backend) {
            var control = backend.ToSwf ();
            if (control != null) {
                _vidgets[control] = backend.Frontend;
                control.Enter -= ControlGotFocus;
                control.MouseUp -= ControlGotFocus;
                control.GotFocus -= ControlGotFocus;
                //control.Enter += ControlGotFocus;
                control.MouseUp += ControlGotFocus;
                control.GotFocus += ControlGotFocus;
            }
        }

        public void ReleaseFocusCatcher (IVidgetBackend backend) {
            var control = backend.ToSwf (); 
            if (control != null) {
                control.Enter -= ControlGotFocus;
                control.MouseUp -= ControlGotFocus;
                control.GotFocus -= ControlGotFocus;
                _vidgets.Remove (control);
            }
        }

        public void ViewInWindow (IVidget vidget, Action onClose) {
			var backend = vidget.Backend;
            var control = backend.ToSwf (); 
            if (control != null) {
                control.Dock = DockStyle.Fill;
                var form = new Form {
                    FormBorderStyle = FormBorderStyle.SizableToolWindow,
                    Icon = GdiIconery.LimadaSubWinIcon,
                    Text = Frontend.CurrentDisplay.Info.Name + " - *"
                };
                form.FormClosing += (s, e) => onClose();
                form.Controls.Add (control);
                form.Show (Control.ParentForm);
                form.Location = Control.PointToScreen(new Point (Control.Location.X, Control.Location.Y+(Control.Height-form.Height)/2));
                //form.Height = this.Height/2;
                Func<Point> calcOffset = () => new Point (form.Location.X - Control.ParentForm.Location.X, form.Location.Y- Control.ParentForm.Location.Y);
                var offset = calcOffset();
                form.LocationChanged += (s, e) =>
                    offset = calcOffset ();
                Control.ParentForm.LocationChanged += (s, e) =>
                     form.Location = new Point (Control.ParentForm.Location.X + offset.X, Control.ParentForm.Location.Y + offset.Y);

            }
        }

        #region View-Switching
        
        public void ToggleView() {
            var one = SplitContainer.Panel1.Controls.Cast<Control>().ToArray();
            var two = SplitContainer.Panel2.Controls.Cast<Control>().ToArray();

            SplitContainer.SuspendLayout();

            SplitContainer.Panel1.Controls.Clear();
            SplitContainer.Panel2.Controls.Clear();
            SplitContainer.Panel1.Controls.AddRange(two);
            SplitContainer.Panel2.Controls.AddRange(one);

            SplitContainer.ResumeLayout();
        }

        public void GraphContentView() {
            // nothing do to; everything is managed by Frontend.ContentViewManager
        }

        public void GraphGraphView () {

            SplitContainer.SuspendLayout();

            Action<IDisplay, SplitterPanel> setDisplay = (display, panel) => {
                panel.SuspendLayout();
                var backend = display.Backend.ToSwf (); 

                if (!panel.Contains(backend)) {
                    panel.Controls.Clear();
                    panel.Controls.Add(backend);
                }
            };

            setDisplay(Frontend.Display1, SplitContainer.Panel1);
            setDisplay(Frontend.Display2, SplitContainer.Panel2);

            SplitContainer.Panel1.ResumeLayout();
            SplitContainer.Panel2.ResumeLayout();
            SplitContainer.ResumeLayout();

        }

        /// <summary>
        /// called in ContentViewManager when a ContentViewer is attached
        /// </summary>
        public void AttachViewer (IVidget viewer, Action onShowAction) {
            if (viewer == null)
                return;
            var backend = viewer.Backend;
            if (backend == null)
                return;

            var control = backend.ToSwf ();
            var currentDisplayBackend = this.Frontend.CurrentDisplay.ToSwf ();

            SplitterPanel panel = null;
            if (currentDisplayBackend != control) {
                if (SplitContainer.Panel1.Controls.Cast<Control>().Contains(currentDisplayBackend)) {
                    panel = SplitContainer.Panel2;
                } else if (SplitContainer.Panel2.Controls.Cast<Control>().Contains(currentDisplayBackend)) {
                    panel = SplitContainer.Panel1;
                }
            } else {
                Trace.WriteLine("SplitViewBackend.AttachBackend: currentDisplayBackend == control");
                return;
            }
            if (panel != null && !panel.Controls.Cast<Control>().Contains(control)) {
                panel.SuspendLayout();
                panel.Controls.Clear();
                panel.Controls.Add(control);
                control.Dock = DockStyle.Fill;
                panel.ResumeLayout();
            }

            if (onShowAction != null) {
                onShowAction();
            }
        }

        #endregion

        bool textDialogVisible = false;

        public void ShowTextDialog (string title, string text, Action<string> onOk) {

            if (textDialogVisible)
                return;

            var display = this.Frontend.CurrentDisplay;

            var textDialog = new TextOkCancelBox {Text = text, Title = title};

            var textDialogBackend = textDialog.Backend as TextOkCancelBoxBackend;
            textDialogBackend.Control.Dock = DockStyle.Top;

            var displayBackend = display.Backend as Control;
            if (SplitContainer.Panel1.Contains (displayBackend)) {
                SplitContainer.Panel1.Controls.Add (textDialogBackend.Control);
            } else if (SplitContainer.Panel2.Contains (displayBackend)) {
                SplitContainer.Panel2.Controls.Add (textDialogBackend.Control);
            }

            Control.ActiveControl = textDialogBackend.TextBox;

            textDialogBackend.Finish += (e) => {
                if (e == DialogResult.Ok) {
                    onOk (textDialog.Text);
                }

                textDialog.Dispose ();

                textDialogVisible = false;

                SplitContainer.ResumeLayout (true);
				display.Backend.QueueDraw ();
                // hide is changing the CurrentDisplay (whyever)
                Frontend.DisplayGotFocus (display);
            };

        }

        #region IDisposable Member

        public override void Dispose() {
            this.Frontend.Dispose ();
            base.Dispose ();
        }

        #endregion

    }
}