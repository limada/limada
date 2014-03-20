using System;
using System.Collections.Generic;
using Limaki.Common;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.Viz.Modelling;

namespace Limaki.View.GraphScene {

    public static class GraphSceneExtensions {

        /// <summary>
        /// if item is not an edge or done == null, a <see cref="DeleteCommand{TItem, TEdge}" is added/> 
        /// if item is an edge, a <see cref="DeleteEdgeCommand{TItem, TEdge}" is added/> 
        /// this does NOT delete the edge
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TEdge"></typeparam>
        /// <param name="scene"></param>
        /// <param name="item"></param>
        /// <param name="done"></param>
        public static void RequestDelete<TItem, TEdge> (this IGraphScene<TItem, TEdge> scene, TItem item, ICollection<TItem> done)
            where TEdge : TItem, IEdge<TItem> {
            if (done == null || !done.Contains (item)) {
                if (item is TEdge && done != null)
                    scene.Requests.Add (new DeleteEdgeCommand<TItem, TEdge> (item, scene));
                else
                    scene.Requests.Add (new DeleteCommand<TItem, TEdge> (item, scene));
                if (done != null)
                    done.Add (item);
            };
        }

        public static void Delete<TItem, TEdge> (this IGraphScene<TItem, TEdge> scene, TItem item, ICollection<TItem> done)
            where TEdge : TItem, IEdge<TItem> {

            if (done == null)
                done = new Set<TItem> ();

            Action<TItem> requestDelete = cand => scene.RequestDelete (cand, done);

            if (!done.Contains (item)) {
                foreach (var edge in scene.Graph.PostorderTwig (item)) {
                    requestDelete (edge);
                }

                var dependencies = Registry.Pooled<GraphDepencencies<TItem, TEdge>> ();
                dependencies.VisitItems (
                    GraphCursor.Create (scene.Graph, item),
                    requestDelete,
                    GraphEventType.Remove);

                // if item is an edge, then use DeleteCommand
                scene.RequestDelete (item, null);
                done.Add (item);
            }
        }

        public static void CreateMarkers<TItem, TEdge> (this IGraphScene<TItem, TEdge> scene) where TEdge : TItem, IEdge<TItem> {
            scene.Markers = Registry.Factory.Create<IMarkerFacade<TItem, TEdge>> (scene.Graph);
        }

    }
}