using Xwt.Drawing;

namespace Xwt.Gdi.Backend {

    public class TextLayoutBackend {
        public GdiContext Context { get; set; }
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
            get { return _font; }
            set {
                if (!Xwt.Drawing.XwtDrawingExtensions.Equals (_font, value))
                    _size = null;
                _font = value;
            }
        }

        Size? _size = null;
        public Size Size {
            get {
                if (_size == null) {
                    var font = Font.ToGdi ();
                    var size = new System.Drawing.SizeF ((float) Width, 0);
                    _size = Context.Graphics.MeasureString (Text, font, size,Format).ToXwt ();
                }
                return _size.Value;
            }
        }

        System.Drawing.StringFormat _format = null;
        public System.Drawing.StringFormat Format {
            get {
                return _format ?? (_format = GdiConverter.GetDefaultStringFormat ());
            }
        }
    }
}