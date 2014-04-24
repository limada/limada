/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Xwt;
using Xwt.Drawing;
using Limaki.Common;

namespace Limaki.Drawing {

    public static class DrawingExtensions {
        #region Recangle

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
        /// TODO: look if obsolete cause Xwt.Rectangle has a Union
        ///	Union Shared Method
        /// </summary>
        ///
        /// <remarks>
        ///	Produces a new Rectangle from the union of 2 existing 
        ///	Rectangles.
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

        /// <summary>
        /// Inflate that doesn't change the rectangle
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
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

        public static Color EmptyColor {
            get { return Colors.White; }
        }

        public static bool Equals (this Color a, Color b) {
            return a.Alpha == b.Alpha && a.Red == b.Red && a.Green == b.Green && a.Blue == b.Blue;
        }

        public static Color Clone (this Color color) {
            return new Color (color.Red, color.Green, color.Blue, color.Alpha);
        }
        #endregion
        

        public static double TransformFontSize (this Matrix matrix, double fIn) {
            return Math.Abs(Math.Min(matrix.M11, matrix.M22) * fIn);
        }

        static IDrawingUtils _drawingUtils = null;
        static IDrawingUtils DrawingUtils { get { return _drawingUtils ?? (_drawingUtils = Registry.Factory.Create<IDrawingUtils>()); } }

        public static Size DpiFactor(Size dpi) {
            return new Size(DrawingUtils.ScreenResolution().Width / dpi.Width,DrawingUtils.ScreenResolution().Height / dpi.Height);
        }

        public static Size DpiFactor(this Context context) {
            return DpiFactor(DrawingUtils.Resolution(context));
        }

    }
}