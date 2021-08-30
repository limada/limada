using Limaki.Common.IOC;
using Limaki.View.Viz.Rendering;

namespace Limaki.View.Viz {

    public class DisplayResourceLoader : ContextResourceLoader {

		protected static bool Applied { get; set; } 

        public override void ApplyResources(IApplicationContext context) {

			if (Applied)
				return;

            context.Factory.Add<IClipper, PolygonClipper>();
            context.Factory.Add<IClipReceiver, ClipReceiver>();
            context.Factory.Add<IViewport, Viewport>();

			Applied = true;
        }
    }
}