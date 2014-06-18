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
using Xwt.GtkBackend;

namespace Limaki.View.GtkBackend {

    public class ToolItem : Gtk.ToolItem {

        public ToolItem (): base () {
            Compose ();
        }

        internal virtual void Compose () {

        }

        public string Label { get; set; }

        protected Xwt.Drawing.Image _image = null;
        public virtual Xwt.Drawing.Image Image {
            get { return _image; }
            set {
                if (_image != value) {
                    _image = value;
                }
            }
        }

        [GLib.ConnectBefore]
        protected virtual void ButtonPressed (object o, Gtk.ButtonPressEventArgs args) {
            Trace.WriteLine ("ButtonPressed");
            OnClick (this, new EventArgs ());
        }

        protected event System.EventHandler _click;
        public virtual event System.EventHandler Click {
            add { _click += value; }
            remove { _click -= value; }
        }

        internal virtual void OnClick (object sender, EventArgs e) {
            if (_click != null)
                _click (this, e);
        }

        public Gtk.Widget AllocEventBox (Gtk.Widget widget, bool visibleWindow = false) {
            // Wraps the widget with an event box. Required for some
            // widgets such as Label which doesn't have its own gdk window

            if (widget is Gtk.EventBox) {
                ((Gtk.EventBox)widget).VisibleWindow = true;
                return widget;
            }

            if (widget.IsNoWindow) {

                var eventBox = new Gtk.EventBox ();
                eventBox.Visible = widget.Visible;
                eventBox.Sensitive = widget.Sensitive;
                eventBox.VisibleWindow = visibleWindow;
                GtkEngine.ReplaceChild (widget, eventBox);
                eventBox.Add (widget);
                return eventBox;
            }
            return widget;
        }

        
    }
}