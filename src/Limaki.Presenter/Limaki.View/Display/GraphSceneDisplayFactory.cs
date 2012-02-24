using Limaki.Drawing;
using Limaki.Graphs;

namespace Limaki.View.Display {
    public class GraphSceneDisplayFactory<TItem, TEdge> : DisplayFactory<IGraphScene<TItem, TEdge>>
        where TEdge : TItem, IEdge<TItem> {
        public override Display<IGraphScene<TItem, TEdge>> Create() {
            var result = new GraphSceneDisplay<TItem, TEdge>();
            return result;
        }

        public override IComposer<Display<IGraphScene<TItem, TEdge>>> DisplayComposer {
            get {
                if (_displayComposer == null) {
                    _displayComposer = new GraphSceneDisplayComposer<TItem, TEdge>();
                }
                return base.DisplayComposer;
            }
            set { base.DisplayComposer = value; }
        }       
        }
}