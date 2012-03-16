namespace Xwt.Drawing {
    public static class XwtDrawingExtensions {
        public static bool Equals (this Font value, Font other) {
            if (value == null || other == null)
                return false;
            return
                value.Family.Equals (other.Family) &&
                value.Size.Equals (other.Size) &&
                value.Stretch.Equals (other.Stretch) &&
                value.Style.Equals (other.Style) &&
                value.Weight.Equals (other.Weight);
        }
    }
}