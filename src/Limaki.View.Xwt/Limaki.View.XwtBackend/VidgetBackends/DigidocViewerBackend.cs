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

using System.Net.Mime;
using Limada.View.Vidgets;
using Limaki.View.Vidgets;
using Xwt;
using Xwt.Drawing;
using System;

namespace Limaki.View.XwtBackend {

    public class DigidocViewerBackend : PanedBackend, IDigidocViewerBackend {

        public new DigidocVidget Frontend { get; set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            this.Frontend = (DigidocVidget)frontend;
            Compose2 ();
        }

        Widget NullContentViewer = new Canvas { BackgroundColor = Colors.White };
        protected virtual void Compose2 () {

            Frontend.Compose ();

            var pagesDisplayBackend = Frontend.PagesDisplay.Backend.ToXwt();
            var width = Frontend.GetDefaultWidth ();
            pagesDisplayBackend.WidthRequest = width;

            bool suspendWidth = false;
            pagesDisplayBackend.BoundsChanged += (s, e) => {
                if (!suspendWidth) {
                    // width = pagesDisplayBackend.Size.Width;
                }
            };
            Widget.Panel2.Content = pagesDisplayBackend;
            Widget.Panel2.Resize = false;
            this.Widget.BoundsChanged += (s, e) => {
                Widget.Position = Widget.Size.Width - width;
            };
            
            Frontend.AttachContentViewer = contentViewer => {
				
                var contentWidget = contentViewer?.Backend.ToXwt () ?? NullContentViewer;

                suspendWidth = true;
                Widget.Panel1.Content = null;

                if (Widget.Position == 0 && Widget.Size.Width > pagesDisplayBackend.Size.Width)
                    Widget.Position = Widget.Size.Width - Math.Max (width, pagesDisplayBackend.Size.Width);

                contentWidget.WidthRequest = Widget.Position;
                Widget.Panel1.Content = contentWidget != NullContentViewer?contentWidget.WithScrollView ():contentWidget;
                Application.MainLoop.DispatchPendingEvents ();
                suspendWidth = false;
            };

        }

    }
}