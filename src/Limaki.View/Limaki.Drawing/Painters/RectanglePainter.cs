using Xwt;
using Limaki.Drawing.Shapes;

namespace Limaki.Drawing.Painters {

    public class RectanglePainter : Painter<Rectangle>, IPainter<IRectangleShape, Rectangle> {

        public override void Render (ISurface surface) {
            var ctx = ((ContextSurface) surface).Context;
            Render (ctx, (c, d) => c.Rectangle (d));
        }
    }
}