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

using System.Collections.Generic;

namespace Limaki.Graphs {
    public interface IGraph<TItem,TEdge>:ICollection<TItem> {
        bool EdgeIsItem {get;}
        bool ItemIsStorable { get; }

        bool Contains ( TEdge edge );
        void Add ( TEdge edge );
        void ChangeEdge ( TEdge edge, TItem oldItem, TItem newItem );
        bool Remove ( TEdge edge );
        int EdgeCount ( TItem item );
        IEnumerable<TEdge> Edges ( TItem item );
        IEnumerable<TEdge> Edges();

        /// <summary>
        /// returns a KVP of all Items which have Edges
        /// if an item has no edges, it will NOT be listed
        /// </summary>
        /// <returns></returns>
        IEnumerable<KeyValuePair<TItem, ICollection<TEdge>>> ItemsWithEdges();

        IEnumerable<TEdge> PreorderEdges ( TItem source );
        IEnumerable<TEdge> PostorderEdges ( TItem source );
    }
}