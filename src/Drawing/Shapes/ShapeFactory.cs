/*
 * Limaki 
 * Version 0.07
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
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Limaki.Common;

namespace Limaki.Drawing.Shapes {
    /// <summary>
    /// The ShapeFactory holds a list about which grafic data object needs which shape
    /// </summary>
    public class ShapeFactory : FactoryBase {
        /// <summary>
        /// Create a Shape with a given location and size 
        /// with a typeof(T) graphic data object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="location"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public IShape<T> Shape<T>(Point location, Size size) {
            T data = (T)System.Activator.CreateInstance (typeof (T), new object[] {location, size});
            Type clazzType = Clazz<T>();
            IShape<T> result = (IShape<T>) System.Activator.CreateInstance (clazzType);
            result.Data = data;
            return result;
            
        }

        /// <summary>
        /// Creates an instance of IShape{T} where typeof(T)==typeofShapeData
        /// </summary>
        /// <param name="typeofShapeData"></param>
        /// <param name="location"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public IShape Shape(Type typeofShapeData, Point location, Size size) {
            object data = System.Activator.CreateInstance(typeofShapeData, new object[] { location, size });
            Type clazzType = Clazz(typeofShapeData);
            IShape result = (IShape)System.Activator.CreateInstance(clazzType, new object[] { data });
            return result;

        }

        protected override void defaultClazzes() {
            Clazzes.Add(typeof(Rectangle),typeof(RectangleShape));
            Clazzes.Add(typeof(Vector), typeof(VectorShape));
        }
    }
}
