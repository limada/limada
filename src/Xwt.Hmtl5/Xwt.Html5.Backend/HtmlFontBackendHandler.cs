using Xwt.Drawing;
using Xwt.Backends;
using System.Collections.Generic;

namespace Xwt.Html5.Backend {

    public class HtmlFontBackendHandler : FontBackendHandler {

        public const string DefaultFamily = "serif";
        public override object GetSystemDefaultFont () {
            return new FontData { Family = DefaultFamily, Size = 10 };
        }

        public override IEnumerable<string> GetInstalledFonts () {
            yield return "serif";
            yield return "sans-serif";
            yield return "monospace";
            yield return "cursive";
            yield return "fantasy";
        }

        public override object Create (string fontName, double size, FontStyle style, FontWeight weight, FontStretch stretch) {
            return new FontData { Family = fontName, Size = size, Style = style, Weight = weight, Stretch = stretch };
        }

        public override object Copy (object handle) {
            return ((FontData) handle).Clone ();
        }

        public override object SetSize (object handle, double size) {
            var d = ((FontData) handle).Clone ();
            d.Size = size;
            return d;
        }

        public override object SetFamily (object handle, string family) {
            var d = ((FontData) handle).Clone ();
            d.Family = family;
            return d;
        }

        public override object SetStyle (object handle, FontStyle style) {
            var d = ((FontData) handle).Clone ();
            d.Style = style;
            return d;

        }

        public override object SetWeight (object handle, FontWeight weight) {
            var d = ((FontData) handle).Clone ();
            d.Weight = weight;
            return d;
        }

        public override object SetStretch (object handle, FontStretch stretch) {
            var d = ((FontData) handle).Clone ();
            d.Stretch = stretch;
            return d;
        }

        public override double GetSize (object handle) {
            var d = (FontData) handle;
            return d.Size;
        }

        public override string GetFamily (object handle) {
            var d = (FontData) handle;
            return d.Family;
        }

        public override FontStyle GetStyle (object handle) {
            var d = (FontData) handle;
            return d.Style;
        }

        public override FontWeight GetWeight (object handle) {
            var d = (FontData) handle;
            return d.Weight;
        }

        public override FontStretch GetStretch (object handle) {
            var d = (FontData) handle;
            return d.Stretch;
        }

    }
}