/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
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
            Clazzes.Add(typeof(IRectangleShape), typeof(RectanglePainter));
            Clazzes.Add(typeof(IVectorShape), typeof(VectorPainter));
            Clazzes.Add(typeof(IRoundedRectangleShape), typeof(RoundedRectanglePainter));
            Clazzes.Add(typeof(IBezierShape), typeof(BezierPainter));

            Clazzes.Add(typeof(RectangleShape), typeof(RectanglePainter));
            Clazzes.Add(typeof(VectorShape), typeof(VectorPainter));
            Clazzes.Add(typeof(RoundedRectangleShape), typeof(RoundedRectanglePainter));
            Clazzes.Add(typeof(BezierShape), typeof(BezierPainter));

            Clazzes.Add(typeof(RectangleI), typeof(RectanglePainter));

            Clazzes.Add(typeof(string), typeof(StringPainter));
        }
    }
}