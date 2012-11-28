using System.Collections.Generic;
using Xwt;

namespace Limaki.Drawing {

    public enum PointOrder {
        X,  // same as Dimension ?
        Y,  // same as Dimension ?
        /// <summary>
        /// round(Y); if(round(Y)==round(X)) then X
        /// </summary>
        TopToBottom,
        /// <summary>
        /// round(X); if(round(X)==round(Y)) then Y
        /// </summary>
        LeftToRight
    }

    public class PointComparer : Comparer<Point> {

        public PointOrder Order { get;set;}
        public double Delta { get; set; }

        public PointComparer() {
            Order = PointOrder.TopToBottom;
            Delta = 10d;
        }

        public double Round(double x) {
            return (int)(x / Delta) * Delta; // Floor
        }
        
        public Point Round(Point p) {
            return new Point(Round(p.X),Round(p.Y));
        }

        public override int Compare(Point a, Point b) {
            if (Order == PointOrder.TopToBottom || Order == PointOrder.LeftToRight) {
                var aX = Round(a.X);
                var bX = Round (b.X);
                var aY = Round(a.Y);
                var bY = Round(b.Y);
                if (Order == PointOrder.TopToBottom)
                    if (aY == bY)
                        return aX.CompareTo(bX);
                    else
                        return aY.CompareTo(bY);
                if (Order == PointOrder.LeftToRight)
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