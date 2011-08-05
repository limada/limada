using System;
using Limaki.Drawing;
using Limaki.Graphs;

namespace Limaki.Presenter.Layout {
    public class AllignComposer<TItem, TEdge> : Placer<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {
        public AllignComposer(PlacerBase<TItem, TEdge> aligner) : base(aligner) { }

        /// <summary>
        /// testing if too much visits
        /// </summary>
        internal int visits = 0;

        public virtual void Allign(ref Action<TItem> visitor, HorizontalAlignment alignment, RectangleI extents) {
            visitor += item => {
                visits++;

                var y = Proxy.GetLocation(item).Y;

                if (alignment == HorizontalAlignment.Left)
                    Proxy.SetLocation(item, new PointI(extents.Left, y));
                else if (alignment == HorizontalAlignment.Right)
                    Proxy.SetLocation(item, new PointI(extents.Right - Proxy.GetSize(item).Width, y));
                else if (alignment == HorizontalAlignment.Center) {
                    var x = extents.Left + (extents.Width - Proxy.GetSize(item).Width) / 2;
                    Proxy.SetLocation(item, new PointI(x, y));
                }
            };
        }

        public virtual void Distribute(ref Action<TItem> visitor, RectangleI extents, RectangleI minExtents, int itemsCount) {
            var space = (extents.Height - minExtents.Height)/itemsCount;
            var y = extents.Y;
            visitor += item => {
                visits++;
                var loc = Proxy.GetLocation(item);
                var size = Proxy.GetSize(item);
                Proxy.SetLocation(item, new PointI(loc.X, y));
                y += size.Height + space;
            };
        }
    }
}