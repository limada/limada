using Limaki.Common;
using Limaki.Common.IOC;

namespace Limaki.Presenter.Display {
    public class PresenterRecourceLoader : ContextRecourceLoader {
        public override void ApplyResources(IApplicationContext context) {
            context.Factory.Add<IClipper, PolygonClipper>();
            context.Factory.Add<IClipReceiver, ClipReceiver>();
            context.Factory.Add<IViewport, ViewPort>();
        }
    }
}