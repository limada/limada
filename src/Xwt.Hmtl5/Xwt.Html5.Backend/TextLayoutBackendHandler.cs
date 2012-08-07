using Xwt.Engine;
using Xwt.Backends;

namespace Xwt.Html5.Backend {

    public class TextLayoutBackendHandler : ITextLayoutBackendHandler {

        public object Create (Drawing.Context context) {
            var c = (Html5Context) Html5Engine.Registry.GetBackend (context);
            var tl = new TextLayoutBackend { Context = c };
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

        public void SetHeight (object backend, double value) {
            var tl = (TextLayoutBackend) backend;
            tl.Heigth = value;
        }

        public void SetTrimming (object backend, Drawing.TextTrimming value) {
            var tl = (TextLayoutBackend) backend;
            tl.Trimming = value;
        }

        public Size GetSize (object backend) {
            var tl = (TextLayoutBackend) backend;
            return tl.Size;
        }

        public object Create (ICanvasBackend canvas) {
            throw new System.NotImplementedException ();
        }


       
    }
}