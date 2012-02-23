﻿using System.Windows;
using Xwt;
using Xwt.Drawing;
using Point = System.Windows.Point;

namespace Limaki.Drawing.WPF {
    public class DrawingConverter {
        public static Point[] Convert(Xwt.Point[] value) {
            var result = new Point[value.Length];
            for (int i = 0; i < value.Length; i++) {
                result[i] = new Point (value[i].X, value[i].Y);
            }
            return result;
        }

        public static Rect Convert(RectangleD value) {
            return new Rect (value.X, value.Y, value.Width, value.Height);
        }

        public static RectangleD Convert(Rect value) {
            return new RectangleD((int)value.X, (int)value.Y, (int)value.Width, (int)value.Height);
        }
        public static Point Convert(Xwt.Point value) {
            return new Point (value.X, value.Y);
        }
        public static Xwt.Point Convert(Point value) {
            return new Xwt.Point(value.X, value.Y);
        }

        public static Color Convert(System.Windows.Media.Color value) {
            return Xwt.WPFBackend.DataConverter.ToXwtColor(value);
        }
        public static System.Windows.Media.Color Convert(Color value) {
            return Xwt.WPFBackend.DataConverter.ToWpfColor(value);
        }
    }
}
