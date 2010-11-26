using System.Drawing;
using Limaki.Actions;
using Limaki.Drawing;
using Limaki.Drawing.Painters;
using Limaki.Drawing.Shapes;
using Limaki.Widgets;
using Limaki.Winform;

namespace Limaki.Winform.Displays {
    public class ImageKit:DisplayKit<Image> {
        public override Layer<Image> Layer(IZoomTarget zoomTarget, IScrollTarget scrollTarget) {
            return new ImageLayer (zoomTarget, scrollTarget);
        }
        public override SelectionBase SelectAction(IWinControl control, ICamera camera) {
            SelectionShape result = new SelectionShape(control, camera);
            result.ShapeDataType = typeof(Rectangle);
            result.PainterFactory = new PainterFactory ();
            result.ShapeFactory = new ShapeFactory ();
            result.Style = StyleSheet.SystemStyle;
            //return new ResizeableSelector (control, camera);
            return result;
        }
    }
}
