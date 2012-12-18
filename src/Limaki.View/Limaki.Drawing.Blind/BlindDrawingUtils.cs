using Xwt;
using Xwt.Drawing;

namespace Limaki.Drawing {

    public class BlindDrawingUtils:IDrawingUtils {
        public object GetCustomLineCap (double arrowWidth, double arrowHeigth) {
            return new object();
        }

        public Pen CreatePen (Color color) {
            return new Pen (color);
        }

        public Size GetTextDimension (string text, IStyle style) {
            return new Size(text.Length * 10,10);
        }

        public Size ScreenResolution () {
            return new Size (96, 96);
        }

        public Size Resolution (Context context) {
            return new Size (96, 96);
        }

      
    }
}