using System.Collections.Generic;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Indexing.QuadTrees;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using Limaki.Visuals;
using Limaki.View.Visuals;
using Limaki.View.Visualizers;
using Xwt;

namespace Limaki.Tests.View.Winform {
    public class QuadTreeVisualizer {
        private Quadtree<IVisual> _data = null;

        public Quadtree<IVisual> Data {
            get { return _data; }
            set {
                _data = value;
                PopulateDisplay();
            }
        }

        public GraphSceneDisplay<IVisual, IVisualEdge> VisualsDisplay {get;set;}
        
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
            VisualsDisplay.SelectAction.Enabled = true;
            
            if (Data != null) {

                var style = VisualsDisplay.StyleSheet.BaseStyle;
                var s = new Point(float.MaxValue, float.MaxValue).ToString() + "\r\n" +
                           new Rectangle(float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue).ToString() + "\r\n";

                style.AutoSize = (Size) drawingUtils.GetTextDimension(s, style);


                this.VisualsDisplay.ZoomState = ZoomState.Original;

                this.VisualsDisplay.Data = new Scene();

                var nodesDone = new Dictionary<Node<IVisual>, IVisual>();
                var itemsDone = new Dictionary<IVisual, IVisual>();

                var rootNode = new Node<IVisual>(new Rectangle(), 0);
                rootNode.Items = Data.Root.Items;
                for (int i = 0; i < Data.Root.Subnodes.Length; i++)
                    rootNode.Subnodes[i] = Data.Root.Subnodes[i];
                var root = VisualNode(rootNode, nodesDone, itemsDone);


                this.VisualsDisplay.Data.Focused = root;
                this.VisualsDisplay.Layout.Centered = true;
                this.VisualsDisplay.Layout.Dimension = Dimension.Y;
                var autoSize = VisualsDisplay.StyleSheet.BaseStyle.AutoSize;
                this.VisualsDisplay.StyleSheet.BaseStyle.AutoSize = new Size ();
                foreach(var visual in VisualsDisplay.Data.Elements) {
                    if (visual.Shape != null)
                        VisualsDisplay.Layout.Justify (visual);
                }
                this.VisualsDisplay.Invoke();
                this.VisualsDisplay.Execute ();
                this.VisualsDisplay.BackendRenderer.Render ();
                this.VisualsDisplay.StyleSheet.BaseStyle.AutoSize = autoSize;
            }
        }

        private IVisual VisualNode(Node<IVisual> node,
                                   IDictionary<Node<IVisual>, IVisual> nodesDone,
                                   IDictionary<IVisual, IVisual> itemsDone) {

            var factory = Registry.Pool.TryGetCreate<IVisualFactory>();

            if (node != null) {
                IVisual result = factory.CreateItem(
                    node.Centre.ToString() + "\r\n" + node.Envelope.ToString());

                result.Shape = new RectangleShape(node.Envelope);
                this.VisualsDisplay.Data.Add(result);
                nodesDone.Add(node, result);
                NodeItems(node, result, itemsDone);

                if (node.HasSubNodes) {
                    var subRoot = factory.CreateItem(" ° ");
                    this.VisualsDisplay.Data.Add(
                        factory.CreateEdge(result, subRoot, string.Empty));

                    foreach (Node<IVisual> sub in node.Subnodes) {
                        if (sub != null) {
                            var subVisual = VisualNode(sub, nodesDone, itemsDone);
                            this.VisualsDisplay.Data.Add(
                                factory.CreateEdge(subRoot, subVisual, string.Empty));
                        }
                    }
                }
                return result;
            } else
                return null;
        }

        private void NodeItems(Node<IVisual> node, IVisual nodeVisual,
                               IDictionary<IVisual, IVisual> itemsDone) {

            var factory = Registry.Pool.TryGetCreate<IVisualFactory>();

            foreach (var visual in node.Items) {
                IVisual childVisual = null;
                itemsDone.TryGetValue(visual, out childVisual);
                if (childVisual == null) {
                    string ws = visual.Data.ToString();
                    if (visual is IVisualEdge) {
                        ws = GraphExtensions.EdgeString<IVisual, IVisualEdge>((IVisualEdge)visual);
                    }
                    string ds = ws + "\r\n" +
                                visual.Shape.BoundsRect.ToString();

                    if (!node.Envelope.Contains(visual.Shape.BoundsRect))
                        ds = "! " + ds;
                    childVisual = factory.CreateItem(ds);
                    childVisual.Shape = this.VisualsDisplay.ShapeFactory.Create<IRoundedRectangleShape>();
                    childVisual.Location = visual.Location;
                    childVisual.Size = visual.Size;

                    this.VisualsDisplay.Data.Add(childVisual);
                    itemsDone.Add(visual, childVisual);
                }

                var edge = factory.CreateEdge(childVisual, nodeVisual, string.Empty);

                edge.Shape = this.VisualsDisplay.Layout.CreateShape(edge);
                this.VisualsDisplay.Data.Add(edge);
            }


        }
    }
}