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

        public Alligner (IGraphScene<TItem, TEdge> data, IGraphSceneLayout<TItem, TEdge> layout) : base (data, layout) { }

        public virtual void Justify (ref Action<TItem> visitor) {
            visitor += item => Locator.Justify (item);
        }

        public virtual void AffectedEdges (ref Action<TItem> visitor) {
            visitor += item => {
                foreach (var edge in this.Graph.Twig (item))
                    Locator.AffectedEdges.Add (edge);
            };
        }

        public virtual void Justify (IEnumerable<TItem> items) {
            Action<TItem> visit = null;

            Justify (ref visit);
            AffectedEdges (ref visit);

            VisitItems (items, visit);
        }

        public virtual void OneColumn (IEnumerable<TItem> items, Point at, double distance) {
            var options = new AllignerOptions { Distance = new Size (distance, distance), AlignX = Alignment.End, AlignY = Alignment.Start, Dimension = Dimension.Y };
            var bounds = new Rectangle (int.MaxValue, int.MaxValue, 0, 0);
            MeasureColumn (items, options, ref bounds);
            LocateColumn (items, bounds, bounds, ref at, options);
        }

        public virtual void OneColumn (IEnumerable<TItem> items, AllignerOptions options) {
            var comparer = new PointComparer { Order = options.PointOrder, Delta = Layout.StyleSheet.AutoSize.Width / 2 };
            var colItems = items.OrderBy (item => Locator.GetLocation (item), comparer);
            var bounds = new Rectangle (int.MaxValue, int.MaxValue, 0, 0);
            MeasureColumn (colItems, options, ref bounds);
            var colPos = bounds.Location;
            LocateColumn (colItems, bounds, bounds, ref colPos, options);

        }

        public virtual void Columns (IEnumerable<TItem> items, AllignerOptions options) {
            var comparer = new PointComparer { Order = options.PointOrder };
            comparer.Delta = Layout.StyleSheet.AutoSize.Width / 2;

            var walk = items.Select (item => new { location = comparer.Round (Locator.GetLocation (item)), item });

            var bounds = new Rectangle (int.MaxValue, int.MaxValue, 0, 0);
            var cols = new Queue<Tuple<IEnumerable<TItem>, Rectangle>> ();

            foreach (var col in walk.GroupBy (row => row.location.X).OrderBy (row => row.Key)) {
                var colItems = col.Select (r => r.item).OrderBy (item => Locator.GetLocation (item).Y);
                cols.Enqueue (MeasureColumn (colItems, options, ref bounds));
            }

            var colPos = bounds.Location;
            while (cols.Count > 0) {
                var col = cols.Dequeue ();
                LocateColumn (col.Item1, col.Item2, bounds, ref colPos, options);
            }
        }

        public virtual void Columns (TItem root, IEnumerable<TItem> items, AllignerOptions options) {
            if (root == null || items == null)
                return;

            var itemCache = new HashSet<TItem> (items);
            if (itemCache.Count == 0)
                return;

            var walk = new Walker<TItem, TEdge> (this.Graph).DeepWalk (root, 1)
                .Where (l => !(l.Node is TEdge) && itemCache.Contains (l.Node));

            var bounds = new Rectangle (int.MaxValue, int.MaxValue, 0, 0);
            var cols = new Queue<Tuple<IEnumerable<TItem>, Rectangle>> ();

            foreach (var col in walk.GroupBy (row => row.Level)) {
                if (col.Count () == 0)
                    continue;

                var colItems = col.Select (r => r.Node).OrderBy (item => Locator.GetLocation (item).Y);
                cols.Enqueue (MeasureColumn (colItems, options, ref bounds));
            }

            var colPos = bounds.Location;
            while (cols.Count > 0) {
                var col = cols.Dequeue ();
                LocateColumn (col.Item1, col.Item2, bounds, ref colPos, options);

            }
        }

        protected virtual Tuple<IEnumerable<TItem>, Rectangle> MeasureColumn (IEnumerable<TItem> colItems, AllignerOptions options, ref Rectangle bounds) {

            Action<TItem> visit = null;

            var items = new Queue<TItem> ();
            visit += i => items.Enqueue (i);
            AffectedEdges (ref visit);

            var measure = new MeasureVisits<TItem> (this.Locator);
            var fBounds = measure.Bounds (ref visit);
            var fSize = measure.SizeToFit (ref visit, options.Distance, options.Dimension);

            VisitItems (colItems, visit);

            var colBounds = fBounds ();
            colBounds.Size = fSize ();

            bounds.Location = bounds.Location.Min (colBounds.Location);
            bounds.Size = bounds.Size.Max (colBounds.Size);

            return new Tuple<IEnumerable<TItem>, Rectangle> (items, colBounds);

        }

        protected virtual void LocateColumn (IEnumerable<TItem> colItems, Rectangle colBounds, Rectangle bounds, ref Point colPos, AllignerOptions options) {
            Action<TItem> visit = null;
            var locator = new LocateVisits<TItem> (this.Locator);
                //new CollissionResolver<TItem>(this.Locator, new GraphSceneLocationDetector<TItem, TEdge>(this.GraphScene));

            if (Alignment.Center == options.AlignY)
                colPos.Y = bounds.Y + (bounds.Height - colBounds.Height) / 2;
            else if (Alignment.End == options.AlignY)
                colPos.Y = bounds.Y + (bounds.Height - colBounds.Height);

            locator.Locate (ref visit,
                locator.Allign (colPos.X, colBounds.Width, options.AlignX, Dimension.X),
                locator.Location (colPos.Y, options.Distance.Height, Dimension.Y)
                //,Dimension.X,colItems
                );

            VisitItems (colItems, visit);

            colPos.X += colBounds.Width + options.Distance.Width;
        }

    }
}