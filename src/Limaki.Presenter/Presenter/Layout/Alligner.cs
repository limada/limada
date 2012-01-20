using System.Collections.Generic;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Drawing;
using System.Linq;
using System;
using System.Diagnostics;
using Limaki.Drawing.Shapes;
using Limaki.Xwt;

namespace Limaki.Presenter.Layout {
    public class Alligner<TItem, TEdge> : Placer<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {

        public Alligner(IGraphScene<TItem, TEdge> data, IGraphLayout<TItem, TEdge> layout) : base(data, layout) { }

        public Alligner(PlacerBase<TItem, TEdge> aligner) : base(aligner) { }

        public virtual void Justify(IEnumerable<TItem> items) {
            var alligner = new AllignComposer<TItem, TEdge>(this);
            var placer = new PlacerComposer<TItem, TEdge>(this);
            Action<TItem> visit = null;

            alligner.Justify(ref visit);
            placer.AffectedEdges(ref visit);

            VisitItems(items, visit);
        }

        public virtual void OneColumn(IEnumerable<TItem> items, PointI at, int distance) {
            Action<TItem> visit = null;
            var placer = new PlacerComposer<TItem, TEdge>(this);
            placer.AffectedEdges(ref visit);
            var bounds = placer.Bounds(ref visit);
            var size = placer.MinSize(ref visit);

            VisitItems(items, visit);

            var space = bounds();
            var minSize = size();
            space.Width = minSize.Width;
            space.X = at.X;
            space.Y = at.Y;
            visit = null;

            var alligner = new AllignComposer<TItem, TEdge>(this);

            alligner.AllignX(ref visit, Alignment.End, space,
                alligner.ItemLocation(at.Y, distance, Dimension.Y));

            VisitItems(items, visit);
        }

        public virtual void OneColumn(IEnumerable<TItem> items, AllignerOptions options) {
            var alignY = options.AlignY;
            var alignX = options.AlignX;
            var order = options.PointOrder;
            var dimension = options.Dimension;

            var comparer = new Limaki.Drawing.Shapes.PointComparer { Order = order };
            comparer.Delta = Layout.StyleSheet.AutoSize.Width / 2;

            Action<TItem> visit = null;
            var placer = new PlacerComposer<TItem, TEdge>(this);
            placer.AffectedEdges(ref visit);
            var fBounds = placer.Bounds(ref visit);
            var fSize = placer.MinSize(ref visit);

            VisitItems(items, visit);

            var bounds = fBounds();
            var size = fSize();

            visit = null;
            var aligner = new AllignComposer<TItem, TEdge>(this);
            if (dimension == Dimension.Y) {
                var at = bounds.Y;
                aligner.AllignY(ref visit, alignY, bounds);
                aligner.AllignX(ref visit, alignX, bounds,
                    aligner.ItemLocation(at, Layout.Distance.Height, dimension));
            } else if (dimension == Dimension.X) {
                var at = bounds.X;
                aligner.AllignX(ref visit, alignX, bounds);
                aligner.AllignY(ref visit, alignY, bounds,
                    aligner.ItemLocation(at, Layout.Distance.Width, dimension));
            }

            VisitItems(items.OrderBy(item => Proxy.GetLocation(item), comparer), visit);

        }

        public virtual void Columns(IEnumerable<TItem> items, AllignerOptions options) {
            var verticalAlignment = options.AlignY;
            var horizontalAlignment = options.AlignX;

            var placer = new PlacerComposer<TItem, TEdge>(this);
            var aligner = new AllignComposer<TItem, TEdge>(this);

            var comparer = new Limaki.Drawing.Shapes.PointComparer { Order = options.PointOrder };
            comparer.Delta = Layout.StyleSheet.AutoSize.Width / 2;

            var walk = items.Select(item => new { location = comparer.Round(Proxy.GetLocation(item)), item });
            Action<TItem> visit = null;


            // align X
            int? rowStart = null;
            int top = int.MaxValue;
            int left = int.MaxValue;
            var rows = new Queue<Tuple<IEnumerable<TItem>, SizeI>>();
            var maxSize = new SizeI();
            foreach (var row in walk.GroupBy(row => row.location.X).OrderBy(row => row.Key)) {
                visit = null;

                placer.AffectedEdges(ref visit);
                var fBounds = placer.Bounds(ref visit);
                var fSize = placer.MinSize(ref visit);

                var rowItems = row.Select(r => r.item).OrderBy(item => Proxy.GetLocation(item).Y);
                VisitItems(rowItems, visit);

                visit = null;

                var bounds = fBounds();
                var size = fSize();
                bounds.Width = size.Width;
                if (rowStart.HasValue)
                    bounds.X = rowStart.Value;

                if (top > bounds.Y)
                    top = bounds.Y;
                if (left > bounds.X)
                    left = bounds.X;
                maxSize = maxSize.Max(bounds.Size);

                rowStart = bounds.X + size.Width + Layout.Distance.Width;

                aligner.AllignX(ref visit, horizontalAlignment, bounds);
                //aligner.Distribute(ref visit, bounds.Y, Layout.Distance.Height, Distribution.Y);
                VisitItems(rowItems, visit);
                rows.Enqueue(Tuple.Create(rowItems as IEnumerable<TItem>, bounds.Size));
            }

            var rowX = left;
            var rowY = top;
            while (rows.Count > 0) {
                var row = rows.Dequeue();
                visit = null;
                var size = row.Item2;

                if (verticalAlignment == Alignment.Center)
                    rowY = top + (maxSize.Height - size.Height) / 2;
                if (verticalAlignment == Alignment.End)
                    rowY = top + (maxSize.Height - size.Height);

                aligner.LocateItem(ref visit, rowY, Layout.Distance.Height, Dimension.Y);
                VisitItems(row.Item1, visit);
            }
        }

        public virtual void Columns(TItem root, IEnumerable<TItem> items, AllignerOptions options) {

            if (root == null || items == null)
                return;

            var itemCache = new HashSet<TItem>(items);
            if (itemCache.Count == 0)
                return;

            var verticalAlignment = options.AlignY;
            var horizontalAlignment = options.AlignX;
            var distance = Layout.Distance;

            var placer = new PlacerComposer<TItem, TEdge>(this);
            var aligner = new AllignComposer<TItem, TEdge>(this);

            var comparer = new Limaki.Drawing.Shapes.PointComparer { Order = options.PointOrder };
            comparer.Delta = Layout.StyleSheet.AutoSize.Width / 2;

            var walk = new Walker<TItem, TEdge>(this.Graph).DeepWalk(root, 1)
                .Where(l => itemCache.Contains(l.Node) && !(l.Node is TEdge));


            int top = int.MaxValue;
            int left = int.MaxValue;

            var rows = new Queue<Tuple<IEnumerable<TItem>, SizeI>>();
            var maxSize = new SizeI();

            Action<TItem> visit = null;

            foreach (var row in walk.GroupBy(row => row.Level)) {
                if (row.Count() == 0)
                    continue;

                visit = null;
                placer.AffectedEdges(ref visit);
                var fBounds = placer.Bounds(ref visit);
                var fSize = placer.MinSize(ref visit);
                var fSizeSum = placer.SizeSum(ref visit, distance);
                var rowItems = row.Select(r => r.Node).OrderBy(item => Proxy.GetLocation(item).Y);
                VisitItems(rowItems, visit);

                visit = null;

                var bounds = fBounds();
                var size = fSize();
                var sizeSum = fSizeSum();
                bounds.Width = size.Width;
                bounds.Height = sizeSum.Height;

                if (top > bounds.Y)
                    top = bounds.Y;
                if (left > bounds.X)
                    left = bounds.X;
                maxSize = maxSize.Max(bounds.Size);

                rows.Enqueue(Tuple.Create(rowItems as IEnumerable<TItem>, bounds.Size));
            }

            var rowX = left;
            var rowY = top;
            while (rows.Count > 0) {
                var row = rows.Dequeue();
                var size = row.Item2;

                if (verticalAlignment == Alignment.Center)
                    rowY = top + (maxSize.Height - size.Height) / 2;
                if (verticalAlignment == Alignment.End)
                    rowY = top + (maxSize.Height - size.Height);

                var bounds = new RectangleI(rowX, rowY, size.Width, size.Height);

                

                visit = null;

                //aligner.AllignX(ref visit, horizontalAlignment, bounds,
                //    aligner.Location(rowY, distance.Height, Dimension.Y));
                aligner.Locate(ref visit, 
                    aligner.Allign(rowX,bounds.Width,horizontalAlignment,Dimension.X),
                    aligner.Location(rowY, distance.Height, Dimension.Y));
                VisitItems(row.Item1, visit);

                rowX = bounds.X + size.Width + distance.Width;
            }
        }

        #region Depracated

        public virtual void Distribute(IEnumerable<TItem> items, AllignerOptions options) {
            var placer = new PlacerComposer<TItem, TEdge>(this);
            var aligner = new AllignComposer<TItem, TEdge>(this);

            var comparer = new Limaki.Drawing.Shapes.PointComparer { Order = options.PointOrder };
            comparer.Delta = Layout.StyleSheet.AutoSize.Width / 2;

            var walk = items.Select(item => new { location = comparer.Round(Proxy.GetLocation(item)), item });
            Action<TItem> visit = null;

            // align X
            int? rowStart = null;
            foreach (var row in walk.GroupBy(row => row.location.X).OrderBy(row => row.Key)) {
                visit = null;

                placer.AffectedEdges(ref visit);
                var bounds = placer.Bounds(ref visit);
                var size = placer.MinSize(ref visit);

                var rowItems = row.Select(r => r.item).OrderBy(item => Proxy.GetLocation(item).Y);
                VisitItems(rowItems, visit);

                visit = null;

                var space = bounds();
                var minSize = size();
                space.Width = minSize.Width;
                if (rowStart.HasValue)
                    space.X = rowStart.Value;

                rowStart = space.X + minSize.Width + Layout.Distance.Width;

                aligner.AllignX(ref visit, options.AlignX, space);

                VisitItems(rowItems, visit);

            }

            // Allign Y
            rowStart = null;
            comparer.Delta = Layout.StyleSheet.AutoSize.Height / 2;
            foreach (var row in walk.GroupBy(row => row.location.Y).OrderBy(row => row.Key)) {
                visit = null;

                var bounds = placer.Bounds(ref visit);
                var size = placer.MinSize(ref visit);

                var rowItems = row.Select(r => r.item).OrderBy(item => Proxy.GetLocation(item).X);
                VisitItems(rowItems, visit);

                visit = null;

                var space = bounds();
                var minSize = size();
                space.Height = minSize.Height;

                if (rowStart.HasValue)
                    space.Y = rowStart.Value;

                rowStart = space.Y + minSize.Height + Layout.Distance.Height;

                aligner.AllignY(ref visit, options.AlignY, space);

                VisitItems(rowItems, visit);

            }
        }
        public virtual void Distribute0(IEnumerable<TItem> items, Dimension dimension) {
            var placer = new PlacerComposer<TItem, TEdge>(this);
            Action<TItem> visit = null;

            placer.AffectedEdges(ref visit);
            var bounds = placer.Bounds(ref visit);
            var size = placer.SizeSum(ref visit);

            VisitItems(items, visit);

            visit = null;
            var composer2 = new AllignComposer<TItem, TEdge>(this);

            var space = bounds();
            var distance = (space.Height - size().Height) / (items.Count() - 1);
            var order = Drawing.Shapes.PointOrder.Top;
            var comparer = new Limaki.Drawing.Shapes.PointComparer { Order = order };
            if (dimension == Dimension.Y) {
                order = Drawing.Shapes.PointOrder.Top;
                distance = Math.Max(distance, Layout.Distance.Height);
                comparer.Delta = Layout.Distance.Height;
            } else {
                order = Drawing.Shapes.PointOrder.Left;
                distance = Math.Max(distance, Layout.Distance.Width);
                comparer.Delta = Layout.Distance.Width;
            }

            var ordered = items.OrderBy(v => Proxy.GetLocation(v), comparer);
            composer2.LocateItem(ref visit, space.Top, distance, dimension);

            VisitItems(ordered, visit);

        }

        public virtual RectangleI AllignBounds(IEnumerable<TItem> items) {
            var placer = new PlacerComposer<TItem, TEdge>(this);
            Action<TItem> firstVisit = null;

            placer.AffectedEdges(ref firstVisit);

            var bounds = placer.Bounds(ref firstVisit);

            VisitItems(items, firstVisit);
            return bounds();
        }

        public virtual void Allign(IEnumerable<TItem> items, AllignerOptions options) {

            var bounds = AllignBounds(items);

            var alligner = new AllignComposer<TItem, TEdge>(this);
            alligner.visits = 0;
            var distribution = options.Dimension;
            var start = bounds.Y;
            Action<TItem> secondVisit = null;

            if (distribution == Dimension.Y) {
                start = bounds.Y;
                alligner.AllignX(ref secondVisit, options.AlignX, bounds);
            } else {
                distribution = Dimension.X;
                start = bounds.X;
                alligner.AllignY(ref secondVisit, options.AlignY, bounds);
            }

            alligner.LocateItem(ref secondVisit, start, Layout.Distance.Height, distribution);
            VisitItems(items, secondVisit);
        }
        #endregion
    }
}