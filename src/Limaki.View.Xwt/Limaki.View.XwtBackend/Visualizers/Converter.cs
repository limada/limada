using Xwt;

namespace Limaki.View.XwtBackend {

    public static class Converter {

        public static UI.MouseActionButtons ToLmk (this PointerButton button) {
            if(button == PointerButton.Left)
                return UI.MouseActionButtons.Left;
            if(button == PointerButton.Middle)
                return UI.MouseActionButtons.Middle;
            if(button == PointerButton.Right)
                return UI.MouseActionButtons.Right;
            if(button == PointerButton.ExtendedButton1)
                return UI.MouseActionButtons.XButton1;
            if(button == PointerButton.ExtendedButton2)
                return UI.MouseActionButtons.XButton2;
            return UI.MouseActionButtons.None;
        }

        public static Point MouseLocation (this Widget widget) {
            var ml = Desktop.MouseLocation;
            var sb = widget.ScreenBounds.Location;
            var scale = Desktop.GetScreenAtLocation(ml).ScaleFactor;

            return new Point(ml.X - sb.X / scale, ml.Y - sb.Y / scale);
        }
    }
}