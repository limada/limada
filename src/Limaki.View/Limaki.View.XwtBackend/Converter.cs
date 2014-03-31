using Limaki.View.Vidgets;
using Xwt;

namespace Limaki.View.XwtBackend {

    public static class Converter {

        public static MouseActionButtons ToLmk (this PointerButton button) {
            if (button == PointerButton.Left)
                return MouseActionButtons.Left;
            if (button == PointerButton.Middle)
                return MouseActionButtons.Middle;
            if (button == PointerButton.Right)
                return MouseActionButtons.Right;
            if (button == PointerButton.ExtendedButton1)
                return MouseActionButtons.XButton1;
            if (button == PointerButton.ExtendedButton2)
                return MouseActionButtons.XButton2;
            return MouseActionButtons.None;
        }

        public static Point MouseLocation (this Widget widget) {
            var ml = Desktop.MouseLocation;
            var sb = widget.ScreenBounds.Location;
            var scale = Desktop.GetScreenAtLocation(ml).ScaleFactor;

            return new Point(ml.X - sb.X / scale, ml.Y - sb.Y / scale);
        }

        public static MouseActionEventArgs ToLmk (this ButtonEventArgs args, MouseActionButtons lastButton) {
            return new MouseActionEventArgs (
                lastButton,
                Keyboard.CurrentModifiers,
                args.MultiplePress,
                args.X,
                args.Y,
                0
                );
        }
    }
}