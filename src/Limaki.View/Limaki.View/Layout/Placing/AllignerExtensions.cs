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

    public static class AlignerExtensions {

        public static AlignerOptions Options<TItem, TEdge> (this IGraphSceneLayout<TItem, TEdge> layout) where TEdge : IEdge<TItem>, TItem {
            var result = new AlignerOptions {
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

        public static void OneColumn<TItem, TEdge> (this Aligner<TItem, TEdge> aligner, IEnumerable<TItem> items, Point at) where TEdge : IEdge<TItem>, TItem {
            aligner.OneColumn(items, at, aligner.Layout.Options());
        }

        public static void OneColumn<TItem, TEdge> (this Aligner<TItem, TEdge> aligner, IEnumerable<TItem> items) where TEdge : IEdge<TItem>, TItem {
            if (items.Count() < 2)
                return;
            aligner.OneColumn(items, aligner.Locator.GetLocation(items.First()), aligner.Layout.Options());
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