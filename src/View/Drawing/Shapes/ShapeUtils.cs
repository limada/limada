/*
 * Limaki 
 * Version 0.071
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Imaging;

namespace Limaki.Drawing.Shapes {
    public class ShapeUtils {
        /// <summary>
        /// Normalize rectangle so, that location becomes top-left point of rectangle 
        /// and size becomes positive
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Rectangle NormalizedRectangle(Point start, Point end) {
            return Rectangle.FromLTRB(
                Math.Min(start.X, end.X),
                Math.Min(start.Y, end.Y),
                Math.Max(start.X, end.X),
                Math.Max(start.Y, end.Y));
        }
        public static Rectangle NormalizedRectangle(Rectangle rect) {
            int rectX = rect.X;
            int rectR = rectX + rect.Width;
            int rectY = rect.Y;
            int rectB = rectY + rect.Height;
            int minX = rectX;
            int maxX = rectR;
            if (rectX > rectR) {
                minX = rectR;
                maxX = rectX;
            }
            int minY = rectY;
            int maxY = rectB;
            if (rectY > rectB) {
                minY = rectB;
                maxY = rectY;
            }
            return Rectangle.FromLTRB(minX, minY, maxX, maxY);
        }
        public static RectangleF NormalizedRectangle(RectangleF rect) {
            float rectX = rect.X;
            float rectR = rectX + rect.Width;
            float rectY = rect.Y;
            float rectB = rectY + rect.Height;
            float minX = rectX;
            float maxX = rectR;
            if (rectX > rectR) {
                minX = rectR;
                maxX = rectX;
            }
            float minY = rectY;
            float maxY = rectB;
            if (rectY > rectB) {
                minY = rectB;
                maxY = rectY;
            }
            return RectangleF.FromLTRB(minX,minY,maxX,maxY);
                
        }
        protected static void TrimTargetToSourceRectangle(ref Rectangle target, ref Rectangle source) {
            target.Location = new Point(
                    Math.Max(target.Location.X, source.Location.X),
                    Math.Max(target.Location.Y, source.Location.Y));
            target.Size = new Size(
                Math.Min(target.Size.Width, source.Size.Width - target.Location.X),
                Math.Min(target.Size.Height, source.Size.Height - target.Location.Y));
        }


        private static System.Drawing.Graphics _deviceContext = null;
        public static Graphics DeviceContext {
            get {
                if (_deviceContext == null) {
                    _deviceContext = (System.Drawing.Graphics)Graphics.FromImage(
                        new Bitmap(1, 1, (PixelFormat)PixelFormat.Format32bppArgb));
                }
                return _deviceContext;
            }
            set { _deviceContext = value; }
        }

        public static StringFormat GetDefaultStringFormat() {
            StringFormat stringFormat = StringFormat.GenericTypographic;
            stringFormat.Trimming = StringTrimming.EllipsisWord;
            //stringFormat.FormatFlags = StringFormatFlags.FitBlackBox;
            stringFormat.FormatFlags = stringFormat.FormatFlags
                            & ~StringFormatFlags.NoClip & ~StringFormatFlags.FitBlackBox 
                            & StringFormatFlags.LineLimit
                            ;
            return stringFormat;
        }
        public static SizeF GetTextDimension(Font font, string text, SizeF textSize) {
            return GetTextDimension(DeviceContext, font, text, GetDefaultStringFormat(), textSize);
        }

        public static SizeF GetTextDimension(Font font, string text, StringFormat stringFormat, SizeF textSize) {
            return GetTextDimension(DeviceContext, font, text, stringFormat, textSize);
        }
        public static SizeF GetTextDimension(Graphics g, Font font, string text, StringFormat stringFormat, SizeF textSize) {
            
            return g.MeasureString(text, font, textSize, stringFormat);
        }


        /// <summary>
        /// Replaces RectangleF.Contains as it gives wrong 
        /// results with x,y < 0 and right, bottom > 0
        /// </summary>
        /// <param name="value"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool Contains ( RectangleF value, RectangleF other ) {
            return 
                other.X >= value.X && 
                other.X+other.Width <= value.X+value.Width &&
                other.Y >= value.Y && 
                other.Y+other.Height <= value.Y+value.Height;
        }

        public static bool Intersects ( RectangleF value, RectangleF other ) {
        //    return !( left > other.right || right < other.left ||
        //top > other.bottom || bottom < other.top );

            return !( value.X > other.X + other.Width ||
                ( value.X + value.Width ) < ( other.X ) ||
                ( value.Y > other.Y + other.Height ) ||
                ( value.Y + value.Height ) < ( other.Y ) );
        }
    }
}
