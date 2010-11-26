/*
 * Limaki 
 * Version 0.064
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

using System.Drawing;
using System;
using System.Collections.Generic;

namespace Limaki.Drawing.Shapes {
    public struct Vector {
        public Point Start;
        public Point End;
        public Vector(Point location, Size size) {
            this.Start = location;
            this.End = location + size;
        }
        public Vector(Point start, Point end) {
            this.Start = start;
            this.End = end;
        }

        public static double Angle(Vector v) {
            double dx = (v.End.X - v.Start.X);
            double dy = (v.End.Y - v.Start.Y);
            return Math.Atan(dy / dx) * (180 / Math.PI);// +((dy < 0) ? 180 : 0);
        }
        public void Transform(Matrice matrice) {
            Point[] p = { Start, End };
            matrice.TransformPoints(p);
            Start = p[0];
            End = p[1];
        }

        #region Hull
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
                new Point ((int)(startX - deltaSinusBeta), (int)(startY + deltaSinusAlpha)),
                new Point ((int)(startX + deltaSinusBeta), (int)(startY - deltaSinusAlpha)),
                new Point((int)(endX + deltaSinusBeta), (int)(endY - deltaSinusAlpha)),
                new Point ((int)(endX - deltaSinusBeta), (int)(endY + deltaSinusAlpha))
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
        public static int Orientation(Point start, Point end, Point p) {
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
        public int Orientation(Point p) {
            return Orientation (Start, End, p);
        }
    }
}