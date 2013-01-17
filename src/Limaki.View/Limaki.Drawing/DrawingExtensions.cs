using System;
using Xwt;
using Xwt.Drawing;
using Limaki.Common;
using System.Collections.Generic;

namespace Limaki.Drawing {
    public static class DrawingExtensions {

        #region Point and Size

        public static Size Max (this Size a, Size b) {
            return new Size (a.Width > b.Width ? a.Width : b.Width, a.Height > b.Height ? a.Height : b.Height);
        }

        public static Point Max (this Point a, Point b) {
            return new Point (a.X > b.X ? a.X : b.X, a.Y > b.Y ? a.Y : b.Y);
        }


        public static Point Min (this Point a, Point b) {
            return new Point (a.X < b.X ? a.X : b.X, a.Y < b.Y ? a.Y : b.Y);
        }

        public static Point Nearest (this Point xy, IEnumerable<Point> points) {
            var result = xy;
            var dmin = double.MaxValue;
            foreach (var pt in points) {
                var x1 = xy.X - pt.X;
                var y1 = xy.Y - pt.Y;
                var min1 = x1 * x1 + y1 * y1; //Squared Euclidean Distance
                if (min1 < dmin) {
                    dmin = min1;
                    result = pt;
                }
            }
            return result;
        }

        public static Point Add (Point pt, Size sz) {
            return new Point (pt.X + sz.Width, pt.Y + sz.Height);
        }

        public static Point Subtract (Point pt, Size sz) {
            return new Point (pt.X - sz.Width, pt.Y - sz.Height);
        }

        #endregion

        #region Recangle

        /// <summary>
        /// Normalize rectangle so, that location becomes top-left point of rectangle 
        /// and size becomes positive
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Rectangle NormalizedRectangle (Point start, Point end) {
            return Rectangle.FromLTRB (
                Math.Min (start.X, end.X),
                Math.Min (start.Y, end.Y),
                Math.Max (start.X, end.X),
                Math.Max (start.Y, end.Y));
        }

        public static Rectangle NormalizedRectangle (this Rectangle rect) {
            var rectX = rect.X;
            var rectR = rectX + rect.Width;
            var rectY = rect.Y;
            var rectB = rectY + rect.Height;
            var minX = rectX;
            var maxX = rectR;
            if ( rectX > rectR ) {
                minX = rectR;
                maxX = rectX;
            }
            var minY = rectY;
            var maxY = rectB;
            if ( rectY > rectB ) {
                minY = rectB;
                maxY = rectY;
            }
            return Rectangle.FromLTRB (minX, minY, maxX, maxY);
        }

        public static void TrimTo (this Rectangle target, Rectangle source) {
            target.Location = new Point (
                Math.Max (target.Location.X, source.Location.X),
                Math.Max (target.Location.Y, source.Location.Y));
            target.Size = new Size (
                Math.Min (target.Size.Width, source.Size.Width - target.Location.X),
                Math.Min (target.Size.Height, source.Size.Height - target.Location.Y));
        }

        /// <summary>
        /// Replaces RectangleF.Contains as it gives wrong 
        /// results with x,y < 0 and right, bottom > 0
        /// </summary>
        /// <param name="value"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool Contains (Rectangle value, Rectangle other) {
            return
                other.X >= value.X &&
                other.X + other.Width <= value.X + value.Width &&
                other.Y >= value.Y &&
                other.Y + other.Height <= value.Y + value.Height;
        }

        public static bool Intersects (Rectangle value, Rectangle other) {
            return !( value.X > other.X + other.Width ||
                      ( value.X + value.Width ) < ( other.X ) ||
                      ( value.Y > other.Y + other.Height ) ||
                      ( value.Y + value.Height ) < ( other.Y ) );
        }

        /// <summary>
        ///	Union Shared Method
        /// </summary>
        ///
        /// <remarks>
        ///	Produces a new RectangleF from the union of 2 existing 
        ///	RectangleFs.
        /// </remarks>

        public static Rectangle Union (Rectangle a, Rectangle b) {
            return Rectangle.FromLTRB (
                Math.Min (a.Left, b.Left),
                Math.Min (a.Top, b.Top),
                Math.Max (a.Right, b.Right),
                Math.Max (a.Bottom, b.Bottom));
        }

        public static Rectangle Intersect (Rectangle a, Rectangle b) {

            Func<bool> intersectsWithInclusive = () => !( ( a.X > b.Right ) || ( a.Right < b.Left ) ||
                                                          ( a.Y > b.Bottom ) || ( b.Bottom < b.Top ) );

            // MS.NET returns a non-empty rectangle if the two rectangles
            // touch each other
            if ( !intersectsWithInclusive () )
                return Rectangle.Zero;

            return Rectangle.FromLTRB (
                Math.Max (a.Left, b.Left),
                Math.Max (a.Top, b.Top),
                Math.Min (a.Right, b.Right),
                Math.Min (a.Bottom, b.Bottom));
        }

        public static Rectangle Inflate (Rectangle rect, double x, double y) {
            var ir = 
                new Rectangle (rect.X, rect.Y, rect.Width, rect.Height)
                .Inflate (x, y);
            return ir;
        }

        #endregion

        #region Color

        public static uint ToArgb (this Color color) {
            return
                (uint) ( color.Alpha * 255 ) << 24
                | (uint) ( color.Red * 255 ) << 16
                | (uint) ( color.Green * 255 ) << 8
                | (uint) ( color.Blue * 255 );

        }
        public static uint ToRgb (this Color color) {
            return
                (uint) (255) << 24
                | (uint) (color.Red * 255) << 16
                | (uint) (color.Green * 255) << 8
                | (uint) (color.Blue * 255);

        }
        public static Color FromArgb (uint argb) {
            var a = ( argb >> 24 ) / 255d;
            var r = ( ( argb >> 16 ) & 0xFF ) / 255d;
            var g = ( ( argb >> 8 ) & 0xFF ) / 255d;
            var b = ( argb & 0xFF ) / 255d;
            return new Color (r, g, b, a);

        }
        public static Color FromArgb (byte a, Color color) {
            return new Color (color.Red, color.Green, color.Blue, ((double)a)/255d);
        }
        public static Color FromArgb (byte a, byte r, byte g, byte b) {
            return Color.FromBytes (r, g, b, a);
        }

        public static Color FromArgb (byte r, byte g, byte b) {
            return Color.FromBytes (r, g, b);
        }

        private static Color _emptyColor = Colors.White;
        public static Color EmptyColor {
            get { return _emptyColor; }
        }

        public static bool Equals (this Color a, Color b) {
            return a.Alpha == b.Alpha && a.Red == b.Red && a.Green == b.Green && a.Blue == b.Blue;
        }

        public static Color Clone (this Color color) {
            return new Color (color.Red, color.Green, color.Blue, color.Alpha);
        }
        #endregion
        
        #region Font
        public static void Dispose (this Font font) {

        }

        public static Font Clone (this Font value) {
            return
                value.WithSize (value.Size)
                    .WithStretch (value.Stretch)
                    .WithStyle (value.Style)
                    .WithWeight (value.Weight);
        }

        #endregion

        public static double TransformFontSize (this Matrix matrix, double fIn) {
            return Math.Abs(Math.Min(matrix.M11, matrix.M22) * fIn);
        }

        static IDrawingUtils _drawingUtils = null;
        public static IDrawingUtils DrawingUtils {
            get {
                if (_drawingUtils == null) {
                    _drawingUtils = Registry.Factory.Create<IDrawingUtils>();
                }
                return _drawingUtils;
            }
        }

        public static Size DpiFactor(Size dpi) {
            return new Size(DrawingUtils.ScreenResolution().Width / dpi.Width,DrawingUtils.ScreenResolution().Height / dpi.Height);
        }

        public static Size DpiFactor(this Context context) {
            return DpiFactor(DrawingUtils.Resolution(context));
        }
    }
}