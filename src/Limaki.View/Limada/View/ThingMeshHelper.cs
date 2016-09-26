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
using Limaki.View.Viz.Mesh;
using Limaki.Common.Linqish;
using Limaki.Contents.IO;

namespace Limada.View.VisualThings {

    public static class ThingMeshHelper {

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

        public static bool HasBackGraph (this IGraphSceneMesh<IVisual, IVisualEdge> mesh) {
            var backHandler = mesh.BackHandler<IThing, ILink> ();
            return backHandler.BackGraphs.Any ();
        }

        public static IThingGraph BackGraph (this IGraphSceneMesh<IVisual, IVisualEdge> mesh, Iori iori) {

            IThingGraph thingGraph = null;

            var backHandler = mesh.BackHandler<IThing, ILink> ();

            Func<IThingGraph, IThingGraph> register = s => {
                if (s == null)
                    return null;
                var g = backHandler.WrapGraph (s) as IThingGraph;
                backHandler.RegisterBackGraph (g);
                return g;
            };

            if (iori != null) {

                thingGraph = backHandler
                    .BackGraphs
                    .Select (g => new {Iori = GetIori (g), Graph = g})
                    .Where (i => i.Iori != null && i.Iori.ToString () == iori.ToString ())
                    .Select (i => i.Graph as IThingGraph)
                    .FirstOrDefault ();

                if (thingGraph == null) {
                    thingGraph = register (ThingMeshHelper.OpenGraph (iori));
                }
            }

            if (thingGraph == null && iori == null) {
                Trace.WriteLine ("Warning! iori is null!");
                thingGraph = register (new ThingGraph ());
            }
            return thingGraph;
        }

        public static void ClearDisplays (this IGraphSceneDisplayMesh<IVisual, IVisualEdge> mesh) {

            mesh.Displays
                .Where (d => d.Data != null)
                .ToArray ()
                .ForEach (d => {
                    mesh.ClearDisplaysOf (d.Data);
                    mesh.RemoveScene (d.Data);
                });


            mesh.Scenes.ToArray ().ForEach (s => mesh.RemoveScene (s));
        }

        public static void RemoveBackGraph (this IGraphSceneDisplayMesh<IVisual, IVisualEdge> mesh, IThingGraph backGraph) {

            var backMesh = mesh.BackHandler<IThing, ILink> ();
            mesh.Displays
                .Join (backMesh.ScenesOfBackGraph (backGraph),
                    d => d.Data,
                    s => s, (d, s) => d)
                .ForEach (d => {
                    mesh.ClearDisplaysOf (d.Data);
                    mesh.RemoveScene (d.Data);
                });

            backMesh.ScenesOfBackGraph (backGraph).ToArray ().ForEach (s=>mesh.RemoveScene (s));
            backMesh.UnregisterBackGraph (backGraph);

        }

        public static void ApplyBackGraph (this IGraphSceneDisplayMesh<IVisual, IVisualEdge> mesh, IThingGraph root) {

            var backMesh = mesh.BackHandler<IThing, ILink> ();

            var g = backMesh.WrapGraph (root);
            backMesh.RegisterBackGraph (g);

            var displays = mesh.Displays;

            displays.ForEach (d => {
                var scene = backMesh.CreateScene (g);
                mesh.AddScene (scene);
                d.Data = scene;
            });
        }
    }
}