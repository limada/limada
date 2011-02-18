using System.Collections.Generic;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Indexing.QuadTrees;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using Limaki.Widgets;
using Limaki.Presenter.Widgets;

namespace Limaki.Tests.Presenter.Winform {
    public class QuadTreeVisualizer {
        private Quadtree<IWidget> _data = null;

        public Quadtree<IWidget> Data {
            get { return _data; }
            set {
                _data = value;
                PopulateDisplay();


            }
        }

        public WidgetDisplay widgetDisplay {get;set;}
        
        static IDrawingUtils _drawingUtils = null;
        protected static IDrawingUtils drawingUtils {
            get {
                if (_drawingUtils == null) {
                    _drawingUtils = Registry.Factory.Create<IDrawingUtils>();
                }
                return _drawingUtils;
            }
        }

        private void PopulateDisplay() {
            widgetDisplay.SelectAction.Enabled = true;
            
            if (Data != null) {

                IStyle style = widgetDisplay.StyleSheet[StyleNames.DefaultStyle];
                string s = new PointS(float.MaxValue, float.MaxValue).ToString() + "\r\n" +
                           new RectangleS(float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue).ToString() + "\r\n";

                style.AutoSize =
                    drawingUtils.GetTextDimension(s, style).ToSize();


                this.widgetDisplay.ZoomState = ZoomState.Original;

                this.widgetDisplay.Data = new Scene();

                IDictionary<Node<IWidget>, IWidget> nodesDone = new Dictionary<Node<IWidget>, IWidget>();
                IDictionary<IWidget, IWidget> itemsDone = new Dictionary<IWidget, IWidget>();

                Node<IWidget> rootNode = new Node<IWidget>(new RectangleS(), 0);
                rootNode.Items = Data.Root.Items;
                for (int i = 0; i < Data.Root.Subnodes.Length; i++)
                    rootNode.Subnodes[i] = Data.Root.Subnodes[i];
                IWidget root = NodeWidget(rootNode, nodesDone, itemsDone);


                this.widgetDisplay.Data.Focused = root;
                this.widgetDisplay.Layout.Centered = true;
                this.widgetDisplay.Layout.Orientation = Orientation.TopBottom;
                var autoSize = widgetDisplay.StyleSheet.DefaultStyle.AutoSize;
                this.widgetDisplay.StyleSheet.DefaultStyle.AutoSize = new SizeI ();
                foreach(var widget in widgetDisplay.Data.Elements) {
                    if (widget.Shape != null)
                        widgetDisplay.Layout.Justify (widget);
                }
                this.widgetDisplay.Invoke();
                this.widgetDisplay.Execute ();
                this.widgetDisplay.DeviceRenderer.Render ();
                this.widgetDisplay.StyleSheet.DefaultStyle.AutoSize = autoSize;
            }
        }

        private IWidget NodeWidget(Node<IWidget> node,
                                   IDictionary<Node<IWidget>, IWidget> nodesDone,
                                   IDictionary<IWidget, IWidget> itemsDone) {

            var factory = Registry.Pool.TryGetCreate<IWidgetFactory>();

            if (node != null) {
                IWidget result = factory.CreateItem(
                    node.Centre.ToString() + "\r\n" + node.Envelope.ToString());

                result.Shape = new RectangleShape(RectangleI.Ceiling(node.Envelope));
                this.widgetDisplay.Data.Add(result);
                nodesDone.Add(node, result);
                NodeItems(node, result, itemsDone);

                if (node.HasSubNodes) {
                    IWidget subRoot = factory.CreateItem(" ° ");
                    this.widgetDisplay.Data.Add(
                        factory.CreateEdge(result, subRoot, string.Empty));

                    foreach (Node<IWidget> sub in node.Subnodes) {
                        if (sub != null) {
                            IWidget subWidget = NodeWidget(sub, nodesDone, itemsDone);
                            this.widgetDisplay.Data.Add(
                                factory.CreateEdge(subRoot, subWidget, string.Empty));
                        }
                    }
                }
                return result;
            } else
                return null;
        }

        private void NodeItems(Node<IWidget> node, IWidget nodeWidget,
                               IDictionary<IWidget, IWidget> itemsDone) {

            var factory = Registry.Pool.TryGetCreate<IWidgetFactory>();

            foreach (IWidget widget in node.Items) {
                IWidget childWidget = null;
                itemsDone.TryGetValue(widget, out childWidget);
                if (childWidget == null) {
                    string ws = widget.Data.ToString();
                    if (widget is IEdgeWidget) {
                        ws = GraphExtensions.EdgeString<IWidget, IEdgeWidget>((IEdgeWidget)widget);
                    }
                    string ds = ws + "\r\n" +
                                widget.Shape.BoundsRect.ToString();

                    if (!node.Envelope.Contains(widget.Shape.BoundsRect))
                        ds = "! " + ds;
                    childWidget = factory.CreateItem(ds);
                    childWidget.Shape =
                        this.widgetDisplay.ShapeFactory.Create<IRoundedRectangleShape>();
                    childWidget.Location = widget.Location;
                    childWidget.Size = widget.Size;

                    this.widgetDisplay.Data.Add(childWidget);
                    itemsDone.Add(widget, childWidget);
                }

                IEdgeWidget edge =
                    factory.CreateEdge(childWidget, nodeWidget, string.Empty);

                edge.Shape = this.widgetDisplay.Layout.CreateShape(edge);
                this.widgetDisplay.Data.Add(edge);
            }


        }
    }
}