using Limaki.Drawing;

namespace Limaki.Drawing {
    public interface IDrawingUtils {
        uint GetSysColor(SysColorIndex index);
        object GetCustomLineCap(float arrowWidth, float arrowHeigth);
        Font CreateFont ( string familiy, double size );
        Pen CreatePen ( Color color );
        Matrice NativeMatrice();
        SizeS GetTextDimension(string text, IStyle style);
    }
}