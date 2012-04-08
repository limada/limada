/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */


using System;
using Limaki.Drawing;
using Xwt;
using Xwt.Drawing;

namespace Limaki.Drawing.Gdi {

    public static class GDIConverter {

        public static System.Drawing.Rectangle Convert(Rectangle value) {
            return new System.Drawing.Rectangle((int)value.X, (int)value.Y, (int)value.Width, (int)value.Height);
        }

        public static System.Drawing.RectangleF ConvertF(Rectangle value) {
            return new System.Drawing.RectangleF((float)value.X, (float)value.Y, (float)value.Width, (float)value.Height);
        }

        public static System.Drawing.Point[] Convert(Point[] value) {
            return Array.ConvertAll<Point, System.Drawing.Point>(value, Convert);
        }

        public static System.Drawing.PointF[] ConvertF(Point[] value) {
            return Array.ConvertAll<Point, System.Drawing.PointF>(value, ConvertF);
        }

        public static Point[] Convert(System.Drawing.PointF[] value) {
            return Array.ConvertAll<System.Drawing.PointF, Point>(value, Convert);
        }

        public static System.Drawing.Point Convert(Point value) {
            return new System.Drawing.Point((int)value.X, (int)value.Y);
        }


        public static System.Drawing.PointF ConvertF(Point value) {
            return new System.Drawing.PointF((float)value.X, (float)value.Y);
        }

        public static Point Convert(System.Drawing.Point value) {
            return new Point(value.X, value.Y);
        }

        public static Point Convert(System.Drawing.PointF value) {
            return new Point(value.X, value.Y);
        }

        public static System.Drawing.Size Convert(Size value) {
            return new System.Drawing.Size((int)value.Width, (int)value.Height);
        }


        public static System.Drawing.SizeF ConvertF(Size value) {
            return new System.Drawing.SizeF((float)value.Width, (float)value.Height);
        }

        public static Size Convert(System.Drawing.Size value) {
            return new Size(value.Width, value.Height);
        }

        public static Size Convert(System.Drawing.SizeF value) {
            return new Size(value.Width, value.Height);
        }

        public static System.Drawing.Drawing2D.LineCap Convert(PenLineCap linecap) {
            var result = new System.Drawing.Drawing2D.LineCap();
            if (linecap == PenLineCap.Flat) {
                result |= System.Drawing.Drawing2D.LineCap.Flat;
            } else if (linecap == PenLineCap.Round) {
                result |= System.Drawing.Drawing2D.LineCap.Round;
            } else if (linecap == PenLineCap.Square) {
                result |= System.Drawing.Drawing2D.LineCap.Square;
            } else if (linecap == PenLineCap.Triangle) {
                result |= System.Drawing.Drawing2D.LineCap.Triangle;
            }
            return result;
        }

        public static System.Drawing.Drawing2D.LineJoin Convert(PenLineJoin linecap) {
            var result = new System.Drawing.Drawing2D.LineJoin();
            if (linecap == PenLineJoin.Bevel) {
                result |= System.Drawing.Drawing2D.LineJoin.Bevel;
            } else if (linecap == PenLineJoin.Miter) {
                result |= System.Drawing.Drawing2D.LineJoin.Miter;
            } else if (linecap == PenLineJoin.Round) {
                result |= System.Drawing.Drawing2D.LineJoin.Round;
            }
            return result;
        }
    }
}