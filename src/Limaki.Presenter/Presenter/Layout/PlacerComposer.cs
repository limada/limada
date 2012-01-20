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

        public Func<RectangleI> Bounds(ref Action<TItem> visitor) {
            var l = int.MaxValue;
            var t = int.MaxValue;
            var b = int.MinValue;
            var r = int.MinValue;

            visitor += item => {
                var loc = Proxy.GetLocation(item);
                var size = Proxy.GetSize(item);
                var il = loc.X;
                var it = loc.Y;
                var ir = il + size.Width;
                var ib = it + size.Height;
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

        public Func<RectangleI> SpacedBounds(ref Action<TItem> visitor, SizeI space) {
            var l = int.MaxValue;
            var t = int.MaxValue;
            var b = int.MinValue;
            var r = int.MinValue;

            visitor += item => {
                var loc = Proxy.GetLocation(item);
                var size = Proxy.GetSize(item);
                var il = loc.X;
                var it = loc.Y;
                var ir = il + size.Width;
                var ib = it + size.Height;
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
        public Func<SizeI> SizeSum(ref Action<TItem> visitor) {
            var w = 0;
            var h = 0;
            visitor += item => {
                var size = Proxy.GetSize(item);
               
                w += size.Width;
                h += size.Height;
            };

            return () => new SizeI(w,h);
        }

        public Func<SizeI> SizeSum(ref Action<TItem> visitor, SizeI space) {
            var w = 0;
            var h = 0;
            visitor += item => {
                var size = Proxy.GetSize(item);

                w += size.Width + space.Width;
                h += size.Height + space.Height;
            };

            w -= space.Width;
            h -= space.Height;

            return () => new SizeI(w, h);
        }

        public Func<SizeI> MinSize(ref Action<TItem> visitor) {
            var w = 0;
            var h = 0;
            visitor += item => {
                var size = Proxy.GetSize(item);
                if (w < size.Width)
                    w = size.Width;
                if (h < size.Height)
                    h = size.Height;
            };

            return () => new SizeI(w, h);
        }

        public virtual void AffectedEdges(ref Action<TItem> visitor) {
            visitor += item => {
                foreach (var edge in this.Graph.Twig(item))
                    Proxy.AffectedEdges.Add(edge);
            };
        }
    }
}