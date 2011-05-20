/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System;
using Limaki.Common;
using Limaki.Drawing.Shapes;

namespace Limaki.Drawing.GDI.Painters {
    public class PainterFactory : FactoryBase, IPainterFactory {
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
            return CreatePainter (shape.GetType());
        }

        protected override void InstrumentClazzes() {
            Clazzes[typeof(IRectangleShape)] = typeof(RectanglePainter);
            Clazzes[typeof(IVectorShape)] = typeof(VectorPainter);
            Clazzes[typeof(IRoundedRectangleShape)] = typeof(RoundedRectanglePainter);
            Clazzes[typeof(IBezierShape)] = typeof(BezierPainter);

            Clazzes[typeof(RectangleShape)] = typeof(RectanglePainter);
            Clazzes[typeof(VectorShape)] = typeof(VectorPainter);
            Clazzes[typeof(RoundedRectangleShape)] = typeof(RoundedRectanglePainter);
            Clazzes[typeof(BezierShape)] = typeof(BezierPainter);

            Clazzes[typeof(RectangleI)] = typeof(RectanglePainter);

            Clazzes[typeof(string)] = typeof(StringPainter);
            
        }
    }
}