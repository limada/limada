/*
 * Limaki 
 * Version 0.08
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

using Limaki.Common.Collections;

namespace Limaki.Graphs {
    /// <summary>
    /// a Graph based on a multidictionary
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    public class Graph<TItem, TEdge>:MultiDictionaryGraph<TItem,TEdge,MultiDictionary<TItem, TEdge>,Set<TEdge>> 
        where TEdge : IEdge<TItem> {}
}
