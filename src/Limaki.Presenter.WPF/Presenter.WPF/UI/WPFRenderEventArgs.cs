using System.Windows.Controls;
using Limaki.Drawing;
using Limaki.Drawing.WPF;
using Limaki.Presenter.UI;

namespace Limaki.Presenter.WPF.UI {
    public class WPFRenderEventArgs : RenderEventArgs {

        public WPFRenderEventArgs(WPFSurface surface) {
            this._surface = surface;
        }


        public WPFRenderEventArgs(WPFSurface surface, RectangleI clipRect): this(surface) {
            this._clipper = new BoundsClipper(clipRect);
        }

        public WPFRenderEventArgs(WPFSurface surface, IClipper clipper): this(surface) {
            this._clipper = clipper;
        }
    }
}