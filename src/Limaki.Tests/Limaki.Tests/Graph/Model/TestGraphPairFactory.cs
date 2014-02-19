using Limaki.Graphs;
using Limaki.Tests.Graph.Model;
using System.Collections.Generic;
using System.Linq;

namespace Limaki.Tests.Graph.Model {

    /// <summary>
    /// a <see cref="ITestGraphFactory{TItemOne, TEdgeOne}"/> producing 
    /// a <see cref="IGraphPair{TItemOne, TItemTwo, TEdgeOne, TEdgeTwo}"/>
    /// </summary>

    public class TestGraphPairFactory<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> : 
        TestGraphFactory<TItemOne, TEdgeOne>
        where TEdgeOne : class, IEdge<TItemOne>, TItemOne
        where TEdgeTwo : IEdge<TItemTwo>, TItemTwo {

        public TestGraphPairFactory(
            ITestGraphFactory<TItemTwo, TEdgeTwo> data, 
            GraphItemTransformer<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne> transformer) {
            this._factory = data;
            this._transformer = transformer;
            this._mapper = 
                new GraphMapper<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne> (transformer);
        }

        public override string Name {
            get { return Factory.Name; }
        }

        public override IGraph<TItemOne, TEdgeOne> Graph {
            get {
                if (_graph == null) {
                    _graph = Mapper.Source;
                }
                return _graph;

            }
            set { _graph = value; }
        }

        protected ITestGraphFactory<TItemTwo, TEdgeTwo> _factory = null;
        public virtual ITestGraphFactory<TItemTwo, TEdgeTwo> Factory {
            get { return _factory; }
        }


        private GraphItemTransformer<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne> _transformer = null;
        public GraphItemTransformer<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne> Transformer {
            get { return _transformer; }
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

            var graphPair = 
                new LiveGraphPair<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne>(
                    new Graph<TItemTwo, TEdgeTwo>(),
                    graph,
                    this.Mapper.Transformer
                    );
            
            graphPair.Mapper = this.Mapper;

            Factory.Graph = graphPair;
            Factory.Populate();

            this.GraphPair = new LiveGraphPair<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo>(
                        graphPair.Source, graphPair.Sink, 
                        Mapper.Transformer.Reverted()
                        );
            this.GraphPair.Mapper = this.Mapper.ReverseMapper ();

        }

        public override IList<TEdgeOne> Edges {
            get {
                return new EdgeList<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo>(
                    this.GraphPair, Factory.Edges);
            }
        }

        public override IList<TItemOne> Nodes {
            get {
                return new ItemList<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo>(
                    this.GraphPair, Factory.Nodes);
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

        public TItemOne this [int index] {
            get { return graphPair.Get (listTwo[index]); } 
            set { throw new System.ArgumentException (); }
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
            get { return Items.Where(i=>i!=null).Count (); }
        }

        public bool IsReadOnly {
            get { throw new System.Exception("The method or operation is not implemented."); }
        }

        public bool Remove(TItemOne item) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable<T> Member
        protected IEnumerable<TItemOne> Items { get { return listTwo.Select (item => graphPair.Get (item)); } }
        public IEnumerator<TItemOne> GetEnumerator() {
            return Items.GetEnumerator ();
        }

        #endregion

        #region IEnumerable Member

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator ();
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
            get { return Items.Count (); }
        }

        public bool IsReadOnly {
            get { return true; }
        }

        public bool Remove(TEdgeOne item) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable<T> Member

        protected IEnumerable<TEdgeOne> Items {
            get { return listTwo.Select (item => graphPair.Get (item) as TEdgeOne); }
        }

        public IEnumerator<TEdgeOne> GetEnumerator () {
            return Items.GetEnumerator ();
        }

        #endregion

        #region IEnumerable Member

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.GetEnumerator ();
        }

        #endregion
    }
}