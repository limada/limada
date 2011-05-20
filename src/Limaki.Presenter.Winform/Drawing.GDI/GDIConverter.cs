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


namespace Limaki.Drawing.GDI {
    public static class GDIConverter{
        public static System.Drawing.Rectangle Convert(RectangleI toolkit) {
            return new System.Drawing.Rectangle (toolkit.X, toolkit.Y, toolkit.Width, toolkit.Height);
        }

        public static System.Drawing.RectangleF Convert(RectangleS toolkit) {
            return new System.Drawing.RectangleF(toolkit.X, toolkit.Y, toolkit.Width, toolkit.Height);
        }

        public static RectangleI Convert(System.Drawing.Rectangle toolkit) {
            return new RectangleI(toolkit.X, toolkit.Y, toolkit.Width, toolkit.Height);
        }

        public static RectangleS Convert(System.Drawing.RectangleF toolkit) {
            return new RectangleS(toolkit.X, toolkit.Y, toolkit.Width, toolkit.Height);
        }

        public static System.Drawing.Point[] Convert(PointI[] toolkit) {
            return Array.ConvertAll<PointI, System.Drawing.Point>(toolkit, Convert);
        }

        public static System.Drawing.PointF[] Convert(PointS[] toolkit) {
            return Array.ConvertAll<PointS, System.Drawing.PointF>(toolkit, Convert);
        }

        public static PointS[] Convert(System.Drawing.PointF[] toolkit) {
            return Array.ConvertAll<System.Drawing.PointF, PointS>(toolkit, Convert);
        }

        public static System.Drawing.Point Convert(PointI toolkit) {
            return new System.Drawing.Point(toolkit.X, toolkit.Y);
        }


        public static System.Drawing.PointF Convert(PointS toolkit) {
            return new System.Drawing.PointF(toolkit.X, toolkit.Y);
        }

        public static PointI Convert(System.Drawing.Point toolkit) {
            return new PointI(toolkit.X, toolkit.Y);
        }

        public static PointS Convert(System.Drawing.PointF toolkit) {
            return new PointS(toolkit.X, toolkit.Y);
        }

        public static System.Drawing.Size Convert(SizeI toolkit) {
            return new System.Drawing.Size(toolkit.Width, toolkit.Height);
        }


        public static System.Drawing.SizeF Convert(SizeS toolkit) {
            return new System.Drawing.SizeF(toolkit.Width, toolkit.Height);
        }

        public static SizeI Convert(System.Drawing.Size toolkit) {
            return new SizeI(toolkit.Width, toolkit.Height);
        }

        public static SizeS Convert(System.Drawing.SizeF toolkit) {
            return new SizeS(toolkit.Width, toolkit.Height);
        }

        public static System.Drawing.Color Convert(Color color) {
            return System.Drawing.Color.FromArgb((int)color.ToArgb());
        }

        public static Color Convert(System.Drawing.Color color) {
            return Color.FromArgb((uint)color.ToArgb());
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
            if ((toolkit & FontStyle.Underline) != 0) {
                result |= System.Drawing.FontStyle.Underline;
            }
            if ((toolkit & FontStyle.Bold) != 0) {
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
            if ((native & System.Drawing.FontStyle.Underline) != 0) {
                result |= FontStyle.Underline;
            }
            if ((native & System.Drawing.FontStyle.Bold) != 0) {
                result |= FontStyle.Bold;
            }
            return result;
        }
    }
}