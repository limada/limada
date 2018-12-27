using System;
using Xwt.Backends;
using Xwt.Drawing;

namespace Xwt.Headless.Backend {
    public class HeadlessTextLayoutBackendHandler : TextLayoutBackendHandler {

        public override object Create (Context context) {
            return Create ();
        }

        public override object Create () {
            var tl = new HeadlessContext ();
            return tl;
        }

        public override void SetWidth (object backend, double value) {
            Width = value;
        }

        double Width { get; set; }
        double Height { get; set; }
        string Text { get; set; }
        Font Font { get; set; }
        TextTrimming TextTrimming { get; set; }
        WrapMode WrapMode { get; set; }

        public override void SetText (object backend, string text) {
            Text = text;
        }

        public override void SetFont (object backend, Font font) {
            Font = font;
        }

        public override void SetHeight (object backend, double value) {
            Height = value;
        }

        public override void SetTrimming (object backend, TextTrimming value) {
            TextTrimming = value;
        }

        public override void SetWrapMode (object backend, WrapMode value) {
            WrapMode = value;
        }

        public override Size GetSize (object backend) {
            return new Size (Text.Length * 10, 10);
        }

        public override int GetIndexFromCoordinates (object backend, double x, double y) {
            throw new System.NotImplementedException ();
        }

        public override Point GetCoordinateFromIndex (object backend, int index) {
            throw new System.NotImplementedException ();
        }

        public override void AddAttribute (object backend, Drawing.TextAttribute attribute) {
            throw new System.NotImplementedException ();
        }

        public override void ClearAttributes (object backend) {
            throw new System.NotImplementedException ();
        }

        public override double GetBaseline(object backend)
        {
            throw new NotImplementedException();
        }

        public override double GetMeanline(object backend)
        {
            throw new NotImplementedException();
        }
    }
}