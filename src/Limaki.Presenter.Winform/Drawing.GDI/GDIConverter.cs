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
using Xwt;
using Xwt.Drawing;


namespace Limaki.Drawing.GDI {
    public static class GDIConverter{
        public static System.Drawing.Rectangle Convert(RectangleD toolkit) {
            return new System.Drawing.Rectangle((int)toolkit.X, (int)toolkit.Y, (int)toolkit.Width, (int)toolkit.Height);
        }

        public static System.Drawing.RectangleF ConvertF(RectangleD toolkit) {
            return new System.Drawing.RectangleF( (float) toolkit.X, (float) toolkit.Y, (float) toolkit.Width, (float) toolkit.Height );
        }

        public static RectangleD Convert(System.Drawing.Rectangle toolkit) {
            return new RectangleD(toolkit.X, toolkit.Y, toolkit.Width, toolkit.Height);
        }

        public static RectangleD Convert(System.Drawing.RectangleF toolkit) {
            return new RectangleD(toolkit.X, toolkit.Y, toolkit.Width, toolkit.Height);
        }

        public static System.Drawing.Point[] Convert(Point[] toolkit) {
            return Array.ConvertAll<Point, System.Drawing.Point>(toolkit, Convert);
        }

        public static System.Drawing.PointF[] ConvertF(Point[] toolkit) {
            return Array.ConvertAll<Point, System.Drawing.PointF>( toolkit, ConvertF );
        }

        public static Point[] Convert(System.Drawing.PointF[] toolkit) {
            return Array.ConvertAll<System.Drawing.PointF, Point>(toolkit, Convert);
        }

        public static System.Drawing.Point Convert(Point toolkit) {
            return new System.Drawing.Point((int)toolkit.X, (int)toolkit.Y);
        }


        public static System.Drawing.PointF ConvertF(Point toolkit) {
            return new System.Drawing.PointF( (float) toolkit.X, (float) toolkit.Y );
        }

        public static Point Convert(System.Drawing.Point toolkit) {
            return new Point(toolkit.X, toolkit.Y);
        }

        public static Point Convert(System.Drawing.PointF toolkit) {
            return new Point(toolkit.X, toolkit.Y);
        }

        public static System.Drawing.Size Convert(Size toolkit) {
            return new System.Drawing.Size((int)toolkit.Width, (int)toolkit.Height);
        }


        public static System.Drawing.SizeF ConvertF(Size toolkit) {
            return new System.Drawing.SizeF( (float) toolkit.Width, (float) toolkit.Height );
        }

        public static Size Convert(System.Drawing.Size toolkit) {
            return new Size(toolkit.Width, toolkit.Height);
        }

        public static Size Convert(System.Drawing.SizeF toolkit) {
            return new Size(toolkit.Width, toolkit.Height);
        }

        public static System.Drawing.Color Convert(Color color) {
            return System.Drawing.Color.FromArgb((int)color.ToArgb());
        }

        public static Color Convert(System.Drawing.Color color) {
            return DrawingExtensions.FromArgb((uint)color.ToArgb());
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

        public static System.Drawing.FontStyle Convert(FontStyle toolkit) {
            var result = System.Drawing.FontStyle.Regular;
            if (toolkit == null)
                return result;
            if ((toolkit & FontStyle.Italic) != 0) {
                result |= System.Drawing.FontStyle.Italic;
            }
            //if ((toolkit & FontStyle.Underline) != 0) {
            //    result |= System.Drawing.FontStyle.Underline;
            //}
            if ((toolkit & FontStyle.Oblique) != 0) {
                result |= System.Drawing.FontStyle.Bold;
            }
            return result;
        }

        public static FontStyle Convert(System.Drawing.FontStyle native) {
            var result = FontStyle.Normal;
            if (native == null)
                return result;
            if ((native & System.Drawing.FontStyle.Italic) != 0) {
                result |= FontStyle.Italic;
            }
            //if ((native & System.Drawing.FontStyle.Underline) != 0) {
            //    result |= FontStyle.Underline;
            //}
            if ((native & System.Drawing.FontStyle.Bold) != 0) {
                result |= FontStyle.Oblique;
            }
            return result;
        }
    }
}