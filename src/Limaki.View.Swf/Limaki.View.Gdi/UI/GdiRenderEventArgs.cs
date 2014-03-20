using System;
using System.Drawing;
using Limaki.View;
using Limaki.Drawing.Gdi;
using Limaki.View.Viz.Rendering;
using Xwt.Gdi;
using Xwt.Gdi.Backend;

namespace Limaki.View.Gdi.UI {

    public class GdiRenderEventArgs : RenderEventArgs {

        private Graphics graphics = null;

        public GdiRenderEventArgs (Graphics graphics, Rectangle clipRect) {
            if (graphics == null)
                throw new ArgumentNullException ("graphics");
            this.graphics = graphics;
            this._clipper = new BoundsClipper (clipRect.ToXwt ());
            this._surface = new GdiSurface { Graphics = graphics };
        }

    }
}