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
        /// if item is not an edge or done == null, a <see cref="DeleteCommand{TItem, TEdge}"/> is added/> 
        /// if item is an edge, a <see cref="DeleteEdgeCommand{TItem, TEdge}"/> is added 
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

        public static void ClearSelection<TItem, TEdge> (this IGraphScene<TItem, TEdge> scene, TItem current = default(TItem)) where TEdge : TItem, IEdge<TItem> {
            foreach (var w in scene.Selected.Elements) {
                if (!w.Equals (current))
                    scene.Requests.Add (new StateChangeCommand<TItem> (w,
                                                                     new Pair<UiState> (UiState.Selected, UiState.None)));
            }
            scene.Selected.Clear ();
            if (scene.Focused != null) {
                scene.Selected.Add (scene.Focused);
            }
        }

        public static void SetFocused<TItem, TEdge> (this IGraphScene<TItem, TEdge> scene, TItem focused) where TEdge : TItem, IEdge<TItem> {
            var recent = scene.Focused;
            if (recent != null)
                new StateChangeCommand<TItem> (focused,
                    new Pair<UiState> (UiState.Selected, UiState.None));
                
            scene.Focused = focused;
            if (focused != null)
                scene.Requests.Add (
                    new StateChangeCommand<TItem> (focused,
                        new Pair<UiState> (UiState.None, UiState.Selected))
                    );
        }

        public static void CheckLayout<TItem, TEdge> (this IGraph<TItem, TEdge> graph, IGraphSceneLayout<TItem, TEdge> layout) where TEdge : TItem, IEdge<TItem> {
            if (graph != layout.Data.Graph) {
                throw new ArgumentException ($"{nameof (layout)}.Graph differs from {nameof (graph)}: {graph.GetHashCode ():X8} != {layout.Data.Graph.GetHashCode ():X8}");
            }
        }

        public static void CheckLayout<TItem, TEdge> (this IGraphScene<TItem, TEdge> scene, IGraphSceneLayout<TItem, TEdge> layout) where TEdge : TItem, IEdge<TItem> {
            if (scene != layout.Data) {
                throw new ArgumentException ($"{nameof (layout)}.Data differs from {nameof (scene)}: {scene.GetHashCode ():X8} != {layout.Data.Graph.GetHashCode ():X8}");
            }
        }

        public static IGraphSceneLayout<TItem, TEdge> CloneLayout<TItem, TEdge> (this IGraphScene<TItem, TEdge> scene, IGraphSceneLayout<TItem, TEdge> layout) where TEdge : TItem, IEdge<TItem> {
            IGraphSceneLayout<TItem, TEdge> result = null;
            Func<IGraphScene<TItem, TEdge>> handler = () => scene;
            result = Activator.CreateInstance (layout.GetType (), new object [] { handler, layout.StyleSheet}) as IGraphSceneLayout<TItem, TEdge>;
            return result;
        }
    }
}