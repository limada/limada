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


using Xwt;
using Xwt.Drawing;
using Xwt.Gdi;
using Xwt.Gdi.Backend;

namespace Limaki.Drawing.GDI {

    public class GDIUtils {

       
        public static void SetToolkitPen(Pen toolkit, System.Drawing.Pen native) {
            
        }

        public static void SetNativePen(Pen frontend, System.Drawing.Pen backend) {
            if (backend == null || frontend == null)
                return;
            backend.Color = GdiConverter.ToGdi(frontend.Color);
            backend.StartCap = GDIConverter.Convert(frontend.StartCap);
            backend.EndCap = GDIConverter.Convert(frontend.EndCap);
            backend.Width = (float)frontend.Thickness;
            backend.LineJoin = GDIConverter.Convert(frontend.LineJoin);
            backend.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
            if (frontend.CustomEndCap!=null)
                backend.CustomEndCap = 
                    frontend.CustomEndCap as System.Drawing.Drawing2D.CustomLineCap ;
            if (frontend.CustomStartCap != null)
                backend.CustomStartCap = 
                    frontend.CustomStartCap as System.Drawing.Drawing2D.CustomLineCap;
        }
        public static System.Drawing.Graphics CreateGraphics () {
            return System.Drawing.Graphics.FromImage (
                        new System.Drawing.Bitmap (1, 1,
                            System.Drawing.Imaging.PixelFormat.Format32bppArgb));
        }

        private static System.Drawing.Graphics _deviceContext = null;
        public static System.Drawing.Graphics DeviceContext {
            get {
                if (_deviceContext == null) {
                    _deviceContext =
                        System.Drawing.Graphics.FromImage(
                        new System.Drawing.Bitmap(1, 1,
                            System.Drawing.Imaging.PixelFormat.Format32bppArgb));
                }
                return _deviceContext;
            }
            set { _deviceContext = value; }
        }

        public static Size GetTextDimension(System.Drawing.Font font, string text, System.Drawing.SizeF textSize) {
            return GetTextDimension(DeviceContext, font, text, GdiConverter.GetDefaultStringFormat(), textSize);
        }

        public static Size GetTextDimension(System.Drawing.Font font, string text,
            System.Drawing.StringFormat stringFormat,
            System.Drawing.SizeF textSize) {

            return GetTextDimension(DeviceContext, font, text, stringFormat, textSize);
        }

        public static Size GetTextDimension(
            System.Drawing.Graphics g,
            System.Drawing.Font font,
            string text,
            System.Drawing.StringFormat stringFormat,
            System.Drawing.SizeF textSize) {
            return GDIConverter.Convert(g.MeasureString(text, font, textSize, stringFormat));
        }


        public static System.Drawing.Drawing2D.CustomLineCap getLineCapReverse(float arrowWidth, float arrowHeigth) {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            var p1 = new System.Drawing.PointF(0, -arrowWidth - 1);
            var p2 = new System.Drawing.PointF(-arrowHeigth / 2, 0);
            var p3 = new System.Drawing.PointF(arrowHeigth, 0);
            path.AddPolygon(new System.Drawing.PointF[3] { p3, p2, p1 });
            path.CloseAllFigures();
            //path.AddLine(p1, p2);
            //path.AddLine(p2, p3);
            //path.AddLine(p3, p1);

            var result = new System.Drawing.Drawing2D.CustomLineCap(path, null);
            result.BaseInset = 0;
            //result.StrokeJoin = LineJoin.Round;

            return result;

        }
    }
}