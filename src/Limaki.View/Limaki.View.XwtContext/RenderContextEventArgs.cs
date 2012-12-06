using System;
using Xwt.Drawing;
using Xwt;
using Limaki.Drawing;
using Limaki.View.Clipping;
using Limaki.View.Rendering;

namespace Limaki.View.XwtContext {
    public class RenderContextEventArgs : RenderEventArgs {

        private Context _context = null;

        public RenderContextEventArgs(Context context, Rectangle clipRect) {
            if (context == null)
                throw new ArgumentNullException("context");
            this._context = context;
            this._clipper = new BoundsClipper(clipRect);
            this._surface = new ContextSurface { Context = context };
        }

    }
}