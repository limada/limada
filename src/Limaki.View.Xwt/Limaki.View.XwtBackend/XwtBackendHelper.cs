using Xwt;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Limaki.View.XwtBackend {

    public static class XwtBackendHelper {

        public static ScrollView ScrollView (this IDisplay display) {
            var backend = display.Backend as Widget;
            backend.VerticalPlacement = WidgetPlacement.Fill;
            backend.HorizontalPlacement = WidgetPlacement.Fill;
            var scroll = new ScrollView();
            //scroll.CanGetFocus = true;
            scroll.Content = backend;
            return scroll;
        }

        public static Widget ScrollViewContent (this Widget sender) {
            if (sender is ScrollView)
                return ((ScrollView) sender).Content;
            return sender;
        }

        public static IEnumerable<Widget> Children (this Widget widget) {
            widget = widget.ScrollViewContent();
            var box = widget as Box;
            if (box != null)
                foreach(var child in box.Children)
                    yield return child.ScrollViewContent();

            var paned = widget as Paned;
            if(paned != null) {
                yield return paned.Panel1.Content.ScrollViewContent();
                yield return paned.Panel2.Content.ScrollViewContent();
            }

            var table = widget as Table;
            if (table != null)
                foreach (var child in table.Children)
                    yield return child.ScrollViewContent();

            var notebook = widget as Notebook;
            if (notebook != null)
                foreach (var child in notebook.Tabs)
                    yield return child.Child.ScrollViewContent();

            var frame = widget as Frame;
            if (frame != null)
                yield return frame.Content.ScrollViewContent();

        }

        public static void VidgetBackendUpdate (Widget widget) {
            widget.QueueForReallocate();
        }

        public static void VidgetBackendInvalidate (Widget widget) {
            widget.QueueForReallocate();
        }

        public static void VidgetBackendInvalidate (Widget widget, Rectangle rect) {
            widget.QueueForReallocate();
        }
    }
}