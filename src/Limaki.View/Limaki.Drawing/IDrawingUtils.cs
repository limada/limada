
using Xwt;
using Xwt.Drawing;

namespace Limaki.Drawing {

    public interface IDrawingUtils {
       
        object GetCustomLineCap(double arrowWidth, double arrowHeigth);
        Size GetTextDimension(string text, IStyle style);

        /// <summary>
        /// x ... DpiX, y ... DpiY of Screen
        /// Remark: points = pixels * 72 / DpiX;
        /// </summary>
        /// <returns></returns>
        Size ScreenResolution();

        
        Size Resolution(Context context);
    }
}