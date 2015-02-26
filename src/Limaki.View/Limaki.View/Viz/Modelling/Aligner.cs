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
using System.Linq;
using Limaki.Common.Linqish;
using Limaki.Drawing;
using Limaki.Graphs;
using Xwt;
using Xwt.Drawing;

namespace Limaki.View.Viz.Modelling {

    public class Aligner<TItem, TEdge> : Placer<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {

        public Aligner (IGraphScene<TItem, TEdge> data, IShaper<TItem> shaper) : base(data, shaper) { }
        public Aligner (IGraphScene<TItem, TEdge> data, IShaper<TItem> shaper, Action<Aligner<TItem, TEdge>> call) :
            base(data, shaper, p => call(p as Aligner<TItem, TEdge>)) { }

        public virtual void Justify (ref Action<TItem> visitor) {
            visitor += item => Locator.Justify(item);
        }

        public virtual void AffectedEdges (ref Action<TItem> visitor) {
            visitor += item => 
                Graph.Twig (item).ForEach (edge => Locator.AffectedEdges.Add (edge));
        }

        public virtual void Justify (IEnumerable<TItem> items) {
            Action<TItem> visit = null;

            Justify(ref visit);
            AffectedEdges(ref visit);

            VisitItems(items, visit);
        }

        public virtual void OneColumn (IEnumerable<TItem> items, Point at, AlignerOptions options) {
            var bounds = new Rectangle(int.MaxValue, int.MaxValue, 0, 0);
            MeasureColumn(items, options, ref bounds);
            if (bounds.Location == new Point (int.MaxValue, int.MaxValue)) // this happens if items have no shape
                bounds.Location = at;
            var locator = new LocateVisitBuilder<TItem>(this.Locator);
            LocateColumn(items, bounds, bounds, ref at, locator, options);
        }

        public virtual void OneColumn (IEnumerable<TItem> items, AlignerOptions options) {
            var comparer = new PointComparer { Order = options.PointOrder, Delta = options.PointOrderDelta };
            var colItems = items.OrderBy(item => Locator.GetLocation(item), comparer);
            var bounds = new Rectangle(int.MaxValue, int.MaxValue, 0, 0);
            MeasureColumn(colItems, options, ref bounds);
            var colPos = bounds.Location;
            var locator = new LocateVisitBuilder<TItem>(this.Locator);
            LocateColumn(colItems, bounds, bounds, ref colPos, locator, options);

        }

        public virtual void Columns (IEnumerable<TItem> items, AlignerOptions options) {
            var comparer = new PointComparer { Order = options.PointOrder, Delta = options.PointOrderDelta };

            var walk = items.Select(item => new { location = comparer.Round(Locator.GetLocation(item)), item });

            var bounds = new Rectangle(int.MaxValue, int.MaxValue, 0, 0);
            var cols = new Queue<Tuple<IEnumerable<TItem>, Rectangle>>();

            foreach (var col in walk.GroupBy(row => row.location.X).OrderBy(row => row.Key)) {
                var colItems = col.Select(r => r.item).OrderBy(item => Locator.GetLocation(item).Y);
                cols.Enqueue(MeasureColumn(colItems, options, ref bounds));
            }

            LocateColumns(cols, ref bounds, options);
        }


        public virtual void Columns (TItem root, IEnumerable<TItem> items, AlignerOptions options) {
            if (root == null || items == null)
                return;

            var itemCache = new HashSet<TItem>(items);
            if (itemCache.Count == 0)
                return;

            var walk = new Walker<TItem, TEdge> (this.Graph).DeepWalk (root, 1)
                .Where (l => !(l.Node is TEdge) && itemCache.Contains (l.Node))
                .ToArray ();

            var bounds = new Rectangle(int.MaxValue, int.MaxValue, 0, 0);
            Columns(walk, ref bounds, options);
        }

        public virtual void Columns (IEnumerable<LevelItem<TItem>> walk, ref Rectangle bounds, AlignerOptions options) {
            var cols = MeasureWalk(walk, ref bounds, options);
            LocateColumns(cols, ref bounds, options);
        }

        public void DequeColumn( Queue<Tuple<IEnumerable<TItem>, Rectangle>> cols,ref Rectangle bounds, AlignerOptions options ) {
            var rootCol = cols.Dequeue();
            if (options.Dimension == Dimension.X) {
                bounds.Location = new Point(
                    bounds.X + rootCol.Item2.Width + options.Distance.Width,
                    bounds.Y - AlignDelta(bounds.Height, rootCol.Item2.Height, options.AlignY));
            } else {
                bounds.Location = new Point(
                    bounds.X - AlignDelta(bounds.Width, rootCol.Item2.Width, options.AlignX),
                    bounds.Y + rootCol.Item2.Height + options.Distance.Height);
            }
        }

        public virtual Queue<Tuple<IEnumerable<TItem>, Rectangle>> MeasureWalk (IEnumerable<LevelItem<TItem>> walk, ref Rectangle bounds, AlignerOptions options) {
            var colPos = bounds.Location;

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

            if (colPos.X == int.MaxValue && colPos.Y == int.MaxValue)
                colPos = bounds.Location;
            else
                bounds.Location = colPos;

            if (options.Collisions.HasFlag(Collisions.NextFree) && !options.Collisions.HasFlag(Collisions.PerColumn)) {
                var ignore = walk.Select(i => i.Node).ToArray();
                var newbounds = NearestNextFreeSpace(bounds.Location, bounds.Size, ignore,
                    options.Collisions.HasFlag(Collisions.Toggle), options.Dimension, options.Distance);
                bounds = newbounds;
            }

            return cols;
        }

        public virtual void LocateColumns (Queue<Tuple<IEnumerable<TItem>, Rectangle>> cols, ref Rectangle bounds, AlignerOptions options) {
            var colPos = bounds.Location;
            var locate = new LocateVisitBuilder<TItem>(this.Locator);

            while (cols.Count > 0) {
                var col = cols.Dequeue();
                LocateColumn(col.Item1, col.Item2, bounds, ref colPos, locate, options);
            }
        }

        public virtual Tuple<IEnumerable<TItem>, Rectangle> MeasureColumn (IEnumerable<TItem> colItems, AlignerOptions options, ref Rectangle bounds) {

            Action<TItem> visit = null;

            var items = new Queue<TItem>();
            visit += i => items.Enqueue(i);
            AffectedEdges(ref visit);

            var measure = new MeasureVisitBuilder<TItem>(this.Locator);
            var fBounds = measure.Bounds(ref visit);
            var fSize = measure.SizeToFit(ref visit, options.Distance, options.Dimension);

            VisitItems(colItems, visit);

            var colBounds = fBounds();
            if (colBounds.IsEmpty)
                colBounds.Location = bounds.Location;
            colBounds.Size = fSize();

            bounds.Location = bounds.Location.Min(colBounds.Location);
            bounds.Size = bounds.Size.SizeToFit(colBounds.Size, options.Dimension);

            return new Tuple<IEnumerable<TItem>, Rectangle>(items, colBounds);

        }

        public double AlignDelta (double boundsSize, double size, Alignment alignement) {
            var delta = 0d;
            if (Alignment.Center == alignement)
                delta = (boundsSize - size) / 2;
            else if (Alignment.End == alignement)
                delta = (boundsSize - size);
            return delta;
        }

        protected virtual void LocateColumn (IEnumerable<TItem> colItems, Rectangle colBounds, Rectangle bounds, ref Point colPos, ILocateVisitBuilder<TItem> locate, AlignerOptions options) {
            Func<Point, Point> nextFree = pos =>
                options.Collisions.HasFlag(Collisions.PerColumn) ?
                    NearestNextFreeSpace(pos, colBounds.Size, colItems,
                        options.Collisions.HasFlag(Collisions.Toggle), options.Dimension, options.Distance).Location :
                pos;

            Action<TItem> visit = null;
            
            if (options.Dimension == Dimension.X) {
                colPos.Y = bounds.Y + AlignDelta(bounds.Height, colBounds.Height, options.AlignY);
                colPos = nextFree(colPos);
                locate.Locate(ref visit,
                               locate.Align(colPos.X, colBounds.Width, options.AlignX, Dimension.X),
                               locate.Location(colPos.Y, options.Distance.Height, Dimension.Y)
                    );
                VisitItems(colItems, visit);

                colPos.X += colBounds.Width + options.Distance.Width;
            } else {
                colPos.X = bounds.X + AlignDelta(bounds.Width, colBounds.Width, options.AlignX); ;
                colPos = nextFree(colPos);

                locate.Locate(ref visit,
                    locate.Location(colPos.X, options.Distance.Width, Dimension.X),
                    locate.Align(colPos.Y, colBounds.Height, options.AlignY, Dimension.Y)

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
                var bounds1 = (this.Locator.Bounds(this.Locator.ElementsIn(result)
                         .Where(e => !(e is TEdge))
                         .Except(ignore)));

                if (bounds.IsEmpty && bounds1.IsEmpty)
                    break;
                if (bounds.IsEmpty)
                    bounds = bounds1;
                else if (bounds1.IsEmpty)
                    bounds1 = bounds;
                else
                    bounds = bounds.Union(bounds1);

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
            if (result != best && toggleDimension) {
                dimension = dimension == Dimension.X ? Dimension.Y : Dimension.X;
                var prove = NextFreeSpace(start, sizeNeeded, ignore, dimension, distance);
                var nearest = best.Location.Nearest(new Point[] { result.Location, prove.Location });
                if (nearest == prove.Location)
                    result = prove;
            }
            return result;
        }
    }
}