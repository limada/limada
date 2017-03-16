/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2012 Lytico
 *
 * http://www.limada.org
 * 
 */

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

    public interface IDataComparer<T> : IComparer<T> { 
    }

    public class DataComparer<T, K> : Comparer<T>, IDataComparer<T> {

        public Comparer<K> KeyComparer { get; set; }

        public Func<T, K> Key { get; set; }

        public override int Compare (T x, T y) => KeyComparer.Compare (Key (x), Key (y));

    }
}