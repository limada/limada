using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Drawing;
using Xwt;
using Xwt.Drawing;

namespace Limaki.View.Html5 {

    public class Html5DrawingUtils : IDrawingUtils {

        public object GetCustomLineCap (double arrowWidth, double arrowHeigth) {
            return new object ();
        }

        public Pen CreatePen (Color color) {
            return new Pen (color);
        }

        public Matrice NativeMatrice () {
            return new Matrice ();
        }

        public Size GetTextDimension (string text, IStyle style) {
            return new Size (text.Length * 10, 10);
        }

        public Size ScreenResolution () {
            return new Size (96, 96);
        }

        public Size Resolution (Context context) {
            return new Size (96, 96);
        }



    }
}
