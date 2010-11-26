/*
 * Limada 
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


using System;
using Limada.Model;
using Limada.Schemata;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Widgets;

namespace Limada.View {
    public class WidgetThingMarkerFacade:MarkerFacade<IWidget,IThing, IEdgeWidget,ILink> {
        public WidgetThingMarkerFacade(IGraph<IWidget, IEdgeWidget> graph): base(graph) {}

        public override IEdge<IWidget> CreateDefaultEdge() {
            return new EdgeWidget<string> (DefaultMarker.ToString ());
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

        IThingGraph GetThingGraph(IGraph<IWidget, IEdgeWidget> source) {
            var graph = new GraphPairFacade<IWidget, IEdgeWidget>()
                .Source<IThing, ILink>(source) as WidgetThingGraph;

            if (graph != null) {
                return graph.Two as IThingGraph;
            }
            return null;
        }

        public override void SetMarkers(IGraph<IWidget, IEdgeWidget> source) {

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

        public virtual WidgetThingAdapter adapter {
            get {
                WidgetThingGraph graph = new GraphPairFacade<IWidget, IEdgeWidget> ()
                    .Source<IThing, ILink> (this.Graph) as WidgetThingGraph;

                if (graph != null) {
                    return ( graph.Mapper.Adapter as WidgetThingAdapter );
                } else {
                    return null;
                }
            }
        }
        public override IThing CreateMarker(object data) {
            if (adapter != null){

                IThing marker = adapter.ThingFactory.CreateThing (data);

                if (!Markers.Contains(marker)) {
                    Markers.Add(marker);
                }

                return marker;
            }
            return null;
        }
        
    }
}