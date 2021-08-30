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
 */


using Limaki.Data;

namespace Limaki.Contents {

    public interface IProxy {

        /// <summary>
        /// realSubject is released and shouldn't 
        /// use any memory after calling ClearRealSubject
        /// </summary>
        void ClearRealSubject();

        /// <summary>
        /// realSubject is released
        /// if clean==false, the realSubject is not disposed or closed
        /// the caller cares about cleaning the ClearRealSubject
        /// </summary>
        void ClearRealSubject(bool clean);

        void Flush();
    }

    /// <summary>
    /// a proxy using a ContentContainer to get the Data
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IContainerProxy<TKey> : IProxy {

        IContentContainer<TKey> ContentContainer { get; set; }

    }

}