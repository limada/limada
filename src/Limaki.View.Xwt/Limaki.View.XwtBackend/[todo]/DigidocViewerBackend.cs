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

using Limada.View.Vidgets;
using Limaki.View.Vidgets;
using Xwt;

namespace Limaki.View.XwtBackend {

    public class DigidocViewerBackend : DummyBackend, IDigidocViewerBackend {

        public DigidocVidget Frontend { get; set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (DigidocVidget)frontend;
            Compose ();
        }

        private void Compose () {

            var hPaned = new HPaned();

            var pagesDisplayBackend = Frontend.PagesDisplay.Backend.ToXwt();

            pagesDisplayBackend.WidthRequest = Frontend.GetDefaultWidth ();
            hPaned.Panel2.Content = pagesDisplayBackend;

            Frontend.Compose ();

            Frontend.AttachContentViewer = contentViewer => {
                var contentControl = (contentViewer.Backend.ToXwt ());

                if (hPaned.Panel1.Content != null) {
                    hPaned.Panel1.Content = null;
                    hPaned.Panel1.Content = contentControl;
                    Application.MainLoop.DispatchPendingEvents ();
                }
            };

        }
    }
}