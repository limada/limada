/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2015 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.IO;
using Limaki.View.Vidgets;
using Xwt;
using Xwt.Backends;

namespace Limaki.View.XwtBackend {

    public class MarkdownEditBackend : VidgetBackend<MarkdownEditBackend.MDBox>, IMarkdownEditBackend {

        public class MDBox : VBox {
            public IVidget Frontend { get; set; }
            public void RaiseFocus (object sender, EventArgs e) {
                if (Frontend != null && base.gotFocus != null) {
                    base.gotFocus (Frontend, e);
                } else {
                    base.OnGotFocus (e);
                }
            }
        }

        public new Vidgets.MarkdownEdit Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            Frontend = (Vidgets.MarkdownEdit) frontend;
            IsEmpty = true;
            //Widget.KeyPressed += ToggleEditMode;
        }

        public bool IsEmpty { get; protected set; }

        public void Compose (IMarkdownViewer viewer) {
            var vidget = viewer as IVidget;
            var viewerBackend = vidget.Backend.ToXwt ();
            Widget.CanGetFocus = true;
            Widget.Frontend = this.Frontend;
            viewerBackend.GotFocus -= Widget.RaiseFocus;
            viewerBackend.GotFocus += Widget.RaiseFocus;
            viewerBackend.KeyPressed -= ToggleEditMode;
            viewerBackend.KeyPressed += ToggleEditMode;
        }

        public void Activate (IMarkdownViewer viewer) {
            this.Widget.Clear ();
            var vidget = viewer as IVidget;
            var viewerBackend = vidget.Backend.ToXwt ();
            this.Widget.PackStart (viewerBackend, true);
            this.Widget.QueueForReallocate ();
            var wb = vidget.Backend as IWebBrowser;
            if (wb != null)
                wb.WaitLoaded ();
            viewerBackend.SetFocus ();
            IsEmpty = false;
        }

        private void ToggleEditMode (object sender, KeyEventArgs e) {
            if (e.Key == Key.F2) {
                Frontend.InEdit = !Frontend.InEdit;
            }
        }

    }

}