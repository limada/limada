using Limaki.View;
using Xwt.Backends;

namespace Limaki.Viewers.Vidgets {

    [BackendType(typeof(ITextViewerBackend))]
    public class TextViewer : Vidget {
        ITextViewerBackend _backend = null;
        public virtual ITextViewerBackend Backend {
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