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
using Limaki.View.Vidgets;

namespace Limaki.View.GtkBackend {
    /// <summary>
    /// ToolbarDropDownButton with Popup-Window
    /// </summary>
    public class ToolbarDropDownWindowButtonBackendBase<T> : ToolbarButtonBackendBase<T>, IToolbarDropDownButtonBackend where T : Gtk.ToolItem, new () {

        public new Vidgets.ToolbarDropDownButton Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            this.Frontend = (Vidgets.ToolbarDropDownButton)frontend;
        }

        protected override Xwt.ButtonType ButtonType { get { return Xwt.ButtonType.DropDown; } }

        private IList<IToolbarItemBackend> _children = null;
        public IList<IToolbarItemBackend> Children { get { return _children ?? (_children = new List<IToolbarItemBackend> ()); } }

        public void AddItems (params IToolbarItemBackend[] children) {
            foreach (var child in children)
                Children.Add (child);
        }

        protected override void DropDownPressed (object o, Gtk.ButtonPressEventArgs e) {
            base.DropDownPressed (o, e);
            if (e.Event.Button != 1)
                return;
            if (PopupWindow == null) {
                ShowDropDown ();
            } 
        }

        protected virtual void ShowDropDown () {
            if (HasChildren) {
                PopupWindow = PopupWindow.Show (this.ContentWidget, Xwt.Rectangle.Zero, ChildBox);
				var tr = this.Size.Width - PopupWindow.SizeRequest ().Width;
				PopupWindow.Tolerance = new Xwt.WidgetSpacing (0, 0, tr, 0);
                PopupWindow.Hidden += (s, e) =>
                    HideDropDown ();
            }
        }

        protected virtual void HideDropDown () {
            if (PopupWindow == null)
                return;
            PopupWindow.Hide ();
            PopupWindow = null;
        }

        public bool HasChildren { get { return _children != null && _children.Count > 0; } }

        protected virtual PopupWindow PopupWindow { get; set; }
        Gtk.VBox _childBox = null;

        public Gtk.VBox ChildBox {
            get {
                if (_childBox == null) {
                    _childBox = new Gtk.VBox (false, 3);
                    foreach (var w in Children) {
                        _childBox.PackStart (w.ToGtk(), false, false, 3);
                        var b = w as IGtkToolbarItemBackend;
                        if (b != null) {
                            b.Clicked -= ChildClicked;
                            b.Clicked += ChildClicked;
                            b.Widget.LeaveNotifyEvent -= ChildClicked;
                            b.Widget.LeaveNotifyEvent += ChildClicked;
                            b.Widget.GrabNotify -= ChildClicked;
                            b.Widget.GrabNotify += ChildClicked;
                        }
                    }

                }
                return _childBox;
            }
        }

        protected void ChildClicked (object sender, System.EventArgs e) {
            HideDropDown ();
        }


        public void InsertItem (int index, IToolbarItemBackend backend) {
            Children.Insert (index, backend);
        }

        public void RemoveItem (IToolbarItemBackend backend) {
            Children.Remove (backend);
        }
    }

	public class ToolbarDropDownPopupButtonBackend : ToolbarDropDownWindowButtonBackendBase<Gtk.ToolItem> {
	}
}