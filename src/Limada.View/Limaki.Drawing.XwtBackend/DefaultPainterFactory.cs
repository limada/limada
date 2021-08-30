/*
 * Limaki 
 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2012 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.Common;
using Limaki.Common.Reflections;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Xwt;

namespace Limaki.Drawing.XwtBackend {

    public class DefaultPainterFactory: Factory, IPainterFactory {
        /// <summary>
        /// gives back a painter provided for shape of type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual IPainter CreatePainter (Type type) {
            if (type.IsInterface) {
                var ptype = typeof (IPainter<>).GetGenericTypeDefinition ().MakeGenericType (type);
                return (IPainter)Create (ptype);
            }
            if (type == typeof (string))
                return Create<IPainter<string>> ();
            if (typeof (Xwt.Drawing.Image).IsAssignableFrom (type))
                return Create<IPainter<Xwt.Drawing.Image>> ();
            if (Reflector.Implements (type, typeof (IRectangleShape)))
                return Create<IPainter<IRectangleShape, Rectangle>> ();
            if (Reflector.Implements (type, typeof (IRoundedRectangleShape)))
                return Create<IPainter<IRoundedRectangleShape, Rectangle>> ();
            if (Reflector.Implements (type, typeof (IBezierRectangleShape)))
                return Create<IPainter<IBezierRectangleShape, Rectangle>> ();
            if (Reflector.Implements (type, typeof (IVectorShape)))
                return Create<IPainter<IVectorShape, Vector>> ();

            return null;
        }

        public virtual IPainter<T> CreatePainter<T>() {
            return Create<IPainter<T>>();
        }

        public virtual IPainter CreatePainter(IShape shape) {
            return CreatePainter(shape.GetType());
        }

        protected override void InstrumentClazzes () {

            Add<IPainter<IShape<Rectangle>, Rectangle>> (() => new RectanglePainter ());
            Add<IPainter<IRectangleShape, Rectangle>> (() => new RectanglePainter ());
            Add<IPainter<Rectangle>> (() => new RectanglePainter ());

            Add<IPainter<IRoundedRectangleShape, Rectangle>> (() => new RoundedRectanglePainter ());
            Add<IPainter<IBezierRectangleShape, Rectangle>> (() => new BezierPainter ());

            Add<IPainter<IShape<Vector>, Vector>> (() => new VectorPainter ());
            Add<IPainter<IVectorShape, Vector>> (() => new VectorPainter ());

            Add<IPainter<string>>(() => new StringPainter());
            Add<IPainter<Xwt.Drawing.Image>> (() => new ImagePainter ());
            

        }
    }
}