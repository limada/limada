using System.Windows;

namespace Limaki.Drawing.WPF {
    public class DrawingConverter {
        public static Point[] Convert(PointI[] value) {
            var result = new Point[value.Length];
            for (int i = 0; i < value.Length; i++) {
                result[i] = new Point (value[i].X, value[i].Y);
            }
            return result;
        }

        public static Rect Convert(RectangleI value) {
            return new Rect (value.X, value.Y, value.Width, value.Height);
        }

        public static RectangleI Convert(Rect value) {
            return new RectangleI((int)value.X, (int)value.Y, (int)value.Width, (int)value.Height);
        }
        public static Point Convert(PointI value) {
            return new Point (value.X, value.Y);
        }
        public static PointI Convert(Point value) {
            return new PointI((int)value.X, (int)value.Y);
        }

        public static Color Convert(System.Windows.Media.Color value) {
            return Color.FromArgb(value.A,value.R,value.G,value.B);
        }
        public static System.Windows.Media.Color Convert(Color value) {
            return System.Windows.Media.Color.FromArgb(value.A, value.R, value.G, value.B);
        }
    }
}
