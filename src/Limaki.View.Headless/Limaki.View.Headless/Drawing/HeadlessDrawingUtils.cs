using Xwt;
using Xwt.Drawing;

namespace Limaki.Drawing {

    public class HeadlessDrawingUtils:IDrawingUtils {

        public Size GetTextDimension (string text, IStyle style) {
            return new Size(text.Length * 10,10);
        }

        public Size GetObjectDimension (object value, IStyle style) {
            var result = new Size ();
            if (!DrawingExtensions.TryGetObjectDimension (value, style, out result)) 
                return Size.Zero;
            return result;
        }

        public Size ScreenResolution () {
            return new Size (96, 96);
        }

        public Size Resolution (Context context) {
            return new Size (96, 96);
        }

      
    }
}