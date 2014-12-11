namespace Xwt.Drawing {

    public sealed class FontDataAttribute : TextAttribute {

        public string FontFamily { get; set; }
        public double FontSize { get; set; }

        public override bool Equals (object t) {
            var ot = t as FontDataAttribute;
            return ot != null &&
                   object.Equals (FontFamily, ot.FontFamily) &&
                   FontSize.Equals (ot.FontSize) &&
                   base.Equals (t);
        }

        public override int GetHashCode () {
            return base.GetHashCode () ^ FontFamily.GetHashCode () ^ FontSize.GetHashCode ();
        }
    }
}