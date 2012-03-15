using System;
using Xwt;
using SystemColors = System.Windows.SystemColors;
using Xwt.Drawing;

namespace Limaki.Drawing.WPF {

    public class WPFDrawingUtils : IDrawingUtils {

        public virtual Size GetTextDimension(string text, IStyle style) {
            return WPFUtils.GetTextDimension(text,style);
        }

        uint ToArgb(System.Windows.Media.Color color) {
            return (uint) ( color.A << 24 | color.R << 16 | color.G << 8 | color.B );
        }

       

        public virtual Font CreateFont(string familiy, double size) {
            var result = Font.FromName(familiy,size);
            return result;
        }

        public Pen CreatePen(Color color) {
            return new Pen(color);
        }

        public Matrice NativeMatrice() {
            return new Limaki.Drawing.Matrice();
        }

        public object GetCustomLineCap(double arrowWidth, double arrowHeigth) {

            if (arrowHeigth == 0 || arrowWidth == 0)
                throw new ArgumentException("ArrowWidth must not be 0");

            var path = new System.Windows.Media.PolyLineSegment();

            var p1 = new System.Windows.Point(0, 1);
            var p2 = new System.Windows.Point(-arrowHeigth, -arrowWidth);
            var p3 = new System.Windows.Point(arrowHeigth, -arrowWidth);
            
            path.Points = new System.Windows.Media.PointCollection ();
            path.Points.Add (p1);
            path.Points.Add(p2);
            path.Points.Add(p3);
#if ! SILVERLIGHT
            path.IsSmoothJoin = true;
#endif
            return path;

        }
    }
}