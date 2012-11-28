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

using System.Collections.Generic;
using Limaki.Graphs;
using Xwt;
using System.Linq;
using System;
using Limaki.Common.Linqish;
using Limaki.Drawing;

namespace Limaki.View.Layout {

    public static class AllignerExtensions {

        public static AllignerOptions Options<TItem, TEdge> (this IGraphSceneLayout<TItem, TEdge> layout) where TEdge : IEdge<TItem>, TItem {
            var result = new AllignerOptions {
                Dimension = layout.Orientation == Orientation.LeftRight ? Dimension.X : Dimension.Y,
                Distance = layout.Distance,
                PointOrder = layout.Orientation == Orientation.LeftRight ? PointOrder.LeftToRight : PointOrder.TopToBottom,

            };
            if (layout.Orientation == Orientation.LeftRight) {
                result.AlignX = Alignment.Start;
                result.AlignY = layout.Centered ? Alignment.Center : Alignment.Start;
            } else {
                result.AlignX = layout.Centered ? Alignment.Center : Alignment.Start;
                result.AlignY = Alignment.Start;

            }
            return result;
        }

        public static void OneColumn<TItem, TEdge> (this Alligner<TItem, TEdge> alligner, IEnumerable<TItem> items, Point at) where TEdge : IEdge<TItem>, TItem {
            alligner.OneColumn(items, at, alligner.Layout.Options());
        }

        public static void OneColumn<TItem, TEdge> (this Alligner<TItem, TEdge> alligner, IEnumerable<TItem> items) where TEdge : IEdge<TItem>, TItem {
            if (items.Count() < 2)
                return;
            alligner.OneColumn(items, alligner.Locator.GetLocation(items.First()), alligner.Layout.Options());
        }
    }

    public static class LocatorExtensions {
        public static Rectangle Bounds<TItem> (this ILocator<TItem> loc, IEnumerable<TItem> items) {
            Action<TItem> visit = null;
            var measure = new MeasureVisitBuilder<TItem>(loc);
            var fBounds = measure.Bounds(ref visit);
            items.ForEach(e => visit(e));
            return fBounds();
        }
    }

}