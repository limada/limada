using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.Gdi.UI;
using Limaki.Viewers;

namespace Limaki.View.Viewers.Swf {
    public class GraphScenePainterGdiComposer<TItem, TEdge> : GraphScenePainterComposer<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {
        public override void Factor (GraphScenePainter<TItem, TEdge> painter) {
            base.Factor (painter);

            var layer = new GdiGraphSceneLayer<TItem, TEdge> ();
            var renderer = new GraphScenePainterRenderer<IGraphScene<TItem, TEdge>> ();
            renderer.Layer = layer;

            painter.DeviceRenderer = renderer;
            painter.DataLayer = layer;

            painter.Viewport = new GdiViewport ();
        }

        public override void Compose (GraphScenePainter<TItem, TEdge> display) {
            base.Compose (display);

        }

    }
}