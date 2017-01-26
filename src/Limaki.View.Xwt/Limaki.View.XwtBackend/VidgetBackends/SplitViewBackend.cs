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
using Limaki.View.Visuals;

namespace Limaki.View.XwtBackend {

    public class SplitViewBackend : PanedBackend, ISplitViewBackend {

        public SplitViewBackend () { }

        public new SplitView0 Frontend { get; protected set; }

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

            var widget = backend.ToXwt ();
            var panel = AdjacentPanelOf (this.Frontend.CurrentDisplay);
            if (panel != null) {
                SetScrollingPanelContent (widget, panel);
            }

            onShowAction?.Invoke ();
            
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
                // Frontend.DisplayGotFocus (display);
            };


        }

        public void ViewInWindow (IVidget vidget, Action onClose) {

            if (vidget != null) {
                var parent = SplitContainer.ParentWindow;
				var vindow = new Vindow();
				vindow.Content = vidget;
				var window = vindow.Backend as Window;

				window.Icon = Iconerias.Iconery.SubWinIcon;
				window.InitialLocation = WindowLocation.Manual;
				window.Title = Frontend.CurrentDisplay.Info.Name + " - *";
				// Decorated = false,
				window.Resizable = true;
				window.Size = new Size(parent.Size.Width / 3, parent.Size.Height / 3);
				window.TransientFor = parent;


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
        }

    }
}