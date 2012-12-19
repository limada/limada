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
 * http://www.limada.org
 * 
 */


using System;
using Limaki.Drawing;
using Xwt;
using Xwt.Drawing;
using Xwt.Gdi.Backend;

namespace Limaki.Drawing.Gdi {

    public static class GDIConverter {

       public static System.Drawing.Point[] Convert(Point[] value) {
            return Array.ConvertAll<Point, System.Drawing.Point> (value, GdiConverter.ToGdi);
        }

        public static System.Drawing.PointF[] ConvertF(Point[] value) {
            return Array.ConvertAll<Point, System.Drawing.PointF> (value, GdiConverter.ToGdiF);
        }

        public static Point[] Convert(System.Drawing.PointF[] value) {
            return Array.ConvertAll<System.Drawing.PointF, Point> (value, GdiConverter.ToXwt);
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

        public static System.Drawing.Drawing2D.Matrix Convert (Matrix matrix) {
            return new System.Drawing.Drawing2D.Matrix(
                (float) matrix.M11,
                (float) matrix.M12,
                (float) matrix.M21,
                (float) matrix.M22,
                (float) matrix.OffsetX,
                (float) matrix.OffsetY);
        }

        public static Matrix Convert (System.Drawing.Drawing2D.Matrix matrice) {
            return new Matrix(
               matrice.Elements[0],
               matrice.Elements[1],
               matrice.Elements[2],
               matrice.Elements[3],
               matrice.Elements[4],
               matrice.Elements[5]);
        }

       
        //internal static Size ToXwt (this System.Drawing.Size size) {
        //    throw new NotImplementedException ();
        //}

        //internal static Point ToXwt (this System.Drawing.Point size) {
        //    throw new NotImplementedException ();
        //}

       
    }
}