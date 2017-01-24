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
using Xwt.GtkBackend;

namespace Limaki.View.GtkBackend {

    public abstract class ToolbarItemBackend<T> : VidgetBackend<T>, IGtkToolbarItemBackend where T : Gtk.ToolItem, new ()  {

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
        }

        protected override void Compose () {
            base.Compose ();
            Widget.AddEvents ((int)Gdk.EventMask.FocusChangeMask);
            Clicked -= this.OnAction;
            Clicked += this.OnAction;
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

        protected event EventHandler _click;
        public virtual event EventHandler Clicked {
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

        [GLib.ConnectBefore]
        protected virtual void ButtonReleased (object o, Gtk.ButtonReleaseEventArgs args) {
            Trace.WriteLine ("ButtonReleased");
        }

        public void SetLabel (string value) {
            if (Label != null)
                Label = value;
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
            Image = image;
            if (!_composed) {
                Compose ();
                _composed = true;
            }
        }
        
        public virtual bool IsEnabled {
            get { return Widget.Sensitive; }
            set { Widget.Sensitive = value; }
        }

        Gtk.ToolItem IGtkToolbarItemBackend.Widget {
            get { return this.Widget; }
        }

        void IGtkToolbarItemBackend.Click (object sender, EventArgs e) {
            this.OnAction (sender, e);
        }
    }

    public interface IGtkToolbarItemBackend {

        event EventHandler Clicked;
        Gtk.ToolItem Widget { get; }

        void Click (object sender, EventArgs e);
    }
}