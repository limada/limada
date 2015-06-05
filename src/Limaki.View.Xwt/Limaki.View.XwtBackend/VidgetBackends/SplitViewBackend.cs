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
using System.Collections.Generic;

namespace Limaki.View.XwtBackend {

    public class SplitViewBackend : PanedBackend, ISplitViewBackend {

        public SplitViewBackend () { }

        public SplitView0 Frontend { get; protected set; }
        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            this.Frontend = (SplitView0) frontend;
            this.Compose2();
        }

        protected void Compose2 () {
            //this.PositionFraction = 50; // crashes on Wpf

            SetScrollingPanelContent(Frontend.Display1.Backend.ToXwt(), SplitContainer.Panel1);
            SetScrollingPanelContent(Frontend.Display2.Backend.ToXwt(), SplitContainer.Panel2);

        }

        public void InitializeDisplay (IVidgetBackend displayBackend) {
            var backend = displayBackend.ToXwt ().PeeledScrollView ();

            backend.GotFocus -= DisplayGotFocus;
            backend.ButtonReleased -= DisplayGotFocus;
            backend.GotFocus += DisplayGotFocus;
            backend.ButtonReleased += DisplayGotFocus;
        }

        protected void DisplayGotFocus (object sender, EventArgs e) {
            sender = (sender as IVidgetBackend).ToXwt().PeeledScrollView ();
            var backend = sender as VisualsDisplayBackend;
            if (backend != null) {
                Frontend.DisplayGotFocus(backend.Display);
            }
        }

        object FocusDone = null;
        protected void WidgetGotFocus (object sender, EventArgs e) {
            if (FocusDone == sender)
                return;
            FocusDone = sender;
            Trace.WriteLine (string.Format ("{0} {1}", sender.GetType ().Name, sender.GetHashCode ()));
            var widget = sender as Widget;
            var displayBackend = (sender as IVidgetBackend).ToXwt ().PeeledScrollView () as VisualsDisplayBackend;
            if (displayBackend != null) {
                Frontend.DisplayGotFocus (displayBackend.Display);
            } else if (widget != null) {
                IVidget vidget = null;
                if (_vidgets.TryGetValue (widget, out vidget)) {
                    if (vidget != null)
                        Frontend.VidgetGotFocus (vidget);
                    else
                        Trace.WriteLine ("\tSplitviewBackend error: frontend is null!");
                } else {
                    Trace.WriteLine ("\tSplitviewBackend error: frontend not registered!");
                }
            }
        }

        protected Dictionary<Widget, IVidget> _vidgets = new Dictionary<Widget, IVidget> ();
        public void SetFocusCatcher (IVidgetBackend backend) {
            var widget = (backend.ToXwt()).PeeledScrollView();
            if (widget != null) {
                _vidgets[widget] = backend.Frontend;
                //widget.MouseEntered += WidgetGotFocus;
                widget.ButtonPressed -= WidgetGotFocus;
                widget.GotFocus -= WidgetGotFocus;
                widget.ButtonPressed += WidgetGotFocus;
                widget.GotFocus += WidgetGotFocus;
            }
        }

        public void ReleaseFocusCatcher (IVidgetBackend backend) {
            var widget = (backend.ToXwt()).PeeledScrollView();
            if (widget != null) {
                //widget.MouseEntered -= WidgetGotFocus;
                widget.ButtonPressed -= WidgetGotFocus;
                widget.GotFocus -= WidgetGotFocus;
                _vidgets.Remove (widget);
            }
        }

        public void GraphGraphView () {

            Action<IDisplay, Panel> setDisplay = (display, panel) => {
                var displayWidget = display.Backend.ToXwt();
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

        public void AttachViewer (IVidget viewer, Action onShowAction) {
            if (viewer == null)
                return;
            var backend = viewer.Backend;
            if (backend == null)
                return;

            if (backend == this.Frontend.CurrentDisplay.Backend) {
                Trace.WriteLine ("SplitViewBackend.AttachBackend: CurrentDisplay.Backend == backend");
                return;
            }

            var widget = backend.ToXwt();
            var panel = AdjacentPanelOf (this.Frontend.CurrentDisplay);
            if (panel != null) {
                SetScrollingPanelContent (widget, panel);
            }

            if (onShowAction != null) {
                onShowAction ();
            }
        }


        bool textDialogVisible = false;
        public void ShowTextDialog (string title, string text, Action<string> onOk) {

            if (textDialogVisible)
                return;

            var display = this.Frontend.CurrentDisplay;

            var textDialog = new TextOkCancelBox {Text = text, Title = title};
            var textDialogBackend = textDialog.Backend as TextOkCancelBoxBackend;
            textDialogBackend.Widget.HorizontalPlacement = WidgetPlacement.Fill;
            textDialogBackend.Widget.VerticalPlacement = WidgetPlacement.Start;

            var panel = PanelOf (this.Frontend.CurrentDisplay);
            var c = panel.Content;
            var box = new VBox {
                HorizontalPlacement = WidgetPlacement.Fill, 
                VerticalPlacement = WidgetPlacement.Fill,
                Spacing = 2
            };
            panel.Content = box;

            box.PackStart (textDialogBackend.Widget);
            box.PackEnd (c, true);

            box.SetFocus ();
            textDialogBackend.SetFocus ();

            textDialogVisible = true;

            textDialogBackend.Finish = (e) => {
                if (e == DialogResult.Ok) {
                    onOk (textDialog.Text);
                }

                box.Remove (c);
                box.Remove (textDialogBackend.Widget);

                panel.Content = c;

                box.Dispose ();
                textDialog.Dispose ();

                textDialogVisible = false;
                // hide is changing the CurrentDisplay (whyever)
                Frontend.DisplayGotFocus (display);
            };


        }

        public void ViewInWindow (IVidgetBackend backend, Action onClose) {
            var widget = backend as Widget;
            if (widget != null) {
                var parent = SplitContainer.ParentWindow;
                var window = new Window {
                    Icon = Iconerias.Iconery.SubWinIcon,
                    InitialLocation = WindowLocation.Manual,
                    Title = Frontend.CurrentDisplay.Info.Name + " - *",
                    // Decorated = false,
                    Resizable = true,
                    Size = new Size (parent.Size.Width / 3, parent.Size.Height / 3),
                    TransientFor = parent,
                };
                window.Content = widget;
                window.Location = 
                    new Point (this.SplitContainer.ScreenBounds.X, this.SplitContainer.ScreenBounds.Y + (this.Size.Height - window.Height) / 2);

                Func<Point> calcOffset = () => 
                    new Point (window.Location.X - parent.Location.X, window.Location.Y - parent.Location.Y);
                var offset = calcOffset ();
                var parentMoving = true;
                window.BoundsChanged += (s, e) => {
                    if (!parentMoving)
                        offset = calcOffset ();
                };
                parent.BoundsChanged += (s, e) => {
                    parentMoving = true;
                    window.Location = new Point (parent.Location.X + offset.X, parent.Location.Y + offset.Y);
                    parentMoving = false;
                };
                window.Show ();
                parentMoving = false;

            }
           // Registry.Pooled<IExceptionHandler> ().Catch (new NotImplementedException (), MessageType.OK);
        }

    }
}