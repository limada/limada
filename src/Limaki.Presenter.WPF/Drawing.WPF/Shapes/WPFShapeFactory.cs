using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Xwt;


namespace Limaki.Drawing.WPF.Shapes {
    public class WPFShapeFactory:ShapeFactoryBase {
        protected override void InstrumentClazzes() {
           Add<IShape<RectangleD>>(()=>new Drawing.WPF.Shapes.RectangleShape());
           Add<IShape<Vector>>(()=>new Drawing.WPF.Shapes.VectorShape());

           Add<IRectangleShape>(()=>new Drawing.WPF.Shapes.RectangleShape());
           Add<IRoundedRectangleShape>(()=>new Drawing.WPF.Shapes.RoundedRectangleShape());
           Add<IBezierShape>(()=>new Drawing.WPF.Shapes.RectangleShape());
           Add<IVectorShape>(()=>new Drawing.WPF.Shapes.VectorShape());

        
          
        }
    }
}