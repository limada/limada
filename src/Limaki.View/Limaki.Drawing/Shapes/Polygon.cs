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
using System.Linq;
using Xwt;
using Xwt.Drawing;

namespace Limaki.Drawing.Shapes {
#if !SILVERLIGHT
    [Serializable]
#endif

    public struct Polygon {

        public Point[] Points { get; set; }

        public Polygon (Point[] points): this() {
            this.Points = points;
        }

        public void Transform (Matrix matrix) {
            matrix.Transform(Points);
        }

        #region Point in Polygon

        /* 
         * Reference:
         * 
         * Fast Winding Number Inclusion of a Point in a Polygon 
         * by Dan Sunday
         * 
         * http://www.geometryalgorithms.com/Archive/algorithm_0103/algorithm_0103.htm
         * 
         */

        /// <summary>
        /// test for a point in a polygon
        ///      Input:   p = a point,
        ///               v[] = vertex points of a polygon where v[0]==v[n]
        ///      Return:  winding = the winding number (=0 only if P is outside V[])
        /// </summary>
        /// <param name="p">the point to test</param>
        /// <param name="v">polygon points</param>
        /// <returns></returns>
        public static int WindingOfPoint (Point p, Point[] v) {
            int winding = 0;    // the winding number counter
            // loop through all edges of the polygon
            for (int i = 0; i < v.Length - 1; i++) {   // edge from V[0] to V[n]
                Winding(v[i], v[i + 1], p, ref winding);
            }
            // edge V[n] to V[0]
            Winding(v[v.Length - 1], v[0], p, ref winding);
            return winding;
        }

        private static void Winding (Point start, Point end, Point p, ref int wn) {
            var pY = p.Y;
            if (start.Y <= pY) {         // start y <= P.y
                if (end.Y > pY)      // an upward crossing
                    if (Vector.Orientation(start, end, p) > 0)  // P left of edge
                        ++wn;            // have a valid up intersect
            } else {                       // start y > P.y (no test needed)
                if (end.Y <= pY)     // a downward crossing
                    if (Vector.Orientation(start, end, p) < 0)  // P right of edge
                        --wn;            // have a valid down intersect
            }
        }

        public int WindingOfPoint (Point p) {
            return WindingOfPoint(p, this.Points);
        }

        #endregion

        public static bool Intersect (Point p, Point[] v) { return WindingOfPoint(p, v) != 0; }

        #region ConvexHull
        /// <summary>
        /// TODO: we have this already, take one of it
        /// 2D cross product of OA and OB vectors
        /// Returns a positive value, if OAB makes a counter-clockwise turn,
        /// negative for clockwise turn, and zero if the points are collinear. 
        /// </summary>
        /// <param name="O"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static double PointCrossing (Point O, Point A, Point B) {
            return (A.X - O.X) * (B.Y - O.Y) - (A.Y - O.Y) * (B.X - O.X);
        }

        /// <summary>
        /// Returns a list of points on the convex hull in counter-clockwise order
        /// see: http://en.wikibooks.org/wiki/Algorithm_Implementation/Geometry/Convex_hull/Monotone_chain
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="isSorted"></param>
        /// <returns></returns>
        public static Point[] AndrewsMonotonChainHull (Point[] polygon, bool isSorted) {
            var n = polygon.Count(); 
            var k = 0;
            var hull = new Point[2 * n];

            // Sort points lexicographically
            if (!isSorted)
                //P = P.OrderBy(p => p, new PointComparer{Order=PointOrder.X}).ToArray();
                Array.Sort<Point>(polygon, (a, b) => a.X == b.X ? a.Y.CompareTo(b.Y) : a.X.CompareTo(b.X));


            // Build lower hull
            for (int i = 0; i < n; i++) {
                while (k >= 2 && PointCrossing(hull[k - 2], hull[k - 1], polygon[i]) <= 0) k--;
                hull[k++] = polygon[i];
            }

            // Build upper hull
            for (int i = n - 2, t = k + 1; i >= 0; i--) {
                while (k >= t && PointCrossing(hull[k - 2], hull[k - 1], polygon[i]) <= 0) k--;
                hull[k++] = polygon[i];
            }

            Array.Resize(ref hull, k - 1);
            return hull;
        }


        #endregion
    }
}