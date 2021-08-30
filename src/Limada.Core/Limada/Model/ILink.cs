/*
 * Limada 
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

using Limaki.Graphs;
using Id = System.Int64;

namespace Limada.Model {

    /// <summary>
    /// ILink is an IEdge with a Marker
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILink<T>:IEdge<T> {

        T Marker { get;set;}
        //IThing Adjacent ( IThing thing );
    }

    /// <summary>
    /// ILink#IThing is a <see cref="ILink#T"/> with
    /// IThings as Root, Link, Marker of ILink#IThing
    /// IDs as Root, Link, Marker of ILink#Id
    /// is a <see cref="IThing#IThing"/>
    /// with Data == Marker
    /// </summary>
    public interface ILink : IThing, IThing<IThing>, ILink<IThing> {

    }
}