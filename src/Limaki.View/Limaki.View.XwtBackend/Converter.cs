using Limaki.View.Vidgets;
using Xwt;
using GridLines = Limaki.View.Common.GridLines;

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
            var scale = 1; //Desktop.GetScreenAtLocation(ml).ScaleFactor;

            return new Point (ml.X - sb.X / scale, ml.Y - sb.Y / scale);
        }

        public static MouseActionEventArgs ToLmk (this ButtonEventArgs args) {
            return new MouseActionEventArgs (
                args.Button.ToLmk (),
                Keyboard.CurrentModifiers,
                args.MultiplePress,
                args.X,
                args.Y,
                0
                );
        }

        public static GridLines ToLmk (this Xwt.GridLines gridLines) => (GridLines)gridLines;
        public static Xwt.GridLines ToXwt (this GridLines gridLines) => (Xwt.GridLines)gridLines;

    }
}