using System;
using Limaki.Usecases;
using Limaki.View.Vidgets;
using Xwt;
using GridLines = Limaki.View.Common.GridLines;
using Alignment = Limaki.View.Common.Alignment;

namespace Limaki.View.XwtBackend {

    public static class Converter {

        public static ToolkitType ToXwtToolkitType (Guid toolkitType) {
            if (toolkitType == LimakiViewGuids.WpfToolkitGuid)
                return Xwt.ToolkitType.Wpf;

            if (toolkitType == LimakiViewGuids.GtkToolkitGuid)
                return Xwt.ToolkitType.Gtk;
                
            if (toolkitType == LimakiViewGuids.Gtk3ToolkitGuid)
                return Xwt.ToolkitType.Gtk3;
            if (toolkitType == LimakiViewGuids.MacOsToolkitGuid)
                return Xwt.ToolkitType.XamMac;
            
            return ToolkitType.Other;
        }

        public static Guid ToLmk (ToolkitType toolkitType) {

            if (toolkitType == Xwt.ToolkitType.Wpf)
                return LimakiViewGuids.WpfToolkitGuid;

            if (toolkitType == Xwt.ToolkitType.Gtk)
                return LimakiViewGuids.GtkToolkitGuid;

            if (toolkitType == Xwt.ToolkitType.Gtk3)
                return LimakiViewGuids.Gtk3ToolkitGuid;
            if (toolkitType == Xwt.ToolkitType.XamMac)
                return LimakiViewGuids.MacOsToolkitGuid;

            return default;
        }

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

        public static GridLines ToLmk (this Xwt.GridLines it) => (GridLines)it;
        public static Xwt.GridLines ToXwt (this GridLines it) => (Xwt.GridLines)it;

        public static Alignment ToLmk (this Xwt.Alignment it) => (Alignment)it;
        public static Xwt.Alignment ToXwt (this Alignment it) => (Xwt.Alignment)it;
    }
}