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

        public virtual RectangleI AllignFirstVisit(IEnumerable<TItem> items) {
            var composer = new PlacerComposer<TItem, TEdge>(this);
            Action<TItem> firstVisit = null;

            composer.AffectedEdges(ref firstVisit);

            var extends = composer.Extents(ref firstVisit);

            VisitItems(items, firstVisit);
            return extends();
        }

        public virtual void Allign(IEnumerable<TItem> items, HorizontalAlignment alignment) {

            var extends = AllignFirstVisit(items);

            var composer = new AllignComposer<TItem, TEdge>(this);
            composer.visits = 0;

            Action<TItem> secondVisit = null;
            composer.Allign(ref secondVisit, alignment, extends);
            
            VisitItems(items, secondVisit);
        }

        public virtual void Justify(IEnumerable<TItem> items) {
            var alligner = new AllignComposer<TItem, TEdge>(this);
            var placer = new PlacerComposer<TItem, TEdge>(this);
            Action<TItem> visit = null;
            
            alligner.Justify(ref visit);
            placer.AffectedEdges(ref visit);

            VisitItems(items, visit);
        }
        
        public virtual void MakeRow(IEnumerable<TItem> items, PointI at) {
            Action<TItem> visit = null;
            var placer = new PlacerComposer<TItem, TEdge>(this);
            placer.AffectedEdges(ref visit);
            var extents = placer.Extents(ref visit);
            var size = placer.MinSize(ref visit);

            VisitItems(items, visit);

            var space = extents();
            var minSize = size();
            space.Width = minSize.Width;
            space.X = at.X;
            space.Y = at.Y;
            visit = null;

            var alligner = new AllignComposer<TItem, TEdge>(this);
            int? rowStart = null;

            Func<TItem, int> yer = item => {
                var result = at.Y;
                if (rowStart.HasValue) {
                    result = rowStart.Value;
                } else {
                    rowStart = at.Y;
                }
                rowStart += Proxy.GetSize(item).Height + Layout.Distance.Height;
                return result;
            };
            alligner.Allign(ref visit, HorizontalAlignment.Right, space, yer);
            
            VisitItems(items, visit);
        }

        public virtual void Distribute0(IEnumerable<TItem> items, Distribution distribution) {
            var composer = new PlacerComposer<TItem, TEdge>(this);
            Action<TItem> visit = null;

            composer.AffectedEdges(ref visit);
            var extents = composer.Extents(ref visit);
            var size = composer.SizeSum(ref visit);

            VisitItems(items, visit);
            
            visit = null;
            var composer2 = new AllignComposer<TItem, TEdge>(this);

            var space = extents();
            var distance = (space.Height - size().Height) / (items.Count() - 1);
            var order = Drawing.Shapes.PointOrder.Top;
            var comparer = new Limaki.Drawing.Shapes.PointComparer { Order = order };
            if (distribution == Distribution.Y) {
                order = Drawing.Shapes.PointOrder.Top;
                distance = Math.Max(distance, Layout.Distance.Height);
                comparer.Delta = Layout.Distance.Height;
            } else {
                order = Drawing.Shapes.PointOrder.Left;
                distance = Math.Max(distance, Layout.Distance.Width);
                comparer.Delta = Layout.Distance.Width;
            }

            var ordered = items.OrderBy(v => Proxy.GetLocation(v), comparer);
            composer2.Distribute(ref visit, space.Top, distance, distribution);
           
            VisitItems(ordered, visit);

        }

        public virtual void Distribute(IEnumerable<TItem> items, VerticalAlignment alignment) {
            var placer = new PlacerComposer<TItem, TEdge>(this);
            var aligner = new AllignComposer<TItem, TEdge>(this);

            var comparer = new Limaki.Drawing.Shapes.PointComparer {Order = PointOrder.TopToBottom};
            comparer.Delta = Layout.StyleSheet.AutoSize.Width /2;

            var walk = items.Select(item => new { location = comparer.Round(Proxy.GetLocation(item)), item });
            Action<TItem> visit = null;

            // align X
            int? rowStart = null;
            foreach (var row in walk.GroupBy(row => row.location.X).OrderBy(row=>row.Key)) {
                visit = null;

                placer.AffectedEdges(ref visit);
                var extents = placer.Extents(ref visit);
                var size = placer.MinSize(ref visit);

                var rowItems = row.Select(r => r.item).OrderBy(item => Proxy.GetLocation(item).Y);
                VisitItems(rowItems, visit);

                visit = null;
                
                var space = extents();
                var minSize = size();
                space.Width = minSize.Width;
                if (rowStart.HasValue)
                    space.X = rowStart.Value;

                rowStart = space.X + minSize.Width + Layout.Distance.Width;

                aligner.Allign(ref visit, (HorizontalAlignment)alignment, space);

                VisitItems(rowItems, visit);
                
            }

            // Allign Y
            rowStart = null;
            comparer.Delta = Layout.StyleSheet.AutoSize.Height/2;
            foreach (var row in walk.GroupBy(row => row.location.Y).OrderBy(row => row.Key)) {
                visit = null;

                var extents = placer.Extents(ref visit);
                var size = placer.MinSize(ref visit);

                var rowItems = row.Select(r => r.item).OrderBy(item => Proxy.GetLocation(item).X);
                VisitItems(rowItems, visit);

                visit = null;

                var space = extents();
                var minSize = size();
                space.Height = minSize.Height;

                if (rowStart.HasValue)
                    space.Y = rowStart.Value;

                rowStart = space.Y + minSize.Height + Layout.Distance.Height;

                aligner.Allign(ref visit, alignment, space);

                VisitItems(rowItems, visit);

            }
        }
    }
}