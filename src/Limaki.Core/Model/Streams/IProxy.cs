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
 */


using Limaki.Data;

namespace Limaki.Model.Streams {

    public interface IProxy {
        /// <summary>
        /// realSubject is released and shouldn't 
        /// use any memory after calling clearRealSubject
        /// </summary>
        void ClearRealSubject();

        /// <summary>
        /// realSubject is released
        /// if clean==false, the object is not disposed or closed
        /// the caller cares about cleaning the object
        /// </summary>
        void ClearRealSubject(bool clean);
        void Flush();
    }

    /// <summary>
    /// a proxy using a DataContainer to get the Data
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IContainerProxy<TKey> : IProxy {
        IDataContainer<TKey> DataContainer { get; set; }
    }

    /// <summary>
    /// gets expensive objects on demand 
    /// </summary>
    //public interface IProxy<TKey,TData> : IContainerProxy<TKey> {
    //    /// <summary>
    //    /// the realSubject of the Proxy
    //    /// </summary>
    //    TData Data { get; set;}
    //}


}
