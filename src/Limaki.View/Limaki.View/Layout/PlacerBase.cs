using Limaki.Drawing;
using Limaki.Graphs;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Limaki.View.Layout {

    public class PlacerBase<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {

        protected PlacerBase(){}

        public PlacerBase(PlacerBase<TItem, TEdge> aligner) {
            this.Data = aligner.Data;
            this.Layout = aligner.Layout;
            this.Proxy = aligner.Proxy;
        }

        public IGraphSceneLayout<TItem, TEdge> Layout { get; protected set; }
        public IGraphScene<TItem, TEdge> Data { get; protected set; }
        public virtual IShapeGraphProxy<TItem, TEdge> Proxy { get; set; }

        public IGraph<TItem, TEdge> Graph {
            get { return Data.Graph; }
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