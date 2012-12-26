using System;
using System.Collections.Generic;

namespace Limaki.Common {
    public abstract class DataComparer<T> : Comparer<T> {
        public virtual int CompareObjects(object aData, object bData) {
            if (aData == null && bData == null)
                return 0;

            if (aData == null || bData == null)
                return -1;

            if (aData is Int32 && bData is Int32) {
                var numbera = (Int32)(object)aData;
                var numberb = (Int32)(object)bData;
                return numbera.CompareTo(numberb);
            }
            if (aData is Int64 && bData is Int64) {
                var numbera = (Int64) (object) aData;
                var numberb = (Int64) (object) bData;
                return numbera.CompareTo(numberb);
            }
            return aData.ToString().CompareTo(bData.ToString());
        }

        public override int Compare(T a, T b) {
            var aData = GetData(a);
            var bData = GetData(b);
            return CompareObjects(aData, bData);

        }

        protected abstract object GetData(T item);
    }
}