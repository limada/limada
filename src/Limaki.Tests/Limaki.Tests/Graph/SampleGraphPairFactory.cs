using Limaki.Common;
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;
using System.Collections.Generic;
using System.Linq;

namespace Limaki.Tests.Graph.Model {

    /// <summary>
    /// a <see cref="ISamleGraphFactorySamleGraphFactory{TItem,TEdge}"/> producing 
    /// a <see cref="IGraphPair{TItemOne, TItemTwo, TEdgeOne, TEdgeTwo}"/>
    /// </summary>

    public class SampleGraphPairFactory<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> : SampleGraphFactoryBase<TSinkItem, TSinkEdge>
        where TSinkEdge : IEdge<TSinkItem>, TSinkItem
        where TSourceEdge : IEdge<TSourceItem>, TSourceItem {

        public SampleGraphPairFactory(
            ISampleGraphFactory<TSourceItem, TSourceEdge> factory,
            GraphItemTransformer<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> transformer) {

            this._factory = factory;
            this._transformer = transformer;
            this._mapper =
                new GraphMapper<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> (Transformer);
        }

        public override string Name {
            get { return Factory.Name; }
        }

        public override IGraph<TSinkItem, TSinkEdge> Graph {
            get {
                if (_graph == null) {
                    _graph = Mapper.Sink;
                    if (Factory.Graph != null)
                        Mapper.Source = Factory.Graph;
                }
                return _graph;

            }
            set {
                _graph = value;
                Mapper.Sink = value;
            }
        }

        protected ISampleGraphFactory<TSourceItem, TSourceEdge> _factory = null;
        public virtual ISampleGraphFactory<TSourceItem, TSourceEdge> Factory {
            get { return _factory; }
        }

        private GraphItemTransformer<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> _transformer = null;
        public GraphItemTransformer<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> Transformer {
            get { return _transformer ?? (Registry.Factory.Create<GraphItemTransformer<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>> ()); }
        }

        private GraphMapper<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> _mapper = null;
        public GraphMapper<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> Mapper {
            get { return _mapper; }
        }

        private IGraphPair<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> _graphPair = null;
        public IGraphPair<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> GraphPair {
            get { return _graphPair; }
            set {
                _graphPair = value;
                //_graph = value;
            }
        }


        public override void Populate(IGraph<TSinkItem, TSinkEdge> graph) {
            Factory.Count = this.Count;
            Factory.AddDensity = this.AddDensity;
            Factory.SeperateLattice = this.SeperateLattice;

            Mapper.Sink = graph;

            var sourceGraph = Mapper.Source ?? new Graph<TSourceItem, TSourceEdge> ();
           
            Factory.Graph = sourceGraph;
            Factory.Populate();

            this.GraphPair = new LiveGraphPair<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>(
                        graph, sourceGraph, 
                        Mapper.Transformer
                        );

            this.GraphPair.Mapper = this.Mapper;
            this.Mapper.ConvertSourceSink();

            foreach (var sink in Mapper.Sink2Source.Keys)
                this.GraphPair.Sink.Add (sink);

        }

        public override IList<TSinkEdge> Edges {
            get {
                return new EdgeList<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>(
                    this.GraphPair, Factory.Edges);
            }
        }

        public override IList<TSinkItem> Nodes {
            get {
                return new ItemList<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>(
                    this.GraphPair, Factory.Nodes);
            }
        }
    }

    public class ItemList<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> : IList<TSinkItem>
        where TSinkEdge : IEdge<TSinkItem>, TSinkItem
        where TSourceEdge : IEdge<TSourceItem>, TSourceItem {


        public ItemList(IGraphPair<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> graphPair, IList<TSourceItem> listTwo) {
            this.graphPair = graphPair;
            this.listTwo = listTwo;
        }

        IList<TSourceItem> listTwo = null;
        IGraphPair<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> graphPair = null;

        #region IList<T> Member

        public int IndexOf(TSinkItem item) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public void Insert(int index, TSinkItem item) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public void RemoveAt(int index) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public TSinkItem this [int index] {
            get { return graphPair.Get (listTwo[index]); } 
            set { throw new System.ArgumentException (); }
        }

        #endregion

        #region ICollection<T> Member

        public void Add(TSinkItem item) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public void Clear() {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public bool Contains(TSinkItem item) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public void CopyTo(TSinkItem[] array, int arrayIndex) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public int Count {
            get { return Items.Where(i=>i!=null).Count (); }
        }

        public bool IsReadOnly {
            get { throw new System.Exception("The method or operation is not implemented."); }
        }

        public bool Remove(TSinkItem item) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable<T> Member
        protected IEnumerable<TSinkItem> Items { get { return listTwo.Select (item => graphPair.Get (item)); } }
        public IEnumerator<TSinkItem> GetEnumerator() {
            return Items.GetEnumerator ();
        }

        #endregion

        #region IEnumerable Member

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator ();
        }

        #endregion
    }

    public class EdgeList<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> : IList<TSinkEdge>
        where TSinkEdge : IEdge<TSinkItem>, TSinkItem
        where TSourceEdge : IEdge<TSourceItem>, TSourceItem {


        public EdgeList(IGraphPair<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> graphPair, IList<TSourceEdge> listTwo) {
            this.graphPair = graphPair;
            this.listTwo = listTwo;
        }


        IList<TSourceEdge> listTwo = null;
        IGraphPair<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> graphPair = null;

        #region IList<T> Member

        public int IndexOf(TSinkEdge item) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public void Insert(int index, TSinkEdge item) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public void RemoveAt(int index) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public TSinkEdge this[int index] {
            get {
                return (TSinkEdge) graphPair.Get(listTwo[index]);
            }
            set {
                throw new System.Exception("The method or operation is not implemented.");
            }
        }

        #endregion

        #region ICollection<T> Member

        public void Add(TSinkEdge item) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public void Clear() {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public bool Contains(TSinkEdge item) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public void CopyTo(TSinkEdge[] array, int arrayIndex) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public int Count {
            get { return Items.Count (); }
        }

        public bool IsReadOnly {
            get { return true; }
        }

        public bool Remove(TSinkEdge item) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable<T> Member

        protected IEnumerable<TSinkEdge> Items {
            get { return listTwo.Select (item => (TSinkEdge) graphPair.Get (item) ); }
        }

        public IEnumerator<TSinkEdge> GetEnumerator () {
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