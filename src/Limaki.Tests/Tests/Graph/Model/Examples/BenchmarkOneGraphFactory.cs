using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.Widgets;
using Limaki.Common;

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
                    _line1 = Registry.Pool.TryGetCreate<IWidgetFactory>()
                        .CreateItem("line");
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
            Edge[1] = edgeWidget;

            edgeWidget = new GraphEdge (Node[3], Edge[1]);
            scene.Add(edgeWidget);
            Edge[2] = edgeWidget;


            edgeWidget = new GraphEdge(Node[4], Edge[2]);
            scene.Add(edgeWidget);
            Edge[3] = edgeWidget;

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
            Edge[4] = edgeWidget;

            edgeWidget = new GraphEdge(Node[7], Node[8]);
            scene.Add(edgeWidget);
            Edge[5] = edgeWidget;


            edgeWidget = new GraphEdge(Edge[4],Edge[5]);
            scene.Add(edgeWidget);
            Edge[6] = edgeWidget;
        }
    }
}