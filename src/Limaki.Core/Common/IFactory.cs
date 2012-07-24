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
using System.Collections.Generic;

namespace Limaki.Common {
    public interface IFactory {
        /// <summary>
        /// returns the associated Type of type
        /// Association is in a Dictionary<Type, Type> 
        /// where key=interface and value=class
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        //Type Clazz ( Type type );

        Type Clazz<T>();
        IEnumerable<Type> KnownClasses { get; }
        bool Contains<T>();
        bool Contains(Type type);
        object Create ( Type type );

        /// <summary>
        /// calls the default-construtor of the class
        /// which is associated with interface T
        /// <code>T o = TranslatedType.New();</code>
        /// <seealso cref="Factory.Clazz"/>
        /// </summary>
        /// <typeparam name="T">interface to translate</typeparam>
        /// <returns>new object of translated class</returns>
        T Create<T>();

        T Create<T>(params object[] args);

        /// <summary>
        /// calls the default-construtor of the class TOuter#TInner#
        /// which is associated with type T
        /// <seealso cref="Factory.Clazz"/>
        /// </summary>
        /// <typeparam name="TInner">type to translate</typeparam>
        /// <returns>new object of R_Of{T}</returns>
        //TOuter Create<TInner, TOuter>();

        void Add ( Type source, Type target );
        void Add<T1, T2>() where T2 : T1;
        void Add<T>(Func<T> creator);
        void Add<T>(Func<object[], T> creator);

        void Clear ();
    }
}