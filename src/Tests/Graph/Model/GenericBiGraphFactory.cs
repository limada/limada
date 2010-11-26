using Limaki.Graphs;
using Limaki.Tests.Graph.Model;
using System.Collections.Generic;

namespace Limaki.Tests.Graph.Model {
    public class GenericBiGraphFactory<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> : GenericGraphFactory<TItemOne, TEdgeOne>
        where TEdgeOne : class, IEdge<TItemOne>, TItemOne
        where TEdgeTwo : IEdge<TItemTwo>, TItemTwo {
        public GenericBiGraphFactory(IGraphFactory<TItemTwo, TEdgeTwo> data, GraphConverter<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne> converter) {
            this._factory = data;
            this._converter = converter;
        }

        public override string Name {
            get { return Factory.Name; }
        }

        public override IGraph<TItemOne, TEdgeOne> Graph {
            get {
                if (_graph == null) {
                    _graph = Converter.Two;
                }
                return _graph;

            }
            set { _graph = value; }
        }

        protected IGraphFactory<TItemTwo, TEdgeTwo> _factory = null;
        public virtual IGraphFactory<TItemTwo, TEdgeTwo> Factory {
            get { return _factory; }
        }


        private GraphConverter<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne> _converter = null;
        public GraphConverter<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne> Converter {
            get { return _converter; }
        }

        public override void Populate(IGraph<TItemOne, TEdgeOne> graph) {
            Factory.Count = this.Count;
            Factory.AddDensity = this.AddDensity;
            Factory.SeperateLattice = this.SeperateLattice;

            IGraphPair<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne> graphPair =
                new GraphPair<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne>(
                    new Graph<TItemTwo, TEdgeTwo>(),
                    graph,
                    Converter
                    );

            Factory.Graph = graphPair;
            Factory.Populate();

            for (int i = 1; i < Factory.Node.Count; i++) {
                this.Node[i] = graphPair.Get(Factory.Node[i]);
            }

            for (int i = 1; i < Factory.Link.Count; i++) {
                this.Link[i] = (TEdgeOne)graphPair.Get(Factory.Link[i]);
            }

        }
    }

}