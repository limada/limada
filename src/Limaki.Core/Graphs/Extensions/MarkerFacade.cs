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
using Limaki.Common.Collections;
using Limaki.Graphs;
using System.Linq;
using Limaki.Common.Linqish;

namespace Limaki.Graphs.Extensions {

    public abstract class MarkerFacade<TOne,TTwo, TEdgeOne,TEdgeTwo>:IMarkerFacade<TOne,TEdgeOne> 
    where TEdgeOne:IEdge<TOne>{
        protected IGraph<TOne, TEdgeOne> Graph = null;
        public MarkerFacade(IGraph<TOne, TEdgeOne> graph) {
            this.Graph = graph;
            SetMarkers(graph);
        }

        public virtual TTwo DefaultMarker { get; set; }

        ICollection<TTwo> _markers = null;
        public virtual ICollection<TTwo> Markers {
            get { return _markers ?? (_markers = new EmptyCollection<TTwo> ()); }
            set { _markers = value; }
        }

        public abstract void SetMarkers(IGraph<TOne, TEdgeOne> graph);

        public virtual void SetMarkers(ICollection<TTwo> sourceMarkers) {
            this.Markers = new Set<TTwo> (sourceMarkers);
        }

        public virtual TTwo FittingMarker(object data) {
            return Markers.Where (marker => marker.ToString().Equals (data.ToString())).FirstOrDefault();
        }

        public virtual void ChangeMarker(IEdge<TOne> edge, TTwo marker) {
            if (edge == null) return;
            Graph.DoChangeData ((TOne) edge, marker);
            Graph.OnDataChanged((TOne)edge);
        }

        public abstract TTwo CreateMarker ( object data );
        public abstract IEdge<TOne> CreateDefaultEdge();

        public virtual void ChangeAndAddMarker(IEdge<TOne> edge, TTwo marker) {
            ChangeMarker(edge, marker);
            if (! Markers.Contains(marker)) {
                Markers.Add (marker);
            }
        }

        public virtual void ChangeMarkers(IEnumerable<TOne> elements, TTwo marker) {
            if (marker == null)
                return;
            this.DefaultMarker = marker;
            elements.OfType<IEdge<TOne>>().ForEach (one => ChangeMarker ((IEdge<TOne>) one, marker));
        }


        #region Markers as text

        public virtual string[] MarkersAsStrings() {
            if (Markers.Count == 0)
                return null;
            return Markers
                .Select (marker => marker.ToString())
                .OrderBy (s => s)
                .ToArray();
        }


        public virtual void ChangeMarkers(IEnumerable<TOne> elements, object data) {
            var marker = FittingMarker (data);
            ChangeMarkers(elements, marker);
        }

        public virtual void ChangeAndAddMarker(IEdge<TOne> edge, object data) {
            var marker = FittingMarker(data);
            ChangeAndAddMarker(edge, marker);
        }

        
        #endregion

        object IMarkerFacade<TOne, TEdgeOne>.FittingMarker(object data) {
            return FittingMarker(data);
        }

        object IMarkerFacade<TOne, TEdgeOne>.CreateMarker(object data) {
            return CreateMarker(data);
        }

        object IMarkerFacade<TOne, TEdgeOne>.DefaultMarker {
            get { return this.DefaultMarker; }
            set {
                if (value is TTwo) {
                    this.DefaultMarker = (TTwo)value;
                }
            }
        }
    }
}