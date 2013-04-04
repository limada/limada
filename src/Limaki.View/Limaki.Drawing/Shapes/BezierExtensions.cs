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
            var cps = new Point[(bezierPoints.Count-1)/3*2 ];
            var cpCount = 0;
            for (int i = 1; i < bezierPoints.Count; i ++) {
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
    }
}