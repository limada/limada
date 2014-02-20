using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.View.Visuals;
using Limaki.Tests.Visuals;
using Limaki.Visuals;
using NUnit.Framework;
using Limaki.Tests.Graph.Basic;

namespace Limaki.Tests.Graph.Wrappers {

    public abstract class BasicGraphPairTest<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> : BasicGraphTests<TItemOne,TEdgeOne>

        where TEdgeOne : IEdge<TItemOne>, TItemOne
        where TEdgeTwo : IEdge<TItemTwo>, TItemTwo {


        public virtual GraphPair<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> Pair {
            get { return Graph as GraphPair<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo>; }
        }

        public override void Contains(TItemOne item) {
            base.Contains(item);
            Pair.Sink.Contains (item);
            TItemTwo item2 = Pair.Get (item);
            Assert.IsNotNull (item2);
            Assert.IsTrue (Pair.Source.Contains (item2));

            if (item is TEdgeOne) {
                TEdgeOne edge = (TEdgeOne)item;
                TEdgeTwo edge2 = (TEdgeTwo)Pair.Get(edge);
                TItemTwo root2 = Pair.Get(edge.Root);
                TItemTwo leaf2 = Pair.Get(edge.Leaf);


                Assert.AreSame (edge2.Root, root2);
                Assert.AreSame(edge2.Leaf, leaf2);
                Assert.IsTrue(Pair.Source.Contains(edge2), edge2.ToString());
                Assert.IsTrue(Pair.Source.Contains(edge2.Root), edge2.Root.ToString());
                Assert.IsTrue(Pair.Source.Contains(edge2.Leaf), edge2.Leaf.ToString());

                Assert.IsTrue(Pair.Source.Contains(root2), root2.ToString());
                Assert.IsTrue(Pair.Source.Contains(leaf2), leaf2.ToString());
            }
        }

        public override void IsEdgeChanged(TEdgeOne edge, TItemOne oldItem, TItemOne newItem, bool newIsRoot) {
            base.IsEdgeChanged(edge, oldItem, newItem, newIsRoot);
            TEdgeTwo edge2 = (TEdgeTwo)Pair.Get(edge);
            TItemTwo newItem2 = Pair.Get (newItem);
            TItemTwo oldItem2 = Pair.Get (oldItem);

            if (newIsRoot)
                Assert.AreEqual(edge2.Root, newItem2);
            else
                Assert.AreEqual(edge2.Leaf, newItem2);

            Assert.IsFalse(Pair.Source.Edges(oldItem2).Contains(edge2));
            Assert.IsTrue(Pair.Source.Edges(newItem2).Contains(edge2));
        }

        public override void IsRemoved(TItemOne item) {
            base.IsRemoved(item);
            Assert.IsFalse(Pair.Contains(item), item.ToString());
            Assert.IsFalse(Pair.Sink.Contains(item), item.ToString());

            TItemTwo item2 = Pair.Get(item);
            Assert.IsNull(item2, "pair.Get(item) must be null\t" + item.ToString());

            item2 = Pair.Mapper.Get(item);
            Assert.IsNull(item2, "pair.Get(item) must be null\t" + item.ToString());

        }

        [Test]
        public override void RemoveItem() {
            base.RemoveItem();

            TItemTwo item2 = Pair.Get(Data.Three);

            Graph.Remove (Data.Three);
            IsRemoved (Data.Three);

            Assert.IsFalse(Pair.Source.Contains(item2), "pair.Source contains:\t" + item2.ToString());

            TItemOne pingback = Pair.Get(item2);
            Assert.IsNull(pingback, "pair.Get(item) must be null\t" + item2.ToString());

            pingback = Pair.Mapper.Get(item2);
            Assert.IsNull(pingback, "pair.Get(item) must be null\t" + item2.ToString());

            
        }

        [Test]
        public override void RemoveEdgeOfEdgeLeaf() {
            base.RemoveEdgeOfEdgeLeaf();
        }
    }
}