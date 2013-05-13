using System.Drawing;
using Limaki.View.Visualizers;
using Limaki.View.Swf.Visualizers;

namespace Limaki.View.Swf.Visualizers {

    public class SwfImageDisplayBackend:SwfWidgetBackend<Image> {
       
        public override DisplayFactory<Image> CreateDisplayFactory(SwfWidgetBackend<Image> device) {
            var result = new DisplayFactory<Image>();
            
            var deviceInstrumenter = new ImageDisplayBackendComposer();
            deviceInstrumenter.Backend = device;
            result.DeviceComposer = deviceInstrumenter;
            
            result.DisplayComposer = new ImageDisplayComposer ();
            //device.AutoScroll = false;
            //device.ScrollBarsVisible = false;

            return result;
        }
    }
}