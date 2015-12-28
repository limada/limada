using Xwt.Backends;

namespace Limaki.View.Vidgets {

    [BackendType(typeof(ITextViewerWithToolstripVidgetBackend0))]
    public class TextViewerWithToolstrip0 : Vidget {
        TextViewer _textViewer = null;
        public TextViewer TextViewer {
            get {
                if (_textViewer == null) {
                    _textViewer = new TextViewer();
                }
                return _textViewer;
            }
        }

        ITextViewerWithToolstripVidgetBackend0 _backend = null;
        public virtual new ITextViewerWithToolstripVidgetBackend0 Backend {
            get {
                if (_backend == null) {
                    _backend = BackendHost.Backend as ITextViewerWithToolstripVidgetBackend0;
                }
                return _backend;
            }
            set { _backend = value; }
        }

        public override void Dispose () {

        }
    }
}