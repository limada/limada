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
 * http://limada.sourceforge.net
 * 
 */


using System;
using Limada.Model;
using Limada.Schemata;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Common;
using Limaki.Visuals;

namespace Limaki.Limada.View {
    public class VisualThingMarkerFacade:MarkerFacade<IVisual,IThing, IVisualEdge,ILink> {
        public VisualThingMarkerFacade(IGraph<IVisual, IVisualEdge> graph): base(graph) {}

        IVisualFactory _factory = null;
        IVisualFactory factory {
            get {
                if (_factory == null) {
                    _factory = Registry.Factory.Create<IVisualFactory> ();
                }
                return _factory;
            }
        }

        public override IEdge<IVisual> CreateDefaultEdge() {
            return factory.CreateEdge (DefaultMarker.ToString ());
        }

        public override IThing DefaultMarker {
            get {
                if (base.DefaultMarker == null) {
                    base.DefaultMarker = CommonSchema.EmptyMarker;
                }
                return base.DefaultMarker;
            }
            set { base.DefaultMarker = value; }
        }


        public override IThing FittingMarker(object data) {
            IThing marker = base.FittingMarker(data);
            if (marker == null) {
                 IThingGraph graph = GetThingGraph(this.Graph);
                 if (graph != null) {
                     foreach (IThing thing in graph.GetByData (data)) {
                         if (!Markers.Contains(thing)) {
                             Markers.Add(thing);
                         }
                         return thing;
                     }
                 }
            }
            return marker;

        }

        IThingGraph GetThingGraph(IGraph<IVisual, IVisualEdge> source) {
            var graph = source.Source<IVisual, IVisualEdge, IThing, ILink>() as VisualThingGraph;

            if (graph != null) {
                return graph.Two as IThingGraph;
            }
            return null;
        }

        public override void SetMarkers(IGraph<IVisual, IVisualEdge> source) {

            this.Markers = new IThing[] {  };
            IThingGraph graph = GetThingGraph(source);
            if (graph != null) {
                SetMarkers(graph.Markers());
                if (!this.Markers.Contains(DefaultMarker) && DefaultMarker != null) {
                    Markers.Add(DefaultMarker);
                }
            }
        }

        public override string[] MarkersAsStrings() {
            
            if (adapter == null) return null;
            IThingGraph graph = GetThingGraph (this.Graph);
            int count = Markers.Count;
            if (count != 0) {
                string[] result = new string[Markers.Count];
                int i = 0;
                foreach (IThing marker in Markers) {
                    result[i] = adapter.ThingDataToDisplay(graph,marker).ToString();
                    i++;
                }
                Array.Sort<string>(result);
                return result;
            } else {
                return null;
            }
        }

        public virtual VisualThingAdapter adapter {
            get {
                var graph = this.Graph.Source<IVisual, IVisualEdge, IThing, ILink>() as VisualThingGraph;

                if (graph != null) {
                    return ( graph.Mapper.Adapter as VisualThingAdapter );
                } else {
                    return null;
                }
            }
        }
        public override IThing CreateMarker(object data) {
            if (adapter != null){

                IThing marker = adapter.ThingFactory.CreateItem (data);

                if (!Markers.Contains(marker)) {
                    Markers.Add(marker);
                }

                return marker;
            }
            return null;
        }
        
    }
}