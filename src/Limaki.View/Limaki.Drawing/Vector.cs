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
using System.Globalization;
using Xwt;
using Xwt.Drawing;

namespace Limaki.Drawing {
#if !SILVERLIGHT
    [Serializable]
#endif
    public struct Vector {

        public Point Start;
        public Point End;

        public double Dx { get => End.X - Start.X; set => End.X = Start.X + value; }
        public double Dy { get => End.Y - Start.Y; set => End.Y = Start.Y + value; }

        public Vector(Point location, Size size) {
            this.Start = location;
            this.End = location + size;
        }

        public Vector(Point start, Point end) {
            this.Start = start;
            this.End = end;
        }

        const double rad = 180d / Math.PI;
        public static double Angle(Vector v, int precision = -1) {
            var dx = (v.End.X - v.Start.X);
            var dy = (v.End.Y - v.Start.Y);
            if (dx + dy == 0d)
                return 0d;
            var a = Math.Atan(dy / dx) * rad;
            return precision >= 0 ? Math.Round (a, precision) : a;
        }

        public static Vector SetAngle (Vector v, double angle, int precision = -1) {
            var length = Length (v);
            var vector = new Vector (v.Start, v.Start);
            if (angle == 0d) {
                vector.Dx = length;
                return vector;
            } else if (angle == 90d) {
                vector.Dy = length;
                return vector;
            } 
            //else if (alpha == 180d) {
            //    vector.DX = -length;
            //    return vector;
            //} else if (alpha == 270d) {
            //    vector.DY = -length;
            //    return vector;
            //}

            var beta =  angle / rad;
            var a = length * Math.Cos (beta);
            var b = Math.Sqrt (length * length - a * a);
            return new Vector (v.Start, new Point (v.Start.X + a, v.Start.Y + b));
        }

        public static double Length(Vector v, int precision = -1) {
            if ((v.Dx + v.Dy) == 0d)
                return 0;
            var a = (v.End.X - v.Start.X);
            var b = (v.End.Y - v.Start.Y);
            var l = Math.Sqrt (a * a + b * b);
            return precision >= 0 ? Math.Round (l, precision) : l;
        }

        public static Vector SetLength (Vector v, double length, int precision = -1) {
            if (length == 0d)
                return new Vector (v.Start, v.Start);
            var oldL = Length (v);
            if (oldL == 0d)
                return new Vector (v.Start, new Point (v.End.X + length, v.Start.Y));

            var dX = (v.Dx * length) / oldL; // do not shorten this to factor; gives rounding errors
            var dY = (v.Dy * length) / oldL; // do not shorten this to factor; gives rounding errors
            if (precision >= 0) {
                dX = Math.Round (dX, precision);
                dY = Math.Round (dY, precision);
            }
            return new Vector (v.Start, new Point (v.Start.X + dX, v.Start.Y + dY));
        }

        public static Vector Normalize (Vector v) {
            var length = Length (v);
            if (length == 0d)
                return SetLength (v, 1d);
            var dX = v.Dx / length;
            var dY = v.Dy / length;
            return new Vector (v.Start, new Point (v.Start.X + dX, v.Start.Y + dY));
        }

        public void Transform(Matrix matrix) {
            if (matrix != null && !matrix.IsIdentity) {
                Point[] p = { Start, End };
                matrix.Transform (p);
                Start = p[0];
                End = p[1];
            }
        }

        #region Hull

        public static Point[] Hull(
            double startX, double startY,
            double endX, double endY,
            double deltaX, double deltaY) {

            double deltaSinusAlpha = 0;
            double deltaSinusBeta = 0;

            double sinusAlpha = 0;
            double sinusBeta = 0;

            double a = endX - startX;
            double b = endY - startY;

            if (a == 0d) { // vertical line
                sinusAlpha = 0;
                if (b > 0d) {
                    deltaSinusBeta = deltaY;
                    sinusBeta = 1;
                } else {
                    deltaSinusBeta = -deltaY;
                    sinusBeta = -1;
                }
            } else if (b == 0d) { // horizontal line
                sinusBeta = 0;
                if (a > 0d) {
                    deltaSinusAlpha = deltaY;
                    sinusAlpha = 1;
                } else {
                    deltaSinusAlpha = -deltaY;
                    sinusAlpha = -1;
                }
            } else {
                // calculation of the hypotenuse:
                double c = Math.Sqrt((a * a + b * b));

                sinusAlpha = (a / c);
                sinusBeta = (b / c);

                // calculation of Sinus Alpha and Beta, factorized with delta:
                deltaSinusAlpha = (deltaY * sinusAlpha);
                deltaSinusBeta = (deltaY * sinusBeta);
            }

            if (deltaX != 0) {
                // extending the original line to make it longer:
                startX = startX - sinusAlpha * deltaX;
                startY = startY - sinusBeta * deltaX;
                endX = endX + sinusAlpha * deltaX;
                endY = endY + sinusBeta * deltaX;
            }

            return new Point[] {
                                   new Point ((startX - deltaSinusBeta), (startY + deltaSinusAlpha)),
                                   new Point ((startX + deltaSinusBeta), (startY - deltaSinusAlpha)),
                                   new Point((endX + deltaSinusBeta), (endY - deltaSinusAlpha)),
                                   new Point ((endX - deltaSinusBeta), (endY + deltaSinusAlpha))
                               };
        }

        public static Point[] Hull(double startX, double startY, double endX, double endY, int delta, bool extend) {
            double deltaSinusAlpha = 0;
            double deltaSinusBeta = 0;

            double a = endX - startX;
            double b = endY - startY;

            if (a == 0d) {
                if (b > 0d) {
                    deltaSinusBeta = delta;
                } else {
                    deltaSinusBeta = -delta;
                }
            } else if (b == 0d) {
                if (a > 0d) {
                    deltaSinusAlpha = delta;
                } else {
                    deltaSinusAlpha = -delta;
                }
            } else {
                // calculation of the hypotenuse:
                double c = Math.Sqrt((a * a + b * b));

                // calculation of Sinus Alpha and Beta, factorized with delta:
                deltaSinusAlpha = (delta * (a / c));
                deltaSinusBeta = (delta * (b / c));
            }

            if (extend) {
                // extending the original line to make it longer:
                startX = startX - deltaSinusAlpha;
                startY = startY - deltaSinusBeta;
                endX = endX + deltaSinusAlpha;
                endY = endY + deltaSinusBeta;
            }

            return new Point[] {
                                   new Point ((startX - deltaSinusBeta), (startY + deltaSinusAlpha)),
                                   new Point ((startX + deltaSinusBeta), (startY - deltaSinusAlpha)),
                                   new Point((endX + deltaSinusBeta), (endY - deltaSinusAlpha)),
                                   new Point ((endX - deltaSinusBeta), (endY + deltaSinusAlpha))
                               };
        }

        public static Point[] Hull(Point start, Point end, int delta, bool extend) {
            return Hull(start.X, start.Y, end.X, end.Y, delta, extend);
        }

        public Point[] Hull(int delta, bool extend) {
            // get it near:
            double startX = Start.X;
            double startY = Start.Y;
            double endX = End.X;
            double endY = End.Y;
            return Hull (startX, startY, endX, endY, delta,extend);
        }

        public Point[] Hull(double deltaX, double deltaY) {
            // get it near:
            double startX = Start.X;
            double startY = Start.Y;
            double endX = End.X;
            double endY = End.Y;
            return Hull(startX, startY, endX, endY, deltaX, deltaY);
        }

        #endregion

        /// <summary>
        /// tests if a point is Left|On|Right of an infinite line.
        /// Input:  three points start, end, and p
        ///    Return: > 0 for p left of the line through start and end
        ///            = 0 for p on the line
        ///            < 0 for p right of the line
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static double Orientation(Point start, Point end, Point p) {
            var startX = start.X;
            var startY = start.Y;
            return ((end.X - startX) * (p.Y - startY)
                    - (p.X - startX) * (end.Y - startY));
        }

        /// <summary>
        /// tests if a point is Left|On|Right of the vector.
        ///    Return: > 0 for p left of the line through start and end
        ///            = 0 for p on the line
        ///            < 0 for p right of the line
        /// </summary>
        /// <param name="p">Point to test</param>
        /// <returns></returns>
        public double Orientation(Point p) {
            return Orientation (Start, End, p);
        }

        public override string ToString () {
            return $"{{Start={Start} End={End}}}";
        }
    }

    public static class VectorExtensions {
        
        public static Vector Normalize (this Vector v) => Vector.Normalize (v);
        public static double Length (this Vector v, int precision = -1) => Vector.Length (v, precision);
        public static Vector WithLength (this Vector v, double l, int precision = -1) => Vector.SetLength (v, l, precision);
        public static double Angle (this Vector v, int precision = -1) => Vector.Angle (v, precision);
        public static Vector WithAngle (this Vector v, double a, int precision = -1) => Vector.SetAngle (v, a, precision);

    }
}