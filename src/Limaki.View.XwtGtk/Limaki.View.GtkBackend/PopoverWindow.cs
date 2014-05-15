using Gtk;

namespace Limaki.View.GtkBackend {

    public class PopoverWindow : Gtk.Window {

        bool supportAlpha;
        Gtk.Alignment alignment;

        public PopoverWindow (Gtk.Widget child)
            : base (WindowType.Toplevel) {
            this.AppPaintable = true;
            this.Decorated = false;
            this.SkipPagerHint = true;
            this.SkipTaskbarHint = true;
            this.TypeHint = Gdk.WindowTypeHint.PopupMenu;
            //this.TransientFor = (Gtk.Window)child.Toplevel;
            this.AddEvents ((int)Gdk.EventMask.FocusChangeMask);
            //this.DefaultHeight = this.DefaultWidth = 400;
            this.alignment = new Gtk.Alignment (0, 0, 1, 1);
            this.Add (alignment);
            this.alignment.Add (child);
            this.FocusOutEvent += HandleFocusOutEvent;
            OnScreenChanged (null);
        }


        public void ReleaseInnerWidget () {
            alignment.Remove (alignment.Child);
        }

        void HandleFocusOutEvent (object o, FocusOutEventArgs args) {
            this.HideAll ();
        }

        public void SetPadding (Xwt.WidgetSpacing spacing) {
            alignment.LeftPadding = (uint)spacing.Left;
            alignment.RightPadding = (uint)spacing.Right;

            alignment.TopPadding =   (uint)spacing.Top;
            alignment.BottomPadding = (uint)spacing.Bottom;

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

        public static PopoverWindow Show (Widget reference, Xwt.Rectangle positionRect, Widget child) {
            
            var popover = new PopoverWindow (child);
            popover.TransientFor = (Window)reference.Toplevel;
            popover.DestroyWithParent = true;
            popover.Hidden += (o, args) => {
                                  popover.ReleaseInnerWidget ();
                                  popover.Destroy ();
                              };

            var screenBounds = new Xwt.Rectangle (ConvertToScreenCoordinates(reference, Xwt.Point.Zero),new Xwt.Size(reference.Allocation.Width,reference.Allocation.Height));
            if (positionRect == Xwt.Rectangle.Zero)
                positionRect = new Xwt.Rectangle (Xwt.Point.Zero, screenBounds.Size);
            positionRect = positionRect.Offset (screenBounds.Location);
            var position = new Xwt.Point (positionRect.X, positionRect.Bottom);
            popover.ShowAll ();
            popover.GrabFocus ();
            int w, h;
            popover.GetSize (out w, out h);
            popover.Move ((int)position.X, (int)position.Y);
            popover.SizeAllocated += (o, args) => {
                                         popover.Move ((int)position.X - args.Allocation.Width / 2, (int)position.Y); popover.GrabFocus ();
                                     };
            
            return popover;
        }

        public static Xwt.Point ConvertToScreenCoordinates (Widget widget, Xwt.Point widgetCoordinates) {
            if (widget.ParentWindow == null)
                return Xwt.Point.Zero;
            int x, y;
            widget.ParentWindow.GetOrigin (out x, out y);
            var a = widget.Allocation;
            x += a.X;
            y += a.Y;
            return new Xwt.Point (x + widgetCoordinates.X, y + widgetCoordinates.Y);
        }
    }
}