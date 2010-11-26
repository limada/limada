using System;
using System.Collections.Generic;


namespace Limaki.Graphs {
    public class GraphView<TItem, TEdge> : GraphBase<TItem, TEdge>
        where TEdge : IEdge<TItem> {

        IGraph<TItem, TEdge> _source = null;
        public IGraph<TItem, TEdge> Data {
            get { return _source; }
            set { _source = value; }
        }

        IGraph<TItem, TEdge> _sub = null;
        public IGraph<TItem, TEdge> View {
            get { return _sub; }
            set { _sub = value; }
        }


        protected override void AddEdge(TEdge edge, TItem item) {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override bool RemoveEdge(TEdge edge, TItem item) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void ChangeEdge(TEdge edge, TItem oldItem, TItem newItem) {
            View.ChangeEdge(edge, oldItem, newItem);
            Data.ChangeEdge(edge, oldItem, newItem);
        }

        public override bool Contains(TEdge edge) {
            return View.Contains(edge);
        }

        public override void Add(TEdge edge) {
            Data.Add(edge);
            View.Add(edge);
        }

        public override bool Remove(TEdge edge) {
            Data.Remove(edge);
            return View.Remove(edge);
        }

        public override int EdgeCount(TItem item) {
            return View.EdgeCount(item);
        }

        public override ICollection<TEdge> Edges(TItem item) {
            return View.Edges(item);
        }

        public override IEnumerable<TEdge> Edges() {
            return View.Edges();
        }

        public override IEnumerable<KeyValuePair<TItem, ICollection<TEdge>>> ItemsWithEdges() {
            return View.ItemsWithEdges();
        }

        public override void Add(TItem item) {
            View.Add(item);
            Data.Add(item);
        }

        public override void Clear() {
            View.Clear();
        }

        public override bool Contains(TItem item) {
            return View.Contains(item);
        }

        public override void CopyTo(TItem[] array, int arrayIndex) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int Count {
            get { return View.Count; }
        }

        public override bool IsReadOnly {
            get { return View.IsReadOnly; }
        }

        public override bool Remove(TItem item) {
            Data.Remove(item);
            return View.Remove(item);
        }

        public override IEnumerator<TItem> GetEnumerator() {
            return View.GetEnumerator();
        }
        public override void OnDataChanged(TItem item) {
            base.OnDataChanged(item);
            Data.OnDataChanged(item);
        }
    }
}