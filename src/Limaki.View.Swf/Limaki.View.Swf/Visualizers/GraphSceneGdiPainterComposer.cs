using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.Gdi.UI;
using Limaki.View.Visualizers;


namespace Limaki.View.Visualizers {

    public class GraphSceneGdiPainterComposer<TItem, TEdge> : GraphScenePainterComposer<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {
        public override void Factor (GraphScenePainter<TItem, TEdge> painter) {
            base.Factor (painter);

            var layer = new GdiGraphSceneLayer<TItem, TEdge> ();
            var renderer = new GraphScenePainterRenderer<IGraphScene<TItem, TEdge>> ();
            renderer.Layer = layer;

            painter.BackendRenderer = renderer;
            painter.DataLayer = layer;

            painter.Viewport = new Viewport ();
        }

        public override void Compose (GraphScenePainter<TItem, TEdge> display) {
            base.Compose (display);

        }

    }
}