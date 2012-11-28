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
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using System.Linq;
using System;
using Xwt;

namespace Limaki.View.Layout {

    public class Alligner<TItem, TEdge> : Placer<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {

        public Alligner (IGraphScene<TItem, TEdge> data, IGraphSceneLayout<TItem, TEdge> layout) : base(data, layout) { }
        public Alligner (IGraphScene<TItem, TEdge> data, IGraphSceneLayout<TItem, TEdge> layout, Action<Alligner<TItem, TEdge>> call) :
            base(data, layout, p => call(p as Alligner<TItem, TEdge>)) { }

        public virtual void Justify (ref Action<TItem> visitor) {
            visitor += item => Locator.Justify(item);
        }

        public virtual void AffectedEdges (ref Action<TItem> visitor) {
            visitor += item => {
                foreach (var edge in this.Graph.Twig(item))
                    Locator.AffectedEdges.Add(edge);
            };
        }

        public virtual void Justify (IEnumerable<TItem> items) {
            Action<TItem> visit = null;

            Justify(ref visit);
            AffectedEdges(ref visit);

            VisitItems(items, visit);
        }


        public virtual void OneColumn (IEnumerable<TItem> items, Point at, AllignerOptions options) {
            var bounds = new Rectangle(int.MaxValue, int.MaxValue, 0, 0);
            MeasureColumn(items, options, ref bounds);
            var locator = new LocateVisitBuilder<TItem>(this.Locator);
            LocateColumn(items, bounds, bounds, ref at, locator, options);
        }

        public virtual void OneColumn (IEnumerable<TItem> items, AllignerOptions options) {
            var comparer = new PointComparer { Order = options.PointOrder, Delta = Layout.StyleSheet.AutoSize.Width / 2 };
            var colItems = items.OrderBy(item => Locator.GetLocation(item), comparer);
            var bounds = new Rectangle(int.MaxValue, int.MaxValue, 0, 0);
            MeasureColumn(colItems, options, ref bounds);
            var colPos = bounds.Location;
            var locator = new LocateVisitBuilder<TItem>(this.Locator);
            LocateColumn(colItems, bounds, bounds, ref colPos, locator, options);

        }

        public virtual void Columns (IEnumerable<TItem> items, AllignerOptions options) {
            var comparer = new PointComparer { Order = options.PointOrder, Delta = Layout.StyleSheet.AutoSize.Width / 2 };

            var walk = items.Select(item => new { location = comparer.Round(Locator.GetLocation(item)), item });

            var bounds = new Rectangle(int.MaxValue, int.MaxValue, 0, 0);
            var cols = new Queue<Tuple<IEnumerable<TItem>, Rectangle>>();

            foreach (var col in walk.GroupBy(row => row.location.X).OrderBy(row => row.Key)) {
                var colItems = col.Select(r => r.item).OrderBy(item => Locator.GetLocation(item).Y);
                cols.Enqueue(MeasureColumn(colItems, options, ref bounds));
            }

            var locator = new LocateVisitBuilder<TItem>(this.Locator);
            var colPos = bounds.Location;
            while (cols.Count > 0) {
                var col = cols.Dequeue();
                LocateColumn(col.Item1, col.Item2, bounds, ref colPos, locator, options);
            }
        }

        public virtual void Columns (TItem root, IEnumerable<TItem> items, AllignerOptions options) {
            if (root == null || items == null)
                return;

            var itemCache = new HashSet<TItem>(items);
            if (itemCache.Count == 0)
                return;

            var walk = new Walker<TItem, TEdge>(this.Graph).DeepWalk(root, 1)
                .Where(l => !(l.Node is TEdge) && itemCache.Contains(l.Node));

            var bounds = new Rectangle(int.MaxValue, int.MaxValue, 0, 0);
            var cols = new Queue<Tuple<IEnumerable<TItem>, Rectangle>>();

            foreach (var col in walk.GroupBy(row => row.Level)) {
                if (col.Count() == 0)
                    continue;

                var colItems = col.Select(r => r.Node);
                if (options.Dimension == Dimension.X)
                    colItems = colItems.OrderBy(item => Locator.GetLocation(item).Y);
                else
                    colItems = colItems.OrderBy(item => Locator.GetLocation(item).X);

                cols.Enqueue(MeasureColumn(colItems, options, ref bounds));
            }

            var colPos = bounds.Location;
            LocateVisitBuilder<TItem> locate = null;
            if (false)
                locate = new CollissionResolver<TItem>(
                    this.Locator,
                    new GraphSceneLocationDetector<TItem, TEdge>(this.GraphScene),
                    itemCache,
                    options.Dimension,
                    options.Distance.Width);
            else
                locate = new LocateVisitBuilder<TItem>(this.Locator);

            while (cols.Count > 0) {
                var col = cols.Dequeue();
                LocateColumn(col.Item1, col.Item2, bounds, ref colPos, locate, options);

            }
        }

        protected virtual Tuple<IEnumerable<TItem>, Rectangle> MeasureColumn (IEnumerable<TItem> colItems, AllignerOptions options, ref Rectangle bounds) {

            Action<TItem> visit = null;

            var items = new Queue<TItem>();
            visit += i => items.Enqueue(i);
            AffectedEdges(ref visit);

            var measure = new MeasureVisitBuilder<TItem>(this.Locator);
            var fBounds = measure.Bounds(ref visit);
            var fSize = measure.SizeToFit(ref visit, options.Distance, options.Dimension);

            VisitItems(colItems, visit);

            var colBounds = fBounds();
            colBounds.Size = fSize();

            bounds.Location = bounds.Location.Min(colBounds.Location);
            bounds.Size = bounds.Size.Max(colBounds.Size);

            return new Tuple<IEnumerable<TItem>, Rectangle>(items, colBounds);

        }

        protected virtual void LocateColumn (IEnumerable<TItem> colItems, Rectangle colBounds, Rectangle bounds, ref Point colPos, ILocateVisitBuilder<TItem> locate, AllignerOptions options) {
            Action<TItem> visit = null;
            if (options.Dimension == Dimension.X) {
                if (Alignment.Center == options.AlignY)
                    colPos.Y = bounds.Y + (bounds.Height - colBounds.Height) / 2;
                else if (Alignment.End == options.AlignY)
                    colPos.Y = bounds.Y + (bounds.Height - colBounds.Height);

                locate.Locate(ref visit,
                               locate.Allign(colPos.X, colBounds.Width, options.AlignX, Dimension.X),
                               locate.Location(colPos.Y, options.Distance.Height, Dimension.Y)
                    );
                VisitItems(colItems, visit);

                colPos.X += colBounds.Width + options.Distance.Width;
            } else {
                if (Alignment.Center == options.AlignX)
                    colPos.X = bounds.X + (bounds.Width - colBounds.Width) / 2;
                else if (Alignment.End == options.AlignX)
                    colPos.X = bounds.X + (bounds.Width - colBounds.Width);

                locate.Locate(ref visit,
                    locate.Location(colPos.X, options.Distance.Width, Dimension.X),
                    locate.Allign(colPos.Y, colBounds.Height, options.AlignY, Dimension.Y)

                    );
                VisitItems(colItems, visit);

                colPos.Y += colBounds.Height + options.Distance.Height;
            }
        }

        public virtual Rectangle NextFreeSpace (Point start, Size sizeNeeded,
           IEnumerable<TItem> ignore, Dimension dimension, Size distance) {

            var locator = new GraphSceneItemShapeLocator<TItem, TEdge> { GraphScene = this.GraphScene };
            var result = new Rectangle(start, sizeNeeded);
            while (true) {
                var bounds = locator.Bounds(
                    this.GraphScene.ElementsIn(result)
                        .Where(e => !(e is TEdge))
                        .Except(ignore));

                if (bounds.IsEmpty)
                    break;

                if (dimension == Dimension.X)
                    start = new Point(bounds.Right + distance.Width, start.Y);
                else
                    start = new Point(start.X, bounds.Bottom + distance.Height);
                result = new Rectangle(start, sizeNeeded);
            }
            return result;
        }

        public virtual Rectangle NearestNextFreeSpace (Point start, Size sizeNeeded,
            IEnumerable<TItem> ignore, bool toggleDimension, Dimension dimension, Size distance) {

            var best = new Rectangle(start, sizeNeeded);
            var result = NextFreeSpace(start, sizeNeeded, ignore, dimension, distance);
            if (result != best) {
                if (toggleDimension) {
                    dimension = dimension == Dimension.X ? Dimension.Y : Dimension.X;
                }
                var prove = NextFreeSpace(start, sizeNeeded, ignore, dimension, distance);
                var nearest = best.Location.Nearest(new Point[] { result.Location, prove.Location });
                if (nearest == prove.Location)
                    result = prove;
            }
            return result;
        }
    }
}