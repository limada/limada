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
using System.Drawing;
using System.Drawing.Drawing2D;
using Limaki.Drawing;
using Xwt.Backends;
using Xwt.Gdi.Backend;
using Font = Xwt.Drawing.Font;
using Matrix = Xwt.Drawing.Matrix;
using Point = Xwt.Point;

namespace Limaki.Drawing.Gdi {

    public static class GDIConverter {

       public static System.Drawing.Point[] Convert(Point[] value) {
            return Array.ConvertAll (value, GdiConverter.ToGdi);
        }

        public static PointF[] ConvertF(Point[] value) {
            return Array.ConvertAll (value, GdiConverter.ToGdiF);
        }

        public static Point[] Convert(PointF[] value) {
            return Array.ConvertAll (value, GdiConverter.ToXwt);
        }

        public static LineCap Convert(PenLineCap linecap) {
            var result = new LineCap();
            if (linecap == PenLineCap.Flat) {
                result |= LineCap.Flat;
            } else if (linecap == PenLineCap.Round) {
                result |= LineCap.Round;
            } else if (linecap == PenLineCap.Square) {
                result |= LineCap.Square;
            } else if (linecap == PenLineCap.Triangle) {
                result |= LineCap.Triangle;
            }
            return result;
        }

        public static LineJoin Convert(PenLineJoin linecap) {
            var result = new LineJoin();
            if (linecap == PenLineJoin.Bevel) {
                result |= LineJoin.Bevel;
            } else if (linecap == PenLineJoin.Miter) {
                result |= LineJoin.Miter;
            } else if (linecap == PenLineJoin.Round) {
                result |= LineJoin.Round;
            }
            return result;
        }

        

       
    }
}