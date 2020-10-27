using Xwt.Drawing;

namespace Xwt.Backends {
    public abstract partial class TextLayoutBackendHandler : DisposableResourceBackendHandler {
        public abstract void SetWrapMode(object backend, WrapMode value);
        
        public abstract object Create (Context context);

    }
}