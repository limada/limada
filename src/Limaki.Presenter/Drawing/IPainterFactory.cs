using System;

namespace Limaki.Drawing {
    public interface IPainterFactory {
        /// <summary>
        /// gives back a painter provided for shape of type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IPainter CreatePainter ( Type type );

        IPainter<T> CreatePainter<T>();
        IPainter CreatePainter ( IShape shape );
    }
}