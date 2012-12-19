using Limaki.Graphs;
using Limaki.View.UI;
using Limaki.Drawing.WPF;
using Limaki.View.Rendering;
using Limaki.View.UI.GraphScene;

namespace Limaki.View.WPF.Display {
    public class WPFGraphSceneLayer<TItem, TEdge> : GraphSceneLayer<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {

        public override void OnPaint(IRenderEventArgs e) {
            var g = ((WPFSurface)e.Surface).Graphics;

            
            var transform = this.Camera.Matrix;
            //g.RenderTransformOrigin = new System.Windows.Point (-transform.OffsetX, -transform.OffsetY);
            g.RenderTransform = new System.Windows.Media.TranslateTransform (transform.OffsetX, transform.OffsetY);
            // see also: MatrixTransform
            base.OnPaint(e);
        }
    }


}