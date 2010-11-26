/*
 * Limaki 
 * Version 0.063
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

using System.Drawing;
using System;
using Limaki.Drawing.Shapes;
using Limaki.Common;

namespace Limaki.Drawing.Painters {
    public class PainterFactory : FactoryBase {
        public virtual IPainter CreatePainter(Type type) {
            return (IPainter)One(type);
        }
        public virtual IPainter<T> CreatePainter<T>() {
            return One<T,IPainter<T>>();
        }
        public virtual IPainter CreatePainter(IShape shape) {
            return CreatePainter (shape.GetType());
        }

        protected override void defaultClazzes() {
            Clazzes.Add(typeof(RectangleShape), typeof(RectanglePainter));
            Clazzes.Add(typeof(VectorShape), typeof(VectorPainter));
            Clazzes.Add(typeof(RoundedRectangleShape), typeof(RoundedRectanglePainter));
        }
    }
}