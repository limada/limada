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

using Atk;
using Gtk;
using Xwt;
using Box = Gtk.Box;
using Rectangle = Gdk.Rectangle;
using Widget = Gtk.Widget;

namespace Limaki.View.GtkBackend {

    public static class GtkBackendHelper {

        public static Size VidgetBackendSize (this Widget widget) {
            var re = widget.Allocation;
            return new Size (re.Width, re.Height);
        }

        public static void VidgetBackendSize (this Widget widget, Size size) {
            var re = widget.Allocation;
            widget.Allocation = new Rectangle (re.X, re.Y, (int)size.Width, (int)size.Height);
        }

        public static void VidgetBackendUpdate (this Widget widget) {
            widget.QueueDraw ();
        }

        public static void VidgetBackendInvalidate (this Widget widget) {
            widget.QueueDraw ();
        }

        public static void VidgetBackendInvalidate (this Widget widget, Xwt.Rectangle rect) {
            widget.QueueDrawArea ((int) rect.X, (int) rect.Y, (int) rect.Width, (int) rect.Height);
        }

        public static void VidgetBackendSetFocus (this Widget widget) {
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
            public PackType PackType;
            public Widget Widget;
        }

        public static void ReorderWidgets (Box box) {

            var items = new ChildPacking[box.Children.Length];
            for (int i = 0; i < items.Length; i++) {
                bool expand, fill;
                uint padding;
                PackType packType;
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

        public static Xwt.Point ConvertToScreenCoordinates (Widget widget, Xwt.Point widgetCoordinates) {
            if (widget.ParentWindow == null)
                return Point.Zero;
            int x, y;
            widget.ParentWindow.GetOrigin (out x, out y);
            var a = widget.Allocation;
            x += a.X;
            y += a.Y;
            return new Xwt.Point (x + widgetCoordinates.X, y + widgetCoordinates.Y);
        }
    }
}