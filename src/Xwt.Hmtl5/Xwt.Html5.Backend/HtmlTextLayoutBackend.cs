using Xwt.Drawing;
using System.Linq;
using System;
using Xwt.Backends;

namespace Xwt.Html5.Backend {

    public class HtmlTextLayoutBackend {

        private double _width;
        private string _text;
        private Xwt.Drawing.Font _font;

        public double Width {
            get { return _width; }
            set {
                if (_width != value)
                    _size = null;
                _width = value;
            }
        }

        public string Text {
            get { return _text; }
            set {
                if (_text != value)
                    _size = null;
                _text = value;
            }
        }

        public Font Font {
            get {
                return _font ?? Font.FromName ("Default 10");
            }
            set {

                _font = value;
            }
        }

        Size? _size = null;
        public Size Size {
            get {
                if (_size == null) {
                    var font = (FontData)this.Font.GetBackend();
                    var size = new Size (this.Width, 0);
                    _size = MeasureString (this.Text, font, size);
                }
                return _size.Value;
            }
        }

        static char[] smallChars = new char[] { 't', 'i', '1', 'l', 'r', 'I', '/', '\\', '.', ':', ';', ',' };
        public Size MeasureString0 (string text, FontData font, Size size) {
            var w = size.Width;
            if (w == 0) {
                foreach (var c in text)
                    if (smallChars.Contains(c))
                        w += (font.Size/2);
                    else
                        w += font.Size*.7;
            }
            var h = size.Height;
            return new Size(w, h);
        }

        private static FontMeasure fontMeasure = new FontMeasureArial();
        public Size MeasureString (string text, FontData font, Size size) {
            var w = size.Width;
            var fact = font.Size / 10 *.7;
            var h = size.Height;
            if (w == 0) {
                foreach (var c in text) {
                    var s = fontMeasure.Measure(c); 
                    w += s.Width * fact;
                    //h = Math.Max(h, s.Height * fact);
                }
            }
           
            return new Size(w, h);
        }

        public double Heigth { get; set; }

        public TextTrimming Trimming { get; set; }
    }
}