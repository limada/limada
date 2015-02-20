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

using Limaki.View.Viz;
using Xwt;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Limaki.View.XwtBackend {

    public static class XwtBackendHelper {

        public static ScrollView WithScrollView (this IDisplay display) {
            return (display.Backend as Widget).WithScrollView();
        }

        public static ScrollView WithScrollView (this Widget backend) {
            if (backend is ScrollView)
                return (ScrollView)backend;
            if (backend.Parent is ScrollView)
                return (ScrollView) backend.Parent;
            backend.VerticalPlacement = WidgetPlacement.Fill;
            backend.HorizontalPlacement = WidgetPlacement.Fill;
            var scroll = new ScrollView();
            scroll.Content = backend;
            return scroll;

        }

        /// <summary>
        /// gives back the Content of the ScrollView if sender is Scrollview
        /// else sender
        /// </summary>
        /// <param name="widget"></param>
        /// <returns></returns>
        public static Widget PeeledScrollView (this Widget widget) {
            if (widget is ScrollView)
                return ((ScrollView) widget).Content;
            return widget;
        }

        /// <summary>
        /// gives back the widged.Peeled 
        /// and all its children if any
        /// </summary>
        /// <param name="widget"></param>
        /// <returns></returns>
        public static IEnumerable<Widget> ScrollPeeledChildren (this Widget widget) {
            widget = widget.PeeledScrollView();
            yield return widget;
            var box = widget as Box;
            if (box != null)
                foreach(var child in box.Children)
                    yield return child.PeeledScrollView();

            var paned = widget as Paned;
            if(paned != null) {
                yield return paned.Panel1.Content.PeeledScrollView();
                yield return paned.Panel2.Content.PeeledScrollView();
            }

            var table = widget as Table;
            if (table != null)
                foreach (var child in table.Children)
                    yield return child.PeeledScrollView();

            var notebook = widget as Notebook;
            if (notebook != null)
                foreach (var child in notebook.Tabs)
                    yield return child.Child.PeeledScrollView();

            var frame = widget as Frame;
            if (frame != null)
                yield return frame.Content.PeeledScrollView();

            var canvas = widget as Canvas;
            if (canvas != null)
                foreach (var child in canvas.Children)
                    yield return child.PeeledScrollView();
        }

        public static void VidgetBackendUpdate (this Widget widget) {
            widget.QueueForReallocate();
        }

        public static void VidgetBackendInvalidate (this Widget widget) {
            widget.QueueForReallocate();
        }

        public static void VidgetBackendInvalidate (this Widget widget, Rectangle rect) {
            widget.QueueForReallocate();
        }

        public static Widget ToXwt (this IVidgetBackend backend) {
            var vb = backend as IXwtBackend;
            if (vb != null)
                return vb.Widget;
            return backend as Widget;

        }

        public static void RemoveParent (this Widget widget) {
            if (widget.Parent != null) {
                // impossible to change widget.Parent???
                //throw new NotImplementedException ();

            }
        }
    }
}