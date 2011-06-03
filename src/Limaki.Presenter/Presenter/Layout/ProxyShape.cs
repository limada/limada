using System;
using Limaki.Drawing;
using Limaki.Graphs;

namespace Limaki.Presenter.Layout {
    /// <summary>
    /// maybe this helps to make arranger more clear?
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    public class ProxyShape<TItem, TEdge>
        where TEdge : IEdge<TItem>, TItem {
        public ProxyShape(IShapeProxy<TItem, TEdge> proxy, TItem item) {
            this.Item = item;
            GetShape = () => proxy.GetShape(item);
            GetLocation = () => proxy.GetLocation(item);
            SetLocation = l => proxy.SetLocation(item, l);

            GetSize = () => proxy.GetSize(item);
            SetSize = s => GetShape().Size = s;
        }

        Func<IShape> GetShape { get; set; }

        Func<PointI> GetLocation { get; set; }
        Action<PointI> SetLocation { get; set; }

        Func<SizeI> GetSize { get; set; }
        Action<SizeI> SetSize { get; set; }

        public TItem Item { get; protected set; }
        public IShape Shape { get { return GetShape(); } }

        public PointI Location {
            get { return GetLocation(); }
            set { SetLocation(value); }
        }

        public SizeI Size {
            get { return GetSize(); }
            set { SetSize(value); }
        }
    }
}