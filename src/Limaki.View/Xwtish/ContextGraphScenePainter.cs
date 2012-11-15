using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.Rendering;
using Xwt;
using Xwt.Drawing;

namespace Limaki.Viewers {

    /// <summary>
    /// a GraphScenePainter using
    /// Xwt.Context to paint
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    public class ContextGraphScenePainter<TItem, TEdge> : GraphScenePainter<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {
        public virtual void Paint (Context context) {
            if (this.Data == null)
                return;

            var size = Data.Shape.Size + new Size (Layout.Border.Width, Layout.Border.Height);
            var f = DrawingExtensions.DpiFactor (new Size (72, 72));
            this.Viewport.ClipOrigin = Data.Shape.Location;

            var clipRect = new Rectangle (0, 0, (int) size.Width, (int) size.Height);

            var e = new ContextRenderEventArgs (context, clipRect);

            OnPaint (e);
        }
    }
}