using System;
using Xwt.Drawing;
using Xwt;
using Limaki.Drawing;
using Limaki.View.Clipping;

namespace Limaki.View.Rendering {
    public class ContextRenderEventArgs : RenderEventArgs {

        private Context _context = null;

        public ContextRenderEventArgs(Context context, Rectangle clipRect) {
            if (context == null)
                throw new ArgumentNullException("context");
            this._context = context;
            this._clipper = new BoundsClipper(clipRect);
            this._surface = new ContextSurface { Context = context };
        }

    }
}