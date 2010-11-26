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
    public class WPFPainterFactory : FactoryBase, IPainterFactory {

        /// <summary>
        /// gives back a painter provided for shape of type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual IPainter CreatePainter(Type type) {
            return (IPainter)Create(type);
        }
        public virtual IPainter<T> CreatePainter<T>() {
            return Create<T, IPainter<T>>();
        }
        public virtual IPainter CreatePainter(IShape shape) {
            return CreatePainter(shape.GetType());
        }

        protected override void InstrumentClazzes() {
            Clazzes.Add(typeof(IRectangleShape), typeof(WPFPainter<RectangleI>));
            Clazzes.Add(typeof(IVectorShape), typeof(WPFPainter<Vector>));
            Clazzes.Add(typeof(IRoundedRectangleShape), typeof(WPFPainter<RectangleI>));
            Clazzes.Add(typeof(IBezierShape), typeof(WPFPainter<RectangleI>));

            Clazzes.Add(typeof(RectangleShape), typeof(WPFPainter<RectangleI>));
            Clazzes.Add(typeof(VectorShape), typeof(WPFPainter<Vector>));
            Clazzes.Add(typeof(RoundedRectangleShape), typeof(WPFPainter<RectangleI>));
            Clazzes.Add(typeof(BezierShape), typeof(WPFPainter<RectangleI>));

            Clazzes.Add(typeof(RectangleI), typeof(WPFPainter<RectangleI>));

            Clazzes.Add(typeof(string), typeof(WPFStringPainter));
        }
    }
}