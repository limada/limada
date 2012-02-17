using Limaki.Graphs;
using Limaki.Tests.Graph.Model;
using System.Collections.Generic;

namespace Limaki.Tests.Graph.Model {
    public class GenericBiGraphFactory<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> : 
        GenericGraphFactory<TItemOne, TEdgeOne>
        where TEdgeOne : class, IEdge<TItemOne>, TItemOne
        where TEdgeTwo : IEdge<TItemTwo>, TItemTwo {
        public GenericBiGraphFactory(
            IGraphFactory<TItemTwo, TEdgeTwo> data, 
            GraphModelAdapter<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne> adapter) {
            this._factory = data;
            this._adapter = adapter;
            this._mapper = 
                new GraphMapper<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne> (adapter);
        }

        public override string Name {
            get { return Factory.Name; }
        }

        public override IGraph<TItemOne, TEdgeOne> Graph {
            get {
                if (_graph == null) {
                    _graph = Mapper.Two;
                }
                return _graph;

            }
            set { _graph = value; }
        }

        protected IGraphFactory<TItemTwo, TEdgeTwo> _factory = null;
        public virtual IGraphFactory<TItemTwo, TEdgeTwo> Factory {
            get { return _factory; }
        }


        private GraphModelAdapter<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne> _adapter = null;
        public GraphModelAdapter<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne> Adapter {
            get { return _adapter; }
        }

        private GraphMapper<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne> _mapper = null;
        public GraphMapper<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne> Mapper {
            get { return _mapper; }
        }

        private IGraphPair<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> _graphPair = null;
        public IGraphPair<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> GraphPair {
            get { return _graphPair; }
            set { _graphPair = value; }
        }


        public override void Populate(IGraph<TItemOne, TEdgeOne> graph) {
            Factory.Count = this.Count;
            Factory.AddDensity = this.AddDensity;
            Factory.SeperateLattice = this.SeperateLattice;

            IGraphPair<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne> graphPair = 
                new LiveGraphPair<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne>(
                    new Graph<TItemTwo, TEdgeTwo>(),
                    graph,
                    this.Mapper.Adapter
                    );
            
            graphPair.Mapper = this.Mapper;

            Factory.Graph = graphPair;
            Factory.Populate();

            this.GraphPair = new LiveGraphPair<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo>(
                        graphPair.Two, graphPair.One, 
                        Mapper.Adapter.ReverseAdapter()
                        );
            this.GraphPair.Mapper = this.Mapper.ReverseMapper ();

        }

        public override IList<TEdgeOne> Edge {
            get {
                return new EdgeList<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo>(
                    this.GraphPair, Factory.Edge);
            }
        }

        public override IList<TItemOne> Node {
            get {
                return new ItemList<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo>(
                    this.GraphPair, Factory.Node);
            }
        }


    }

    public class ItemList<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> : IList<TItemOne>
        where TEdgeOne : class, IEdge<TItemOne>, TItemOne
        where TEdgeTwo : IEdge<TItemTwo>, TItemTwo {


        public ItemList(IGraphPair<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> graphPair, IList<TItemTwo> listTwo) {
            this.graphPair = graphPair;
            this.listTwo = listTwo;
        }

        IList<TItemTwo> listTwo = null;
        IGraphPair<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> graphPair = null;

        #region IList<T> Member

        public int IndexOf(TItemOne item) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public void Insert(int index, TItemOne item) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public void RemoveAt(int index) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public TItemOne this[int index] {
            get {
                return graphPair.Get(listTwo[index]);
            }
            set {
                throw new System.Exception("The method or operation is not implemented.");
            }
        }

        #endregion

        #region ICollection<T> Member

        public void Add(TItemOne item) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public void Clear() {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public bool Contains(TItemOne item) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public void CopyTo(TItemOne[] array, int arrayIndex) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public int Count {
            get { throw new System.Exception("The method or operation is not implemented."); }
        }

        public bool IsReadOnly {
            get { throw new System.Exception("The method or operation is not implemented."); }
        }

        public bool Remove(TItemOne item) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable<T> Member

        public IEnumerator<TItemOne> GetEnumerator() {
            throw new System.Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable Member

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            throw new System.Exception("The method or operation is not implemented.");
        }

        #endregion
    }

    public class EdgeList<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> : IList<TEdgeOne>
        where TEdgeOne : class, IEdge<TItemOne>, TItemOne
        where TEdgeTwo : IEdge<TItemTwo>, TItemTwo {


        public EdgeList(IGraphPair<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> graphPair, IList<TEdgeTwo> listTwo) {
            this.graphPair = graphPair;
            this.listTwo = listTwo;
        }


        IList<TEdgeTwo> listTwo = null;
        IGraphPair<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> graphPair = null;

        #region IList<T> Member

        public int IndexOf(TEdgeOne item) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public void Insert(int index, TEdgeOne item) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public void RemoveAt(int index) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public TEdgeOne this[int index] {
            get {
                return (TEdgeOne) graphPair.Get(listTwo[index]);
            }
            set {
                throw new System.Exception("The method or operation is not implemented.");
            }
        }

        #endregion

        #region ICollection<T> Member

        public void Add(TEdgeOne item) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public void Clear() {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public bool Contains(TEdgeOne item) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public void CopyTo(TEdgeOne[] array, int arrayIndex) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public int Count {
            get { throw new System.Exception("The method or operation is not implemented."); }
        }

        public bool IsReadOnly {
            get { throw new System.Exception("The method or operation is not implemented."); }
        }

        public bool Remove(TEdgeOne item) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable<T> Member

        public IEnumerator<TEdgeOne> GetEnumerator() {
            throw new System.Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable Member

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            throw new System.Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}