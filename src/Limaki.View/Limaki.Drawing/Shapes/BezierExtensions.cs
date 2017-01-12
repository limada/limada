/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */


using Xwt;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Limaki.Drawing.Shapes {
    public class BezierExtensions {

        public static Point[] GetRoundedRectBezier (Rectangle rect, double aoffset) {
            double grow = 0d;
            var r = new Rectangle(rect.X + grow, rect.Y + grow, rect.Width + grow, rect.Height + grow);
            // Create points for curve.
            double offset = 0;
            var start = new Point(r.Left, r.Top + (r.Height / 2));
            var control1 = new Point(r.Left - offset, r.Top - offset);
            var control2 = new Point(r.Left - offset, r.Top - offset);
            offset = 0;
            var end1 = new Point(r.Left + (r.Width / 2), r.Top);
            var control3 = new Point(r.Right + offset, r.Top - offset);
            var control4 = new Point(r.Right + offset, r.Top - offset);
            var end2 = new Point(r.Right, r.Top + (r.Height / 2));
            var control5 = new Point(r.Right + offset, r.Bottom + offset);
            offset = aoffset;
            var control6 = new Point(r.Right + offset, r.Bottom + offset);
            var end3 = new Point(r.Left + (r.Width / 2), r.Bottom);

            var control7 = new Point(r.Left - offset, r.Bottom + offset);
            offset = 0;
            var control8 = new Point(r.Left - offset, r.Bottom + offset);
            return new Point[] {
                                   start, 
                                   control1, control2, end1,
                                   control3, control4, end2,
                                   control5, control6, end3,
                                   control7, control8, start
                               };


        }

        public static Point[] ControlPoints (IList<Point> bezierPoints) {
            var cps = new Point[(bezierPoints.Count - 1) / 3 * 2];
            var cpCount = 0;
            for (int i = 1; i < bezierPoints.Count; i++) {
                if (i % 3 != 0) {
                    cps[cpCount] = bezierPoints[i];
                    cpCount++;
                }
            }
            return cps;
        }

        /// <summary>
        /// the convex hull of a bezier is the convex hull of its control points
        /// </summary>
        /// <param name="bezierPoints"></param>
        /// <returns></returns>
        public static Point[] BezierHull (IList<Point> bezierPoints) {
            return Polygon.AndrewsMonotonChainHull(bezierPoints.ToArray(), false);
        }


        /// <summary>
        /// The bezierPoint function evalutes quadratic bezier at point t for points a, b, c, d.
        /// The parameter t varies between 0 and 1. The a and d parameters are the
        /// on-curve points, b and c are the control points. 
        /// To make a two-dimensional  curve, call this function once with the x coordinates 
        /// and a second time with the y coordinates to get the location of a bezier curve at t.
        /// <remarks>converted from https://github.com/processing-js/processing-js </remarks>
        /// <param name="a">a coordinate of first point on the curve</param>
        /// <param name="b">b coordinate of first control point</param>
        /// <param name="c">c coordinate of second control point</param>
        /// <param name="d">d coordinate of second point on the curve</param>
        /// <param name="t">t value between 0 and 1</param>
        /// <returns></returns>
        /// </summary>
        public static double BezierPoint (double a, double b, double c, double d, double t) {
            return (1 - t) * (1 - t) * (1 - t) * a + 3 * (1 - t) * (1 - t) * t * b + 3 * (1 - t) * t * t * c + t * t * t * d;
        }

        /// <summary>
        /// The bezierPoint function evalutes quadratic bezier at point t for points a, b, c, d.
        /// The parameter t varies between 0 and 1. The a and d parameters are the
        /// on-curve points, b and c are the control points. 
        /// <remarks>converted from https://github.com/processing-js/processing-js </remarks>
        /// <param name="a">a coordinate of first point on the curve</param>
        /// <param name="b">b coordinate of first control point</param>
        /// <param name="c">c coordinate of second control point</param>
        /// <param name="d">d coordinate of second point on the curve</param>
        /// <param name="t">t value between 0 and 1</param>
        /// <returns></returns>
        /// </summary>
        public static Point BezierPoint (Point a, Point b, Point c, Point d, double t) {
            return new Point(
                BezierPoint(a.X, b.X, c.X, d.X, t),
                BezierPoint(a.Y, b.Y, c.Y, d.Y, t));
        }

        public static IEnumerable<BezierSegment> BezierSegments (IList<Point> bezier) {
            var pos = 0;
            while ((pos + 1) < bezier.Count()) {
                yield return new BezierSegment(bezier[pos], bezier[pos + 1], bezier[pos + 2], bezier[pos + 3]);
                pos += 3;
            }
        }

        public static Rectangle BezierBoundingBox (IList<Point> bezier) {
            var result = Rectangle.Zero;
            var first = true;
            foreach (var seg in BezierSegments(bezier)) {
                var bb = BezierBoundingBox(seg.Start, seg.Cp1, seg.Cp2, seg.End);
                if (first) {
                    result = bb;
                    first = false;
                } else
                    result = result.Union(bb);
            }
            return result;
        }

        public static Rectangle BezierBoundingBox (Point a, Point b, Point c, Point d) {
            var result = computeCubicBoundingBox(a.X, a.Y, b.X, b.Y, c.X, c.Y, d.X, d.Y);
            return Rectangle.FromLTRB(result[0], result[1], result[4], result[3]);
        }

        /// <summary>
        /// compute the value for the first derivative of the cubic bezier function at time=t
        /// http://processingjs.nihongoresources.com/bezierinfo/sketchsource.php?sketch=boundsCubicBezier
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double[] computeCubicFirstDerivativeRoots (double a, double b, double c, double d) {
            double[] ret = { -1, -1 };
            var tl = -a + 2 * b - c;
            var tr = -Math.Sqrt( (-a * (c - d) + b * b - b * (c + d) + c * c));
            var dn = -a + 3 * b - 3 * c + d;
            if (dn != 0) { ret[0] = (tl + tr) / dn; ret[1] = (tl - tr) / dn; }
            return ret;
        }

        /// <summary>
        /// compute the value for the second derivative of the cubic bezier function at time=t
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double computeCubicSecondDerivativeRoot (double a, double b, double c, double d) {
            double ret = -1;
            double tt = a - 2 * b + c;
            double dn = a - 3 * b + 3 * c - d;
            if (dn != 0) { ret = tt / dn; }
            return ret;
        }

        /// <summary>
        /// compute the value for the cubic bezier function at time=t
        /// http://processingjs.nihongoresources.com/bezierinfo/sketchsource.php?sketch=boundsCubicBezier
        /// </summary>
        /// <param name="t"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double computeCubicBaseValue (double t, double a, double b, double c, double d) {
            double mt = 1 - t;
            return mt * mt * mt * a + 3 * mt * mt * t * b + 3 * mt * t * t * c + t * t * t * d;
        }

        /// <summary>
        /// compute the derivative value for the cubic bezier function at time=t
        /// http://processingjs.nihongoresources.com/bezierinfo/sketchsource.php?sketch=boundsCubicBezier
        /// </summary>
        /// <param name="t"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double computeCubicDerivativeValue (double t, double a, double b, double c, double d) {
            double t2 = t * t, t6 = 6 * t, t12 = 2 * t6, t92 = t2 * 9, mt2 = (t - 1) * (t - 1);
            return 3 * d * t2 + c * (t6 - t92) + b * (t92 - t12 + 3) - 3 * a * mt2;
        }

        /// <summary>
        /// Compute the bounding box based on the straightened curve, for best fit
        /// </summary>
        /// <param name="xa"></param>
        /// <param name="ya"></param>
        /// <param name="xb"></param>
        /// <param name="yb"></param>
        /// <param name="xc"></param>
        /// <param name="yc"></param>
        /// <param name="xd"></param>
        /// <param name="yd"></param>
        /// <returns>left,top,left,bottom,right,bottom,right,top</returns>
        public static double[] computeCubicBoundingBox (double xa, double ya, double xb, double yb, double xc, double yc, double xd, double yd) {
            // find the zero point for x and y in the derivatives
            var minx = double.MaxValue;
            var maxx = double.MinValue;
            if (xa < minx) { minx = xa; }
            if (xa > maxx) { maxx = xa; }
            if (xd < minx) { minx = xd; }
            if (xd > maxx) { maxx = xd; }
            var ts = computeCubicFirstDerivativeRoots(xa, xb, xc, xd);
            for (int i = 0; i < ts.Length; i++) {
                var t = ts[i];
                if (t >= 0 && t <= 1) {
                    var x = computeCubicBaseValue(t, xa, xb, xc, xd);
                    var y = computeCubicBaseValue(t, ya, yb, yc, yd);
                    if (x < minx) { minx = x; }
                    if (x > maxx) { maxx = x; }
                }
            }

            var miny = double.MaxValue;
            var maxy = double.MinValue;
            if (ya < miny) { miny = ya; }
            if (ya > maxy) { maxy = ya; }
            if (yd < miny) { miny = yd; }
            if (yd > maxy) { maxy = yd; }
            ts = computeCubicFirstDerivativeRoots(ya, yb, yc, yd);
            for (int i = 0; i < ts.Length; i++) {
                var t = ts[i];
                if (t >= 0 && t <= 1) {
                    var x = computeCubicBaseValue(t, xa, xb, xc, xd);
                    var y = computeCubicBaseValue(t, ya, yb, yc, yd);
                    if (y < miny) { miny = y; }
                    if (y > maxy) { maxy = y; }
                }
            }

            // bounding box corner coordinates
            double[] bbox = { minx, miny, minx, maxy, maxx, maxy, maxx, miny };
            return bbox;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xa"></param>
        /// <param name="ya"></param>
        /// <param name="xb"></param>
        /// <param name="yb"></param>
        /// <param name="xc"></param>
        /// <param name="yc"></param>
        /// <param name="xd"></param>
        /// <param name="yd"></param>
        /// <returns></returns>
        public static double[] computeCubicTightBoundingBox (double xa, double ya, double xb, double yb, double xc, double yc, double xd, double yd) {
            // translate to 0,0
            var np2 = new Point(xb - xa, yb - ya);
            var np3 = new Point(xc - xa, yc - ya);
            var np4 = new Point(xd - xa, yd - ya);

            // get angle for rotation
            var angle = Math.Atan2(np4.X, np4.Y) - Math.PI; ;
            var cosAngleNeg = Math.Cos(-angle);
            var sinAngleNeg = Math.Sin(-angle);
            var cosAngle = Math.Cos(angle);
            var sinAngle = Math.Sin(angle);

            // rotate everything counter-angle so that it's aligned with the x-axis
            var rnp2 = new Point(np2.X * cosAngleNeg - np2.Y * sinAngleNeg, np2.X * sinAngleNeg + np2.Y * cosAngleNeg);
            var rnp3 = new Point(np3.X * cosAngleNeg - np3.Y * sinAngleNeg, np3.X * sinAngleNeg + np3.Y * cosAngleNeg);
            var rnp4 = new Point(np4.X * cosAngleNeg - np4.Y * sinAngleNeg, np4.X * sinAngleNeg + np4.Y * cosAngleNeg);

            // find the zero point for x and y in the derivatives
            var minx = double.MaxValue;
            var maxx = double.MinValue;
            if (0 < minx) { minx = 0; }
            if (0 > maxx) { maxx = 0; }
            if (rnp4.X < minx) { minx = rnp4.X; }
            if (rnp4.X > maxx) { maxx = rnp4.X; }
            var ts = computeCubicFirstDerivativeRoots(0, rnp2.X, rnp3.X, rnp4.X);
 
            for (int i = 0; i < ts.Length; i++) {
                var t = ts[i];
                if (t >= 0 && t <= 1) {
                    var x = (int) computeCubicBaseValue(t, 0, rnp2.X, rnp3.X, rnp4.X);
                    if (x < minx) { minx = x; }
                    if (x > maxx) { maxx = x; }
                }
            }

            var miny = double.MaxValue;
            var maxy = double.MinValue;

            if (0 < miny) { miny = 0; }
            if (0 > maxy) { maxy = 0; }
            ts = computeCubicFirstDerivativeRoots(0, rnp2.Y, rnp3.Y, 0);

            for (var i = 0; i < ts.Length; i++) {
                var t = ts[i];
                if (t >= 0 && t <= 1) {
                    var y = (int) computeCubicBaseValue(t, 0, rnp2.Y, rnp3.Y, 0);
                    if (y < miny) { miny = y; }
                    if (y > maxy) { maxy = y; }
                }
            }

            // bounding box corner coordinates
            var bb1 = new Point(minx, miny);
            var bb2 = new Point(minx, maxy);
            var bb3 = new Point(maxx, maxy);
            var bb4 = new Point(maxx, miny);

            // rotate back
            var nbb1 = new Point(bb1.X * cosAngle - bb1.Y * sinAngle, bb1.X * sinAngle + bb1.Y * cosAngle);
            var nbb2 = new Point(bb2.X * cosAngle - bb2.Y * sinAngle, bb2.X * sinAngle + bb2.Y * cosAngle);
            var nbb3 = new Point(bb3.X * cosAngle - bb3.Y * sinAngle, bb3.X * sinAngle + bb3.Y * cosAngle);
            var nbb4 = new Point(bb4.X * cosAngle - bb4.Y * sinAngle, bb4.X * sinAngle + bb4.Y * cosAngle);

            // move back
            nbb1.X += xa; nbb1.Y += ya;
            nbb2.X += xa; nbb2.Y += ya;
            nbb3.X += xa; nbb3.Y += ya;
            nbb4.X += xa; nbb4.Y += ya;

            double[] bbox = { nbb1.X, nbb1.Y, nbb2.X, nbb2.Y, nbb3.X, nbb3.Y, nbb4.X, nbb4.Y };
            return bbox;
        }


        
    }
}