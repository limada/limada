using Limaki.Drawing;
using Limaki.Graphs;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Limaki.View.Layout {

    public class Placer<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {

        public Placer (IGraphScene<TItem, TEdge> scene, IGraphSceneLayout<TItem, TEdge> layout) {
            this.GraphScene = scene;
            this.Layout = layout;
        }

        protected IGraphSceneLocator<TItem, TEdge> _locator = null;
        public virtual IGraphSceneLocator<TItem, TEdge> Locator {
            get { return _locator ?? (_locator = CreateLocator(this.Layout)); }
            set { _locator = value; }
        }
        
        protected virtual IGraphSceneLocator<TItem, TEdge> CreateLocator(IGraphSceneLayout<TItem, TEdge> layout) {
            return new GraphSceneLocator<TItem, TEdge>(layout);
        }
     
        public IGraphSceneLayout<TItem, TEdge> Layout { get; protected set; }
        public IGraphScene<TItem, TEdge> GraphScene { get; protected set; }

        protected IGraph<TItem, TEdge> Graph {
            get { return GraphScene.Graph; }
        }

        /// <summary>
        /// testing if too much visits
        /// </summary>
        internal int visits = 0;

        public virtual void VisitItems (IEnumerable<TItem> items, Action<TItem> visitor) {
            foreach (var item in items.Where (i => !(i is TEdge))) {
                visits++;
                visitor (item);
            }
        }
        
            
    }
}