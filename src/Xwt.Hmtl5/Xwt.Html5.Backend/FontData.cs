using Xwt.Drawing;

namespace Xwt.Html5.Backend {

    public class FontData {

        public string Family { get; set; }
        public double Size { get; set; }
        public FontStyle Style { get; set; }
        public FontWeight Weight { get; set; }
        public FontStretch Stretch { get; set; }

        public void CopyFrom (Font font) {
            Family = font.Family;
            Size = font.Size;
            Style = font.Style;
            Weight = font.Weight;
            Stretch = font.Stretch;
        }

        public void CopyFrom (FontData font) {
            Family = font.Family;
            Size = font.Size;
            Style = font.Style;
            Weight = font.Weight;
            Stretch = font.Stretch;
        }
    }
}