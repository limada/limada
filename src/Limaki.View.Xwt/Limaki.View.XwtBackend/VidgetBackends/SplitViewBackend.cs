/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Diagnostics;
using System.Linq;
using Limaki.Viewers;
using Xwt;

namespace Limaki.View.XwtBackend {

    public class SplitViewBackend : HPaned, ISplitViewBackend {

        public SplitViewBackend () { }

        public SplitView0 Frontend { get; protected set; }
        public void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (SplitView0) frontend;
            this.Compose();
        }

        protected HPaned SplitContainer { get { return this; } }

        protected void Compose () {
            //this.PositionFraction = 50; // crashes on Wpf

            var displayBackend1 = Frontend.Display1.ScrollView();
            SplitContainer.Panel1.Content = displayBackend1;

            var displayBackend2 = Frontend.Display2.ScrollView();
            SplitContainer.Panel2.Content = displayBackend2;


        }

        public void InitializeDisplay (IVidgetBackend displayBackend) {
            var backend = (displayBackend as Widget).ScrollViewContent();

            backend.GotFocus -= DisplayGotFocus;
            backend.ButtonReleased -= DisplayGotFocus;
            backend.GotFocus += DisplayGotFocus;
            backend.ButtonReleased += DisplayGotFocus;
        }

        protected void DisplayGotFocus (object sender, EventArgs e) {
            sender = (sender as Widget).ScrollViewContent();
            var backend = sender as VisualsDisplayBackend;
            if (backend != null) {
                Frontend.DisplayGotFocus(backend.Display);
            }
        }

        protected void ControlGotFocus (object sender, EventArgs e) {
            Trace.WriteLine(string.Format("{0} {1}", sender.GetType().Name, sender.GetHashCode()));
            var displayBackend = (sender as Widget).ScrollViewContent() as VisualsDisplayBackend;
            if (displayBackend != null) {
                Frontend.DisplayGotFocus(displayBackend.Display);
            } else {
                Frontend.WidgetGotFocus(sender);
            }
        }

        public void SetFocusCatcher (IVidgetBackend backend) {
            var widget = (backend as Widget).ScrollViewContent();
            if (widget != null) {
                widget.MouseEntered += ControlGotFocus;
                widget.ButtonReleased += ControlGotFocus;
                widget.GotFocus += ControlGotFocus;
            }
        }

        public void ReleaseFocusCatcher (IVidgetBackend backend) {
            var widget = (backend as Widget).ScrollViewContent();
            if (widget != null) {
                widget.MouseEntered -= ControlGotFocus;
                widget.ButtonReleased -= ControlGotFocus;
                widget.GotFocus -= ControlGotFocus;
            }
        }

        public void GraphGraphView () {

            Action<IDisplay, Panel> setDisplay = (display, panel) => {
                var backend = display.Backend as Widget;
                if (backend.Parent is ScrollView)
                    backend = backend.Parent;
                if (!panel.Content.Children().Contains(backend)) {
                    panel.Content = null;
                    panel.Content = backend;
                }
            };
            setDisplay(Frontend.Display1, SplitContainer.Panel1);
            setDisplay(Frontend.Display2, SplitContainer.Panel2);


            SplitContainer.QueueForReallocate();
        }

        public void GraphContentView () {
            // nothing do to; everything is managed by Frontend.ContentViewManager  
        }

        public void ToggleView () {
            var one = SplitContainer.Panel1.Content;
            var two = SplitContainer.Panel2.Content;


            SplitContainer.Panel1.Content = null;
            SplitContainer.Panel2.Content = null;
            SplitContainer.Panel1.Content = two;
            SplitContainer.Panel2.Content = one;

            SplitContainer.QueueForReallocate();
        }


        public void AttachViewerBackend (IVidgetBackend backend, Action onShowAction) {
            if (backend == null)
                return;

            var control = backend as Widget;
            var currentDisplayBackend = this.Frontend.CurrentDisplay.Backend as Widget;

            Panel panel = null;
            if (currentDisplayBackend != control) {
                if (SplitContainer.Panel1.Content.Children().Contains(currentDisplayBackend)) {
                    panel = SplitContainer.Panel2;
                } else if (SplitContainer.Panel2.Content.Children().Contains(currentDisplayBackend)) {
                    panel = SplitContainer.Panel1;
                }
            } else {
                Trace.WriteLine("SplitViewBackend.AttachBackend: currentDisplayBackend == control");
            }
            if (panel != null && !panel.Content.Children().Contains(control)) {
                panel.Content = null;
                panel.Content = control;
                control.VerticalPlacement = WidgetPlacement.Fill;
                control.HorizontalPlacement = WidgetPlacement.Fill;
            }

            SplitContainer.QueueForReallocate();

            if (onShowAction != null) {
                onShowAction();
            }
        }

        public void ShowTextDialog (string title, string text, System.Action<string> onOk) {
            var nameDialog = new TextOkCancelBoxBackend();
        }


        void IVidgetBackend.Update () { XwtBackendHelper.VidgetBackendUpdate(this); }

        void IVidgetBackend.Invalidate () { XwtBackendHelper.VidgetBackendInvalidate(this); }

        void IVidgetBackend.Invalidate (Rectangle rect) { XwtBackendHelper.VidgetBackendInvalidate(this, rect); }

        public void Dispose () {

        }
    }
}