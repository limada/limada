using Limaki.Graphs;
using Limaki.Presenter.UI;
using Limaki.Drawing.WPF;

namespace Limaki.Presenter.WPF.Display {
    public class WPFGraphSceneLayer<TItem, TEdge> : GraphSceneLayer<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {

        public override void OnPaint(IRenderEventArgs e) {
            var g = ((WPFSurface)e.Surface).Graphics;

            
            var transform = this.Camera.Matrice;
            //g.RenderTransformOrigin = new System.Windows.Point (-transform.OffsetX, -transform.OffsetY);
            g.RenderTransform = new System.Windows.Media.TranslateTransform (transform.OffsetX, transform.OffsetY);
            // see also: MatrixTransform
            base.OnPaint(e);
        }
    }


}