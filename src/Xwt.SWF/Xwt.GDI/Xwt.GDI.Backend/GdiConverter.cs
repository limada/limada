using Xwt.Drawing;
namespace Xwt.GDI {

    public static class GdiConverter {

        public static System.Drawing.Color Convert(Color color) {
            return System.Drawing.Color.FromArgb((int)ToArgb(color));
        }

        public static Color Convert(System.Drawing.Color color) {
            return FromArgb((uint)color.ToArgb());
        }
        public static uint ToArgb(Color color) {
            return
                (uint)(color.Alpha * 255) << 24
                | (uint)(color.Red * 255) << 16
                | (uint)(color.Green * 255) << 8
                | (uint)(color.Blue * 255);

        }

        public static Color FromArgb(uint argb) {
            var a = (argb >> 24) / 255d;
            var r = ((argb >> 16) & 0xFF) / 255d;
            var g = ((argb >> 8) & 0xFF) / 255d;
            var b = (argb & 0xFF) / 255d;
            return new Color(r, g, b, a);

        }
        public static Color FromArgb(byte a, Color color) {
            return new Color(color.Red, color.Green, color.Blue, a);
        }
        public static Color FromArgb(byte a, byte r, byte g, byte b) {
            return new Color(r, g, b, a);
        }

        public static Color FromArgb(byte r, byte g, byte b) {
            return new Color(r, g, b);
        }

        public static System.Drawing.FontStyle Convert(FontStyle value) {
            var result = System.Drawing.FontStyle.Regular;
            if (value == null)
                return result;
            if ((value & FontStyle.Italic) != 0) {
                result |= System.Drawing.FontStyle.Italic;
            }
            //if ((value & FontStyle.Underline) != 0) {
            //    result |= System.Drawing.FontStyle.Underline;
            //}
            if ((value & FontStyle.Oblique) != 0) {
                result |= System.Drawing.FontStyle.Bold;
            }
            return result;
        }

        public static FontStyle Convert(System.Drawing.FontStyle value) {
            var result = FontStyle.Normal;
            if (value == null)
                return result;
            if ((value & System.Drawing.FontStyle.Italic) != 0) {
                result |= FontStyle.Italic;
            }
            //if ((native & System.Drawing.FontStyle.Underline) != 0) {
            //    result |= FontStyle.Underline;
            //}
            if ((value & System.Drawing.FontStyle.Bold) != 0) {
                result |= FontStyle.Oblique;
            }
            return result;
        }
    }
}