using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.UI;

namespace Limaki.Winform.Displays {
    public class ImageKit:DisplayKit<System.Drawing.Image> {
        //public override Layer<System.Drawing.Image> Layer(IZoomTarget zoomTarget, IScrollTarget scrollTarget) {
        //    return new ImageLayer (zoomTarget, scrollTarget);
        //}
        public override Layer<System.Drawing.Image> Layer(ICamera camera) {
            return new ImageLayer(camera);
        }
        public override SelectionBase SelectAction(IWinControl control, ICamera camera) {
            SelectionShape result = new SelectionShape(control, camera);
            result.ShapeDataType = typeof(RectangleI);
            result.SelectionPainter.Style = 
                Limaki.Drawing.StyleSheet.CreateStyleWithSystemSettings ();
            //return new ResizeableSelector (control, camera);
            return result;
        }
    }
}
