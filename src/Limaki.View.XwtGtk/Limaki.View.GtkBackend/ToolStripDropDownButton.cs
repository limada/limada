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

using System.Collections.Generic;

namespace Limaki.View.GtkBackend {

    public class ToolStripDropDownButton : ToolStripButton {

        protected override Xwt.ButtonType ButtonType { get { return Xwt.ButtonType.DropDown; } }

        private IList<Gtk.Widget> _children = null;
        protected IList<Gtk.Widget> Children { get { return _children ?? (_children = new List<Gtk.Widget> ()); } }

        public void AddItems (params Gtk.Widget[] children) {
            foreach (var child in children)
                Children.Add (child);
        }

        protected override void DropDownPressed (object o, Gtk.ButtonPressEventArgs e) {
            base.DropDownPressed (o, e);
            if (e.Event.Button != 1)
                return;
            if (PopupWindow == null) {
                ShowDropDown ();
            } else {
                HideDropDown ();
            }
        }

        protected void ShowDropDown () {
            if (HasChildren) {
                PopupWindow = PopupWindow.Show (this.ButtonWidget, Xwt.Rectangle.Zero, ChildBox);
            }
        }

        protected void HideDropDown () {
            if (PopupWindow == null)
                return;
            PopupWindow.Hide ();
            PopupWindow = null;
        }

        public bool HasChildren { get { return _children != null && _children.Count > 0; } }

        public PopupWindow PopupWindow { get; set; }
        Gtk.VBox _childBox = null;

        public Gtk.VBox ChildBox {
            get {
                if (_childBox == null) {
                    _childBox = new Gtk.VBox (false, 3);
                    foreach (var w in Children) {
                        _childBox.PackStart (w, false, false, 3);
                        var b = w as ToolStripButton0;
                        if (b != null) {
                            b.Click -= ChildClicked;
                            b.Click += ChildClicked;
                            b.LeaveNotifyEvent += ChildClicked;
                            b.GrabNotify += ChildClicked;
                        }
                    }

                }
                return _childBox;
            }
        }

        protected void ChildClicked (object sender, System.EventArgs e) {
            HideDropDown ();
        }
    }
}