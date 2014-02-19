using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.Visuals;
using Limaki.Common;

namespace Limaki.Tests.Visuals {
    public class BenchmarkOneGraphFactory<IGraphEntity, IGraphEdge> : BasicTestGraphFactory<IGraphEntity, IGraphEdge>
        where IGraphEdge : IEdge<IGraphEntity>, IGraphEntity {
        public override string Name {
            get { return "Benchmark 1"; }
        }

        IVisual _line1 = null;
        public IVisual Line1 {
            get {
                if (_line1 == null) {
                    var vector = new Vector ();
                    _line1 = Registry.Pool.TryGetCreate<IVisualFactory>()
                        .CreateItem("line");
                    _line1.Shape = new VectorShape(vector);
                }
                return _line1;

            }
        }

        public override void Populate(IGraph<IGraphEntity, IGraphEdge> scene, int start) {
            var item = CreateItem<string>("first node");
            scene.Add(item);
            Nodes[1] = item;


            item = CreateItem<string>("second node");
            scene.Add(item);
            Nodes[2] = item;


            item = CreateItem<string>("third node");
            scene.Add(item);
            Nodes[3] = item;

            item = CreateItem<string>("fourth node");
            scene.Add(item);
            Nodes[4] = item;

            var edge = CreateEdge(Nodes[1],Nodes[2]);
            scene.Add(edge);
            Edges[1] = edge;

            edge = CreateEdge (Nodes[3], Edges[1]);
            scene.Add(edge);
            Edges[2] = edge;


            edge = CreateEdge(Nodes[4], Edges[2]);
            scene.Add(edge);
            Edges[3] = edge;

            // second lattice

            item = CreateItem<string>("fifth node");
            scene.Add(item);
            Nodes[5] = item;

            item = CreateItem<string>("sixth node");
            scene.Add(item);
            Nodes[6] = item;


            item = CreateItem<string>("seventh node");
            scene.Add(item);
            Nodes[7] = item;

            item = CreateItem<string>("eigth node");
            scene.Add(item);
            Nodes[8] = item;


            edge = CreateEdge(Nodes[5],Nodes[6]);
            scene.Add(edge);
            Edges[4] = edge;

            edge = CreateEdge(Nodes[7], Nodes[8]);
            scene.Add(edge);
            Edges[5] = edge;


            edge = CreateEdge(Edges[4],Edges[5]);
            scene.Add(edge);
            Edges[6] = edge;
        }
    }
}