using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.View.Display;
using Limaki.View.UI;
using Limaki.View.WPF.UI;

namespace Limaki.View.WPF.Display {
    public class WpfBackendComposer<TData> : BackendComposer<TData, WPFDisplay<TData>> {
        public EventControler EventControler { get; set; }
        public override void Factor(Display<TData> display) {

            Backend.Display = display;
            display.Backend = Backend;

            var deviceRenderer = new WPFRenderer<TData>();
            deviceRenderer.Device = this.Backend;
            deviceRenderer.Display = display;

            this.BackendRenderer = deviceRenderer;

            this.EventControler = new WPFEventControler();
            this.ViewPort = new WpfViewport<TData>(Backend);
            this.CursorHandler = new CursorHandlerBackend(Backend);

            this.SelectionRenderer = new SelectionRenderer();
            this.MoveResizeRenderer = new MoveResizeRenderer();
        }

        public override void Compose(Display<TData> display) {
            this.BackendRenderer.BackColor = () => display.BackColor;
            Backend.DeviceRenderer = this.BackendRenderer;
            Backend.DeviceViewPort = this.ViewPort;

            display.DeviceRenderer = this.BackendRenderer;
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




