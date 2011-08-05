using System.Collections.Generic;
using Limaki.Graphs;
using Limaki.Drawing;
using System.Linq;
using System;
using System.Diagnostics;

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
            
            // yes, it visits for every Allign-Call; stupid, no?
            //Allign(ref secondVisit, alignment, extends());

            VisitItems(items, secondVisit);

            Trace.WriteLine(string.Format("items\t{0}\tvisits\t{1}", items.Count(), composer.visits));

        }

        public virtual void Distribute(IEnumerable<TItem> items) {
            var composer = new PlacerComposer<TItem, TEdge>(this);
            Action<TItem> visit = null;

            composer.AffectedEdges(ref visit);
            var extends = composer.Extents(ref visit);
            var minExtends = composer.MinExtents(ref visit);

            VisitItems(items, visit);
            
            visit = null;
            var composer2 = new AllignComposer<TItem, TEdge>(this);
            composer2.Distribute(ref visit, extends(), minExtends(),items.Count());
            VisitItems(items.OrderBy(v => Proxy.GetLocation(v), new Limaki.Drawing.Shapes.LeftRightTopBottomComparer()), visit);

        }
    }
}