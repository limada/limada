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

using Gtk;
using Atk;

namespace Limaki.View.GtkBackend {
    public static class GtkBackendHelper {

        public static Xwt.Size VidgetBackendSize (this Widget widget) {
            var re = widget.Allocation;
            return new Xwt.Size (re.Width, re.Height);
        }

        public static void VidgetBackendSize (this Widget widget, Xwt.Size size) {
            var re = widget.Allocation;
            widget.Allocation = new Gdk.Rectangle (re.X, re.Y, (int)size.Width, (int)size.Height);
        }

        public static void VidgetBackendUpdate (this Gtk.Widget widget) {
            widget.QueueDraw ();
        }

        public static void VidgetBackendInvalidate (this Gtk.Widget widget) {
            widget.QueueDraw ();
        }

        public static void VidgetBackendInvalidate (this Gtk.Widget widget, Xwt.Rectangle rect) {
            widget.QueueDrawArea ((int) rect.X, (int) rect.Y, (int) rect.Width, (int) rect.Height);
        }

        public static void VidgetBackendSetFocus (this Gtk.Widget widget) {
            if (widget.Parent != null)
                VidgetBackendSetFocus (widget.Parent);
			widget.GrabFocus ();
			widget.IsFocus = true;
			widget.HasFocus = true;
        }

        public struct ChildPacking {
            public bool Expand;
            public bool Fill;
            public uint Padding;
            public Gtk.PackType PackType;
            public Gtk.Widget Widget;
        }

        public static void ReorderWidgets (Gtk.Box box) {

            var items = new ChildPacking[box.Children.Length];
            for (int i = 0; i < items.Length; i++) {
                bool expand, fill;
                uint padding;
                Gtk.PackType packType;
                box.QueryChildPacking (box.Children[i], out expand, out fill, out padding, out packType);
                items[i] = new ChildPacking { Expand = expand, Fill = fill, Padding = padding, PackType = packType, Widget = box.Children[i] };
            }

            foreach (var item in items) {
                box.Remove (item.Widget);
            }

            foreach (var item in items) {

                box.PackEnd (item.Widget);
                box.SetChildPacking (item.Widget, item.Expand, item.Fill, item.Padding, item.PackType);
            }
        }
    }
}