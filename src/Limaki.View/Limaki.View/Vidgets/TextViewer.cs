using Xwt.Backends;

namespace Limaki.View.Vidgets {

    [BackendType(typeof(ITextViewerBackend))]
    public class TextViewer : Vidget {

        ITextViewerBackend _backend = null;
        public virtual new ITextViewerBackend Backend {
            get {
                if (_backend == null) {
                    _backend = BackendHost.Backend as ITextViewerBackend;
                }
                return _backend;
            }
            set { _backend = value; }
        }

        public override void Dispose () {}
    }
}