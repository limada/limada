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

        public Alligner (IGraphScene<TItem, TEdge> data, IGraphLayout<TItem, TEdge> layout) : base (data, layout) { }

        public Alligner (PlacerBase<TItem, TEdge> aligner) : base (aligner) { }

        public virtual void Justify (ref Action<TItem> visitor) {
            visitor += item => Proxy.Justify (item);
        }

        public virtual void AffectedEdges (ref Action<TItem> visitor) {
            visitor += item => {
                foreach (var edge in this.Graph.Twig (item))
                    Proxy.AffectedEdges.Add (edge);
            };
        }

        public virtual void Justify (IEnumerable<TItem> items) {
            Action<TItem> visit = null;

            Justify (ref visit);
            AffectedEdges (ref visit);

            VisitItems (items, visit);
        }

        public virtual void OneColumn (IEnumerable<TItem> items, Point at, double distance) {
            Action<TItem> visit = null;
            var measure = new MeasureVisits<TItem> (this.Proxy);
            AffectedEdges (ref visit);
            var bounds = measure.Bounds (ref visit);
            var size = measure.MinSize (ref visit);

            VisitItems (items, visit);

            var space = bounds ();
            var minSize = size ();
            space.Width = minSize.Width;
            space.X = at.X;
            space.Y = at.Y;
            visit = null;

            var locator = new LocateVisits<TItem> (this.Proxy);

            locator.AllignX (ref visit, Alignment.End, space,
                locator.ItemLocation (at.Y, distance, Dimension.Y));

            VisitItems (items, visit);
        }

        public virtual void OneColumn (IEnumerable<TItem> items, AllignerOptions options) {
            var alignY = options.AlignY;
            var alignX = options.AlignX;
            var order = options.PointOrder;
            var dimension = options.Dimension;

            var comparer = new PointComparer { Order = order };
            comparer.Delta = Layout.StyleSheet.AutoSize.Width / 2;

            Action<TItem> visit = null;
            var measure = new MeasureVisits<TItem> (this.Proxy);
            AffectedEdges (ref visit);
            var fBounds = measure.Bounds (ref visit);
            var fSize = measure.MinSize (ref visit);

            VisitItems (items, visit);

            var bounds = fBounds ();
            var size = fSize ();

            visit = null;
            var locator = new LocateVisits<TItem> (this.Proxy);
            if (dimension == Dimension.Y) {
                var at = bounds.Y;
                locator.AllignY (ref visit, alignY, bounds);
                locator.AllignX (ref visit, alignX, bounds,
                    locator.ItemLocation (at, Layout.Distance.Height, dimension));
            } else if (dimension == Dimension.X) {
                var at = bounds.X;
                locator.AllignX (ref visit, alignX, bounds);
                locator.AllignY (ref visit, alignY, bounds,
                    locator.ItemLocation (at, Layout.Distance.Width, dimension));
            }

            VisitItems (items.OrderBy (item => Proxy.GetLocation (item), comparer), visit);

        }

        public virtual void Columns (IEnumerable<TItem> items, AllignerOptions options) {
            var verticalAlignment = options.AlignY;
            var horizontalAlignment = options.AlignX;

            var measure = new MeasureVisits<TItem> (this.Proxy);
            var locator = new LocateVisits<TItem> (this.Proxy);

            var comparer = new PointComparer { Order = options.PointOrder };
            comparer.Delta = Layout.StyleSheet.AutoSize.Width / 2;

            var walk = items.Select (item => new { location = comparer.Round (Proxy.GetLocation (item)), item });
            Action<TItem> visit = null;


            // align X
            double? rowStart = null;
            double top = double.MaxValue;
            double left = double.MaxValue;
            var rows = new Queue<Tuple<IEnumerable<TItem>, Size>> ();
            var maxSize = new Size ();
            foreach (var row in walk.GroupBy (row => row.location.X).OrderBy (row => row.Key)) {
                visit = null;

                AffectedEdges (ref visit);
                var fBounds = measure.Bounds (ref visit);
                var fSize = measure.MinSize (ref visit);

                var rowItems = row.Select (r => r.item).OrderBy (item => Proxy.GetLocation (item).Y);
                VisitItems (rowItems, visit);

                visit = null;

                var bounds = fBounds ();
                var size = fSize ();
                bounds.Width = size.Width;
                if (rowStart.HasValue)
                    bounds.X = rowStart.Value;

                if (top > bounds.Y)
                    top = bounds.Y;
                if (left > bounds.X)
                    left = bounds.X;
                maxSize = maxSize.Max (bounds.Size);

                rowStart = bounds.X + size.Width + Layout.Distance.Width;

                locator.AllignX (ref visit, horizontalAlignment, bounds);
                //aligner.Distribute(ref visit, bounds.Y, Layout.Distance.Height, Distribution.Y);
                VisitItems (rowItems, visit);

                rows.Enqueue (Tuple.Create (rowItems as IEnumerable<TItem>, bounds.Size));
            }

            var rowX = left;
            var rowY = top;
            while (rows.Count > 0) {
                var row = rows.Dequeue ();
                visit = null;
                var size = row.Item2;

                if (verticalAlignment == Alignment.Center)
                    rowY = top + (maxSize.Height - size.Height) / 2;
                if (verticalAlignment == Alignment.End)
                    rowY = top + (maxSize.Height - size.Height);

                locator.LocateItem (ref visit, rowY, Layout.Distance.Height, Dimension.Y);
                VisitItems (row.Item1, visit);
            }
        }


        public virtual void Columns (TItem root, IEnumerable<TItem> items, AllignerOptions options) {

            if (root == null || items == null)
                return;

            var itemCache = new HashSet<TItem> (items);
            if (itemCache.Count == 0)
                return;

            var verticalAlignment = options.AlignY;
            var horizontalAlignment = options.AlignX;
            var distance = Layout.Distance;
            var dimension = options.Dimension;

            var measure = new MeasureVisits<TItem> (this.Proxy);
            var locator = new LocateVisits<TItem> (this.Proxy);

            var comparer = new PointComparer { Order = options.PointOrder };
            comparer.Delta = Layout.StyleSheet.AutoSize.Width / 2;

            var walk = new Walker<TItem, TEdge> (this.Graph).DeepWalk (root, 1)
                .Where (l => !(l.Node is TEdge) && itemCache.Contains (l.Node));

            var bounds = new Rectangle (int.MaxValue, int.MaxValue, 0, 0);

            var rows = new Queue<Tuple<IEnumerable<TItem>, Rectangle>> ();

            Action<TItem> visit = null;

            foreach (var row in walk.GroupBy (row => row.Level)) {
                if (row.Count () == 0)
                    continue;

                var rowItems = row.Select (r => r.Node).OrderBy (item => Proxy.GetLocation (item).Y);

                visit = null;
                AffectedEdges (ref visit);
                var fBounds = measure.Bounds (ref visit);
                var fSize = measure.SizeToFit (ref visit, distance, dimension);

                VisitItems (rowItems, visit);

                visit = null;

                var rowBounds = fBounds ();
                rowBounds.Size = fSize ();

                bounds.Location = bounds.Location.Min (rowBounds.Location);
                bounds.Size = bounds.Size.Max (rowBounds.Size);

                rows.Enqueue (Tuple.Create (rowItems as IEnumerable<TItem>, rowBounds));
            }

            var rowPos = bounds.Location;
            while (rows.Count > 0) {
                var row = rows.Dequeue ();

                var rowBounds = row.Item2;

                if (verticalAlignment == Alignment.Center)
                    rowPos.Y = bounds.Y + (bounds.Height - rowBounds.Height) / 2;
                if (verticalAlignment == Alignment.End)
                    rowPos.Y = bounds.Y + (bounds.Height - rowBounds.Height);
                rowBounds.Location = rowPos;

                visit = null;

                locator.Locate (ref visit,
                    locator.Allign (rowBounds.X, rowBounds.Width, horizontalAlignment, Dimension.X),
                    locator.Location (rowBounds.Y, distance.Height, Dimension.Y));
                VisitItems (row.Item1, visit);

                rowPos.X += rowBounds.Width + distance.Width;

            }
        }


    }
}