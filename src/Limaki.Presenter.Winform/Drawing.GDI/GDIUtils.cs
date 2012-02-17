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


namespace Limaki.Drawing.GDI {

    public class GDIUtils {

        public static void SetFontStyle(Font toolkit, System.Drawing.FontStyle native) {
            if (toolkit == null) return;

            if ((native & System.Drawing.FontStyle.Italic) != 0) {
                toolkit.Style |= FontStyle.Italic;
            } 
            
            if ((native & System.Drawing.FontStyle.Underline) != 0) {
                toolkit.Style |= FontStyle.Underline;
            } 

            if ((native & System.Drawing.FontStyle.Bold) != 0) {
                toolkit.Style |= FontStyle.Bold;
            };

            if (native == System.Drawing.FontStyle.Regular) {
                toolkit.Style = FontStyle.Normal;
            }
        }

        public static void SetFont(Font toolkit, System.Drawing.Font native) {
            SetFontStyle (toolkit, native.Style);
            toolkit.Size = native.Size;
            toolkit.Family = native.Name;
        }

        public static System.Drawing.FontStyle GetFontStyle(Font toolkit) {
            var result = System.Drawing.FontStyle.Regular;
            if (toolkit == null) return result;
            return GDIConverter.Convert (toolkit.Style);
        }

        public static void SetToolkitPen(Pen toolkit, System.Drawing.Pen native) {
            
        }

        public static void SetNativePen(Pen toolkit, System.Drawing.Pen native) {
            if (native == null || toolkit == null)
                return;
            native.Color = GDIConverter.Convert(toolkit.Color);
            native.StartCap = GDIConverter.Convert(toolkit.StartCap);
            native.EndCap = GDIConverter.Convert(toolkit.EndCap);
            native.Width = (float)toolkit.Thickness;
            native.LineJoin = GDIConverter.Convert(toolkit.LineJoin);
            native.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
            if (toolkit.CustomEndCap!=null)
                native.CustomEndCap = 
                    toolkit.CustomEndCap as System.Drawing.Drawing2D.CustomLineCap ;
            if (toolkit.CustomStartCap != null)
                native.CustomStartCap = 
                    toolkit.CustomStartCap as System.Drawing.Drawing2D.CustomLineCap;
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

        public static System.Drawing.StringFormat GetDefaultStringFormat() {
            var stringFormat =
                System.Drawing.StringFormat.GenericTypographic;
            stringFormat.Trimming = System.Drawing.StringTrimming.EllipsisWord;
            //stringFormat.FormatFlags = StringFormatFlags.FitBlackBox;
            stringFormat.FormatFlags = stringFormat.FormatFlags
                                       & ~System.Drawing.StringFormatFlags.NoClip
                                       & ~System.Drawing.StringFormatFlags.FitBlackBox
                                       & System.Drawing.StringFormatFlags.LineLimit
                ;
            return stringFormat;
        }

        public static SizeS GetTextDimension(System.Drawing.Font font, string text, System.Drawing.SizeF textSize) {
            return GetTextDimension(DeviceContext, font, text, GetDefaultStringFormat(), textSize);
        }

        public static SizeS GetTextDimension(System.Drawing.Font font, string text,
            System.Drawing.StringFormat stringFormat,
            System.Drawing.SizeF textSize) {

            return GetTextDimension(DeviceContext, font, text, stringFormat, textSize);
        }

        public static SizeS GetTextDimension(
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