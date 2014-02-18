using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Limaki.View.Headless.VidgetBackends {
    public class HeadlessGraphSceneLayer<TItem, TEdge> : GraphSceneLayer<TItem, TEdge>
     where TEdge : TItem, IEdge<TItem> {

        public override void OnPaint (IRenderEventArgs e) {
            var surface = (ContextSurface)e.Surface;
            var ctx = surface.Context;
            var matrix = this.Camera.Matrix;
            surface.Matrix = matrix;
            ctx.Save ();

            try {
                if (!matrix.IsIdentity)
                    ctx.ModifyCTM (matrix);
                this.Renderer.Render (this.Data, e);

            } catch (Exception ex) {
                throw;
            } finally {
                ctx.Restore ();
            }

        }
    }
}
