using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Viewers;
using Limaki.View.Visualizers;

namespace Limaki.View.XwtContext {

    public class GraphSceneContextPainterComposer<TItem, TEdge> : GraphScenePainterComposer<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {
        public override void Factor(GraphScenePainter<TItem, TEdge> painter) {
            base.Factor(painter);

            var layer = new GraphSceneContextLayer<TItem, TEdge>();
            var renderer = new GraphSceneContextPainterRenderer<IGraphScene<TItem, TEdge>>();
            renderer.Layer = layer;

            painter.BackendRenderer = renderer;
            painter.DataLayer = layer;

            painter.Viewport = new Viewport();
        }

        public override void Compose(GraphScenePainter<TItem, TEdge> painter) {
            base.Compose(painter);
        }

    }
}