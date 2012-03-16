using System.Collections.Generic;
using Xwt;

namespace Limaki.Drawing {

    public enum PointOrder {
        Left,
        Top,
        LeftToRight,
        TopToBottom
    }

    public class PointComparer : Comparer<Point> {

        public PointOrder Order { get;set;}
        public double Delta { get; set; }

        public PointComparer() {
            Order = PointOrder.LeftToRight;
            Delta = 10d;
        }

        public double Round(double x) {
            return (int)(x / Delta) * Delta; // Floor
        }
        
        public Point Round(Point p) {
            return new Point(Round(p.X),Round(p.Y));
        }

        public override int Compare(Point a, Point b) {
            if (Order == PointOrder.LeftToRight || Order == PointOrder.TopToBottom) {
                var aX = Round(a.X);
                var bX = Round (b.X);
                var aY = Round(a.Y);
                var bY = Round(b.Y);
                if (Order == PointOrder.LeftToRight)
                    if (aY == bY)
                        return aX.CompareTo(bX);
                    else
                        return aY.CompareTo(bY);
                if (Order == PointOrder.TopToBottom)
                    if (aX == bX)
                        return aY.CompareTo(bY);
                    else
                        return aX.CompareTo(bX);
            }
            if (Order == PointOrder.Left) {
                var aX = Round (a.X);
                var bX = Round (b.X);
                return aX.CompareTo(bX);
            }
            if (Order == PointOrder.Top) {
                var aY = Round(a.Y);
                var bY = Round(b.Y);
                return aY.CompareTo(bY);
            }
            return 0;
        }
    }
}