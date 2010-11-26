/*
 * Limaki 
 * Version 0.064
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
    public class Graph<TItem, TEdge>:MultiDictionaryGraph<TItem,TEdge,MultiDictionary<TItem,TEdge>,Set<TEdge>> 
        where TEdge : IEdge<TItem> {}
}
