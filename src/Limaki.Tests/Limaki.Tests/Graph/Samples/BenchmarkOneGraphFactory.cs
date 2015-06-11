using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using Limaki.View.Visuals;

namespace Limaki.Tests.Graph.Model {

    public class BenchmarkOneGraphFactory<TItem, TEdge> : SampleGraphFactory<TItem, TEdge>
        where TEdge : IEdge<TItem>, TItem {
        public override string Name {
            get { return "Benchmark 1"; }
        }

        IVisual _line1 = null;
        public IVisual Line1 {
            get {
                if (_line1 == null) {
                    var vector = new Vector ();
                    _line1 = Registry.Pooled<IVisualFactory>()
                        .CreateItem("line");
                    _line1.Shape = new VectorShape(vector);
                }
                return _line1;

            }
        }

        public override void Populate(IGraph<TItem, TEdge> graph, int start) {
            var item = CreateItem<string>("first node");
            graph.Add(item);
            Nodes[1] = item;


            item = CreateItem<string>("second node");
            graph.Add(item);
            Nodes[2] = item;


            item = CreateItem<string>("third node");
            graph.Add(item);
            Nodes[3] = item;

            item = CreateItem<string>("fourth node");
            graph.Add(item);
            Nodes[4] = item;

            var edge = CreateEdge(Nodes[1],Nodes[2]);
            graph.Add(edge);
            Edges[1] = edge;

            edge = CreateEdge (Nodes[3], Edges[1]);
            graph.Add(edge);
            Edges[2] = edge;


            edge = CreateEdge(Nodes[4], Edges[2]);
            graph.Add(edge);
            Edges[3] = edge;

            // second lattice

            item = CreateItem<string>("fifth node");
            graph.Add(item);
            Nodes[5] = item;

            item = CreateItem<string>("sixth node");
            graph.Add(item);
            Nodes[6] = item;


            item = CreateItem<string>("seventh node");
            graph.Add(item);
            Nodes[7] = item;

            item = CreateItem<string>("eigth node");
            graph.Add(item);
            Nodes[8] = item;


            edge = CreateEdge(Nodes[5],Nodes[6]);
            graph.Add(edge);
            Edges[4] = edge;

            edge = CreateEdge(Nodes[7], Nodes[8]);
            graph.Add(edge);
            Edges[5] = edge;


            edge = CreateEdge(Edges[4],Edges[5]);
            graph.Add(edge);
            Edges[6] = edge;
        }
    }
}