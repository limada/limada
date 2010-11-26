/*
 * Limaki 
 * Version 0.063
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
        public Point[] PolygonHull(int delta) {
            // get it near:
            double startX = Start.X;
            double startY = Start.Y;
            double endX = End.X;
            double endY = End.Y;

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

            // extending the original line to make it longer:
            startX = startX - deltaSinusAlpha;
            startY = startY - deltaSinusBeta;
            endX = endX + deltaSinusAlpha;
            endY = endY + deltaSinusBeta;


            return new Point[] {
                new Point ((int)(startX - deltaSinusBeta), (int)(startY + deltaSinusAlpha)),
                new Point ((int)(startX + deltaSinusBeta), (int)(startY - deltaSinusAlpha)),
                new Point((int)(endX + deltaSinusBeta), (int)(endY - deltaSinusAlpha)),
                new Point ((int)(endX - deltaSinusBeta), (int)(endY + deltaSinusAlpha))
                                             };
        }
    }
}