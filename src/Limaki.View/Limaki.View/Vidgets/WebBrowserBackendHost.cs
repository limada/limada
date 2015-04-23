using Limaki.Common;

namespace Limaki.View.Vidgets {

    public class WebBrowserBackendHost : VidgetBackendHost {
        protected override IVidgetBackend OnCreateBackend () {
            this.ToolkitEngine.Backend.CheckInitialized ();
            // WebBrowserBackend needs special support of factory to decide which backend is available 
            return Registry.Factory.Create<IWebBrowserBackend> ();
        }
    }
}