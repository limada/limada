using Limaki.Drawing;
using Limaki.Graphs;
using System;
using Xwt;

namespace Limaki.Presenter.Layout {

    public class PlacerComposer<TItem, TEdge> : PlacerBase<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {

        public PlacerComposer(PlacerBase<TItem, TEdge> aligner):base(aligner) {
            this.Data = aligner.Data;
            this.Layout = aligner.Layout;
            this.Proxy = aligner.Proxy;
        }

        public Func<RectangleD> Bounds(ref Action<TItem> visitor) {
            var l = double.MaxValue;
            var t = double.MaxValue;
            var b = double.MinValue;
            var r = double.MinValue;

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

            return () => RectangleD.FromLTRB(l, t, r, b);
        }

        
        public Func<Size> SizeSum(ref Action<TItem> visitor) {
            var w = 0d;
            var h = 0d;
            visitor += item => {
                var size = Proxy.GetSize(item);
               
                w += size.Width;
                h += size.Height;
            };

            return () => new Size(w,h);
        }

        public Func<Size> SizeSum(ref Action<TItem> visitor, Size distance) {
            var w = 0d;
            var h = 0d;
            visitor += item => {
                var size = Proxy.GetSize(item);

                w += size.Width + distance.Width;
                h += size.Height + distance.Height;
            };

            w -= distance.Width;
            h -= distance.Height;

            return () => new Size(w, h);
        }

        public Func<Size> SizeToFit(ref Action<TItem> visitor, Size distance,  Dimension dimension) {
            var w = 0d;
            var h = 0d;
            
            visitor += item => {
                var size = Proxy.GetSize(item);
                if (dimension == Dimension.X) {
                    w += size.Width + distance.Width;
                    if (h < size.Height)
                        h = size.Height;
                } else {
                    h += size.Height + distance.Height;
                    if (w < size.Width)
                        w = size.Width;
                }
            };
            if (dimension == Dimension.X)
                w -= distance.Width;
            else
                h -= distance.Height;

            return () => new Size(w, h);
        }

        public Func<Size> MinSize(ref Action<TItem> visitor) {
            var w = 0d;
            var h = 0d;
            visitor += item => {
                var size = Proxy.GetSize(item);
                if (w < size.Width)
                    w = size.Width;
                if (h < size.Height)
                    h = size.Height;
            };

            return () => new Size(w, h);
        }

        public virtual void AffectedEdges(ref Action<TItem> visitor) {
            visitor += item => {
                foreach (var edge in this.Graph.Twig(item))
                    Proxy.AffectedEdges.Add(edge);
            };
        }
    }
}