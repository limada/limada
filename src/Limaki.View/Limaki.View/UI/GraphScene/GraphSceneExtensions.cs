using System;
using System.Collections.Generic;
using Limaki.Common;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.UI.GraphScene;

namespace Limaki.View.UI.GraphScene {

    public static class GraphSceneExtensions {

        public static void RequestDelete<TItem, TEdge> (this IGraphScene<TItem, TEdge> scene, TItem delete, ICollection<TItem> done)
        where TEdge : TItem, IEdge<TItem> {
            if (done == null || !done.Contains (delete)) {
                if (delete is TEdge)
                    scene.Requests.Add (new DeleteEdgeCommand<TItem, TEdge> (delete, scene));
                else
                    scene.Requests.Add (new DeleteCommand<TItem, TEdge> (delete, scene));
                if (done != null)
                    done.Add (delete);
            };
        }

        public static void Delete<TItem, TEdge> (this IGraphScene<TItem,TEdge> scene, TItem item, ICollection<TItem> done) 
            where TEdge:TItem,IEdge<TItem> {

            Action<TItem> requestDelete = cand => scene.RequestDelete (cand, done);

            if (!done.Contains (item)) {
                foreach (var edge in scene.Graph.PostorderTwig (item)) {
                    requestDelete (edge);
                }

                var dependencies = Registry.Pool.TryGetCreate<GraphDepencencies<TItem, TEdge>> ();
                dependencies.VisitItems (
                    GraphCursor.Create (scene.Graph, item),
                    requestDelete,
                    GraphEventType.Remove);

                requestDelete (item);
            }
        }

    }
}