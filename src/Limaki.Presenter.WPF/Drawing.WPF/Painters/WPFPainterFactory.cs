using System;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using Limaki.Drawing.WPF.Painters;
using RectangleShape=Limaki.Drawing.WPF.Shapes.RectangleShape;
using RoundedRectangleShape=Limaki.Drawing.WPF.Shapes.RoundedRectangleShape;
using VectorShape=Limaki.Drawing.WPF.Shapes.VectorShape;

namespace Limaki.Drawing.WPF.Painters {
    public class WPFPainterFactory : Limaki.Drawing.Painters.PainterFactoryBase, IPainterFactory {

       
        protected override void InstrumentClazzes() {
            //Clazzes[typeof(IRectangleShape)] =  typeof(WPFPainter<RectangleI>);
            //Clazzes[typeof(IVectorShape)] =  typeof(WPFPainter<Vector>);
            //Clazzes[typeof(IRoundedRectangleShape)] =  typeof(WPFPainter<RectangleI>);
            //Clazzes[typeof(IBezierShape)] =  typeof(WPFPainter<RectangleI>);

            //Clazzes[typeof(RectangleShape)] =  typeof(WPFPainter<RectangleI>);
            //Clazzes[typeof(VectorShape)] =  typeof(WPFPainter<Vector>);
            //Clazzes[typeof(RoundedRectangleShape)] =  typeof(WPFPainter<RectangleI>);
            //Clazzes[typeof(BezierShape)] =  typeof(WPFPainter<RectangleI>);

            //Clazzes[typeof(RectangleI)] =  typeof(WPFPainter<RectangleI>);

            //Clazzes[typeof(string)] =  typeof(WPFStringPainter);

            Add<IPainter<IShape<RectangleI>, RectangleI>>(() => new WPFRectanglePainter());
            Add<IPainter<IShape<Vector>, Vector>>(() => new WPFVectorPainter());
            Add<IPainter<IRoundedRectangleShape, RectangleI>>(() => new WPFRoundedRectanglePainter());
            Add<IPainter<IRectangleShape, RectangleI>>(() => new WPFRoundedRectanglePainter());
            Add<IPainter<IBezierShape, RectangleI>>(() => new WPFBezierPainter());
            Add<IPainter<IVectorShape, Vector>>(() => new WPFVectorPainter());

            

            Add<IPainter<RectangleI>>(() => new WPFPainter<RectangleI>());

            Add<IPainter<string>>(() => new WPFStringPainter());
        }
    }
}