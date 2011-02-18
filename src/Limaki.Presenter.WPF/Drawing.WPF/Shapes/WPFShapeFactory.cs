using Limaki.Drawing;
using Limaki.Drawing.Shapes;


namespace Limaki.Drawing.WPF.Shapes {
    public class WPFShapeFactory:ShapeFactoryBase {
        protected override void InstrumentClazzes() {
            Clazzes[typeof(RectangleI)] =  typeof(Drawing.WPF.Shapes.RectangleShape);
            Clazzes[typeof(Vector)] =  typeof(Drawing.WPF.Shapes.VectorShape);

            Clazzes[typeof(IRectangleShape)] =  typeof(Drawing.WPF.Shapes.RectangleShape);
            Clazzes[typeof(IRoundedRectangleShape)] =  typeof(Drawing.WPF.Shapes.RoundedRectangleShape);
            Clazzes[typeof(IBezierShape)] =  typeof(Drawing.WPF.Shapes.RectangleShape);
            Clazzes[typeof(IVectorShape)] =  typeof(Drawing.WPF.Shapes.VectorShape);
        }
    }
}