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
using Gdk;
using Window = Gtk.Window;
using WindowType = Gtk.WindowType;

namespace Limaki.View.GtkBackend {

    public class PopupWindow : Gtk.Window {

        bool supportAlpha;
        Gtk.Alignment _alignment;

		/// <summary>
		/// adds a space around the window where it is NOT hidden
		/// </summary>
		/// <value>The tolerance.</value>
		public Xwt.WidgetSpacing Tolerance { get; set; }

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

            // bounds of reference widget in screen coordinates
            var referenceBounds = Xwt.Rectangle.Zero;

            // position of popup
            Func<Xwt.Point> calcPosition = () => {
                referenceBounds = new Xwt.Rectangle (
                    GtkBackendHelper.ConvertToScreenCoordinates (reference, Xwt.Point.Zero),
                    new Xwt.Size (reference.Allocation.Width, reference.Allocation.Height));

                if (positionRect == Xwt.Rectangle.Zero)
                    positionRect = new Xwt.Rectangle (Xwt.Point.Zero, referenceBounds.Size);
                positionRect = positionRect.Offset (referenceBounds.Location);
                return new Xwt.Point (positionRect.X, positionRect.Bottom);
            };

            var popupPosition = calcPosition ();
            if (child == null)
                popup.SetSizeRequest ((int) referenceBounds.Width, (int) referenceBounds.Height);
            else {
                popup.DefaultWidth = 10;
                child.ShowAll ();
            }
            var topLevelPos = GtkBackendHelper.ConvertToScreenCoordinates (topLevel, Xwt.Point.Zero);
            popup.TransientPosition = popupPosition.Offset (-topLevelPos.X, -topLevelPos.Y);
            Gtk.SizeAllocatedHandler sizeAllocated = (o, args) => {
                popup.Move ((int) popupPosition.X, (int) popupPosition.Y);
                popup.GrabFocus ();
            };
            popup.SizeAllocated += sizeAllocated;

            topLevel.AddEvents ((int) Gdk.EventMask.StructureMask);

            topLevel.ConfigureEvent -= popup.TopLevelConfigureEvent;
            topLevel.ConfigureEvent += popup.TopLevelConfigureEvent;

            // if the mouse is moved in toplevel-window:
            Gtk.MotionNotifyEventHandler topLevelMotion = (s, args) => {
                if (topLevel == null)
                    return;

                topLevelPos = GtkBackendHelper.ConvertToScreenCoordinates (topLevel, Xwt.Point.Zero);
                var referencePos = GtkBackendHelper.ConvertToScreenCoordinates (reference, Xwt.Point.Zero);

                // take args in sceen coordinates:
                var motionPos = new Xwt.Point(args.Event.XRoot, args.Event.YRoot);//topLevelPos.Offset (args.Event.X, args.Event.Y);

                var tolerance = popup.Tolerance;
                var popupSize = new Xwt.Size (popup.Allocation.Width, popup.Allocation.Height);

                // var refBounds = new Xwt.Rectangle (refPos, screenBounds.Size);
                var motionBounds = new Xwt.Rectangle (
                    referencePos.X - tolerance.Left,
                    referencePos.Y - tolerance.Top,
                    popupSize.Width + tolerance.HorizontalSpacing,
                    popupSize.Height + tolerance.VerticalSpacing);

                // TODO: hide if other event than move-event occurs outside of popup window
                // TODO: something is wrong with referencepos; maybe ConvertToScreenCoordinates has an error

                if (!motionBounds.Contains (motionPos))
                    popup.HideAll ();
            };
            topLevel.MotionNotifyEvent += topLevelMotion;

            //ClientEvent: a message has been received from another application.

            popup.ShowAll ();

            popup.Hidden += (o, args) => {
                topLevel.ConfigureEvent -= popup.TopLevelConfigureEvent;
                topLevel.MotionNotifyEvent -= topLevelMotion;
                popup.ReleaseInnerWidget ();
                popup.Destroy ();
            }; 
            return popup;
        }
        /// <summary>
        /// position relative to toplevel-window
        /// </summary>
        public Xwt.Point TransientPosition { get; set; }
        [GLib.ConnectBefore]
        void TopLevelConfigureEvent (object o, ConfigureEventArgs args) {
            this.Move (args.Event.X + (int) this.TransientPosition.X, args.Event.Y + (int) this.TransientPosition.Y);
        }

    }
}