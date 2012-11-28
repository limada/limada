using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Styles;
using Limaki.Graphs;
using Limaki.View.Modelling;
using Limaki.View.Rendering;
using Limaki.View.UI.GraphScene;
using Limaki.Viewers;
using Xwt.Drawing;

namespace Limaki.View.Display {

    public class GraphSceneVisualizer<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {

        public IGraphScene<TItem, TEdge> Scene { get; set; }
        public IGraphSceneLayout<TItem, TEdge> Layout { get; set; }
        public GraphSceneFacade<TItem, TEdge> Folder { get; set; }
        public IGraphSceneReceiver<TItem, TEdge> Receiver { get; set; }
        public GraphSceneContextPainter<TItem, TEdge> Painter { get; set; }
        public IStyleSheet StyleSheet { get; set; }

        public virtual void Compose (IGraphScene<TItem, TEdge> scene, IGraphItemRenderer<TItem, TEdge> itemRenderer) {

            this.Scene = scene;
            Get<IGraphScene<TItem, TEdge>> fScene = () => scene;

            if (StyleSheet == null) {
                StyleSheet = Registry.Pool.TryGetCreate<StyleSheets>().DefaultStyleSheet;

            }

            Layout = Registry.Factory.Create<IGraphSceneLayout<TItem, TEdge>>(fScene, StyleSheet);
            Layout.Orientation = Limaki.Drawing.Orientation.LeftRight;

            Painter = new GraphSceneContextPainter<TItem, TEdge>(Scene, Layout, itemRenderer);

            var modelReceiver = new GraphItemReceiver<TItem, TEdge>();
            Receiver = new GraphSceneReceiver<TItem, TEdge>();
            Receiver.GraphScene = fScene;
            Receiver.Layout = () => Layout;
            Receiver.Camera = () => Painter.Viewport.Camera;
            Receiver.Clipper = () => Painter.Clipper;
            Receiver.ModelReceiver = () => modelReceiver;

            Folder = new GraphSceneFacade<TItem, TEdge>(fScene, Layout);

        }
    }
}