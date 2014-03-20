using Xwt;
using Xwt.Backends;

namespace Limaki.View.Vidgets {
    [BackendType(typeof(IVidgetBackend))]
    public abstract class Vidget : IVidget {

        private VidgetBackendHost _backendHost;
        public Vidget () {
            _backendHost = CreateBackendHost ();
            _backendHost.Frontend = this;
        }

        protected virtual VidgetBackendHost CreateBackendHost () {
            return new VidgetBackendHost ();
        }

        protected VidgetBackendHost BackendHost {
            get { return _backendHost; }
        }

        public abstract void Dispose();

        public virtual IVidgetBackend Backend {
            get { return BackendHost.Backend; }
        }

        public virtual Size Size { get { return Backend.Size; } }

        public virtual void SetFocus() { Backend.SetFocus (); }
    }
}