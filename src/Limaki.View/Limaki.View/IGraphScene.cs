/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2012 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Xwt;

namespace Limaki.View {
    /// <summary>
    /// the spatial environment
    /// of a graph
    /// assumes that the spatial represenation
    /// is a shape
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    public interface IGraphScene<TItem,TEdge> 
    where TEdge:TItem, IEdge<TItem> {
        
        IGraph<TItem, TEdge> Graph { get; set; }
        int Count { get; }

        /// <summary>
        /// Requests
        /// the commands to be performed by a Receiver
        /// </summary>
        ICollection<ICommand<TItem>> Requests {get;set;}

        TItem Focused { get; set; }
        TItem Hovered { get; set; }
        IComposite<TItem> Selected { get; set; }
        Action<IGraphScene<TItem, TEdge>, TItem> FocusChanged { get; set; }
        
        IShape Shape { get; }
        
        ISpatialZIndex<TItem> SpatialIndex { get; }
        void ClearSpatialIndex();

        IEnumerable<TItem> Elements {get;}
        IEnumerable<TItem> ElementsIn(Rectangle clipBounds);
        IEnumerable<TItem> ElementsIn(Rectangle clipBounds, ZOrder order);

        State State { get; }

        void Add(TItem visual);
        bool Remove(TItem visual);
        bool ChangeEdge(TEdge edge, TItem target, bool asRoot);
        bool Contains(TItem visual);

        void RemoveBounds(TItem visual);
        void AddBounds(TItem visual);
        void UpdateBounds(TItem visual, Rectangle invalid);

        TItem Hit ( Point p, int hitSize );
        TItem HitBorder ( Point p, int hitSize );
        Point NoHit { get; }
        IShape ItemShape ( TItem item );

        void Clear();
        void ClearView();

        IMarkerFacade<TItem, TEdge> Markers { get; set; }
        IEnumerable<TEdge> Twig(TItem visual);

        
    }

	public class SceneInfo {

        public Int64 Id { get; set; }
        public string Name { get; set; } = "";
        private State _state;
        public State State { get { return _state ?? (_state = new State { Hollow = true }); } }

        public static SceneInfo FromInfo(SceneInfo other) {
    }

    public interface ISpatialZIndex<TItem> : ISpatialIndex<TItem> {
        IEnumerable<TItem> Query (Rectangle clipBounds, ZOrder zOrder);
    }

    public enum ZOrder {
        NodesFirst,
        EdgesFirst
    }
}