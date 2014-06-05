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

        private ICollection<Gtk.Widget> _children = null;
        protected ICollection<Gtk.Widget> Children { get { return _children ?? (_children = new List<Gtk.Widget> ()); } }

        public void AddItems (params Gtk.Widget[] children) {
            foreach (var child in children)
                Children.Add (child);
        }

        protected override void DropDownPressed (object o, Gtk.ButtonPressEventArgs e) {
            base.DropDownPressed (o, e);
            if (e.Event.Button != 1)
                return;
            if (PopupWindow == null) {
                ShowDropDown();
            } else {
                HideDropDown ();
            }
        }

        private void ShowDropDown () {
            if (HasChildren) {
                PopupWindow = PopupWindow.Show (this.ButtonWidget, Xwt.Rectangle.Zero, ChildBox);
            }
        }

        private void HideDropDown () {
            if (PopupWindow == null)
                return;
            PopupWindow.Hide ();
            PopupWindow = null;
        }

        #region prototype
        private void ShowMenu () {
            var menu = CreateMenu ();

            if (menu != null) {
                isOpen = true;
                var button = this.ButtonWidget as Gtk.Button;
                var oldRelief = Gtk.ReliefStyle.Normal;
                if (button != null) {
                    //make sure the button looks depressed
                    oldRelief = button.Relief;
                    button.Relief = Gtk.ReliefStyle.Normal;
                }
                this.ButtonWidget.State = Gtk.StateType.Active;
                //clean up after the menu's done
                menu.Hidden += (s, args) => {
                    if (button != null) {
                        button.Relief = oldRelief;
                    }
                    isOpen = false;
                    this.ButtonWidget.State = Gtk.StateType.Normal;

                    //FIXME: for some reason the menu's children don't get activated if we destroy 
                    //directly here, so use a timeout to delay it
                    GLib.Timeout.Add (100, delegate {
                        //menu.Destroy ();
                        return false;
                    });
                };
                menu.Popup (null, null, PositionFunc, 1, Gtk.Global.CurrentEventTime);
            }
        }

        void PositionFunc (Gtk.Widget mn, out int x, out int y, out bool push_in) {
            var w = (Gtk.Widget)this;
            w.GdkWindow.GetOrigin (out x, out y);
            var rect = w.Allocation;
            x += rect.X;
            y += rect.Y + rect.Height;

            //if the menu would be off the bottom of the screen, "drop" it upwards
            if (y + mn.Requisition.Height > w.Screen.Height) {
                y -= mn.Requisition.Height;
                y -= rect.Height;
            }

            //let GTK reposition the button if it still doesn't fit on the screen
            push_in = true;
        }

        private Gtk.Menu CreateMenu () {
            return null;
        }

        #endregion


        public bool HasChildren { get { return _children != null && _children.Count > 0; } }

        public PopupWindow PopupWindow { get; set; }
        Gtk.VBox _childBox = null;

        public Gtk.VBox ChildBox {
            get {
                if (_childBox == null) {
                    _childBox = new Gtk.VBox (false,3);
                    foreach (var w in Children) {
                        _childBox.PackStart (w, false, false, 3);
                        var b = w as ToolStripButton;
                        if (b != null) {
                            b.Click -= b_Click;
                            b.Click += b_Click;
                            b.LeaveNotifyEvent += b_Click;
                            b.GrabNotify += b_Click;
                        }
                    }

                }
                return _childBox;
            }
        }

        void b_Click (object sender, System.EventArgs e) {
            HideDropDown ();
        }
    }
}