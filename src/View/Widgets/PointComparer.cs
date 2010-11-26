using System.Collections.Generic;
using System.Drawing;

namespace Limaki.Drawing {
    public class PointComparer:IComparer<Point> {

        #region IComparer<Point> Member

        public int Compare(Point a, Point b) {
            int result = a.X.CompareTo (b.X);
            if (result == 0) {
                result = a.Y.CompareTo (b.Y);
            }
            return result;
        }

        #endregion
    }
}