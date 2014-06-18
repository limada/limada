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
using System.Diagnostics;

namespace Limaki.View.GtkBackend {

    public abstract class ToolStripItemBackend<T> : VidgetBackend<T> where T:Gtk.ToolItem, new() {

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            
        }

        protected override void Compose () {
            base.Compose ();
            Widget.AddEvents ((int)Gdk.EventMask.FocusChangeMask);
            var toolItem = Widget as ToolItem;
            if (toolItem != null)
                toolItem.Click += this.OnAction;
        }

        public string ToolTipText {
            get { return Widget.TooltipText; }
            set { Widget.TooltipText = value; }
        }

        [GLib.ConnectBefore]
        protected virtual void ButtonReleased (object o, Gtk.ButtonReleaseEventArgs args) {
            Trace.WriteLine ("ButtonReleased");
        }

        public void SetLabel (string value) {
            var toolItem = Widget as ToolItem;
            if (toolItem != null)
                toolItem.Label = value;
        }

        public void SetToolTip (string value) {
            this.ToolTipText = value;
        }

        private Action<object> _action;
        public void SetAction (Action<object> value) {
            _action = value;
        }

        protected virtual void OnAction (object sender, EventArgs e) {
            if (_action != null)
                _action (this);
        }

        protected bool _composed = false;

        public virtual void SetImage (Xwt.Drawing.Image image) {
            var toolItem = Widget as ToolItem;
            if (toolItem != null) {
                toolItem.Image = image;
                if (!_composed) {
                    toolItem.Compose ();
                    _composed = true;
                }
            }
        }

        public virtual bool IsEnabled {
            get { return Widget.Sensitive; }
            set { Widget.Sensitive = value; }
        }
    }
}