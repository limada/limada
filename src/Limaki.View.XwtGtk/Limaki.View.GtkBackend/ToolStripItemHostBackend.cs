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
using Limaki.View.Vidgets;

namespace Limaki.View.GtkBackend {

    public class ToolStripItemHostBackend : ToolItem, IToolStripItemHostBackend {

        #region IVidgetBackend Member

        public Vidgets.ToolStripItemHost Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (Vidgets.ToolStripItemHost)frontend;
        }

        public void Update () { this.VidgetBackendUpdate (); }

        public void Invalidate () { this.VidgetBackendInvalidate (); }

        public void SetFocus () { this.VidgetBackendSetFocus (); }

        public void Invalidate (Xwt.Rectangle rect) { this.VidgetBackendInvalidate (rect); }

        #endregion

        public void SetLabel (string value) {
            base.Label = value;
        }

        public void SetToolTip (string value) {
            base.TooltipText = value;
        }

        private Action<object> _action;
        public void SetAction (Action<object> value) {
            _action = value;
        }

        protected override void OnButtonClicked (object sender, EventArgs e) {
            base.OnButtonClicked (sender, e);
            if (_action != null)
                _action (this);
        }

        bool _composed = false;
        public void SetImage (Xwt.Drawing.Image image) {
            base.Image = image;
            if (!_composed) {
                Compose ();
                _composed = true;
            }
        }

        public void SetChild (Vidget value) {
            bool done = false;
            Action<Gtk.Widget> setChild = child => {
                if (child != null) {
                    if (base.Child != child) {
                        base.Remove (base.Child);
                        base.Child = child;
                    }
                }
            };
            setChild (value.Backend as Gtk.Widget);
            if (!done) {
                var xwtBackend = value.Backend as Xwt.Backends.IFrontend;
                if (xwtBackend != null) {
                    var gtkBackend = xwtBackend.Backend as Xwt.GtkBackend.WidgetBackend;
                    if (gtkBackend != null) {
                        setChild (gtkBackend.Widget);
                    }
                }
            }
        }
    }
}