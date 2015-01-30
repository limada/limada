using Limaki.Drawing.XwtBackend;
using Limaki.Drawing;
using Limaki.View.Viz.Rendering;
using Limaki.View.Viz.UI;
using Limaki.View.Viz.Visualizers;

namespace Limaki.View.Headless.VidgetBackends {

    public class DisplayBackendComposer<TData> : BackendComposer<TData, DisplayBackend<TData>> {

        public ActionDispatcher ActionDispatcher { get; set; }
        public override void Factor (Display<TData> display) {

            display.Backend = Backend;

            var surfaceRenderer = new HeadlessBackendRenderer<TData> {
                                                                    Backend = this.Backend,
                                                                    Display = display
                                                                };

            this.BackendRenderer = surfaceRenderer;

            this.ActionDispatcher = new ActionDispatcher ();
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
            display.ActionDispatcher = this.ActionDispatcher;
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