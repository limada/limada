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
 * http://limada.sourceforge.net
 */

using System;
using System.Linq;
using System.Windows.Forms;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.View.Display;
using Limaki.View.Winform.Display;
using Limaki.Viewers;
using Limaki.Visuals;
using Limaki.Winform.Controls;
using DialogResult=Limaki.Viewers.DialogResult;

namespace Limaki.SWF.Viewers {
    public class  WinformSplitView : IDisposable {

        public WinformSplitView(Control parent) {
            this.Parent = parent;
            Initialize();
        }

        #region Parent-Handling
        
        public Control Parent = null;
        
        public Control ActiveControl {
            get { return ViewDevice.ActiveControl as Control; }
            set { ViewDevice.ActiveControl = value; }
        }
        
        #endregion

        public SplitContainer ViewDevice { get; set; }
        public SplitView0 View { get; set; }
        
        public WinformVisualsDisplay Display1 { get; set; }
        public WinformVisualsDisplay Display2 { get; set; }


        public void InitializeComponent() {
            var parent = this.Parent as Control;

            parent.SuspendLayout();
            
            if (ViewDevice == null) {
                ViewDevice = new SplitContainer();
                parent.Controls.Add(ViewDevice);
                ViewDevice.Dock = System.Windows.Forms.DockStyle.Fill;
                ViewDevice.Margin = new System.Windows.Forms.Padding(2);
                ViewDevice.Name = "SplitContainer";
                ViewDevice.Panel2MinSize = 0;
                ViewDevice.SplitterWidth = 3;
                ViewDevice.TabIndex = 1;
            }
            if (Display1 == null) {
                Display1 = new WinformVisualsDisplay();
                Display1.Dock = DockStyle.Fill;
                ViewDevice.Panel1.Controls.Add(Display1);
            }
            if (Display2 == null) {
                Display2 = new WinformVisualsDisplay();
                Display2.Dock = DockStyle.Fill;
                ViewDevice.Panel2.Controls.Add(Display2);
            }
            parent.ResumeLayout();
            parent.PerformLayout();

            ViewDevice.SplitterDistance = (int)(parent.Width / 2);
        }

        public void Initialize() {
            InitializeComponent();

            View = new SplitView0 ();
            View.Display1 = this.Display1.Display as IGraphSceneDisplay<IVisual, IVisualEdge>;
            View.Display2 = this.Display2.Display as IGraphSceneDisplay<IVisual, IVisualEdge>;
            View.Parent = this.Parent;
            View.DeviceInitializeDisplay += this.InitializeDisplay;
            

            View.DeviceGraphGraphView += this.GraphGraphView;
            //View.DeviceGraphStreamView += this.GraphStreamView;
            View.DeviceToggleView += this.ToggleView;
            
            View.FocusCatcher = this.SetFocusCatcher;
            View.ShowTextDialog = this.ShowTextOkCancelDialog;
            View.AttachControl = this.AttachControl;

            View.Initialize ();

            ViewDevice.Panel1.BackColor = Display1.BackColor;
            ViewDevice.Panel2.BackColor = Display1.BackColor;
            
            // this is for mono on linux:
            ActiveControl = Display1;
        }


        void InitializeDisplay(IGraphSceneDisplay<IVisual, IVisualEdge> target) {
            var display = target.Device as Control;
            display.Enter -= DisplayGotFocus;
            display.MouseUp -= DisplayGotFocus;
            display.Enter += DisplayGotFocus;
            display.MouseUp += DisplayGotFocus;
        }

        void DisplayGotFocus(object sender, EventArgs e) {
            var display = sender as WinformVisualsDisplay;
            if (display != null) {
                View.DisplayGotFocus (display.Display);
            }
        }

        void ControlGotFocus(object sender, EventArgs e) {
            var display = sender as WinformVisualsDisplay;
            if (display != null) sender = display.Display;

            View.ControlGotFocus (sender);
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
            var one = ViewDevice.Panel1.Controls.Cast<Control>().ToArray();
            var two = ViewDevice.Panel2.Controls.Cast<Control>().ToArray();

            ViewDevice.SuspendLayout();

            ViewDevice.Panel1.Controls.Clear();
            ViewDevice.Panel2.Controls.Clear();
            ViewDevice.Panel1.Controls.AddRange(two);
            ViewDevice.Panel2.Controls.AddRange(one);

            ViewDevice.ResumeLayout();
        }

        protected void GraphGraphView() {
            ViewDevice.SuspendLayout();

            ViewDevice.Panel1.SuspendLayout();
            var display1 = this.View.Display1.Device as Control;
            
            if (!ViewDevice.Panel1.Contains(display1)) {
                ViewDevice.Panel1.Controls.Clear();
                ViewDevice.Panel1.Controls.Add(display1);
            }

            ViewDevice.Panel2.SuspendLayout();
            var display2 = this.View.Display2.Device as Control;
            if (!ViewDevice.Panel2.Contains(display2)) {
                ViewDevice.Panel2.Controls.Clear();
                ViewDevice.Panel2.Controls.Add(display2);
            }

            ViewDevice.Panel1.ResumeLayout();
            ViewDevice.Panel2.ResumeLayout();
            ViewDevice.ResumeLayout();

        }
        
        private void AttachControl(object sender, Action onShowAction) {
            if (sender == null)
                return;

            var control = sender as Control;
            var device = sender as GraphSceneDisplay<IVisual, IVisualEdge>;
            if (device != null) {
                control = device.Device as Control;
            }
            var currentDisplay = this.View.CurrentDisplay.Device as Control;

            SplitterPanel panel = null;
            if (ViewDevice.Panel1.Controls.Contains(currentDisplay)) {
                panel = ViewDevice.Panel2;
            } else {
                panel = ViewDevice.Panel1;
            }

            if (!panel.Controls.Contains(control)) {
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

            var currentDisplay = this.View.CurrentDisplay.Device as Control;
            if (ViewDevice.Panel1.Contains(currentDisplay)) {
                ViewDevice.Panel1.Controls.Add(nameDialog);
            } else if (ViewDevice.Panel2.Contains(currentDisplay)) {
                ViewDevice.Panel2.Controls.Add(nameDialog);
            }
            nameDialog.Dock = DockStyle.Top;
            nameDialog.TextBox.Text = text;
            nameDialog.Title = title;
            this.ActiveControl = nameDialog.TextBox;

        }



        #region IDisposable Member

        public void Dispose() {
            this.View.Dispose ();
            this.ViewDevice.Dispose ();
        }

        #endregion
    }
}