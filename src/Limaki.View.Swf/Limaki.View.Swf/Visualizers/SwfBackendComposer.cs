using Limaki.View.Visualizers;
using Limaki.View.Swf.UI;
using Limaki.View.UI;

namespace Limaki.View.Swf.Visualizers {

    public class SwfBackendComposer<TData> : BackendComposer<TData, DisplayBackend<TData>> {
        
        public EventControler EventControler { get; set; }
        public override void Factor(Display<TData> display) {
            
            display.Backend = Backend;

            var surfaceRenderer = new SwfBackendRenderer<TData> {
                Backend = this.Backend,
                Display = display
            };
			
            this.BackendRenderer = surfaceRenderer;

            this.EventControler = new SwfEventControler ();
            this.ViewPort = new SwfViewport (Backend);
            this.CursorHandler = new CursorHandlerBackend (Backend);

            this.SelectionRenderer = new  SelectionRenderer();
            this.MoveResizeRenderer = new MoveResizeRenderer ();
        }

        public override void Compose(Display<TData> display) {
            this.BackendRenderer.BackColor = () => display.BackColor;
            Backend.BackendRenderer = this.BackendRenderer;
            Backend.BackendViewPort = this.ViewPort;

            display.BackendRenderer = this.BackendRenderer;
            display.DataLayer = this.DataLayer;
            display.EventControler = this.EventControler;
            display.Viewport = this.ViewPort;
            display.CursorHandler = this.CursorHandler;

            this.MoveResizeRenderer.Backend = this.Backend;
            display.MoveResizeRenderer = this.MoveResizeRenderer;

            this.SelectionRenderer.Backend = this.Backend;
            display.SelectionRenderer = this.SelectionRenderer;
        }
    }

}