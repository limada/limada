
using Xwt;
using Xwt.Drawing;

namespace Limaki.Drawing {
    public interface IDrawingUtils {
       
        object GetCustomLineCap(double arrowWidth, double arrowHeigth);
       
        Pen CreatePen ( Color color );
        Size GetTextDimension(string text, IStyle style);

        /// <summary>
        /// x ... DpiX, y ... DpiY of Screen
        /// </summary>
        /// <returns></returns>
        Size ScreenResolution();

        Size Resolution(Context context);
    }
}