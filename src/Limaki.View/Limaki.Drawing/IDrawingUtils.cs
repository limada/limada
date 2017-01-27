
using Xwt;
using Xwt.Drawing;

namespace Limaki.Drawing {

    public interface IDrawingUtils {
       
        Size GetTextDimension(string text, IStyle style);

        Size GetObjectDimension (object value, IStyle style);

        /// <summary>
        /// x ... DpiX, y ... DpiY of Screen
        /// Remark: points = pixels * 72 / DpiX;
        /// </summary>
        /// <returns></returns>
        Size ScreenResolution();

        Size Resolution(Context context);
    }
}