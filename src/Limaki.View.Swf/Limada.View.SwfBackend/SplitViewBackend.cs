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

namespace Limada.View.SwfBackend {

    public class SplitViewBackend : SplitContainer, ISplitViewBackend, IDisposable {

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
                var form = new Form {
                    FormBorderStyle = FormBorderStyle.SizableToolWindow,
                    Icon = GdiIconery.LimadaSubWinIcon,
                    Text = Frontend.CurrentDisplay.Info.Name + " - *"
                    
                };
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
        public void AttachViewer (IVidget viewer, Action onShowAction) {
            if (viewer == null)
                return;
            var backend = viewer.Backend;
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

        bool textDialogVisible = false;

        public void ShowTextDialog (string title, string text, Action<string> onOk) {

            if (textDialogVisible)
                return;

            var display = this.Frontend.CurrentDisplay;

            var textDialog = new TextOkCancelBox {Text = text, Title = title};

            var textDialogBackend = textDialog.Backend as TextOkCancelBoxBackend;
            textDialogBackend.Dock = DockStyle.Top;

            var displayBackend = display.Backend as Control;
            if (SplitContainer.Panel1.Contains (displayBackend)) {
                SplitContainer.Panel1.Controls.Add (textDialogBackend);
            } else if (SplitContainer.Panel2.Contains (displayBackend)) {
                SplitContainer.Panel2.Controls.Add (textDialogBackend);
            }

            this.ActiveControl = textDialogBackend.TextBox;

            textDialogBackend.Finish += (e) => {
                if (e == DialogResult.Ok) {
                    onOk (textDialog.Text);
                }

                textDialog.Dispose ();

                textDialogVisible = false;

                SplitContainer.ResumeLayout (true);
				display.Backend.Invalidate ();
                // hide is changing the CurrentDisplay (whyever)
                Frontend.DisplayGotFocus (display);
            };

        }

        #region IVidgetBackend-Implementation

        Xwt.Size IVidgetBackend.Size {
            get { return this.Size.ToXwt(); }
        }

        void IVidgetBackend.Invalidate (Xwt.Rectangle rect) {
            this.Invalidate(rect.ToGdi());
        }

        void IVidgetBackend.SetFocus () { this.Focus (); }

        #endregion

        #region IDisposable Member

        public void Dispose() {
            this.Frontend.Dispose ();
        }

        #endregion

    }
}