using System;
using Limaki.Graphs;

namespace Limaki.View.Viz.UI.GraphScene {

    public class GraphSceneEventArgs<TItem,TEdge> : EventArgs 
        where TEdge:TItem, IEdge<TItem>{

        public GraphSceneEventArgs(IGraphScene<TItem, TEdge> scene, TItem item) {
            this.Scene = scene;
            this.Item = item;
        }
        public IGraphScene<TItem, TEdge> Scene;
        public TItem Item;
    }
}