using System.Collections.Generic;
using Limada.Model;
using Limada.VisualThings;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Tests.View.Visuals;
using Limaki.Visuals;

namespace Limada.Tests.View {

    public abstract class ThingSceneFactory0:ISampleGraphSceneFactory {
        public ThingSceneFactory0() { }

        #region ISampleGraphSceneFactory Member

        IGraph<IVisual, IVisualEdge> _graph = null;
        public virtual IGraph<IVisual, IVisualEdge> Graph {
            get {
                if ( _graph == null ) {
                    _graph = new Graph<IVisual, IVisualEdge>();
                }
                return _graph;

            }
            set { _graph = value; }
        }

        protected IThingGraph _thingGraph = null;
        public abstract IThingGraph ThingGraph { get; set; }

        public virtual IGraphScene<IVisual, IVisualEdge> Scene {
            get {
                var result = new Scene();
                PopulateScene(result);
                return result;
            }
        }

        private int _count = 1;
        public int Count {
            get { return _count; }
            set { _count = value; }
        }

        public string Name { get { return this.GetType().Name; } }

        public void PopulateScene (IGraphScene<IVisual, IVisualEdge> scene) {
            if (ThingGraph != null) {
                this.Graph = new VisualThingGraph (new VisualGraph (), this.ThingGraph);
                scene.Graph = this.Graph;
            }
        }
        public void Populate() {
            PopulateScene (this.Scene);
        }

        protected bool _seperateLattice = false;
        public bool SeperateLattice {
            get { return _seperateLattice; }
            set { _seperateLattice = value; }
        }

        protected bool _addDensity = false;
        public bool AddDensity {
            get { return _addDensity; }
            set { _addDensity = value; }
        }
        IList<IVisual> _node = null;
        public IList<IVisual> Nodes {
            get {
                if (_node == null) {
                    _node = new IVisual[11];
                }
                return _node;
            }
        }

        IList<IVisualEdge> _link = null;
        public IList<IVisualEdge> Edges {
            get {
                if (_link == null) {
                    _link = new IVisualEdge[11];
                }
                return _link;
            }
        }
        #endregion

        #region IGraphFactory<IVisual, IVisualEdge> Member


        public void Populate(IGraph<IVisual, IVisualEdge> graph) {
            var adapter = new VisualThingTransformer().Reverted();
            var mapper = new GraphMapper<IThing, IVisual, ILink, IVisualEdge>(ThingGraph, graph, adapter);

            mapper.ConvertSinkSource();
        }

        #endregion
    
    }
}