using System;
using Limaki.Common;

namespace Limaki.Drawing {
    public interface IShapeFactory:IFactory {
        /// <summary>
        /// Create a Shape with a given location and size 
        /// with a typeof(T) graphic data object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="location"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        IShape<T> Shape<T> ( PointI location, SizeI size );

    

        /// <summary>
        /// Creates an instance of IShape{T} where typeof(T)==typeofShapeData
        /// </summary>
        /// <param name="typeofShapeData"></param>
        /// <param name="location"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        IShape Shape ( Type typeofShapeData, PointI location, SizeI size );
    }
}