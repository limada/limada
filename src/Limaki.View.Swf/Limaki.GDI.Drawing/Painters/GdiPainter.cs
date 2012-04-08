using System.Drawing;
using Limaki.Drawing.Painters;
using System;

namespace Limaki.Drawing.Gdi.Painters {

    public abstract class GdiPainter<T>:Painter<T> {

        public override void Render (ISurface surface) {
            RenderXwt (surface);
        }

        public abstract void RenderXwt (ISurface surface);
        public abstract void RenderGdi (ISurface surface);

        protected SolidBrush _brush = null;
        protected SolidBrush GetSolidBrush (System.Drawing.Color color) {
            if ((_brush != null) && (_brush.Color == color)) {
                return _brush;
            }
            if (_brush != null) {
                _brush.Dispose ();
                _brush = null;
            }
            _brush = new SolidBrush (color);
            return _brush;
        }

        public override void Dispose (bool disposing) {
            base.Dispose (disposing);
            if (disposing) {
                if (_brush != null) {
                    _brush.Dispose ();
                    _brush = null;
                }
            }
        }
    }
}