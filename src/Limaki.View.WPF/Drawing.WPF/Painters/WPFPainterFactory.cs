using System;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using Limaki.Drawing.WPF.Painters;
using Xwt;
using RectangleShape=Limaki.Drawing.WPF.Shapes.RectangleShape;
using RoundedRectangleShape=Limaki.Drawing.WPF.Shapes.RoundedRectangleShape;
using VectorShape=Limaki.Drawing.WPF.Shapes.VectorShape;

namespace Limaki.Drawing.WPF.Painters {
    public class WPFPainterFactory : Limaki.Drawing.Painters.DefaultPainterFactory, IPainterFactory {

       
        protected override void InstrumentClazzes() {
            //Clazzes[typeof(IRectangleShape)] =  typeof(WPFPainter<Rectangle>);
            //Clazzes[typeof(IVectorShape)] =  typeof(WPFPainter<Vector>);
            //Clazzes[typeof(IRoundedRectangleShape)] =  typeof(WPFPainter<Rectangle>);
            //Clazzes[typeof(IBezierShape)] =  typeof(WPFPainter<Rectangle>);

            //Clazzes[typeof(RectangleShape)] =  typeof(WPFPainter<Rectangle>);
            //Clazzes[typeof(VectorShape)] =  typeof(WPFPainter<Vector>);
            //Clazzes[typeof(RoundedRectangleShape)] =  typeof(WPFPainter<Rectangle>);
            //Clazzes[typeof(BezierShape)] =  typeof(WPFPainter<Rectangle>);

            //Clazzes[typeof(Rectangle)] =  typeof(WPFPainter<Rectangle>);

            //Clazzes[typeof(string)] =  typeof(WPFStringPainter);

            Add<IPainter<IShape<Rectangle>, Rectangle>>(() => new WPFRectanglePainter());
            Add<IPainter<IShape<Vector>, Vector>>(() => new WPFVectorPainter());
            Add<IPainter<IRoundedRectangleShape, Rectangle>>(() => new WPFRoundedRectanglePainter());
            Add<IPainter<IRectangleShape, Rectangle>>(() => new WPFRoundedRectanglePainter());
            Add<IPainter<IBezierShape, Rectangle>>(() => new WPFBezierPainter());
            Add<IPainter<IVectorShape, Vector>>(() => new WPFVectorPainter());

            

            Add<IPainter<Rectangle>>(() => new WPFPainter<Rectangle>());

            Add<IPainter<string>>(() => new WPFStringPainter());
        }
    }
}