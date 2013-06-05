/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2011-2013 Lytico
 *
 * http://www.limada.org
 * 
 */


using System.Collections.Generic;
using Xwt;

namespace Limaki.Drawing {

    public enum PointOrder {
        X,  // same as Dimension ?
        Y,  // same as Dimension ?
        /// <summary>
        /// compare Y; if(aY == bY) then compare X
        /// </summary>
        YX,
        /// <summary>
        /// compare X; if(aX == bX ) then compare Y
        /// </summary>
        XY
    }

    public class PointComparer : Comparer<Point> {

        public PointOrder Order { get;set;}
        public double Delta { get; set; }

        public PointComparer() {
            Order = PointOrder.YX;
            Delta = 10d;
        }

        public double Round(double x) {
            if (Delta == 0)
                return x;
            return (int)(x / Delta) * Delta; // Floor
        }
        
        public Point Round(Point p) {
            return new Point(Round(p.X),Round(p.Y));
        }

        public override int Compare(Point a, Point b) {
            if (Order == PointOrder.YX || Order == PointOrder.XY) {
                var aX = Round(a.X);
                var bX = Round (b.X);
                var aY = Round(a.Y);
                var bY = Round(b.Y);
                if (Order == PointOrder.YX)
                    if (aY == bY)
                        return aX.CompareTo(bX);
                    else
                        return aY.CompareTo(bY);
                if (Order == PointOrder.XY)
                    if (aX == bX)
                        return aY.CompareTo(bY);
                    else
                        return aX.CompareTo(bX);
            }
            if (Order == PointOrder.X) {
                var aX = Round (a.X);
                var bX = Round (b.X);
                return aX.CompareTo(bX);
            }
            if (Order == PointOrder.Y) {
                var aY = Round(a.Y);
                var bY = Round(b.Y);
                return aY.CompareTo(bY);
            }
            return 0;
        }
    }
}