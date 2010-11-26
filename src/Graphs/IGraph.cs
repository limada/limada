/*
 * Limaki 
 * Version 0.071
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
using System;

namespace Limaki.Graphs {
    /// <summary>
    /// a graph consists of items which are connected with links
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    public interface IGraph<TItem,TEdge>:ICollection<TItem>
    where TEdge : IEdge<TItem> {
        /// <summary>
        /// returns TEdge is TItem
        /// </summary>
        bool EdgeIsItem {get;}

        /// <summary>
        /// returns true if TItem is a class or a structure
        /// returns false if TItem is a primitive class
        /// </summary>
        bool ItemIsStorable { get; }

        bool Contains ( TEdge edge );
        void Add ( TEdge edge );
        void ChangeEdge ( TEdge edge, TItem oldItem, TItem newItem );
        bool Remove ( TEdge edge );
        int EdgeCount ( TItem item );
        
        ICollection<TEdge> Edges ( TItem item );
        
        IEnumerable<TEdge> Edges();

        /// <summary>
        /// iterates over all Egdes(item) in source
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        IEnumerable<TEdge> Edges(IEnumerable<TItem> source);

        /// <summary>
        /// returns a KVP of all Items which have Edges
        /// if an item has no edges, it will NOT be listed
        /// </summary>
        /// <returns></returns>
        IEnumerable<KeyValuePair<TItem, ICollection<TEdge>>> ItemsWithEdges();

        /// <summary>
        /// A Twig of an item is a sequence of edges where
        /// - that item is a root or leaf of an edge (same as: Edges(item))
        /// - if TEdge is TItem: every Twig(edge as TItem) of the edges in that sequence.
        /// 
        /// Implementation: this method should implement a pre-order walk 
        /// 
        /// Usage: 
        /// if an item is visually moved, all edges in the Twig of that item are moved to
        /// if an item is deleted, all edges in Twig(item) have to be deleted  
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        IEnumerable<TEdge> Twig ( TItem source );

        IEnumerable<TEdge> PostorderTwig ( TItem source );
        IEnumerable<TEdge> DepthFirstTwig(TItem source);

        /// <summary>
        /// each Vein(edge) of Edges(Source) 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        IEnumerable<TEdge> Fork ( TItem source );

        /// <summary>
        /// an enumeration containing
        /// - the source
        /// - Vein(leaf) / Vein(root) if leaf / root is TEdge
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        IEnumerable<TEdge> Vein(TEdge source);

        /// <summary>
        /// gives back all edge.Root and edge.Leaf in egdes 
        /// if Root/Leaf is not TEdge
        /// </summary>
        /// <param name="edges"></param>
        /// <returns></returns>
        IEnumerable<TItem> Foliage ( IEnumerable<TEdge> edges );
        
        Action<TItem> DataChanged { get;set;}

        void OnDataChanged( TItem item );
    }
}