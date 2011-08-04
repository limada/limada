using System.Collections.Generic;
using Limaki.Graphs;
using Limaki.Drawing;
using System.Linq;
using System;

namespace Limaki.Presenter.Layout {
    public class Alligner<TItem, TEdge> : AllignerBase<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {
        public Alligner(IGraphScene<TItem, TEdge> data, IGraphLayout<TItem, TEdge> layout):base(data,layout) {
        }

        public RectangleI Extends(IEnumerable<TItem> items) {
            var extends = from item in items
                          where !(item is TEdge)
                          select Proxy.GetShape(item).BoundsRect;

            int l = int.MaxValue;
            int t = int.MaxValue;
            int b = int.MinValue;
            int r = int.MinValue;
            foreach (var rect in extends.Where(i => !i.IsEmpty)) {
                if (rect.Left < l)
                    l = rect.Left;
                if (rect.Top < t)
                    t = rect.Top;
                if (rect.Right > r)
                    r = rect.Right;
                if (rect.Bottom > b)
                    b = rect.Bottom;
            }
            return RectangleI.FromLTRB(l, t, r, b);

            //var ext = new RectangleI();
            //foreach (var r in extends.Where(r => !r.IsEmpty)) {
            //    ext = RectangleI.Union(ext, r);
            //}
            //return ext;
        }

        public virtual void AffectedEdges(IEnumerable<TItem> items) {
            foreach (var item in items) {
                foreach (var edge in this.Graph.Twig(item)) {
                    Proxy.AffectedEdges.Add(edge);
                }
            }
        }

        public virtual void Allign(IEnumerable<TItem> items, HorizontalAlignment alignment) {
            

            var extends = Extends(items);

            foreach (var item in items.Where(i => !(i is TEdge))) {
                if (alignment == HorizontalAlignment.Left)
                    Proxy.SetLocation(item, new PointI(extends.Left, Proxy.GetLocation(item).Y));
                else if (alignment == HorizontalAlignment.Right)
                    Proxy.SetLocation(item, new PointI(extends.Right - Proxy.GetSize(item).Width, Proxy.GetLocation(item).Y));
            }
        }
    }
}