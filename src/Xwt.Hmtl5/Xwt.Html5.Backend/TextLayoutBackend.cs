using Xwt.Drawing;
using Xwt.Engine;

namespace Xwt.Html5.Backend {
    public class TextLayoutBackend {

        public Html5Context Context { get; set; }
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
            get { return _font ?? Font.FromName ("Default", 10); }
            set {

                _font = value;
            }
        }

        Size? _size = null;
        public Size Size {
            get {
                if (_size == null) {
                    var font = (FontData) WidgetRegistry.GetBackend (this.Font);
                    var size = new Size (this.Width, 0);
                    _size = MeasureString (this.Text, font, size);
                }
                return _size.Value;
            }
        }

        public Size MeasureString (string text, FontData font, Size size) {
            return size;
        }



        public double Heigth { get; set; }

        public TextTrimming Trimming { get; set; }
    }
}