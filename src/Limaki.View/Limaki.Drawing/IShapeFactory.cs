using System;
using Limaki.Common;
using Xwt;

namespace Limaki.Drawing {
    public interface IShapeFactory:IFactory {
        /// <summary>
        /// Create a Shape with a given location and size 
        /// with a typeof(T) graphic data object
        /// if sizingForData == true, then the inner DataSize of the Shape is set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="location"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        IShape<T> Shape<T> ( Point location, Size size, bool sizingForData );

    

        /// <summary>
        /// Creates an instance of IShape{T} where typeof(T)==typeofShapeData
        /// </summary>
        /// <param name="typeofShapeData"></param>
        /// <param name="location"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        IShape Shape (Type typeofShapeData, Point location, Size size, bool sizingForData);
    }
}