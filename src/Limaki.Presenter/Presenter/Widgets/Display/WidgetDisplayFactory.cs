using Limaki.Drawing;
using Limaki.Presenter.Display;
using Limaki.Widgets;

namespace Limaki.Presenter.Widgets {
    public class WidgetDisplayFactory:GraphSceneDisplayFactory<IWidget,IEdgeWidget> {
        public override Display<IGraphScene<IWidget, IEdgeWidget>> Create() {
            var result = new WidgetDisplay();
            return result;           
        }

        public override IComposer<Display<IGraphScene<IWidget, IEdgeWidget>>> DisplayComposer {
            get {
                if (_displayComposer == null) {
                    _displayComposer = new WidgetDisplayComposer();
                }
                return base.DisplayComposer;
            }
            set {
                base.DisplayComposer = value;
            }
        }       
    }
}