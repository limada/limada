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
using Limaki.View.XwtBackend;
using Xwt;
using Box = Gtk.Box;
using Rectangle = Gdk.Rectangle;
using Widget = Gtk.Widget;
using LVV = Limaki.View.Vidgets;
using Xwt.GtkBackend;

namespace Limaki.View.GtkBackend {

    public static class GtkBackendHelper {

        public static Size VidgetBackendSize (this Widget widget) {
            var re = widget.Allocation;
            return new Size (re.Width, re.Height);
        }

        public static void VidgetBackendSize (this Widget widget, Size size) {
            var allocation = widget.Allocation;
            allocation = new Rectangle (allocation.X, allocation.Y, (int)size.Width, (int)size.Height);
#if XWT_GTKSHARP3
            widget.SizeAllocate (allocation);
#else
            widget.Allocation = allocation;
#endif
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

        public static Widget ToGtk (this IVidgetBackend backend) {
            if (backend is IGtkBackend vb)
                return vb.Widget;
            if (backend is IXwtBackend xb && ((Xwt.Backends.IFrontend)xb.Widget).Backend is Xwt.GtkBackend.WidgetBackend)
                return ((Xwt.GtkBackend.WidgetBackend)((Xwt.Backends.IFrontend)xb.Widget).Backend).Widget;

            return backend as Widget;

        }

        public static Gtk.ToolItem ToGtk (this LVV.IToolbarItemBackend backend) {
            if (backend is IGtkBackend vb)
                return (Gtk.ToolItem) vb.Widget;
            return backend as Gtk.ToolItem;

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
                box.QueryChildPacking (box.Children[i], out var expand, out var fill, out var padding, out var packType);
                items[i] = new ChildPacking { Expand = expand, Fill = fill, Padding = padding, PackType = packType, Widget = box.Children[i] };
            }

            foreach (var item in items) {
                box.Remove (item.Widget);
            }

            foreach (var item in items) {

                box.PackEnd (item.Widget, item.Expand, item.Fill, item.Padding);
                box.SetChildPacking (item.Widget, item.Expand, item.Fill, item.Padding, item.PackType);
            }
        }

        public static Xwt.Point ConvertToScreenCoordinates (Widget widget, Xwt.Point point) {
            var parentWindow = widget.ParentWindow;
            if (parentWindow == null) {
                var win = widget as Gtk.Window;
                if (win != null)
                    parentWindow = win.GdkWindow;
                else
                    return Point.Zero;
            } 
            int x, y;
            parentWindow.GetOrigin (out x, out y);
            var a = widget.Allocation;
            x += a.X;
            y += a.Y;

            return new Xwt.Point (x + point.X, y + point.Y);
        }

        public static Gtk.Widget AllocEventBox (this Gtk.Widget widget, bool visibleWindow = false) {
            // Wraps the widget with an event box. Required for some
            // widgets such as Label which doesn't have its own gdk window

            if (widget is Gtk.EventBox) {
                ((Gtk.EventBox) widget).VisibleWindow = true;
                return widget;
            }

#if XWT_GTK3
            if (!widget.HasWindow) {
#else
            if (widget.IsNoWindow) {
#endif

                var eventBox = new Gtk.EventBox ();
                eventBox.Visible = widget.Visible;
                eventBox.Sensitive = widget.Sensitive;
                eventBox.VisibleWindow = visibleWindow;
                Xwt.GtkBackend.GtkEngine.ReplaceChild (widget, eventBox);
                eventBox.Add (widget);
                return eventBox;
            }
            return widget;
        }

        public static void ReplaceChild (Gtk.Widget oldWidget, Gtk.Widget newWidget) {
            Xwt.GtkBackend.GtkEngine.ReplaceChild (oldWidget, newWidget);
        }

        public static Gtk.Widget EventsRootWidget(Widget widget) {
            if (widget.Parent is Gtk.EventBox)
                return widget.Parent;
            return widget; 
        }

        public static ButtonEventArgs ToXwt (this Gtk.ButtonReleaseEventArgs args, Widget widget) {
            var a = new ButtonEventArgs ();

            var pointer_coords = EventsRootWidget(widget).CheckPointerCoordinates (args.Event.Window, args.Event.X, args.Event.Y);
            a.X = pointer_coords.X;
            a.Y = pointer_coords.Y;

            a.Button = (PointerButton)args.Event.Button;
            return a;
        }
    }
}