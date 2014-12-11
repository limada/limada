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
using System.Windows;
using Xwt.Backends;

namespace Limaki.View.WpfBackend {

    public static class WpfBackendHelper {

        public static Xwt.Size VidgetBackendSize (this FrameworkElement backend) {
            return new Xwt.Size(backend.ActualWidth, backend.ActualHeight);
        }

        public static void VidgetBackendSize (this FrameworkElement backend, Xwt.Size value) {
            backend.Width = value.Width;
            backend.Height = value.Height;
        }

        public static void VidgetBackendUpdate (this FrameworkElement widget) {
            VidgetBackendInvalidate (widget);
            widget.UpdateLayout ();
        }

        public static void VidgetBackendInvalidate (this FrameworkElement widget) {
            widget.InvalidateMeasure ();
            widget.InvalidateVisual ();
        }

        public static void VidgetBackendInvalidate (this FrameworkElement widget, Xwt.Rectangle rect) {
            VidgetBackendInvalidate (widget);
        }

        public static void VidgetBackendSetFocus (this FrameworkElement widget ) {
            widget.Focus ();
        }
        
        // example code for listening global clipboard changes
        public static void ListenClipboard () {
            //System.Windows.Input.ApplicationCommands.Paste.CanExecuteChanged +=
            //   Paste_CanExecuteChanged;
            // not working, fires only in current applicaton and too often
            System.Windows.Input.ApplicationCommands.Copy.CanExecuteChanged +=
                Paste_CanExecuteChanged;
        }

        public static IDataObject clipo = null;
        public static void Paste_CanExecuteChanged (object sender, EventArgs e) {
            if (clipo==null || !Clipboard.IsCurrent (clipo)) {
                clipo = Clipboard.GetDataObject();
            }
        }

        public static FrameworkElement ToWpf (this IVidgetBackend backend) {
            var vb = backend as IWpfBackend;
            if (vb != null)
                return vb.Control;
            var xb = backend as Limaki.View.XwtBackend.IXwtBackend;
            if (xb != null && ((Xwt.Backends.IFrontend)xb.Widget).Backend is Xwt.WPFBackend.WidgetBackend)
                return ((Xwt.WPFBackend.WidgetBackend)((Xwt.Backends.IFrontend)xb.Widget).Backend).Widget;

            var widget = backend as Xwt.Widget;
            if (widget != null) {
                var wpfBackend = widget.GetBackend () as Xwt.WPFBackend.WidgetBackend;
                if (wpfBackend != null)
                    return Xwt.WPFBackend.WidgetBackend.GetFrameworkElement (wpfBackend);
            }

            return backend as FrameworkElement;

        }

        public static FrameworkElement GetFrameworkElement (this IVidgetBackend vidgetBackend) {
            var backend = vidgetBackend as FrameworkElement;
            if (backend != null)
                return backend;
            var widget = vidgetBackend as Xwt.Widget;
            if (widget != null) {
                var wpfBackend = widget.GetBackend () as Xwt.WPFBackend.WidgetBackend;
                if (wpfBackend != null)
                    backend = Xwt.WPFBackend.WidgetBackend.GetFrameworkElement (wpfBackend);
            } else {
                var wpfBackend = vidgetBackend as IWpfBackend;
                if (wpfBackend != null)
                    return wpfBackend.Control;

            }
            return backend;
        }
    }
}