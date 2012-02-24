using System;
using Limaki.Common;
using Limaki.Drawing.Shapes;
using Xwt;

namespace Limaki.Drawing.Painters {
    public abstract class PainterFactoryBase: Factory {
        /// <summary>
        /// gives back a painter provided for shape of type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual IPainter CreatePainter(Type type) {
            if (type.IsInterface) {
                var ptype = typeof(IPainter<>).GetGenericTypeDefinition().MakeGenericType(type);
                return (IPainter)Create(ptype);
            } else {
                if (type == typeof(string))
                    return Create<IPainter<string>>();
                if (Reflector.Implements(type, typeof(IRectangleShape)))
                    return Create<IPainter<IRectangleShape, RectangleD>>();
                if (Reflector.Implements(type, typeof(IRoundedRectangleShape)))
                    return Create<IPainter<IRoundedRectangleShape, RectangleD>>();
                if (Reflector.Implements(type, typeof(IBezierShape)))
                    return Create<IPainter<IBezierShape, RectangleD>>();
                if (Reflector.Implements(type, typeof(IVectorShape)))
                    return Create<IPainter<IVectorShape, Vector>>();
            }
            return null;
        }

        public virtual IPainter<T> CreatePainter<T>() {
            return Create<IPainter<T>>();
        }

        public virtual IPainter CreatePainter(IShape shape) {

            return CreatePainter(shape.GetType());
        }
    }
}