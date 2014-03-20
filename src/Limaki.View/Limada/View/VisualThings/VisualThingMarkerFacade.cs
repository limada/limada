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
 * http://www.limada.org
 * 
 */


using Limada.Model;
using Limada.Schemata;
using Limaki.Graphs;
using System.Linq;
using Limaki.View.Visuals;

namespace Limada.View.VisualThings {

    public class VisualThingMarkerFacade:MarkerFacade<IVisual,IThing, IVisualEdge,ILink> {

        public VisualThingMarkerFacade(IGraph<IVisual, IVisualEdge> graph): base(graph) {}

        public override IEdge<IVisual> CreateDefaultEdge() {
            return Transformer.VisualFactory.CreateEdge (DefaultMarker.ToString());
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


        public override IThing FittingMarker (object data) {

            var marker = base.FittingMarker (data);
            if (marker != null)
                return marker;

            var graph = this.Graph.ThingGraph ();
            if (graph == null)
                return null;

            marker = graph.GetByData (data).FirstOrDefault ();
            if (marker != null && !Markers.Contains (marker))
                Markers.Add (marker);

            return marker;

        }

        public override void SetMarkers(IGraph<IVisual, IVisualEdge> source) {

            this.Markers = new IThing[] {  };
            var graph = source.ThingGraph();
            if (graph != null) {
                SetMarkers(graph.Markers());
                if (!this.Markers.Contains(DefaultMarker) && DefaultMarker != null) {
                    Markers.Add(DefaultMarker);
                }
            }
        }

        public override string[] MarkersAsStrings () {

            if (Transformer == null) return null;
            var graph = this.Graph.ThingGraph ();
            if (graph == null || Markers.Count == 0)
                return null;
            return Markers
                .Select (marker => Transformer.ThingDataToDisplay (graph, marker).ToString ())
                .OrderBy (m => m)
                .ToArray ();

        }

        public virtual VisualThingTransformer Transformer {
            get {
                var graph = this.Graph.Source<IVisual, IVisualEdge, IThing, ILink>() as VisualThingGraph;

                if (graph != null) {
                    return ( graph.Mapper.Transformer as VisualThingTransformer );
                } else {
                    return null;
                }
            }
        }

        public override IThing CreateMarker(object data) {
            if (Transformer != null){

                var marker = Transformer.ThingFactory.CreateItem (data);

                if (!Markers.Contains(marker)) {
                    Markers.Add(marker);
                }

                return marker;
            }
            return null;
        }
        
    }
}