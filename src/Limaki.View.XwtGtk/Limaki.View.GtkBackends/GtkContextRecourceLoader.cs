using Limaki.Common.IOC;
using Limaki.View.UI;
using Limaki.View.XwtContext;
using Xwt;
using Xwt.Backends;

namespace Limaki.View.GtkBackends {

    public class GtkContextRecourceLoader : ContextRecourceLoader, IToolkitAware {
        public override void ApplyResources (IApplicationContext context) {
            var tk = Toolkit.CurrentEngine;
            //tk.RegisterBackend<SystemColorsBackend, XwtSystemColorsBackend>();

            context.Factory.Add<IUISystemInformation, GtkSystemInformation>();
        }

        public Xwt.ToolkitType ToolkitType {
            get { return Xwt.ToolkitType.Gtk; }
        }
    }
}