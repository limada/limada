using Limaki.Drawing;
using Limaki.Drawing.Shapes;


namespace Limaki.Drawing.WPF.Shapes {
    public class WPFShapeFactory:ShapeFactoryBase {
        protected override void InstrumentClazzes() {
            Clazzes.Add(typeof(RectangleI), typeof(Drawing.WPF.Shapes.RectangleShape));
            Clazzes.Add(typeof(Vector), typeof(Drawing.WPF.Shapes.VectorShape));

            Clazzes.Add(typeof(IRectangleShape), typeof(Drawing.WPF.Shapes.RectangleShape));
            Clazzes.Add(typeof(IRoundedRectangleShape), typeof(Drawing.WPF.Shapes.RoundedRectangleShape));
            Clazzes.Add(typeof(IBezierShape), typeof(Drawing.WPF.Shapes.RectangleShape));
            Clazzes.Add(typeof(IVectorShape), typeof(Drawing.WPF.Shapes.VectorShape));
        }
    }
}