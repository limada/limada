using System;
using Limaki.Drawing;
using Limaki.Graphs;

namespace Limaki.Presenter.UI {
    public class GraphSceneEventArgs<TItem,TEdge> : EventArgs 
    where TEdge:TItem, IEdge<TItem>{
        public GraphSceneEventArgs(IGraphScene<TItem, TEdge> scene, TItem widget) {
            this.Scene = scene;
            this.Item = widget;
        }
        public IGraphScene<TItem, TEdge> Scene;
        public TItem Item;
    }
}