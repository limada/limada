using Xwt.Backends;

namespace Xwt.Html5.Backend {

    public class HmtlTextLayoutBackendHandler : TextLayoutBackendHandler {

        public override object Create (Drawing.Context context) {
            return Create();
        }

        public override object Create () {
            var tl = new HtmlTextLayoutBackend();
            return tl;
        }

        public override void SetWidth (object backend, double value) {
            var tl = (HtmlTextLayoutBackend) backend;
            tl.Width = value;
        }

        public override void SetText (object backend, string text) {
            var tl = (HtmlTextLayoutBackend) backend;
            tl.Text = text;
        }

        public override void SetFont (object backend, Xwt.Drawing.Font font) {
            var tl = (HtmlTextLayoutBackend) backend;
            tl.Font = font;
        }

        public override void SetHeight (object backend, double value) {
            var tl = (HtmlTextLayoutBackend) backend;
            tl.Heigth = value;
        }

        public override void SetTrimming (object backend, Drawing.TextTrimming value) {
            var tl = (HtmlTextLayoutBackend) backend;
            tl.Trimming = value;
        }

        public override Size GetSize (object backend) {
            var tl = (HtmlTextLayoutBackend) backend;
            return tl.Size;
        }

        

        public override int GetIndexFromCoordinates (object backend, double x, double y) {
            throw new System.NotImplementedException();
        }

        public override Point GetCoordinateFromIndex (object backend, int index) {
            throw new System.NotImplementedException();
        }

        public override void AddAttribute (object backend, Drawing.TextAttribute attribute) {
            throw new System.NotImplementedException();
        }

        public override void ClearAttributes (object backend) {
            throw new System.NotImplementedException();
        }
    }
}