using Limaki.Common.IOC;
using Limaki.View.Clipping;

namespace Limaki.View.Display {
    public class DisplayRecourceLoader : ContextRecourceLoader {
        public override void ApplyResources(IApplicationContext context) {
            context.Factory.Add<IClipper, PolygonClipper>();
            context.Factory.Add<IClipReceiver, ClipReceiver>();
            context.Factory.Add<IViewport, Viewport>();
        }
    }
}