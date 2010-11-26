using System.Collections.Generic;

namespace Limaki.Drawing {
    public class PointComparer:IComparer<PointI> {

        #region IComparer<PointI> Member

        public int Compare(PointI a, PointI b) {
            int result = a.X.CompareTo (b.X);
            if (result == 0) {
                result = a.Y.CompareTo (b.Y);
            }
            return result;
        }

        #endregion
    }
}