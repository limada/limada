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
using Limada.View.Vidgets;
using Limaki.Common;
using Limaki.Usecases.Vidgets;
using Limaki.View.Vidgets;
using Limaki.View.Viz;
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

        protected virtual Widget SetScrollingPanelContent (Widget widget, Panel panel) {
            var panelScroll = panel.Content as ScrollView;
            if (panelScroll != null) {
                panelScroll.Content = widget;
            } else {
                panel.Content = widget.WithScrollView();
            }
            return panel.Content;
        }

        protected void Compose () {
            //this.PositionFraction = 50; // crashes on Wpf

            SetScrollingPanelContent(Frontend.Display1.Backend as Widget, SplitContainer.Panel1);
            SetScrollingPanelContent(Frontend.Display2.Backend as Widget, SplitContainer.Panel2);

        }

        public void InitializeDisplay (IVidgetBackend displayBackend) {
            var backend = (displayBackend as Widget).PeeledScrollView();

            backend.GotFocus -= DisplayGotFocus;
            backend.ButtonReleased -= DisplayGotFocus;
            backend.GotFocus += DisplayGotFocus;
            backend.ButtonReleased += DisplayGotFocus;
        }

        protected void DisplayGotFocus (object sender, EventArgs e) {
            sender = (sender as Widget).PeeledScrollView();
            var backend = sender as VisualsDisplayBackend;
            if (backend != null) {
                Frontend.DisplayGotFocus(backend.Display);
            }
        }

        protected void ControlGotFocus (object sender, EventArgs e) {
            Trace.WriteLine(string.Format("{0} {1}", sender.GetType().Name, sender.GetHashCode()));
            var displayBackend = (sender as Widget).PeeledScrollView() as VisualsDisplayBackend;
            if (displayBackend != null) {
                Frontend.DisplayGotFocus(displayBackend.Display);
            } else {
                Frontend.WidgetGotFocus(sender);
            }
        }

        public void SetFocusCatcher (IVidgetBackend backend) {
            var widget = (backend as Widget).PeeledScrollView();
            if (widget != null) {
                //widget.MouseEntered += ControlGotFocus;
                widget.ButtonPressed += ControlGotFocus;
                widget.GotFocus += ControlGotFocus;
            }
        }

        public void ReleaseFocusCatcher (IVidgetBackend backend) {
            var widget = (backend as Widget).PeeledScrollView();
            if (widget != null) {
                //widget.MouseEntered -= ControlGotFocus;
                widget.ButtonReleased -= ControlGotFocus;
                widget.GotFocus -= ControlGotFocus;
            }
        }

        public void GraphGraphView () {

            Action<IDisplay, Panel> setDisplay = (display, panel) => {
                var displayWidget = display.Backend as Widget;
                var backend = displayWidget.PeeledScrollView();
                if (!panel.Content.ScrollPeeledChildren().Contains(backend)) {
                    SetScrollingPanelContent(backend, panel);
                }
            };
            setDisplay(Frontend.Display1, SplitContainer.Panel1);
            setDisplay(Frontend.Display2, SplitContainer.Panel2);
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

        }

        protected Panel AdjacentPanelOf (IVidget vidget) {
            var widget = vidget.Backend as Widget;

            if (SplitContainer.Panel1.Content.ScrollPeeledChildren ().Contains (widget)) {
                return SplitContainer.Panel2;
            } else if (SplitContainer.Panel2.Content.ScrollPeeledChildren ().Contains (widget)) {
                return SplitContainer.Panel1;
            }
            return null;
        }

        public void AttachViewerBackend (IVidgetBackend backend, Action onShowAction) {
            if (backend == null)
                return;

            if (backend == this.Frontend.CurrentDisplay.Backend) {
                Trace.WriteLine ("SplitViewBackend.AttachBackend: CurrentDisplay.Backend == backend");
                return;
            }

            var widget = backend as Widget;
            var panel = AdjacentPanelOf (this.Frontend.CurrentDisplay);
            if (panel != null && !panel.Content.ScrollPeeledChildren ().Contains (widget)) {
                SetScrollingPanelContent (widget, panel);
            }

            if (onShowAction != null) {
                onShowAction ();
            }
        }

        protected Panel PanelOf (IVidget vidget) {
            var widget = vidget.Backend as Widget;

            if (SplitContainer.Panel1.Content.ScrollPeeledChildren ().Contains (widget)) {
                return SplitContainer.Panel1;
            } else if (SplitContainer.Panel2.Content.ScrollPeeledChildren ().Contains (widget)) {
                return SplitContainer.Panel2;
            }
            return null;
        }

        bool textDialogVisible = false;
        public void ShowTextDialog (string title, string text, Action<string> onOk) {

            if (textDialogVisible)
                return;

            var display = this.Frontend.CurrentDisplay;

            var textDialog = new TextOkCancelBox {Text = text, Title = title};
            var textDialogBackend = textDialog.Backend as TextOkCancelBoxBackend;
            textDialogBackend.HorizontalPlacement = WidgetPlacement.Fill;
            textDialogBackend.VerticalPlacement = WidgetPlacement.Start;

            var panel = PanelOf (this.Frontend.CurrentDisplay);
            var c = panel.Content;
            var box = new VBox {HorizontalPlacement = WidgetPlacement.Fill, VerticalPlacement = WidgetPlacement.Fill};
            panel.Content = box;

            box.PackStart (textDialogBackend);
            box.PackEnd (c, true);

            box.SetFocus ();
            textDialogBackend.SetFocus ();

            textDialogVisible = true;

            textDialogBackend.Finish = (e) => {
                if (e == DialogResult.Ok) {
                    onOk (textDialog.Text);
                }

                box.Remove (c);
                box.Remove (textDialogBackend);

                panel.Content = c;

                box.Dispose ();
                textDialog.Dispose ();

                textDialogVisible = false;
                // hide is changing the CurrentDisplay (whyever)
                Frontend.DisplayGotFocus (display);
            };


        }

        public void ViewInWindow (IVidgetBackend backend, Action onClose) {
            Registry.Pooled<IExceptionHandler> ().Catch (new NotImplementedException (), MessageType.OK);
        }

        void IVidgetBackend.Update () { this.VidgetBackendUpdate(); }

        void IVidgetBackend.Invalidate () { this.VidgetBackendInvalidate(); }

        void IVidgetBackend.Invalidate (Rectangle rect) { this.VidgetBackendInvalidate(rect); }

        public void Dispose () { }


    }
}