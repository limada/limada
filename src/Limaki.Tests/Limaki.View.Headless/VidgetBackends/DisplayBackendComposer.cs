using Limaki.View.Rendering;
using Limaki.View.UI;
using Limaki.View.Visualizers;
using Limaki.Drawing;

namespace Limaki.View.Headless.VidgetBackends {

    public class DisplayBackendComposer<TData> : BackendComposer<TData, DisplayBackend<TData>> {

        public EventControler EventControler { get; set; }
        public override void Factor (Display<TData> display) {

            display.Backend = Backend;

            var surfaceRenderer = new HeadlessBackendRenderer<TData> {
                                                                    Backend = this.Backend,
                                                                    Display = display
                                                                };

            this.BackendRenderer = surfaceRenderer;

            this.EventControler = new EventControler ();
            this.ViewPort = new HeadlessViewport (Backend);
            this.CursorHandler = new HeadlessCursorHandlerBackend (Backend);

            this.SelectionRenderer = new SelectionRenderer ();
            this.MoveResizeRenderer = new MoveResizeRenderer ();

        }

        public override void Compose (Display<TData> display) {
            this.BackendRenderer.BackColor = () => display.BackColor;
            Backend.BackendRenderer = this.BackendRenderer;
            Backend.BackendViewPort = this.ViewPort;

            display.BackendRenderer = this.BackendRenderer;
            display.DataLayer = this.DataLayer;
            display.EventControler = this.EventControler;
            display.Viewport = this.ViewPort;
            display.CursorHandler = this.CursorHandler;

            this.MoveResizeRenderer.Backend = this.Backend;
            ComposeSelectionRenderer (MoveResizeRenderer);
            display.MoveResizeRenderer = this.MoveResizeRenderer;


            this.SelectionRenderer.Backend = this.Backend;
            ComposeSelectionRenderer (SelectionRenderer);
            display.SelectionRenderer = this.SelectionRenderer;


        }

        public void ComposeSelectionRenderer (ISelectionRenderer renderer) {
            renderer.SaveMatrix = s => {
                                      ((ContextSurface)s).Context.Save ();
                                      return null;
                                  };
            renderer.RestoreMatrix = (s, o) => ((ContextSurface)s).Context.Restore ();
            renderer.SetMatrix = s => {
                                     ((ContextSurface)s).Context.Scale (1, 1);
                                 };
        }
    }
}