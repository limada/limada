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

using System;
using System.Linq;
using System.Windows.Forms;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.View.Visualizers;
using Limaki.View.Swf.Visualizers;
using Limaki.Viewers;
using Limaki.Visuals;
using Limaki.Swf.Backends;
using DialogResult=Limaki.Viewers.DialogResult;
using System.Diagnostics;
using Limaki.View;

namespace Limaki.Swf.Backends.Viewers {

    public class  SplitViewBackend : IDisposable, ISplitViewBackend {

        public SplitViewBackend(Control parent) {
            this.Parent = parent;
            Initialize();
        }

        #region Parent-Handling

        object ISplitViewBackend.Parent { get { return this.Parent; } }

        protected Control Parent { get; set; }

        protected Control ActiveControl {
            get { return Container.ActiveControl as Control; }
            set { Container.ActiveControl = value; }
        }
        
        #endregion
       
        public SplitView0 Frontend { get; set; }

        protected SplitContainer Container { get; set; }
        public VisualsDisplayBackend DisplayBackend1 { get; set; }
        protected VisualsDisplayBackend DisplayBackend2 { get; set; }


        protected void InitializeComponent () {
            var parent = this.Parent as Control;

            parent.SuspendLayout();
            
            if (Container == null) {
                Container = new SplitContainer();
                parent.Controls.Add(Container);
                Container.Dock = System.Windows.Forms.DockStyle.Fill;
                Container.Margin = new System.Windows.Forms.Padding(2);
                Container.Name = "SplitContainer";
                Container.Panel2MinSize = 0;
                Container.SplitterWidth = 3;
                Container.TabIndex = 1;
            }
            if (DisplayBackend1 == null) {
                DisplayBackend1 = new VisualsDisplayBackend();
                DisplayBackend1.Dock = DockStyle.Fill;
                Container.Panel1.Controls.Add(DisplayBackend1);
            }
            if (DisplayBackend2 == null) {
                DisplayBackend2 = new VisualsDisplayBackend();
                DisplayBackend2.Dock = DockStyle.Fill;
                Container.Panel2.Controls.Add(DisplayBackend2);
            }
            parent.ResumeLayout();
            parent.PerformLayout();

            Container.SplitterDistance = (int)(parent.Width / 2);
        }

        protected void Initialize () {
            InitializeComponent();

            Frontend = new SplitView0 ();
            Frontend.Backend = this;

            Frontend.Display1 = this.DisplayBackend1.Display as IGraphSceneDisplay<IVisual, IVisualEdge>;
            Frontend.Display2 = this.DisplayBackend2.Display as IGraphSceneDisplay<IVisual, IVisualEdge>;

            Frontend.Initialize ();

            Container.Panel1.BackColor = DisplayBackend1.BackColor;
            Container.Panel2.BackColor = DisplayBackend1.BackColor;
            
            // this is for mono on linux:
            ActiveControl = DisplayBackend1;
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
        #region View-Switching
        
        public void ToggleView() {
            var one = Container.Panel1.Controls.Cast<Control>().ToArray();
            var two = Container.Panel2.Controls.Cast<Control>().ToArray();

            Container.SuspendLayout();

            Container.Panel1.Controls.Clear();
            Container.Panel2.Controls.Clear();
            Container.Panel1.Controls.AddRange(two);
            Container.Panel2.Controls.AddRange(one);

            Container.ResumeLayout();
        }

        public void GraphContentView() {
            // nothing do to; everything is managed by Frontend.ContentViewManager
        }

        public void GraphGraphView () {
            Container.SuspendLayout();

            Container.Panel1.SuspendLayout();
            var display1 = this.Frontend.Display1.Backend as Control;
            
            if (!Container.Panel1.Contains(display1)) {
                Container.Panel1.Controls.Clear();
                Container.Panel1.Controls.Add(display1);
            }

            Container.Panel2.SuspendLayout();
            var display2 = this.Frontend.Display2.Backend as Control;
            if (!Container.Panel2.Contains(display2)) {
                Container.Panel2.Controls.Clear();
                Container.Panel2.Controls.Add(display2);
            }

            Container.Panel1.ResumeLayout();
            Container.Panel2.ResumeLayout();
            Container.ResumeLayout();

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
                if (Container.Panel1.Controls.Cast<Control>().Contains(currentDisplayBackend)) {
                    panel = Container.Panel2;
                } else if (Container.Panel2.Controls.Cast<Control>().Contains(currentDisplayBackend)) {
                    panel = Container.Panel1;
                }
            } else {
                Trace.WriteLine("SplitViewBackend.AttachBackend: currentDisplayBackend == control");
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

            if (e.Arg == DialogResult.OK) {
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
            if (Container.Panel1.Contains(currentDisplay)) {
                Container.Panel1.Controls.Add(nameDialog);
            } else if (Container.Panel2.Contains(currentDisplay)) {
                Container.Panel2.Controls.Add(nameDialog);
            }
            nameDialog.Dock = DockStyle.Top;
            nameDialog.TextBox.Text = text;
            nameDialog.Title = title;
            this.ActiveControl = nameDialog.TextBox;

        }

        #region IDisposable Member

        public void Dispose() {
            this.Frontend.Dispose ();
            this.Container.Dispose ();
        }

        #endregion
    }
}