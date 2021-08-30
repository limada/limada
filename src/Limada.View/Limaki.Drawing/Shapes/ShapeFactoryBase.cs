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
 * http://www.limada.org
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
        public IShape<T> Shape<T>(Point location, Size size, bool isSizingForData) {
            var data = (T)Activator.CreateInstance(typeof(T), new object[] { location,size });

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
        public IShape Shape (Type typeofShapeData, Point location, Size size, bool isSizingForData) {
            //var rect = new RectangleD(location, size);
            var data = Activator.CreateInstance(typeofShapeData); //, new object[] { rect });
            var clazzType = typeof (IShape<>).GetGenericTypeDefinition().MakeGenericType(typeofShapeData);
            var result = (IShape) this.Create(clazzType);
            result.Location = location;
            if (isSizingForData)
                result.DataSize = size;
            else
                result.Size = size;
            return result;

        }


    }
}