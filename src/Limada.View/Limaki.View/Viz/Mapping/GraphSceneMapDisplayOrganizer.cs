/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Limaki.Common.Linqish;
using Limaki.Graphs;
using Limaki.View.GraphScene;

namespace Limaki.View.Viz.Mapping {

    /// <summary>
    /// a central place to register Displays and Scenes
    /// registered scenes and their mapped graphs 
    /// Graphs are notified of changes
    /// </summary>
    public abstract class GraphSceneMapDisplayOrganizer<TItem, TEdge> : IGraphSceneMapDisplayOrganizer<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {

        public ICollection<IGraphScene<TItem, TEdge>> Scenes { get; } = new HashSet<IGraphScene<TItem, TEdge>>();
        public ICollection<IGraphSceneDisplay<TItem, TEdge>> Displays { get; } = new HashSet<IGraphSceneDisplay<TItem, TEdge>>();

        public void AddScene (IGraphScene<TItem, TEdge> scene) {
            if (scene != null && !Scenes.Contains (scene)) {
                Scenes.Add (scene);
                var graph = scene.Graph;
                RegisterBackGraph (graph);
                graph.GraphChange -= this.VisualGraphChange;
                graph.GraphChange += this.VisualGraphChange;
                graph.ChangeData -= this.VisualGraphChangeData;
                graph.ChangeData += this.VisualGraphChangeData;
            }
        }

        public void RemoveScene (IGraphScene<TItem, TEdge> scene) {
            if (scene != null) {
                var graph = scene.Graph;
                graph.GraphChange -= this.VisualGraphChange;
                graph.ChangeData -= this.VisualGraphChangeData;
                Scenes.Remove (scene);
                // UnregisterBackGraph (graph);

            }
        }

        public void AddDisplay (IGraphSceneDisplay<TItem, TEdge> display) {
            if (display != null) {
                Displays.Add (display);
                AddScene (display.Data);
            }
        }

        public void RemoveDisplay (IGraphSceneDisplay<TItem, TEdge> display) {
            if (display != null) {
                Displays.Remove (display);
                if (display.Data != null &&
                    !Displays.Any (d => d != display && display.Data == d.Data))
                    RemoveScene (display.Data);
            }
        }

        public void Clear () {
            foreach (var display in Displays.ToArray ()) {
                RemoveDisplay (display);
            }
            foreach (var scene in Scenes.ToArray ()) {
                RemoveScene (scene);
            }
            foreach (var backhandler in _mapInteractors.Values.ToArray ()) {
                backhandler.Clear ();
            }
        }

        #region mapped graph handling

        private IDictionary<Tuple<Type, Type>, IGraphSceneDisplayMapInteractor<TItem, TEdge>> _mapInteractors = new Dictionary<Tuple<Type, Type>, IGraphSceneDisplayMapInteractor<TItem, TEdge>>();

		public virtual IGraphSceneDisplayMapInteractor<TItem, TEdge> MapInteractor (IGraph<TItem, TEdge> graph) {
            return MapInteractor (graph.RootSink().GraphPairTypes());
        }

		public virtual IGraphSceneMapInteractor<TItem,TSourceItem, TEdge, TSourceEdge> MapInteractor<TSourceItem, TSourceEdge> () 
			where TSourceEdge:TSourceItem, IEdge<TSourceItem> {
			return MapInteractor ( new Type[] { typeof ( TItem ), typeof ( TSourceItem ), typeof ( TEdge ), typeof ( TSourceEdge ) } )
				as IGraphSceneMapInteractor<TItem, TSourceItem, TEdge, TSourceEdge>;

		}

        protected virtual IGraphSceneDisplayMapInteractor<TItem, TEdge> MapInteractor (Type[] types) {
            if (types != null) {
                var key = Tuple.Create (types[1], types[3]);
                IGraphSceneDisplayMapInteractor<TItem, TEdge> mapInteractor = null;
                if (!_mapInteractors.TryGetValue (key, out mapInteractor)) {
                    mapInteractor = Activator.CreateInstance (typeof (GraphSceneDisplayMapInteractor<,,,>).MakeGenericType (types)) as IGraphSceneDisplayMapInteractor<TItem, TEdge>;
                    _mapInteractors.Add (key, mapInteractor);
                    mapInteractor.Scenes = () => this.Scenes;
                    mapInteractor.Displays = () => this.Displays;
                }

                return mapInteractor;
            }
            return null;
        }

        public TItem LookUp (IGraph<TItem, TEdge> sourceGraph, IGraph<TItem, TEdge> sinkGraph, TItem lookItem) {
            var mapInteractor = MapInteractor (sourceGraph);
            if (mapInteractor != null) {
                return mapInteractor.LookUp (sourceGraph, sinkGraph, lookItem);
            }
            return default (TItem);
        }

        private void RegisterBackGraph (IGraph<TItem, TEdge> graph) {
            var mapInteractor = MapInteractor (graph);
            if (mapInteractor != null)
                mapInteractor.RegisterBackGraph (graph);
        }

        private void UnregisterBackGraph (IGraph<TItem, TEdge> graph) {
            var mapInteractor = MapInteractor (graph);
            if (mapInteractor != null)
                mapInteractor.UnregisterMappedGraph (graph);
        }

        #endregion

        #region VisualGraphEvents

        private void VisualGraphChangeData (IGraph<TItem, TEdge> graph, TItem visual, object data) {
        }

        protected virtual IGraphSceneDisplayMapVisitor<TItem, TEdge> CreateVisitor (IGraph<TItem, TEdge> graph, TItem item) {
            var types = graph.RootSink().GraphPairTypes();
            var mapInteractor = MapInteractor (types);
            return Activator.CreateInstance (typeof (GraphSceneDisplayMapVisitor<,,,>).MakeGenericType (types), this, mapInteractor, graph, item)
                   as IGraphSceneDisplayMapVisitor<TItem, TEdge>;
        }

        [Obsolete]
        protected abstract IGraphSceneDisplayEvents<TItem, TEdge> CreateSceneEvents();

        protected void VisualGraphChange (object sender, GraphChangeArgs<TItem, TEdge> args) {
            var visitor = CreateVisitor (args.Graph, args.Item);
            var sceneEvents = CreateSceneEvents ();
            visitor.GraphChangedVisit (sceneEvents.GraphChanged, sender, args);
        }

        #endregion

        #region compose

        /// <summary>
        /// copies some propterties of sourceDisplay into sinkDisplay
        /// they will have the same SourceGraph
        /// BackColor, ZoomState
        /// sets Action.Enabled of SelectAction and ScrollAction
        /// </summary>
        /// <param name="sourceDisplay"></param>
        /// <param name="sinkDisplay"></param>
        public virtual void CopyDisplayProperties (IGraphSceneDisplay<TItem, TEdge> sourceDisplay, IGraphSceneDisplay<TItem, TEdge> sinkDisplay) {
            sinkDisplay.BackColor = sourceDisplay.BackColor;
            sinkDisplay.ZoomState = sourceDisplay.ZoomState;
            sinkDisplay.SelectAction.Enabled = sourceDisplay.SelectAction.Enabled;
            sinkDisplay.MouseScrollAction.Enabled = sourceDisplay.MouseScrollAction.Enabled;
        }

        public abstract IGraphScene<TItem, TEdge> CreateSinkScene (IGraph<TItem, TEdge> sourceGraph);

        public virtual  IGraph<TItem, TEdge> CreateSinkGraph (IGraph<TItem, TEdge> source) {

            IGraphPair<TItem, TItem, TEdge, TEdge> sourceGraph = source as SubGraph<TItem, TEdge>;

            if (sourceGraph != null) {
                sourceGraph = sourceGraph.RootSource ();

                var mapInteractor = MapInteractor (sourceGraph);

                if (mapInteractor != null) {
                    var result = mapInteractor.CreateSinkGraph (sourceGraph.Source);

                    if (result != null) {
                        // souround with a view
                        var view = Activator.CreateInstance ((result as ISinkGraph<TItem, TEdge>).Sink.GetType ())
                            as IGraph<TItem, TEdge>;
                        return new SubGraph<TItem, TEdge> (result, view);
                    }
                } else {
                    // asuming a graph without backing graph
                    var view = Activator.CreateInstance(sourceGraph.Source.GetType()) as IGraph<TItem, TEdge>;
                    return new SubGraph<TItem, TEdge> (sourceGraph.Source, view);
                }
            }
            return null;
        }

        #endregion
               
        IGraphSceneMapInteractor<TItem, TEdge> IGraphSceneMapOrganizer<TItem, TEdge>.MapInteractor (IGraph<TItem, TEdge> graph) {
           return this.MapInteractor(graph);
        }

        IGraphSceneMapInteractor<TItem, TSourceItem, TEdge, TSourceEdge> IGraphSceneMapOrganizer<TItem, TEdge>.MapInteractor<TSourceItem, TSourceEdge> () {
            return this.MapInteractor<TSourceItem, TSourceEdge> () as IGraphSceneMapInteractor<TItem, TSourceItem, TEdge, TSourceEdge>;
        }
    }

    public static class GraphSceneMapExtensions {
        /// <summary>
        /// clears all Displays where scene's backend is the same
        /// </summary>
        /// <param name="scene"></param>
        public static void 
            ClearDisplaysOf<TItem, TEdge> (this IGraphSceneMapDisplayOrganizer<TItem, TEdge> organizer, IGraphScene<TItem, TEdge> scene) where TEdge : TItem, IEdge<TItem> {
            
            if (scene == null)
                return;

            organizer.DisplaysOfBackGraph (scene.Graph)
                               .ForEach (d => {
                                   if (d.Data != null) {
                                       d.Data.ClearView ();
                                   }
                                   d.Data = null;
                                   d.Perform ();
                               });

        }

        public static IEnumerable<IGraphSceneDisplay<TItem, TEdge>>
            DisplaysOfBackGraph<TItem, TEdge> (this IGraphSceneMapDisplayOrganizer<TItem, TEdge> organizer, IGraph<TItem, TEdge> graph) where TEdge : TItem, IEdge<TItem> {

            return organizer.Displays
                .Join (organizer.ScenesOfBackGraph (graph),
                d => d.Data,
                s => s, (d, s) => d);
        }

        public static IEnumerable<IGraphScene<TItem, TEdge>>
            ScenesOfBackGraph<TItem, TEdge> (this IGraphSceneMapDisplayOrganizer<TItem, TEdge> organizer, IGraph<TItem, TEdge> graph) where TEdge : TItem, IEdge<TItem> {

            var mapInteractor = organizer.MapInteractor (graph);
            if (mapInteractor == null)
                return new IGraphScene<TItem, TEdge>[0];
            return mapInteractor.ScenesOfMappedGraph (graph);
        }
    }
}


