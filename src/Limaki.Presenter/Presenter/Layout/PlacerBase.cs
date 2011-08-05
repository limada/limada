using Limaki.Drawing;
using Limaki.Graphs;

namespace Limaki.Presenter.Layout {
    public class PlacerBase<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {
        protected PlacerBase(){}
        public PlacerBase(PlacerBase<TItem, TEdge> aligner) {
            this.Data = aligner.Data;
            this.Layout = aligner.Layout;
            this.Proxy = aligner.Proxy;
        }
        public IGraphLayout<TItem, TEdge> Layout { get; protected set; }
        public IGraphScene<TItem, TEdge> Data { get; protected set; }
        public IGraph<TItem, TEdge> Graph {
            get { return Data.Graph; }
        }

       
        public virtual IShapeProxy<TItem, TEdge> Proxy {get;set;}
            
    }
}