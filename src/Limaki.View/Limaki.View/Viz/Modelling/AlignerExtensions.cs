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

using System.Collections.Generic;
using Limaki.Drawing;
using Limaki.Graphs;
using Xwt;
using System.Linq;
using System;
using Limaki.Common.Linqish;

namespace Limaki.View.Viz.Modelling {

    public static class AlignerExtensions {

        public static AlignerOptions Options<TItem, TEdge> (this IGraphSceneLayout<TItem, TEdge> layout) where TEdge : IEdge<TItem>, TItem {
            var result = new AlignerOptions {
                Dimension = layout.Dimension,
                Distance = layout.Distance,
                PointOrder = layout.Dimension == Dimension.X ? PointOrder.XY : PointOrder.YX,

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

        public static void FullLayout<TItem, TEdge> (this Aligner<TItem, TEdge> aligner, TItem focused, Point pos, AlignerOptions options, IComparer<TItem> comparer)
        where TEdge : IEdge<TItem>, TItem {

            var data = aligner.GraphScene;
            var shaper = aligner.Shaper;

            var roots = data.Graph.FindRoots(focused);
            if (comparer == null)
                roots = roots
                    .OrderBy(e => shaper.GetShape(e).Location, new PointComparer { Order = options.Dimension == Dimension.X ? PointOrder.Y : PointOrder.X });
            else
                roots = roots.OrderBy(e => e, comparer);

            var walker = new Walker<TItem, TEdge> (data.Graph);
            roots.ForEach (root => {
                var walk = new List<LevelItem<TItem>> ();
                walker.DeepWalk (root, 1)
                    .ForEach (l => {
                        if (l.Node is TEdge)
                            aligner.Locator.AffectedEdges.Add ((TEdge) l.Node);
                        else
                            walk.Add (l);
                    });
                var bounds = new Rectangle (pos, Size.Zero);
                aligner.Columns (walk, ref bounds, options);
                pos = options.Dimension == Dimension.X ?
                      new Point (pos.X, pos.Y + bounds.Size.Height + options.Distance.Height) :
                      new Point (pos.X + bounds.Size.Width + options.Distance.Width, pos.Y);
            });
        }

    }
}