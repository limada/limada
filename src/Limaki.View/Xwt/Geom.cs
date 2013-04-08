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
using System.Collections.Generic;

namespace Xwt.Drawing {

    public static class Geom {

        #region Point and Size

        public static Size Max (this Size a, Size b) {
            return new Size(a.Width > b.Width ? a.Width : b.Width, a.Height > b.Height ? a.Height : b.Height);
        }

        public static Point Max (this Point a, Point b) {
            return new Point(a.X > b.X ? a.X : b.X, a.Y > b.Y ? a.Y : b.Y);
        }

        public static Point Min (this Point a, Point b) {
            return new Point(a.X < b.X ? a.X : b.X, a.Y < b.Y ? a.Y : b.Y);
        }

        public static Point Offset (this Point a, Point b) {
            return a.Offset(b.X,b.Y);
        }

        public static Point Nearest (this Point xy, IEnumerable<Point> points) {
            var result = xy;
            var dmin = Double.MaxValue;
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

        #endregion

        #region Rectangle

        /// <summary>
        /// Normalize rectangle so, that location becomes top-left point of rectangle 
        /// and size becomes positive
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Rectangle NormalizedRectangle (Point start, Point end) {
            return Rectangle.FromLTRB(
                Math.Min(start.X, end.X),
                Math.Min(start.Y, end.Y),
                Math.Max(start.X, end.X),
                Math.Max(start.Y, end.Y));
        }

        public static Rectangle NormalizedRectangle (this Rectangle rect) {
            var rectX = rect.X;
            var rectR = rectX + rect.Width;
            var rectY = rect.Y;
            var rectB = rectY + rect.Height;
            var minX = rectX;
            var maxX = rectR;
            if (rectX > rectR) {
                minX = rectR;
                maxX = rectX;
            }
            var minY = rectY;
            var maxY = rectB;
            if (rectY > rectB) {
                minY = rectB;
                maxY = rectY;
            }
            return Rectangle.FromLTRB(minX, minY, maxX, maxY);
        }

        public static void TrimTo (this Rectangle target, Rectangle source) {
            target.Location = new Point(
                Math.Max(target.Location.X, source.Location.X),
                Math.Max(target.Location.Y, source.Location.Y));
            target.Size = new Size(
                Math.Min(target.Size.Width, source.Size.Width - target.Location.X),
                Math.Min(target.Size.Height, source.Size.Height - target.Location.Y));
        }

        #endregion

    }
}