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

namespace Limaki.Swf.Backends.Viewers {
    public class  SwfSplitView : IDisposable {

        public SwfSplitView(Control parent) {
            this.Parent = parent;
            Initialize();
        }

        #region Parent-Handling
        
        public Control Parent = null;
        
        public Control ActiveControl {
            get { return ViewBackend.ActiveControl as Control; }
            set { ViewBackend.ActiveControl = value; }
        }
        
        #endregion

        public SplitContainer ViewBackend { get; set; }
        public SplitView0 View { get; set; }
        
        public SwfVisualsDisplayBackend Display1 { get; set; }
        public SwfVisualsDisplayBackend Display2 { get; set; }


        public void InitializeComponent() {
            var parent = this.Parent as Control;

            parent.SuspendLayout();
            
            if (ViewBackend == null) {
                ViewBackend = new SplitContainer();
                parent.Controls.Add(ViewBackend);
                ViewBackend.Dock = System.Windows.Forms.DockStyle.Fill;
                ViewBackend.Margin = new System.Windows.Forms.Padding(2);
                ViewBackend.Name = "SplitContainer";
                ViewBackend.Panel2MinSize = 0;
                ViewBackend.SplitterWidth = 3;
                ViewBackend.TabIndex = 1;
            }
            if (Display1 == null) {
                Display1 = new SwfVisualsDisplayBackend();
                Display1.Dock = DockStyle.Fill;
                ViewBackend.Panel1.Controls.Add(Display1);
            }
            if (Display2 == null) {
                Display2 = new SwfVisualsDisplayBackend();
                Display2.Dock = DockStyle.Fill;
                ViewBackend.Panel2.Controls.Add(Display2);
            }
            parent.ResumeLayout();
            parent.PerformLayout();

            ViewBackend.SplitterDistance = (int)(parent.Width / 2);
        }

        public void Initialize() {
            InitializeComponent();

            View = new SplitView0 ();
            View.Display1 = this.Display1.Display as IGraphSceneDisplay<IVisual, IVisualEdge>;
            View.Display2 = this.Display2.Display as IGraphSceneDisplay<IVisual, IVisualEdge>;
            View.Parent = this.Parent;
            View.BackendInitializeDisplay += this.InitializeDisplay;
            

            View.BackendGraphGraphView += this.GraphGraphView;
            //View.DeviceGraphStreamView += this.GraphStreamView;
            View.BackendToggleView += this.ToggleView;
            
            View.FocusCatcher = this.SetFocusCatcher;
            View.ShowTextDialog = this.ShowTextOkCancelDialog;
            View.AttachControl = this.AttachControl;

            View.Initialize ();

            ViewBackend.Panel1.BackColor = Display1.BackColor;
            ViewBackend.Panel2.BackColor = Display1.BackColor;
            
            // this is for mono on linux:
            ActiveControl = Display1;
        }


        void InitializeDisplay(IGraphSceneDisplay<IVisual, IVisualEdge> target) {
            var display = target.Backend as Control;
            display.Enter -= DisplayGotFocus;
            display.MouseUp -= DisplayGotFocus;
            display.Enter += DisplayGotFocus;
            display.MouseUp += DisplayGotFocus;
        }

        void DisplayGotFocus(object sender, EventArgs e) {
            var display = sender as SwfVisualsDisplayBackend;
            if (display != null) {
                View.DisplayGotFocus (display.Display);
            }
        }

        void ControlGotFocus(object sender, EventArgs e) {
            var display = sender as SwfVisualsDisplayBackend;
            if (display != null) sender = display.Display;

            View.WidgetGotFocus (sender);
        }

        void SetFocusCatcher(object target) {
            var control = target as Control;
            if (control != null) {
                control.Enter += ControlGotFocus;
                control.MouseUp += ControlGotFocus;
                control.GotFocus += ControlGotFocus;
            }
        }
        
        #region View-Switching
        
        public void ToggleView() {
            var one = ViewBackend.Panel1.Controls.Cast<Control>().ToArray();
            var two = ViewBackend.Panel2.Controls.Cast<Control>().ToArray();

            ViewBackend.SuspendLayout();

            ViewBackend.Panel1.Controls.Clear();
            ViewBackend.Panel2.Controls.Clear();
            ViewBackend.Panel1.Controls.AddRange(two);
            ViewBackend.Panel2.Controls.AddRange(one);

            ViewBackend.ResumeLayout();
        }

        protected void GraphGraphView() {
            ViewBackend.SuspendLayout();

            ViewBackend.Panel1.SuspendLayout();
            var display1 = this.View.Display1.Backend as Control;
            
            if (!ViewBackend.Panel1.Contains(display1)) {
                ViewBackend.Panel1.Controls.Clear();
                ViewBackend.Panel1.Controls.Add(display1);
            }

            ViewBackend.Panel2.SuspendLayout();
            var display2 = this.View.Display2.Backend as Control;
            if (!ViewBackend.Panel2.Contains(display2)) {
                ViewBackend.Panel2.Controls.Clear();
                ViewBackend.Panel2.Controls.Add(display2);
            }

            ViewBackend.Panel1.ResumeLayout();
            ViewBackend.Panel2.ResumeLayout();
            ViewBackend.ResumeLayout();

        }
        
        private void AttachControl(object sender, Action onShowAction) {
            if (sender == null)
                return;

            var control = sender as Control;
            var display = sender as GraphSceneDisplay<IVisual, IVisualEdge>;
            if (display != null) {
                control = display.Backend as Control;
            }
            var currentDisplay = this.View.CurrentDisplay.Backend as Control;

            SplitterPanel panel = null;
            if (currentDisplay != control) {
                if (ViewBackend.Panel1.Controls.Cast<Control>().Contains(currentDisplay)) {
                    panel = ViewBackend.Panel2;
                } else if (ViewBackend.Panel2.Controls.Cast<Control>().Contains(currentDisplay)) {
                    panel = ViewBackend.Panel1;
                }
            } else {
                Trace.WriteLine("SplitView.AttachControl: currentDisplay == control");
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
            var cd = this.View.CurrentDisplay;
            var control = (sender as TextOkCancelBox);

            if (e.Arg == DialogResult.OK) {
                e.OnOK (control.Text);

            }
            //control.Finish -= SaveSheetDialog_Finish;
            control.Hide();
            control.Parent = null;
            control.Dispose();
            // hide is changing the CurrentDisplay (whyever)
            View.DisplayGotFocus(cd);

        }

        protected void ShowTextOkCancelDialog(string title, string text, Action<string> OnOk) {
            var nameDialog = new TextOkCancelBox();
            nameDialog.Finish += FinishTextOkCancelDialog;
            nameDialog.OnOk = OnOk;

            var currentDisplay = this.View.CurrentDisplay.Backend as Control;
            if (ViewBackend.Panel1.Contains(currentDisplay)) {
                ViewBackend.Panel1.Controls.Add(nameDialog);
            } else if (ViewBackend.Panel2.Contains(currentDisplay)) {
                ViewBackend.Panel2.Controls.Add(nameDialog);
            }
            nameDialog.Dock = DockStyle.Top;
            nameDialog.TextBox.Text = text;
            nameDialog.Title = title;
            this.ActiveControl = nameDialog.TextBox;

        }

        #region IDisposable Member

        public void Dispose() {
            this.View.Dispose ();
            this.ViewBackend.Dispose ();
        }

        #endregion
    }
}