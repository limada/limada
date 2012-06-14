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
using Xwt.Drawing;

using System.Drawing.Drawing2D;
using SystemColors = System.Drawing.SystemColors;
using Xwt;
using Xwt.Engine;

namespace Limaki.Drawing.Gdi {

    public class GdiDrawingUtils : IDrawingUtils {

        public Pen CreatePen(Color color) {
            return new GdiPen(color);
        }

        public Matrice NativeMatrice() {
            return new GdiMatrice ();
        }

        public object GetCustomLineCap(double arrowWidth, double arrowHeigth) {
            if (arrowHeigth == 0 || arrowWidth == 0)
                throw new ArgumentException ("ArrowWidth must not be 0");
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            var w = (float) arrowWidth;
            var h = (float) arrowHeigth;
            var p1 = new System.Drawing.PointF(0, 1);
            var p2 = new System.Drawing.PointF(-h, -w);
            var p3 = new System.Drawing.PointF(h, -w);
            path.AddPolygon(new System.Drawing.PointF[3] { p1, p2, p3 });
            //path.AddLine(p1, p2);
            //path.AddLine(p2, p3);
            //path.AddLine(p3, p1);

            var result = new CustomLineCap(path, null);
            result.BaseInset = 1;
            //result.StrokeJoin = LineJoin.Round;

            return result;

        }

        public virtual Size GetTextDimension(string text, IStyle style) {
            return GdiUtils.GetTextDimension(
                (System.Drawing.Font)WidgetRegistry.GetBackend(style.Font),
                text,
                GDIConverter.Convert(style.AutoSize));
        }

        public Size ScreenResolution() {
            return new Size(GdiUtils.DeviceContext.DpiX, GdiUtils.DeviceContext.DpiY);
        }

        public Size Resolution(Context context) {
            var ctx = (Xwt.Gdi.Backend.GdiContext)WidgetRegistry.GetBackend(context);
            if (ctx.Graphics.PageUnit == System.Drawing.GraphicsUnit.Point)
                return new Size(72, 72);
            return new Size(ctx.Graphics.DpiX, ctx.Graphics.DpiY);
        }

    }
}