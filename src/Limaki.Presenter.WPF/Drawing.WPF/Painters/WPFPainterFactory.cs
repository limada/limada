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
    public class WPFPainterFactory : Limaki.Drawing.Painters.PainterFactoryBase, IPainterFactory {

       
        protected override void InstrumentClazzes() {
            //Clazzes[typeof(IRectangleShape)] =  typeof(WPFPainter<RectangleD>);
            //Clazzes[typeof(IVectorShape)] =  typeof(WPFPainter<Vector>);
            //Clazzes[typeof(IRoundedRectangleShape)] =  typeof(WPFPainter<RectangleD>);
            //Clazzes[typeof(IBezierShape)] =  typeof(WPFPainter<RectangleD>);

            //Clazzes[typeof(RectangleShape)] =  typeof(WPFPainter<RectangleD>);
            //Clazzes[typeof(VectorShape)] =  typeof(WPFPainter<Vector>);
            //Clazzes[typeof(RoundedRectangleShape)] =  typeof(WPFPainter<RectangleD>);
            //Clazzes[typeof(BezierShape)] =  typeof(WPFPainter<RectangleD>);

            //Clazzes[typeof(RectangleD)] =  typeof(WPFPainter<RectangleD>);

            //Clazzes[typeof(string)] =  typeof(WPFStringPainter);

            Add<IPainter<IShape<RectangleD>, RectangleD>>(() => new WPFRectanglePainter());
            Add<IPainter<IShape<Vector>, Vector>>(() => new WPFVectorPainter());
            Add<IPainter<IRoundedRectangleShape, RectangleD>>(() => new WPFRoundedRectanglePainter());
            Add<IPainter<IRectangleShape, RectangleD>>(() => new WPFRoundedRectanglePainter());
            Add<IPainter<IBezierShape, RectangleD>>(() => new WPFBezierPainter());
            Add<IPainter<IVectorShape, Vector>>(() => new WPFVectorPainter());

            

            Add<IPainter<RectangleD>>(() => new WPFPainter<RectangleD>());

            Add<IPainter<string>>(() => new WPFStringPainter());
        }
    }
}