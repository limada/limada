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
using System;
using System.Diagnostics;

namespace Limaki.View.GtkBackend {

    public class PopupWindow : Gtk.Window {

        bool supportAlpha;
        Gtk.Alignment _alignment;

        public PopupWindow (Gtk.Widget child) : base (WindowType.Popup) {

            AppPaintable = true;
            Decorated = false;
            SkipPagerHint = true;
            SkipTaskbarHint = true;
            TypeHint = Gdk.WindowTypeHint.PopupMenu;

            AddEvents ((int) Gdk.EventMask.FocusChangeMask);
            DefaultHeight = DefaultWidth = 1;
            var _frame = new Frame () { Shadow = ShadowType.EtchedIn };

            _alignment = new Gtk.Alignment (0, 0, 1, 1);
            _frame.Add (_alignment);
            Add (_frame);
            if (child != null) {
                this._alignment.Add (child);
                //TransientFor = (Gtk.Window)child.Toplevel;
            }
            FocusOutEvent += HandleFocusOutEvent;
            AcceptFocus = true;
            CanFocus = true;

            Resizable = true;
            OnScreenChanged (null);
        }

        public void ReleaseInnerWidget () {
            _alignment.Remove (_alignment.Child);
        }

        [GLib.ConnectBefore]
        void HandleFocusOutEvent (object o, FocusOutEventArgs args) {
            this.HideAll ();
        }

        public void SetPadding (Xwt.WidgetSpacing spacing) {
            _alignment.LeftPadding = (uint) spacing.Left;
            _alignment.RightPadding = (uint) spacing.Right;

            _alignment.TopPadding = (uint) spacing.Top;
            _alignment.BottomPadding = (uint) spacing.Bottom;

        }

        protected override void OnScreenChanged (Gdk.Screen previous_screen) {
            // To check if the display supports alpha channels, get the colormap
            var colormap = this.Screen.RgbaColormap;
            if (colormap == null) {
                colormap = this.Screen.RgbColormap;
                supportAlpha = false;
            } else {
                supportAlpha = true;
            }
            this.Colormap = colormap;
            base.OnScreenChanged (previous_screen);
        }

        public static PopupWindow Show (Widget reference, Xwt.Rectangle positionRect, Widget child) {

            var popup = new PopupWindow (child);
            var topLevel = (Window) reference.Toplevel;

            popup.DestroyWithParent = true;

            popup.BorderWidth = topLevel.BorderWidth;
            var screenBounds = Xwt.Rectangle.Zero;

            Func<Xwt.Point> calcPosition = () => {
                screenBounds = new Xwt.Rectangle (
                    GtkBackendHelper.ConvertToScreenCoordinates (reference, Xwt.Point.Zero),
                    new Xwt.Size (reference.Allocation.Width, reference.Allocation.Height));

                if (positionRect == Xwt.Rectangle.Zero)
                    positionRect = new Xwt.Rectangle (Xwt.Point.Zero, screenBounds.Size);
                positionRect = positionRect.Offset (screenBounds.Location);
                return new Xwt.Point (positionRect.X, positionRect.Bottom);
            };
            var position = calcPosition ();
            if (child == null)
                popup.SetSizeRequest ((int) screenBounds.Width, (int) screenBounds.Height);
            else {
                popup.DefaultWidth = 10;
                child.ShowAll ();
            }
            var transPos = GtkBackendHelper.ConvertToScreenCoordinates (topLevel, Xwt.Point.Zero);
            var refPos = GtkBackendHelper.ConvertToScreenCoordinates (reference, Xwt.Point.Zero);
            popup.TransientPosition = position.Offset (-transPos.X, -transPos.Y);
            Gtk.SizeAllocatedHandler sizeAllocated = (o, args) => {
                popup.Move ((int) position.X, (int) position.Y);
                popup.GrabFocus ();
            };
            popup.SizeAllocated += sizeAllocated;

            topLevel.AddEvents ((int) Gdk.EventMask.StructureMask);

            topLevel.ConfigureEvent -= popup.TopLevelConfigureEvent;
            topLevel.ConfigureEvent += popup.TopLevelConfigureEvent;

            Gtk.MotionNotifyEventHandler topLevelMotion = (s, args) => {
                if (topLevel == null)
                    return;
                transPos = GtkBackendHelper.ConvertToScreenCoordinates (topLevel, Xwt.Point.Zero);
                refPos = GtkBackendHelper.ConvertToScreenCoordinates (reference, Xwt.Point.Zero);
                var motionPos = transPos.Offset (new Xwt.Point (args.Event.X, args.Event.Y));
                var refBounds = new Xwt.Rectangle (refPos, screenBounds.Size);

                if (!refBounds.Contains (motionPos))
                    popup.HideAll ();
            };

            topLevel.MotionNotifyEvent += topLevelMotion;

            popup.ShowAll ();

            popup.Hidden += (o, args) => {
                topLevel.ConfigureEvent -= popup.TopLevelConfigureEvent;
                topLevel.MotionNotifyEvent -= topLevelMotion;
                popup.ReleaseInnerWidget ();
                popup.Destroy ();
            };
            return popup;
        }

        public Xwt.Point TransientPosition { get; set; }
        [GLib.ConnectBefore]
        void TopLevelConfigureEvent (object o, ConfigureEventArgs args) {
            this.Move (args.Event.X + (int) this.TransientPosition.X, args.Event.Y + (int) this.TransientPosition.Y);
        }

    }
}