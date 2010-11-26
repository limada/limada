using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;
using Limaki.Widgets;

namespace Limaki.Tests.Widget {
    public class BenchmarkOneGraphFactory : GraphFactoryBase {
        public override string Name {
            get { return "Benchmark 1"; }
        }

        IWidget _line1 = null;
        public IWidget Line1 {
            get {
                if (_line1 == null) {
                    Vector vector = new Vector ();
                    _line1 = new Widget<string>("line");
                    _line1.Shape = new VectorShape(vector);
                }
                return _line1;

            }
        }

        public override void Populate(IGraph<IGraphItem, IGraphEdge> scene, int start) {
            IGraphItem widget = new GraphItem<string>("first node");
            scene.Add(widget);
            Node[1] = widget;


            widget = new GraphItem<string>("second node");
            scene.Add(widget);
            Node[2] = widget;


            widget = new GraphItem<string>("third node");
            scene.Add(widget);
            Node[3] = widget;

            widget = new GraphItem<string>("fourth node");
            scene.Add(widget);
            Node[4] = widget;

            IGraphEdge edgeWidget = new GraphEdge(Node[1],Node[2]);
            scene.Add(edgeWidget);
            Link[1] = edgeWidget;

            edgeWidget = new GraphEdge (Node[3], Link[1]);
            scene.Add(edgeWidget);
            Link[2] = edgeWidget;


            edgeWidget = new GraphEdge(Node[4], Link[2]);
            scene.Add(edgeWidget);
            Link[3] = edgeWidget;

            // second lattice

            widget = new GraphItem<string>("fifth node");
            scene.Add(widget);
            Node[5] = widget;

            widget = new GraphItem<string>("sixth node");
            scene.Add(widget);
            Node[6] = widget;


            widget = new GraphItem<string>("seventh node");
            scene.Add(widget);
            Node[7] = widget;

            widget = new GraphItem<string>("eigth node");
            scene.Add(widget);
            Node[8] = widget;


            edgeWidget = new GraphEdge(Node[5],Node[6]);
            scene.Add(edgeWidget);
            Link[4] = edgeWidget;

            edgeWidget = new GraphEdge(Node[7], Node[8]);
            scene.Add(edgeWidget);
            Link[5] = edgeWidget;


            edgeWidget = new GraphEdge(Link[4],Link[5]);
            scene.Add(edgeWidget);
            Link[6] = edgeWidget;
        }
    }
}