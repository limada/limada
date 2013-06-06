using System;

namespace Limaki.View {
    
    public class VidgetBackendHost<T, B> : VidgetBackendHost
        where T : IVidget
        where B : IVidgetBackend {
        public new T Frontend {
            get { return (T)base.Frontend; }
            set { base.Frontend = value; }
        }

        public new B Backend {
            get { return (B)base.Backend; }
        }
    }

    public class VidgetBackendHost {

        public IVidget Frontend { get; internal set; }

        IVidgetBackend _backend;
        public IVidgetBackend Backend {
            get {
                LoadBackend();
                return _backend;
            }
        }

        bool usingCustomBackend;
        public void SetCustomBackend (IVidgetBackend backend) {
            this._backend = backend;
            usingCustomBackend = true;
        }

        VidgetToolkit _engine;
        public VidgetToolkit ToolkitEngine {
            get { return _engine ?? (_engine = VidgetToolkit.CurrentEngine); }
            protected set { _engine = value; }
        }

        public VidgetToolkitEngineBackend EngineBackend {
            get { return ToolkitEngine.Backend; }
        }

        public bool BackendCreated {
            get { return _backend != null; }
        }

        protected virtual void OnBackendCreated () { }

        protected virtual IVidgetBackend OnCreateBackend () {
            return EngineBackend.CreateBackendForFrontend(Frontend.GetType());
        }

        public void EnsureBackendLoaded () {
            if (_backend == null)
                LoadBackend();
        }

        protected virtual void LoadBackend () {
            if (usingCustomBackend) {
                usingCustomBackend = false;
                _backend.InitializeBackend(Frontend, _engine.Context);
                OnBackendCreated();
            } else if (_backend == null) {
                _backend = OnCreateBackend();
                if (_backend == null)
                    throw new InvalidOperationException("No backend found for object: " + Frontend.GetType());
                _backend.InitializeBackend(Frontend, _engine.Context);
                OnBackendCreated();
            }
        }

    }
}