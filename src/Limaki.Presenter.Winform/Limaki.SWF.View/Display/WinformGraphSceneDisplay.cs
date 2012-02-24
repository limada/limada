using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.Display;
using Limaki.View.GDI.UI;

namespace Limaki.View.Winform.Display {
    public abstract class WinformGraphSceneDisplay<TItem, TEdge> : WinformDisplay<IGraphScene<TItem, TEdge>>
        where TEdge : TItem, IEdge<TItem> {
    }

    public class WinformGraphSceneDeviceComposer<TItem, TEdge> : WinformDeviceComposer<IGraphScene<TItem, TEdge>>
    where TEdge : TItem, IEdge<TItem> {
        public override void Factor(Display<IGraphScene<TItem, TEdge>> display) {
            base.Factor(display);
            this.DataLayer = new GDIGraphSceneLayer<TItem, TEdge>();
        }
    }
}