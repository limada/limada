using Limaki.Graphs;
using System;
using Limaki.Drawing;

namespace Limaki.Presenter.Layout {
    public class PlacerComposer<TItem, TEdge> : PlacerBase<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {
        public PlacerComposer(PlacerBase<TItem, TEdge> aligner):base(aligner) {
            this.Data = aligner.Data;
            this.Layout = aligner.Layout;
            this.Proxy = aligner.Proxy;
        }

        public Func<RectangleI> Extents(ref Action<TItem> visitor) {
            var l = int.MaxValue;
            var t = int.MaxValue;
            var b = int.MinValue;
            var r = int.MinValue;

            visitor += item => {
                var rect = Proxy.GetShape(item).BoundsRect;
                var il = rect.Left;
                var it = rect.Top;
                var ir = rect.Right;
                var ib = rect.Bottom;
                if (il < l)
                    l = il;
                if (it < t)
                    t = it;
                if (ir > r)
                    r = ir;
                if (ib > b)
                    b = ib;
            };

            return () => RectangleI.FromLTRB(l, t, r, b);
        }

        public Func<RectangleI> MinExtents(ref Action<TItem> visitor) {
            var l = int.MaxValue;
            var t = int.MaxValue;
            var w = 0;
            var h = 0;

            visitor += item => {
                var rect = Proxy.GetShape(item).BoundsRect;
                var il = rect.Left;
                var it = rect.Top;
                if (il < l) l = il;
                if (it < t) t = it;
                w += rect.Width;
                h += rect.Height;
            };

            return () => new RectangleI(l, t, w, h);
        }
        public virtual void AffectedEdges(ref Action<TItem> visitor) {
            visitor += item => {
                foreach (var edge in this.Graph.Twig(item))
                    Proxy.AffectedEdges.Add(edge);
            };
        }
    }
}