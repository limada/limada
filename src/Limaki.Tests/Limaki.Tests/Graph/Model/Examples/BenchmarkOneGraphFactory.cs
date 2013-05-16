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
            Node[1] = item;


            item = new GraphEntity<string>("second node");
            scene.Add(item);
            Node[2] = item;


            item = new GraphEntity<string>("third node");
            scene.Add(item);
            Node[3] = item;

            item = new GraphEntity<string>("fourth node");
            scene.Add(item);
            Node[4] = item;

            var edge = new GraphEdge(Node[1],Node[2]);
            scene.Add(edge);
            Edge[1] = edge;

            edge = new GraphEdge (Node[3], Edge[1]);
            scene.Add(edge);
            Edge[2] = edge;


            edge = new GraphEdge(Node[4], Edge[2]);
            scene.Add(edge);
            Edge[3] = edge;

            // second lattice

            item = new GraphEntity<string>("fifth node");
            scene.Add(item);
            Node[5] = item;

            item = new GraphEntity<string>("sixth node");
            scene.Add(item);
            Node[6] = item;


            item = new GraphEntity<string>("seventh node");
            scene.Add(item);
            Node[7] = item;

            item = new GraphEntity<string>("eigth node");
            scene.Add(item);
            Node[8] = item;


            edge = new GraphEdge(Node[5],Node[6]);
            scene.Add(edge);
            Edge[4] = edge;

            edge = new GraphEdge(Node[7], Node[8]);
            scene.Add(edge);
            Edge[5] = edge;


            edge = new GraphEdge(Edge[4],Edge[5]);
            scene.Add(edge);
            Edge[6] = edge;
        }
    }
}