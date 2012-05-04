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

namespace Limaki.Graphs.Extensions {
    public abstract class MarkerFacade<TOne,TTwo, TEdgeOne,TEdgeTwo>:IMarkerFacade<TOne,TEdgeOne> 
    where TEdgeOne:IEdge<TOne>{
        protected IGraph<TOne, TEdgeOne> Graph = null;
        public MarkerFacade(IGraph<TOne, TEdgeOne> graph) {
            this.Graph = graph;
            SetMarkers(graph);
        }

        TTwo _defaultMarker = default(TTwo);
        public virtual TTwo DefaultMarker {
            get { return _defaultMarker; }
            set { _defaultMarker = value; }
        }

        ICollection<TTwo> _markers = null;
        public virtual ICollection<TTwo> Markers {
            get {
                if (_markers == null) {
                    return new EmptyCollection<TTwo>();
                }
                return _markers;
            }
            set { _markers = value; }
        }

        public abstract void SetMarkers(IGraph<TOne, TEdgeOne> graph);

        public virtual void SetMarkers(ICollection<TTwo> sourceMarkers) {
            ICollection<TTwo> markers = new Set<TTwo>();
            foreach (TTwo thing in sourceMarkers) {
                markers.Add(thing);
            }
            this.Markers = markers;
        }

        public virtual TTwo FittingMarker(object data) {
            TTwo result = default(TTwo);
            foreach (TTwo marker in Markers) {
                if (marker.ToString().Equals(data.ToString())) {
                    result = marker;
                    break;
                }
            }
            return result;
        }

        public virtual void ChangeMarker(IEdge<TOne> edge, TTwo marker) {
            if (edge == null) return;
            Graph.OnChangeData ((TOne) edge, marker);
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
            foreach (TOne one in elements) {
                if (one is IEdge<TOne>) {
                    ChangeMarker((IEdge<TOne>)one, marker);
                }
            }
        }


        #region Markers as text
        public virtual string[] MarkersAsStrings() {
            int count = Markers.Count;
            if (count != 0) {
                string[] result = new string[Markers.Count];
                int i = 0;
                foreach (TTwo marker in Markers) {
                    result[i] = marker.ToString();
                    i++;
                }
                Array.Sort<string>(result);
                return result;
            } else {
                return null;
            }
        }


        public virtual void ChangeMarkers(IEnumerable<TOne> elements, object data) {
            TTwo marker = FittingMarker (data);
            ChangeMarkers(elements, marker);
        }

        public virtual void ChangeAndAddMarker(IEdge<TOne> edge, object data) {
            TTwo marker = FittingMarker(data);
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