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
using Xwt.Drawing;

using System.Drawing.Drawing2D;
using SystemColors = System.Drawing.SystemColors;
using Xwt;
using Xwt.Engine;

namespace Limaki.Drawing.GDI {

    public class GDIDrawingUtils : IDrawingUtils {

        public Pen CreatePen(Color color) {
            return new GDIPen(color);
        }

        public Matrice NativeMatrice() {
            return new GDIMatrice ();
        }

        public object GetCustomLineCap(double arrowWidth, double arrowHeigth) {
            if (arrowHeigth == 0 || arrowWidth == 0)
                throw new ArgumentException ("ArrowWidth must not be 0");
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            float w = (float) arrowWidth;
            float h = (float) arrowHeigth;
            System.Drawing.PointF p1 = new System.Drawing.PointF(0, 1);
            System.Drawing.PointF p2 = new System.Drawing.PointF(-h, -w);
            System.Drawing.PointF p3 = new System.Drawing.PointF(h, -w);
            path.AddPolygon(new System.Drawing.PointF[3] { p1, p2, p3 });
            //path.AddLine(p1, p2);
            //path.AddLine(p2, p3);
            //path.AddLine(p3, p1);

            CustomLineCap result = new CustomLineCap(path, null);
            result.BaseInset = 1;
            //result.StrokeJoin = LineJoin.Round;

            return result;

        }

        public virtual Size GetTextDimension(string text, IStyle style) {
            return GDIUtils.GetTextDimension(
                (System.Drawing.Font)WidgetRegistry.GetBackend(style.Font),
                text,
                GDIConverter.Convert(style.AutoSize));
        }


      

    }
}