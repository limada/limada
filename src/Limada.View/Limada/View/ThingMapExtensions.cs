using System;
using System.Diagnostics;
using System.Linq;
using Limada.IO;
using Limada.Model;
using Limaki.Common;
using Limaki.Contents;
using Limaki.Data;
using Limaki.Graphs;
using Limaki.View.GraphScene;
using Limaki.View.Visuals;
using Limaki.View.Viz.Mapping;
using Limaki.Common.Linqish;
using Limaki.Contents.IO;

namespace Limada.View.VisualThings {

    public static class ThingMapExtensions {

        public static Iori GetIori (IGraph<IThing, ILink> g) {
            var graphContent = new ThingGraphContent {Data = g.UnWrapped () as IThingGraph};
            return Registry.Pooled<ThingGraphIoPool> ()
                .OfType<ThingGraphIo> ()
                .Select (io => io.Use (graphContent))
                .Where (i => i != null)
                .FirstOrDefault ();
        }

		public static ThingGraphContent GetContent(IGraph<IThing, ILink> g)
		{
			var graphContent = new ThingGraphContent { Data = g.UnWrapped() as IThingGraph };
			var iog = Registry.Pooled<ThingGraphIoPool>()
				.OfType<ThingGraphIo>()
				//.Select(io => io.Use(graphContent))
			                   .Where(io => io.Use(graphContent)!=null)
				.FirstOrDefault();
			
			if (iog == null)
				return graphContent;
			
			var iori = iog.Use(graphContent);
			var ct  = iog.Use(iori);
			graphContent.Source = iori;
			graphContent.ContentType = ct.ContentType;
			return graphContent;
		}

        public static IThingGraph OpenGraph (Iori iori) {

            if (iori == null)
                return null;

            var io=  new ThingGraphIoManager ().GetSinkIO (iori,IoMode.Read) as ThingGraphIo;

            var ct = io.Open (iori);
            return ct.Data;

        }

        public static bool HasMappedGraph (this IGraphSceneMapOrganizer<IVisual, IVisualEdge> mapOrganizer) {
            var mapInteractor = mapOrganizer.MapInteractor<IThing, ILink> ();
            return mapInteractor.MappedGraphs.Any ();
        }

        public static IThingGraph MappedGraph (this IGraphSceneMapOrganizer<IVisual, IVisualEdge> mapOrganizer, Iori iori) {

            IThingGraph thingGraph = null;

            var mapInteractor = mapOrganizer.MapInteractor<IThing, ILink> ();

            Func<IThingGraph, IThingGraph> register = s => {
                if (s == null)
                    return null;
                var g = mapInteractor.WrapGraph (s) as IThingGraph;
                mapInteractor.RegisterMappedGraph (g);
                return g;
            };

            if (iori != null) {

                thingGraph = mapInteractor
                    .MappedGraphs
                    .Select (g => new {Iori = GetIori (g), Graph = g})
                    .Where (i => i.Iori != null && i.Iori.ToString () == iori.ToString ())
                    .Select (i => i.Graph as IThingGraph)
                    .FirstOrDefault ();

                if (thingGraph == null) {
                    thingGraph = register (ThingMapExtensions.OpenGraph (iori));
                }
            }

            if (thingGraph == null && iori == null) {
                Trace.WriteLine ("Warning! iori is null!");
                thingGraph = register (new ThingGraph ());
            }
            return thingGraph;
        }

        public static void ClearDisplays (this IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge> organizer) {

            organizer.Displays
                .Where (d => d.Data != null)
                .ToArray ()
                .ForEach (d => {
                    organizer.ClearDisplaysOf (d.Data);
                    organizer.RemoveScene (d.Data);
                });


            organizer.Scenes.ToArray ().ForEach (s => organizer.RemoveScene (s));
        }

        public static void RemoveMappedGraph (this IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge> organizer, IThingGraph backGraph) {

            var mapInteractor = organizer.MapInteractor<IThing, ILink> ();
            organizer.Displays
                .Join (mapInteractor.ScenesOfMappedGraph (backGraph),
                    d => d.Data,
                    s => s, (d, s) => d)
                .ForEach (d => {
                    organizer.ClearDisplaysOf (d.Data);
                    organizer.RemoveScene (d.Data);
                });

            mapInteractor.ScenesOfMappedGraph (backGraph).ToArray ().ForEach (s=>organizer.RemoveScene (s));
            mapInteractor.UnregisterMappedGraph (backGraph);

        }

        public static void ApplyBackGraph (this IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge> organizer, IThingGraph root) {

            var mapInteractor = organizer.MapInteractor<IThing, ILink> ();

            var g = mapInteractor.WrapGraph (root);
            mapInteractor.RegisterMappedGraph (g);

            var displays = organizer.Displays;

            displays.ForEach (d => {
                var scene = mapInteractor.CreateScene (g);
                organizer.AddScene (scene);
                d.Data = scene;
            });
        }
    }
}