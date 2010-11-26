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

namespace Limaki.Common {
    /// <summary>
    /// a list of objects, stored in a dictionary#System.Type,object>
    /// uses the PoolFactory to create the object
    /// the objects are "singletons" in that way that they can be resused
    /// </summary>
    public interface IPool {
        /// <summary>
        /// the factory used to create objects
        /// </summary>
        IFactory PoolFactory { set;}
        
        /// <summary>
        /// Creates a new instance of type if it is not in the pool
        /// uses PoolFactory for this
        /// </summary>
        /// <returns></returns>
        object TryGetCreate ( Type type );

        /// <summary>
        /// Creates a new instance of type if it is not in the pool
        /// uses PoolFactory for this
        /// </summary>
        /// <returns></returns>
        T TryGetCreate<T>();

        bool Remove<T>();
    }
}