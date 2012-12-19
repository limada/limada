using Limaki.Drawing;
using Limaki.Drawing.Gdi;
using Limaki.View.Visualizers;
using Xwt.Drawing;

namespace Limaki.View.Gdi.UI {

    public class GdiViewport : Viewport {
        public override Matrix CreateMatrix() {
            return new GdiMatrice();
        }
    }
}