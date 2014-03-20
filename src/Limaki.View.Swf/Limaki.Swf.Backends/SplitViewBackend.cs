/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 */

using Limada.View.Vidgets;
using Limaki.Usecases.Vidgets;
using Limaki.View;
using Limaki.View.Swf;
using Limaki.View.Swf.Visualizers;
using Limaki.View.Vidgets;
using Limaki.View.Viz;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Xwt.Gdi.Backend;
using DialogResult = Limaki.View.Vidgets.DialogResult;
using Point = System.Drawing.Point;

namespace Limaki.Swf.Backends.Viewers {

    public class SplitViewBackend : SplitContainer, ISplitViewBackend, IDisposable, IDragDropControl {

        public SplitViewBackend() {
            InitializeComponent();
        }

        public SplitView0 Frontend { get; protected set; }
        public void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (SplitView0) frontend;
            this.Compose();
        }

        protected SplitContainer SplitContainer { get { return this; } }


        protected void InitializeComponent () {

            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "SplitView";
            this.Panel2MinSize = 0;
            this.SplitterWidth = 3;
            this.TabIndex = 1;
        }

        protected void Compose () {

            var displayBackend1 = Frontend.Display1.Backend as Control;
            displayBackend1.Dock = DockStyle.Fill;
            SplitContainer.Panel1.Controls.Add(displayBackend1);

            var displayBackend2 = Frontend.Display2.Backend as Control;
            displayBackend2.Dock = DockStyle.Fill;
            SplitContainer.Panel2.Controls.Add(displayBackend2);

            SplitContainer.Panel1.BackColor = displayBackend1.BackColor;
            SplitContainer.Panel2.BackColor = displayBackend1.BackColor;

            // this is for mono on linux:
            ActiveControl = displayBackend1;
        }


        public void InitializeDisplay (IVidgetBackend displayBackend) {
            var backend = displayBackend as Control;
            backend.Enter -= DisplayGotFocus;
            backend.MouseUp -= DisplayGotFocus;
            backend.Enter += DisplayGotFocus;
            backend.MouseUp += DisplayGotFocus;
        }

        void DisplayGotFocus(object sender, EventArgs e) {
            var backend = sender as VisualsDisplayBackend;
            if (backend != null) {
                Frontend.DisplayGotFocus (backend.Display);
            }
        }

        void ControlGotFocus(object sender, EventArgs e) {
            Trace.WriteLine(string.Format("{0} {1}", sender.GetType().Name,sender.GetHashCode()));
            var displayBackend = sender as VisualsDisplayBackend;
            if (displayBackend != null) {
                Frontend.DisplayGotFocus(displayBackend.Display);
            } else {
                Frontend.WidgetGotFocus(sender);
            }
        }

        public void SetFocusCatcher (IVidgetBackend backend) {
            var control = backend as Control;
            if (control != null) {
                control.Enter += ControlGotFocus;
                control.MouseUp += ControlGotFocus;
                control.GotFocus += ControlGotFocus;
            }
        }

        public void ReleaseFocusCatcher (IVidgetBackend backend) {
            var control = backend as Control;
            if (control != null) {
                control.Enter -= ControlGotFocus;
                control.MouseUp -= ControlGotFocus;
                control.GotFocus -= ControlGotFocus;
            }
        }

        public void ViewInWindow (IVidgetBackend backend, Action onClose) {
            var control = backend as Control;
            if (control != null) {
                control.Dock = DockStyle.Fill;
                var form = new Form() { FormBorderStyle = FormBorderStyle.SizableToolWindow };
                form.FormClosing += (s, e) => onClose();
                form.Controls.Add (control);
                form.Show (this.ParentForm);
                form.Location = this.PointToScreen(new Point (this.Location.X, this.Location.Y+(this.Height-form.Height)/2));
                //form.Height = this.Height/2;
                Func<Point> calcOffset = () => new Point (form.Location.X - this.ParentForm.Location.X, form.Location.Y- this.ParentForm.Location.Y);
                var offset = calcOffset();
                form.LocationChanged += (s, e) =>
                    offset = calcOffset ();
                this.ParentForm.LocationChanged += (s, e) =>
                                                   form.Location = new Point (this.ParentForm.Location.X + offset.X, this.ParentForm.Location.Y + offset.Y);

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
                var backend = display.Backend as Control;

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
        public void AttachViewerBackend (IVidgetBackend backend, Action onShowAction) {
            if (backend == null)
                return;

            var control = backend as Control;
            var currentDisplayBackend = this.Frontend.CurrentDisplay.Backend as Control;

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

        void FinishTextOkCancelDialog(object sender, TextOkCancelBoxEventArgs e) {
            var cd = this.Frontend.CurrentDisplay;
            var control = (sender as TextOkCancelBoxBackend);

            if (e.Arg == DialogResult.Ok) {
                e.OnOK (control.Text);

            }
            //control.Finish -= SaveSheetDialog_Finish;
            control.Hide();
            control.Parent = null;
            control.Dispose();
            // hide is changing the CurrentDisplay (whyever)
            Frontend.DisplayGotFocus(cd);

        }

        public void ShowTextDialog(string title, string text, Action<string> onOk) {
            var nameDialog = new TextOkCancelBoxBackend();
            nameDialog.Finish += FinishTextOkCancelDialog;
            nameDialog.OnOk = onOk;

            var currentDisplay = this.Frontend.CurrentDisplay.Backend as Control;
            if (SplitContainer.Panel1.Contains(currentDisplay)) {
                SplitContainer.Panel1.Controls.Add(nameDialog);
            } else if (SplitContainer.Panel2.Contains(currentDisplay)) {
                SplitContainer.Panel2.Controls.Add(nameDialog);
            }
            nameDialog.Dock = DockStyle.Top;
            nameDialog.TextBox.Text = text;
            nameDialog.Title = title;
            this.ActiveControl = nameDialog.TextBox;

        }

        #region IVidgetBackend-Implementation

        Xwt.Size IVidgetBackend.Size {
            get { return this.Size.ToXwt(); }
        }

        void IVidgetBackend.Invalidate (Xwt.Rectangle rect) {
            this.Invalidate(rect.ToGdi());
        }

        Xwt.Point IDragDropControl.PointToClient (Xwt.Point source) { return PointToClient(source.ToGdi()).ToXwt(); }

        #endregion

        #region IDisposable Member

        public void Dispose() {
            this.Frontend.Dispose ();
        }

        #endregion

    }
}