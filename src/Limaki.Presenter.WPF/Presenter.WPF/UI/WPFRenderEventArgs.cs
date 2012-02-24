using System.Windows.Controls;
using Limaki.Drawing;
using Limaki.Drawing.WPF;
using Limaki.Presenter.Clipping;
using Limaki.Presenter.Rendering;
using Limaki.Presenter.UI;
using Xwt;

namespace Limaki.Presenter.WPF.UI {
    public class WPFRenderEventArgs : RenderEventArgs {

        public WPFRenderEventArgs(WPFSurface surface) {
            this._surface = surface;
        }


        public WPFRenderEventArgs(WPFSurface surface, RectangleD clipRect): this(surface) {
            this._clipper = new BoundsClipper(clipRect);
        }

        public WPFRenderEventArgs(WPFSurface surface, IClipper clipper): this(surface) {
            this._clipper = clipper;
        }
    }
}