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
using Xwt.GdiBackend;
using Point = Xwt.Point;

namespace Limaki.View.GdiBackend {

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



        

       
    }
}