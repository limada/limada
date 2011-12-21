using System.Collections.Generic;
using Limaki.Graphs;
using Limaki.Drawing;
using System.Linq;
using System;
using System.Diagnostics;
using Limaki.Drawing.Shapes;

namespace Limaki.Presenter.Layout {
    public class Alligner<TItem, TEdge> : Placer<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {
        
        public Alligner(IGraphScene<TItem, TEdge> data, IGraphLayout<TItem, TEdge> layout):base(data,layout) {}

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

            alligner.Allign(ref visit, HorizontalAlignment.Right, space,
                alligner.Location(at.Y, distance, Distribution.Vertical));
            
            VisitItems(items, visit);
        }
        
        public virtual void OneColumn(IEnumerable<TItem> items, AllignerOptions options) {
            var verticalAlignment = options.VerticalAlignment;
            var horizontalAlignment = options.HorizontalAlignment;
            var order = options.PointOrder;
            var distribution = options.Distribution;

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
            if (distribution == Distribution.Vertical) {
                var at = bounds.Y;
                aligner.Allign(ref visit, verticalAlignment, bounds);
                aligner.Allign(ref visit, horizontalAlignment, bounds,
                    aligner.Location(at, Layout.Distance.Height, distribution));
            } else if (distribution == Distribution.Horizontal) {
                var at = bounds.X;
                aligner.Allign(ref visit, horizontalAlignment, bounds);
                aligner.Allign(ref visit, verticalAlignment, bounds, 
                    aligner.Location(at, Layout.Distance.Width, distribution));
            }

            VisitItems(items.OrderBy(item => Proxy.GetLocation(item), comparer), visit);
            
        }

        public virtual void Columns(IEnumerable<TItem> items, AllignerOptions options) {
            var verticalAlignment = options.VerticalAlignment;
            var horizontalAlignment = options.HorizontalAlignment;
           
            var placer = new PlacerComposer<TItem, TEdge>(this);
            var aligner = new AllignComposer<TItem, TEdge>(this);

            var comparer = new Limaki.Drawing.Shapes.PointComparer { Order = PointOrder.TopToBottom };
            comparer.Delta = Layout.StyleSheet.AutoSize.Width /2;

            var walk = items.Select(item => new { location = comparer.Round(Proxy.GetLocation(item)), item });
            Action<TItem> visit = null;

          
            // align X
            int? rowStart = null;
            int top = int.MaxValue;
            var rows = new Queue<Tuple<IEnumerable<TItem>, SizeI>>();
            var maxSize = new SizeI();
            foreach (var row in walk.GroupBy(row => row.location.X).OrderBy(row=>row.Key)) {
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

                maxSize = maxSize.Max(size);
                
                rowStart = bounds.X + size.Width + Layout.Distance.Width;

                aligner.Allign(ref visit, horizontalAlignment, bounds);
                //aligner.Distribute(ref visit, bounds.Y, Layout.Distance.Height, Distribution.Y);
                VisitItems(rowItems, visit);
                rows.Enqueue(Tuple.Create(rowItems as IEnumerable<TItem>, size));
            }


            while (rows.Count > 0) {
                var row = rows.Dequeue();
                visit = null;
                aligner.Locate(ref visit, top, Layout.Distance.Height, Distribution.Vertical);
                VisitItems(row.Item1, visit);
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

                aligner.Allign(ref visit, options.HorizontalAlignment, space);

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

                aligner.Allign(ref visit, options.VerticalAlignment, space);

                VisitItems(rowItems, visit);

            }
        }
        public virtual void Distribute0(IEnumerable<TItem> items, Distribution distribution) {
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
            if (distribution == Distribution.Vertical) {
                order = Drawing.Shapes.PointOrder.Top;
                distance = Math.Max(distance, Layout.Distance.Height);
                comparer.Delta = Layout.Distance.Height;
            } else {
                order = Drawing.Shapes.PointOrder.Left;
                distance = Math.Max(distance, Layout.Distance.Width);
                comparer.Delta = Layout.Distance.Width;
            }

            var ordered = items.OrderBy(v => Proxy.GetLocation(v), comparer);
            composer2.Locate(ref visit, space.Top, distance, distribution);

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
            var distribution = options.Distribution;
            var start = bounds.Y;
            Action<TItem> secondVisit = null;

            if (distribution == Distribution.Vertical) {
                start = bounds.Y;
                alligner.Allign(ref secondVisit, options.HorizontalAlignment, bounds);
            } else {
                distribution = Distribution.Horizontal;
                start = bounds.X;
                alligner.Allign(ref secondVisit, options.VerticalAlignment, bounds);
            }

            alligner.Locate(ref secondVisit, start, Layout.Distance.Height, distribution);
            VisitItems(items, secondVisit);
        }
        #endregion
    }
}