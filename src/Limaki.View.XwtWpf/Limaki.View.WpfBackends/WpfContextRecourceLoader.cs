using Limaki.Common.IOC;
using Limaki.Drawing;
using Limaki.Drawing.WpfBackend;
using Limaki.View.UI;
using Limaki.View.WpfBackends;
using Limaki.View.XwtContext;
using Limaki.Viewers;
using Xwt;
using Xwt.Backends;

namespace Limaki.View.WpfBackend {

    public class WpfContextRecourceLoader : ContextRecourceLoader, IToolkitAware {

        public Xwt.ToolkitType ToolkitType {
            get { return Xwt.ToolkitType.Wpf; }
        }

        public override void ApplyResources(IApplicationContext context) {
            var tk = Toolkit.CurrentEngine;
            tk.RegisterBackend<SystemColorsBackend, WpfSystemColorsBackend>();

            context.Factory.Add<ISystemFonts, WpfSystemFonts>();
            context.Factory.Add<IUISystemInformation, WpfSystemInformation>();

            // register special IVidgetBackends here, eg. webbrowser
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IWebBrowserBackend, WebBrowserBackend>();

        }

        
    }
}