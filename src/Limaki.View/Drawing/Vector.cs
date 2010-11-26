/*
 * Limaki 
 * Version 0.08
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
using System.Collections.Generic;

namespace Limaki.Drawing {
#if !SILVERLIGHT
    [Serializable]
#endif
    public struct Vector {
        public PointI Start;
        public PointI End;

        public Vector(PointI location, SizeI size) {
            this.Start = location;
            this.End = location + size;
        }
        public Vector(PointI start, PointI end) {
            this.Start = start;
            this.End = end;
        }

        public static double Angle(Vector v) {
            double dx = (v.End.X - v.Start.X);
            double dy = (v.End.Y - v.Start.Y);
            return Math.Atan(dy / dx) * (180 / Math.PI);// +((dy < 0) ? 180 : 0);
        }

        public static double Length(Vector v) {
            double startX = v.Start.X;
            double startY = v.Start.Y;
            double endX = v.End.X;
            double endY = v.End.Y;
            double a = endX - startX;
            double b = endY - startY;
            return Math.Sqrt ( a*a + b*b );
        }

        public void Transform(Matrice matrice) {
            PointI[] p = { Start, End };
            matrice.TransformPoints(p);
            Start = p[0];
            End = p[1];
        }

        #region Hull

        public static PointI[] Hull(
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

            return new PointI[] {
                                   new PointI ((int)(startX - deltaSinusBeta), (int)(startY + deltaSinusAlpha)),
                                   new PointI ((int)(startX + deltaSinusBeta), (int)(startY - deltaSinusAlpha)),
                                   new PointI((int)(endX + deltaSinusBeta), (int)(endY - deltaSinusAlpha)),
                                   new PointI ((int)(endX - deltaSinusBeta), (int)(endY + deltaSinusAlpha))
                               };
        }

        public static PointI[] Hull(double startX, double startY, double endX, double endY, int delta, bool extend) {
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

            return new PointI[] {
                                   new PointI ((int)(startX - deltaSinusBeta), (int)(startY + deltaSinusAlpha)),
                                   new PointI ((int)(startX + deltaSinusBeta), (int)(startY - deltaSinusAlpha)),
                                   new PointI((int)(endX + deltaSinusBeta), (int)(endY - deltaSinusAlpha)),
                                   new PointI ((int)(endX - deltaSinusBeta), (int)(endY + deltaSinusAlpha))
                               };
        }

        public static PointI[] Hull(PointI start, PointI end, int delta, bool extend) {
            return Hull(start.X, start.Y, end.X, end.Y, delta, extend);
        }

        public PointI[] Hull(int delta, bool extend) {
            // get it near:
            double startX = Start.X;
            double startY = Start.Y;
            double endX = End.X;
            double endY = End.Y;
            return Hull (startX, startY, endX, endY, delta,extend);
        }

        public PointI[] Hull(double deltaX, double deltaY) {
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
        ///    Return: >0 for p left of the line through start and end
        ///            =0 for p on the line
        ///            <0 for p right of the line
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static int Orientation(PointI start, PointI end, PointI p) {
            int startX = start.X;
            int startY = start.Y;
            return ((end.X - startX) * (p.Y - startY)
                    - (p.X - startX) * (end.Y - startY));
        }

        /// <summary>
        /// tests if a point is Left|On|Right of the vector.
        ///    Return: >0 for p left of the line through start and end
        ///            =0 for p on the line
        ///            <0 for p right of the line
        /// </summary>
        /// <param name="p">Point to test</param>
        /// <returns></returns>
        public int Orientation(PointI p) {
            return Orientation (Start, End, p);
        }
    }
}