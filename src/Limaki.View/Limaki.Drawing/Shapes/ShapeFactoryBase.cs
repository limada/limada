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
using Xwt;

namespace Limaki.Drawing.Shapes {
    /// <summary>
    /// The ShapeFactory holds a list about which grafic data object needs which shape
    /// </summary>
    public abstract class ShapeFactoryBase : Factory, IShapeFactory {
        /// <summary>
        /// Create a Shape with a given location and size 
        /// with a typeof(T) graphic data object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="location"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public IShape<T> Shape<T>(Point location, Size size) {
            T data = (T)System.Activator.CreateInstance(typeof(T), new object[] { location,size });

            var result = Create<IShape<T>>();
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
            //var rect = new RectangleD(location, size);
            object data = System.Activator.CreateInstance(typeofShapeData);//, new object[] { rect });
            Type clazzType = typeof (IShape<>).GetGenericTypeDefinition().MakeGenericType(typeofShapeData);
            IShape result = (IShape) this.Create(clazzType);
            result.Location = location;
            result.Size = size;
            return result;

        }


    }
}