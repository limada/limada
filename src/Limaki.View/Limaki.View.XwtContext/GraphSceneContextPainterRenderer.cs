using System;

using Limaki.Common;
using Limaki.View.Rendering;
using Limaki.Drawing;
using Xwt.Drawing;

namespace Limaki.View.XwtContext {

    public class GraphSceneContextPainterRenderer<T> : IBackendRenderer {

        public ILayer<T> Layer { get; set; }

        public Func<Color> BackColor { get; set; }

        public void Render() {
            throw new NotImplementedException();
        }

        public void Render(IClipper clipper) {
            throw new NotImplementedException();
        }

        public void OnPaint(IRenderEventArgs e) {
            var surface = (ContextSurface)e.Surface;
            var ctx = surface.Context;
            ctx.SetColor (BackColor());
            ctx.Rectangle (e.Clipper.Bounds); //??
            ctx.Fill();
            Layer.OnPaint (e);
        }
    }
}