/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2012 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Drawing;
using Limaki.Graphs;
using System.Collections.Generic;
using Limaki.Actions;
using Xwt;

namespace Limaki.View.Viz.Modelling {
    /// <summary>
    /// a Locator for Graph-Items
    /// assuming that an item has a Shape
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    public interface IGraphSceneLocator<TItem,TEdge>:ILocator<TItem> where TEdge : IEdge<TItem>, TItem {

        ICollection<TEdge> AffectedEdges { get; set; }

        /// <summary>
        /// gives back a valid shape
        /// if item has no shape:
        /// CreateShape is called 
        /// item, shape is added to invokeList
        /// shape is Justified
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        IShape GetOrCreateShape ( TItem item );

        /// <summary>
        /// adds item to invokeList
        /// if item.Shape == null, Invoke(item) is called
        /// else item,null is added to invokeList
        /// </summary>
        /// <param name="item"></param>
        IShape EnsureInvoke ( TItem item );

        /// <summary>
        /// resets the size of a shape
        /// ensures that item has a shape
        /// </summary>
        /// <param name="item"></param>
        void Justify ( TItem item );

        /// <summary>
        /// the implementor can do operations as commands
        /// they are added to requests with Commit
        /// </summary>
        /// <param name="requests"></param>
        void Commit (ICollection<ICommand<TItem>> requests);

        IEnumerable<TItem> ElementsIn (Rectangle bounds);
    }
}