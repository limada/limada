/*
 * Limaki 
 * Version 0.063
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */


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
