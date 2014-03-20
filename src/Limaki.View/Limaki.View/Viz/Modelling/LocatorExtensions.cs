/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2012 - 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using Limaki.Common.Linqish;
using Limaki.Drawing;
using Xwt;

namespace Limaki.View.Viz.Modelling {

    public static class LocatorExtensions {

        public static Rectangle Bounds<TItem> (this ILocator<TItem> loc, IEnumerable<TItem> items) {
            Action<TItem> visit = null;
            var measure = new MeasureVisitBuilder<TItem>(loc);
            var fBounds = measure.Bounds(ref visit);
            items.ForEach(e => visit(e));
            return fBounds();
        }

        public static Size SizeToFit (this Size self, Size add,Dimension dimension) {
            var w = self.Width;
            var h = self.Height;
            
            if (dimension == Dimension.Y) {
                h += add.Height;
                if (w < add.Width)
                    w = add.Width;

            } else {
                w += add.Width;
                if (h < add.Height)
                    h = add.Height;
            }
            return new Size(w, h);
        }
    }
}