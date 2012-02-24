using Limaki.Drawing;
using Limaki.Presenter.Display;
using Limaki.Visuals;

namespace Limaki.Presenter.Visuals.Display {
    public class VisualsDisplayFactory:GraphSceneDisplayFactory<IVisual,IVisualEdge> {
        public override Display<IGraphScene<IVisual, IVisualEdge>> Create() {
            var result = new GraphSceneDisplay<IVisual, IVisualEdge>();
            return result;           
        }

        public override IComposer<Display<IGraphScene<IVisual, IVisualEdge>>> DisplayComposer {
            get {
                if (_displayComposer == null) {
                    _displayComposer = new VisualsDisplayComposer();
                }
                return base.DisplayComposer;
            }
            set {
                base.DisplayComposer = value;
            }
        }       
    }
}