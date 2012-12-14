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
                Dimension = layout.Dimension,
                Distance = layout.Distance,
                PointOrder = layout.Dimension == Dimension.X ? PointOrder.LeftToRight : PointOrder.TopToBottom,

            };
            if (layout.Dimension == Dimension.X) {
                result.AlignX = Alignment.Start;
                result.AlignY = layout.Centered ? Alignment.Center : Alignment.Start;
            } else {
                result.AlignX = layout.Centered ? Alignment.Center : Alignment.Start;
                result.AlignY = Alignment.Start;

            }
            if (layout.Dimension == Dimension.X)
                result.PointOrderDelta = layout.StyleSheet.AutoSize.Width / 2;
            else
                result.PointOrderDelta = layout.StyleSheet.AutoSize.Height / 2;
            return result;
        }

        public static void SetOptions<TItem, TEdge> (this IGraphSceneLayout<TItem, TEdge> layout, AlignerOptions options) where TEdge : IEdge<TItem>, TItem {
            layout.Dimension = options.Dimension;
            if (options.Distance.IsZero)
                options.Distance = layout.Distance;
            else
                layout.Distance = options.Distance;
            if (options.Dimension == Dimension.X) {
                layout.Centered = options.AlignX == Alignment.Center;
            } else {
                layout.Centered = options.AlignY == Alignment.Center;
            }

        }

        public static void OneColumn<TItem, TEdge> (this Aligner<TItem, TEdge> aligner, IEnumerable<TItem> items, Point at, AlignerOptions options) where TEdge : IEdge<TItem>, TItem {
            aligner.OneColumn(items, at, options);
        }

        public static void OneColumn<TItem, TEdge> (this Aligner<TItem, TEdge> aligner, IEnumerable<TItem> items, AlignerOptions options) where TEdge : IEdge<TItem>, TItem {
            if (items.Count() < 2)
                return;
            aligner.OneColumn(items, aligner.Locator.GetLocation(items.First()), options);
        }

        public static void AffectedEdges<TItem, TEdge> (this Aligner<TItem, TEdge> aligner, IEnumerable<TItem> items) where TEdge : IEdge<TItem>, TItem {
            Action<TItem> visit = null;
            aligner.AffectedEdges(ref visit);
            aligner.VisitItems(items, visit);
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