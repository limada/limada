using Xwt.Backends;
using Xwt.Engine;

namespace Xwt.Gdi.Backend {

    public class TextLayoutBackendHandler : ITextLayoutBackendHandler {
        public object Create (Drawing.Context context) {
            var c = (GdiContext) WidgetRegistry.GetBackend (context);
            var tl = new TextLayoutBackend {Context = c};
            return tl;
        }

        public void SetWidth (object backend, double value) {
            var tl = (TextLayoutBackend) backend;
            tl.Width = value;
        }

        public void SetText (object backend, string text) {
            var tl = (TextLayoutBackend) backend;
            tl.Text = text;
        }

        public void SetFont (object backend, Xwt.Drawing.Font font) {
            var tl = (TextLayoutBackend) backend;
            tl.Font = font;
        }

        public Size GetSize (object backend) {
            var tl = (TextLayoutBackend) backend;
            return tl.Size;
        }
    }
}
