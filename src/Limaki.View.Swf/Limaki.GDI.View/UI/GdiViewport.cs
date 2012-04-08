using Limaki.Drawing;
using Limaki.Drawing.Gdi;
using Limaki.View.Display;

namespace Limaki.View.Gdi.UI {

    public class GdiViewport : Viewport {
        public override Matrice CreateMatrix() {
            return new GdiMatrice();
        }
    }
}