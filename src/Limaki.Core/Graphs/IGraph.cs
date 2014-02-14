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
        void ChangeEdge ( TEdge edge, TItem newItem, bool changeRoot );
        void RevertEdge ( TEdge edge );
        bool Remove ( TEdge edge );
        TItem Adjacent ( TEdge edge, TItem item );
        int EdgeCount ( TItem item );
        
        ICollection<TEdge> Edges ( TItem item );
        
        IEnumerable<TEdge> Edges();

        /// <summary>
        /// iterates over all Egdes(item) in source
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        IEnumerable<TEdge> Edges(IEnumerable<TItem> source);

        bool RootIsEdge(TEdge curr);
        bool LeafIsEdge(TEdge curr);

        /// <summary>
        /// returns a KVP of all Items which have Edges
        /// if an item has no edges, it will NOT be listed
        /// </summary>
        /// <returns></returns>
        IEnumerable<KeyValuePair<TItem, ICollection<TEdge>>> ItemsWithEdges();

        #region basic algorithms
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

        /// <summary>
        /// same as Twig, but with a post-order walk
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        IEnumerable<TEdge> PostorderTwig ( TItem source );
        IEnumerable<TEdge> DepthFirstTwig(TItem source);


        /// <summary>
        /// an enumeration containing
        /// - the source
        /// - Vein(leaf) / Vein(root) if leaf / root is TEdge
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        IEnumerable<TEdge> Vein(TEdge source);

        /// <summary>
        /// each Vein(edge) of Edges(Source) 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        IEnumerable<TEdge> Fork ( TItem source );
        
        /// <summary>
        /// an enumeration containing:
        /// all fork-edges where link.Root && link.Leaf meats pred
        /// </summary>
        /// <param name="sources"></param>
        /// <returns></returns>
        IEnumerable<TEdge> Fork(TItem source, Func<TItem,bool> pred);

      




        /// <summary>
        /// gives back all edge.Root and edge.Leaf in egdes 
        /// if Root/Leaf is not TEdge
        /// </summary>
        /// <param name="edges"></param>
        /// <returns></returns>
        IEnumerable<TItem> Foliage ( IEnumerable<TEdge> edges );
        #endregion

        #region events
        /// <summary>
        /// this is fired "outside" of the graph by the user-interface
        /// </summary>
        Action<IGraph<TItem, TEdge>,TItem> DataChanged { get;set;}

        /// <summary>
        /// fires the DataChanged-event
        /// this is called "outside" of the graph by the user-interface
        /// </summary>
        /// <param name="item"></param>
        void OnDataChanged( TItem item );


        /// <summary>
        /// changes the items data
        /// this is fired "outside" of the graph by calling DoChangeData
        /// </summary>
        Action<IGraph<TItem, TEdge>, TItem, object> ChangeData { get;set;}

        /// <summary>
        /// changes the items data
        /// by calling the ChangeData-event
        /// this is called "outside" of the graph by the user-interface
        /// </summary>
        /// <param name="item"></param>
        /// <param name="data"></param>
        void DoChangeData(TItem item, object data);

        /// <summary>
        /// this is fired "outside" of the graph by the user-interface
        /// </summary>
        Action<IGraph<TItem, TEdge>, TItem, GraphEventType> GraphChanged { get;set;}

        /// <summary>
        /// fires the GraphChange-event
        /// this is called "outside" of the graph by the user-interface
        /// a graph 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="eventType"></param>
        void OnGraphChanged( TItem item, GraphEventType eventType );

        #endregion

        IEnumerable<TItem> Where(System.Linq.Expressions.Expression<Func<TItem, bool>> predicate);

        bool ValidEdge(TEdge edge);

        bool HasSingleEdge (TItem item);
    }
}