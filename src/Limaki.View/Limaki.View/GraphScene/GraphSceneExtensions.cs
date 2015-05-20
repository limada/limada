using System;
using System.Collections.Generic;
using System.Linq;
using Limaki.Common;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.Viz.Modelling;
using Limaki.Actions;

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
                // prove if already in Requests? but RemoveBoundsCommand is a DeleteCommand too!
                //if (scene.Requests.OfType<DeleteCommand<TItem, TEdge>> ().Any (c => c.Subject.Equals (item)))
                //    return;
                ICommand<TItem> command = null;
                if (item is TEdge && done != null)
                    command = new DeleteEdgeCommand<TItem, TEdge> (item, scene);
                else
                    command = new DeleteCommand<TItem, TEdge> (item, scene);
                scene.Requests.Add (command);
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

                scene.RequestDelete (item, null);
                done.Add (item);
            }
        }

        public static void CreateMarkers<TItem, TEdge> (this IGraphScene<TItem, TEdge> scene) where TEdge : TItem, IEdge<TItem> {
            scene.Markers = Registry.Factory.Create<IMarkerFacade<TItem, TEdge>> (scene.Graph);
        }

    }
}