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

namespace Limaki.Drawing.Shapes {
#if !SILVERLIGHT
    [Serializable]
#endif

    public struct Polygon {
        public PointI[] Points;
        public Polygon(PointI[] points) {
            this.Points = points;
        }

        public void Transform(Matrice matrice) {
            matrice.TransformPoints(Points);
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
        public static int WindingOfPoint(PointI p, PointI[] v) {
            int winding = 0;    // the winding number counter
            // loop through all edges of the polygon
            for (int i = 0; i < v.Length-1; i++) {   // edge from V[0] to V[n]
                Winding (v[i], v[i + 1], p, ref winding);
            }
            // edge V[n] to V[0]
            Winding(v[v.Length-1], v[0], p, ref winding);
            return winding;
        }

        private static void Winding(PointI start, PointI end, PointI p, ref int wn) {
            int pY = p.Y;
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

        public int WindingOfPoint(PointI p) {
            return WindingOfPoint (p, this.Points);
        }

        #endregion
        public static bool Intersect(PointI p, PointI[]v){ return WindingOfPoint (p, v) != 0; }
    }
}
