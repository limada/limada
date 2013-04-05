using System;
using Xwt;
using System.Collections.Generic;
using Limaki.Drawing.Shapes;

namespace Limaki.Tests.Drawing {

    public class PolyAlgoPrototyper {

        #region SoftSurfer-Implementation
        // isLeft(): tests if a point is Left|On|Right of an infinite line.
        //    Input:  three points P0, P1, and P2
        //    Return: >0 for P2 left of the line through P0 and P1
        //            =0 for P2 on the line
        //            <0 for P2 right of the line
        //    See: the January 2001 Algorithm on Area of Triangles
        static double isLeftSSF (Point P0, Point P1, Point P2) {
            return (P1.X - P0.Y) * (P2.Y - P0.Y) - (P2.X - P0.X) * (P1.Y - P0.Y);
        }

        //===================================================================


        // chainHull_2D(): Andrew's monotone chain 2D convex hull algorithm
        //     Input:  poligon[] = an array of 2D points
        //     Return: hull[] = an array of the convex hull vertices
        // Copyright 2001, softSurfer (www.softsurfer.com)
        // This code may be freely used and modified for any purpose
        // providing that this copyright notice is included with it.
        public static Point[] AndrewsMonotonChainHullSSF (Point[] poligon, bool isSorted) {

            if (!isSorted)
                Array.Sort<Point>(poligon, (a, b) => a.X == b.X ? a.Y.CompareTo(b.Y) : a.X.CompareTo(b.X));

            int n = poligon.Length;
            var hull = new Point[poligon.Length];
            // the output array hull[] will be used as the stack
            int bot = 0, top = (-1); // indices for bottom and top of the stack
            int i; // array scan index

            // Get the indices of points with min x-coord and min|max y-coord
            int minmin = 0, minmax;
            var xmin = poligon[0].X;
            for (i = 1; i < n; i++)
                if (poligon[i].X != xmin)
                    break;
            minmax = i - 1;
            if (minmax == n - 1) {
                // degenerate case: all x-coords == xmin
                hull[++top] = poligon[minmin];
                if (poligon[minmax].Y != poligon[minmin].Y) // a nontrivial segment
                    hull[++top] = poligon[minmax];
                //no endpoint needed?
                hull[++top] = poligon[minmin]; // add polygon endpoint
                Array.Resize(ref hull, top + 1);//return top + 1;
                return hull;
            }

            // Get the indices of points with max x-coord and min|max y-coord
            int maxmin, maxmax = n - 1;
            var xmax = poligon[n - 1].X;
            for (i = n - 2; i >= 0; i--)
                if (poligon[i].X != xmax)
                    break;
            maxmin = i + 1;

            // Compute the lower hull on the stack hull
            hull[++top] = poligon[minmin]; // push minmin point onto stack
            i = minmax;
            while (++i <= maxmin) {
                // the lower line joins poligon[minmin] with poligon[maxmin]
                if (isLeftSSF(poligon[minmin], poligon[maxmin], poligon[i]) >= 0 && i < maxmin)
                    continue; // ignore poligon[i] above or on the lower line

                while (top > 0) // there are at least 2 points on the stack
                {
                    // test if poligon[i] is left of the line at the stack top
                    if (isLeftSSF(hull[top - 1], hull[top], poligon[i]) > 0)
                        break; // poligon[i] is a new hull vertex
                    else
                        top--; // pop top point off stack
                }
                hull[++top] = poligon[i]; // push poligon[i] onto stack
            }

            // Next, compute the upper hull on the stack hull above the bottom hull
            if (maxmax != maxmin) // if distinct xmax points
                hull[++top] = poligon[maxmax]; // push maxmax point onto stack
            bot = top; // the bottom point of the upper hull stack
            i = maxmin;
            while (--i >= minmax) {
                // the upper line joins poligon[maxmax] with poligon[minmax]
                if (isLeftSSF(poligon[maxmax], poligon[minmax], poligon[i]) >= 0 && i > minmax)
                    continue; // ignore poligon[i] below or on the upper line

                while (top > bot) // at least 2 points on the upper stack
                {
                    // test if poligon[i] is left of the line at the stack top
                    if (isLeftSSF(hull[top - 1], hull[top], poligon[i]) > 0)
                        break; // poligon[i] is a new hull vertex
                    else
                        top--; // pop top point off stack
                }
                hull[++top] = poligon[i]; // push poligon[i] onto stack
            }

            if (minmax != minmin)
                // no endpoint needed?
                hull[++top] = poligon[minmin]; // push joining endpoint onto stack

            Array.Resize(ref hull, top + 1);//return top + 1;
            return hull;
        }
        #endregion

        private IList<Point> BezierHull0 (IList<Point> bezier) {
            var cps = BezierExtensions.ControlPoints(bezier);

            var hull = PolyAlgoPrototyper.AndrewsMonotonChainHullSSF(cps, false);
            if (hull[0] != bezier[0]) {
                var extend = new Point[hull.Length + 2];
                Array.Copy(hull, 0, extend, 1, hull.Length);
                extend[0] = bezier[0];
                extend[extend.Length - 1] = bezier[bezier.Count - 1];
                hull = extend;
            }
            return hull;
        }
    }
}