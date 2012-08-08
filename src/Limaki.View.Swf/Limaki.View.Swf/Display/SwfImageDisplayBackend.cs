using System.Drawing;
using Limaki.View.Display;
using Limaki.View.Swf.Display;

namespace Limaki.View.Swf {
    public class SwfImageDisplayBackend:SwfWidgetBackend<Image> {
       
        public override DisplayFactory<Image> CreateDisplayFactory(SwfWidgetBackend<Image> device) {
            var result = new DisplayFactory<Image>();
            
            var deviceInstrumenter = new ImageBackendComposer();
            deviceInstrumenter.Backend = device;
            result.DeviceComposer = deviceInstrumenter;
            
            result.DisplayComposer = new ImageDisplayComposer ();
            //device.AutoScroll = false;
            //device.ScrollBarsVisible = false;

            return result;
        }
    }
}