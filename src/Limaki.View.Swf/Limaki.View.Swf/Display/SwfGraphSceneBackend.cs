using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.Display;
using Limaki.View.Gdi.UI;

namespace Limaki.View.Swf.Display {
    public abstract class SwfGraphSceneBackend<TItem, TEdge> : SwfWidgetBackend<IGraphScene<TItem, TEdge>>
        where TEdge : TItem, IEdge<TItem> {
    }

    public class SwfGraphSceneBackendComposer<TItem, TEdge> : SwfBackendComposer<IGraphScene<TItem, TEdge>>
    where TEdge : TItem, IEdge<TItem> {
        public override void Factor(Display<IGraphScene<TItem, TEdge>> display) {
            base.Factor(display);
            this.DataLayer = new GdiGraphSceneLayer<TItem, TEdge>();
        }
    }
}