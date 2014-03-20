using Limaki.Common.IOC;
using Limaki.View.Viz.Rendering;

namespace Limaki.View.Viz {

    public class DisplayResourceLoader : ContextResourceLoader {

        public override void ApplyResources(IApplicationContext context) {

            context.Factory.Add<IClipper, PolygonClipper>();
            context.Factory.Add<IClipReceiver, ClipReceiver>();
            context.Factory.Add<IViewport, Viewport>();
        }
    }
}