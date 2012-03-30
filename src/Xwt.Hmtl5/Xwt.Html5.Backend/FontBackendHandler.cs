using Xwt.Drawing;
using Xwt.Backends;

namespace Xwt.Html5.Backend {

    public class FontBackendHandler : IFontBackendHandler {

        public object CreateFromName (string fontName, double size) {
            return new FontData{Family=fontName,Size=size};
        }

        public object Copy (object handle) {
            var d = (FontData) handle;
            var f = new FontData();
            f.CopyFrom(d);
            return f;
        }
        
        public object SetSize (object handle, double size) {
            var d = (FontData) handle;
            d.Size = size;
            return d;
        }

        public object SetFamily (object handle, string family) {
            var d = (FontData) handle;
            d.Family = family;
            return d;
        }
    
        public object SetStyle (object handle, FontStyle style) {
            var d = (FontData) handle;
            d.Style = style;
            return d;

        }

        public object SetWeight (object handle, FontWeight weight) {
            var d = (FontData) handle;
            d.Weight = weight;
            return d;
        }

        public object SetStretch (object handle, FontStretch stretch) {
            var d = (FontData) handle;
            d.Stretch = stretch;
            return d;
        }

        public double GetSize (object handle) {
            var d = (FontData) handle;
            return d.Size;
        }

        public string GetFamily (object handle) {
            var d = (FontData) handle;
            return d.Family;
        }

        public FontStyle GetStyle (object handle) {
            var d = (FontData) handle;
            return d.Style;
        }

        public FontWeight GetWeight (object handle) {
            var d = (FontData) handle;
            return d.Weight;
        }

        public FontStretch GetStretch (object handle) {
            var d = (FontData) handle;
            return d.Stretch;
        }

    }
}