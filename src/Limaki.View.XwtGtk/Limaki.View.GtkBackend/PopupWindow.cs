using Gtk;
using System;

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
           
            AddEvents ((int)Gdk.EventMask.FocusChangeMask);
            DefaultHeight = DefaultWidth = 1;
            _alignment = new Gtk.Alignment (0, 0, 1, 1);
            Add (_alignment);
            if (child != null) {
                this._alignment.Add (child);
                TransientFor = (Gtk.Window)child.Toplevel;
            }
            FocusOutEvent += HandleFocusOutEvent;
            Resizable = true;
            OnScreenChanged (null);
        }


        public void ReleaseInnerWidget () {
            _alignment.Remove (_alignment.Child);
        }

        void HandleFocusOutEvent (object o, FocusOutEventArgs args) {
            this.HideAll ();
        }

        public void SetPadding (Xwt.WidgetSpacing spacing) {
            _alignment.LeftPadding = (uint)spacing.Left;
            _alignment.RightPadding = (uint)spacing.Right;

            _alignment.TopPadding =   (uint)spacing.Top;
            _alignment.BottomPadding = (uint)spacing.Bottom;

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
            popup.TransientFor = (Window)reference.Toplevel;
            popup.DestroyWithParent = true;
            popup.Hidden += (o, args) => {
                                  popup.ReleaseInnerWidget ();
                                  popup.Destroy ();
                              };

            var screenBounds = new Xwt.Rectangle (
                GtkBackendHelper.ConvertToScreenCoordinates(reference, Xwt.Point.Zero),
                new Xwt.Size(reference.Allocation.Width,reference.Allocation.Height));

            if (positionRect == Xwt.Rectangle.Zero)
                positionRect = new Xwt.Rectangle (Xwt.Point.Zero, screenBounds.Size);
            positionRect = positionRect.Offset (screenBounds.Location);
            var position = new Xwt.Point (positionRect.X, positionRect.Bottom);
            if (child == null)
                popup.SetSizeRequest ((int) screenBounds.Width, (int) screenBounds.Height);
            else {
                popup.DefaultWidth = 10;
                child.ShowAll();
            }
            popup.ShowAll ();

            Gtk.SizeAllocatedHandler sizeAllocated = (o, args) => {
                popup.Move ((int)position.X, (int)position.Y);
                popup.GrabFocus ();
            };
            popup.SizeAllocated += sizeAllocated;
            sizeAllocated (popup, null);

            return popup;
        }
    }
}