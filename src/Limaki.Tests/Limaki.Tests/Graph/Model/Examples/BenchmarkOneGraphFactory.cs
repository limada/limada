using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.Visuals;
using Limaki.Common;

namespace Limaki.Tests.Visuals {
    public class BenchmarkOneGraphFactory : GraphFactoryBase {
        public override string Name {
            get { return "Benchmark 1"; }
        }

        IVisual _line1 = null;
        public IVisual Line1 {
            get {
                if (_line1 == null) {
                    Vector vector = new Vector ();
                    _line1 = Registry.Pool.TryGetCreate<IVisualFactory>()
                        .CreateItem("line");
                    _line1.Shape = new VectorShape(vector);
                }
                return _line1;

            }
        }

        public override void Populate(IGraph<IGraphEntity, IGraphEdge> scene, int start) {
            var item = new GraphEntity<string>("first node");
            scene.Add(item);
            Nodes[1] = item;


            item = new GraphEntity<string>("second node");
            scene.Add(item);
            Nodes[2] = item;


            item = new GraphEntity<string>("third node");
            scene.Add(item);
            Nodes[3] = item;

            item = new GraphEntity<string>("fourth node");
            scene.Add(item);
            Nodes[4] = item;

            var edge = new GraphEdge(Nodes[1],Nodes[2]);
            scene.Add(edge);
            Edges[1] = edge;

            edge = new GraphEdge (Nodes[3], Edges[1]);
            scene.Add(edge);
            Edges[2] = edge;


            edge = new GraphEdge(Nodes[4], Edges[2]);
            scene.Add(edge);
            Edges[3] = edge;

            // second lattice

            item = new GraphEntity<string>("fifth node");
            scene.Add(item);
            Nodes[5] = item;

            item = new GraphEntity<string>("sixth node");
            scene.Add(item);
            Nodes[6] = item;


            item = new GraphEntity<string>("seventh node");
            scene.Add(item);
            Nodes[7] = item;

            item = new GraphEntity<string>("eigth node");
            scene.Add(item);
            Nodes[8] = item;


            edge = new GraphEdge(Nodes[5],Nodes[6]);
            scene.Add(edge);
            Edges[4] = edge;

            edge = new GraphEdge(Nodes[7], Nodes[8]);
            scene.Add(edge);
            Edges[5] = edge;


            edge = new GraphEdge(Edges[4],Edges[5]);
            scene.Add(edge);
            Edges[6] = edge;
        }
    }
}