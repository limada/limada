using System;
using System.Collections.Generic;

namespace Limaki.Drawing.Shapes {
    public class LeftRightTopBottomComparer : Comparer<PointI> {
        public int Round(int x) {
            var d = 10f;
            return Convert.ToInt32((x / d));
        }

        public override int Compare(PointI a, PointI b) {

            var aX = Round(a.X);
            var aY = Round(a.Y);
            var bX = Round(b.X);
            var bY = Round(b.Y);
            if (aY == bY)
                return aX.CompareTo(bX);
            else
                return aY.CompareTo(bY);

        }
    }
}