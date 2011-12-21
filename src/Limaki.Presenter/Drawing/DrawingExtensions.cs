namespace Limaki.Drawing {
    public static class DrawingExtensions {
        public static SizeI Max(this SizeI a, SizeI b) {
            return new SizeI(a.Width > b.Width ? a.Width : b.Width, a.Height > b.Height ? a.Height : b.Height);
        }
        public static SizeS Max(this SizeS a, SizeS b) {
            return new SizeS(a.Width > b.Width ? a.Width : b.Width, a.Height > b.Height ? a.Height : b.Height);
        }
        public static PointI Max(this PointI a, PointI b) {
            return new PointI(a.X > b.X ? a.X : b.X, a.Y > b.Y ? a.Y : b.Y);
        }
        public static PointS Max(this PointS a, PointS b) {
            return new PointS(a.X > b.X ? a.X : b.X, a.Y > b.Y ? a.Y : b.Y);
        }
    }
}