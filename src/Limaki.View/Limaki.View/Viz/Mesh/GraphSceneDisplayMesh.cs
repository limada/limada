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

namespace Limaki.View.Viz.Mesh {

    /// <summary>
    /// a central place to register Displays and Scenes
    /// registered scenes and their backing 
    /// Graphs are notified of changes
    /// </summary>
    public abstract class GraphSceneDisplayMesh<TItem, TEdge> : IGraphSceneDisplayMesh<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {

        private ICollection<IGraphScene<TItem, TEdge>> _scenes = new HashSet<IGraphScene<TItem, TEdge>>();
        public ICollection<IGraphScene<TItem, TEdge>> Scenes { get { return _scenes; } }

        private ICollection<IGraphSceneDisplay<TItem, TEdge>> _displays = new HashSet<IGraphSceneDisplay<TItem, TEdge>>();
        public ICollection<IGraphSceneDisplay<TItem, TEdge>> Displays { get { return _displays; } }

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
            foreach (var backhandler in _backHandlers.Values.ToArray ()) {
                backhandler.Clear ();
            }
        }

        #region BackGraph handling

        private IDictionary<Tuple<Type, Type>, IGraphSceneDisplayMeshBackHandler<TItem, TEdge>> _backHandlers = new Dictionary<Tuple<Type, Type>, IGraphSceneDisplayMeshBackHandler<TItem, TEdge>>();

		public virtual IGraphSceneDisplayMeshBackHandler<TItem, TEdge> BackHandler (IGraph<TItem, TEdge> graph) {
            return BackHandler (graph.RootSink().GraphPairTypes());
        }

		public virtual IGraphSceneMeshBackHandler<TItem,TSourceItem, TEdge, TSourceEdge> BackHandler<TSourceItem, TSourceEdge> () 
			where TSourceEdge:TSourceItem, IEdge<TSourceItem> {
			return BackHandler ( new Type[] { typeof ( TItem ), typeof ( TSourceItem ), typeof ( TEdge ), typeof ( TSourceEdge ) } )
				as IGraphSceneMeshBackHandler<TItem, TSourceItem, TEdge, TSourceEdge>;

		}

        //IGraphSceneMeshBackHandler<TItem, TEdge> BackHandler (IGraph<TItem, TEdge> graph);
        //IGraphSceneMeshBackHandler<TItem, TEdge> BackHandler<TSourceItem, TSourceEdge> ();

        protected virtual IGraphSceneDisplayMeshBackHandler<TItem, TEdge> BackHandler (Type[] types) {
            if (types != null) {
                var key = Tuple.Create (types[1], types[3]);
                IGraphSceneDisplayMeshBackHandler<TItem, TEdge> backHandler = null;
                if (!_backHandlers.TryGetValue (key, out backHandler)) {
                    backHandler = Activator.CreateInstance (typeof (GraphSceneDisplayMeshBackHandler<,,,>).MakeGenericType (types)) as IGraphSceneDisplayMeshBackHandler<TItem, TEdge>;
                    _backHandlers.Add (key, backHandler);
                    backHandler.Scenes = () => this.Scenes;
                    backHandler.Displays = () => this.Displays;
                }

                return backHandler;
            }
            return null;
        }

        public TItem LookUp (IGraph<TItem, TEdge> sourceGraph, IGraph<TItem, TEdge> sinkGraph, TItem lookItem) {
            var backhandler = BackHandler (sourceGraph);
            if (backhandler != null) {
                return backhandler.LookUp (sourceGraph, sinkGraph, lookItem);
            }
            return default (TItem);
        }

        private void RegisterBackGraph (IGraph<TItem, TEdge> graph) {
            var backMesh = BackHandler (graph);
            if (backMesh != null)
                backMesh.RegisterBackGraph (graph);
        }

        private void UnregisterBackGraph (IGraph<TItem, TEdge> graph) {
            var backMesh = BackHandler (graph);
            if (backMesh != null)
                backMesh.UnregisterBackGraph (graph);
        }

        #endregion

        #region VisualGraphEvents

        private void VisualGraphChangeData (IGraph<TItem, TEdge> graph, TItem visual, object data) {
        }

        protected virtual IGraphSceneDisplayMeshVisitor<TItem, TEdge> CreateVisitor (IGraph<TItem, TEdge> graph, TItem item) {
            var types = graph.RootSink().GraphPairTypes();
            var backMesh = BackHandler (types);
            return Activator.CreateInstance (typeof (GraphSceneDisplayMeshVisitor<,,,>).MakeGenericType (types), this, backMesh, graph, item)
                   as IGraphSceneDisplayMeshVisitor<TItem, TEdge>;
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
        /// copies some propterties of sourceDisplay into targetDisplay
        /// they will have the same SourceGraph
        /// BackColor, ZoomState
        /// sets Action.Enabled of SelectAction and ScrollAction
        /// </summary>
        /// <param name="sourceDisplay"></param>
        /// <param name="targetDisplay"></param>
        public virtual void CopyDisplayProperties (IGraphSceneDisplay<TItem, TEdge> sourceDisplay, IGraphSceneDisplay<TItem, TEdge> targetDisplay) {
            targetDisplay.BackColor = sourceDisplay.BackColor;
            targetDisplay.ZoomState = sourceDisplay.ZoomState;
            targetDisplay.SelectAction.Enabled = sourceDisplay.SelectAction.Enabled;
            targetDisplay.MouseScrollAction.Enabled = sourceDisplay.MouseScrollAction.Enabled;
        }

        public abstract IGraphScene<TItem, TEdge> CreateSinkScene (IGraph<TItem, TEdge> sourceGraph);

        public virtual  IGraph<TItem, TEdge> CreateSinkGraph (IGraph<TItem, TEdge> source) {

            IGraphPair<TItem, TItem, TEdge, TEdge> sourceGraph = source as SubGraph<TItem, TEdge>;

            if (sourceGraph != null) {
                sourceGraph = sourceGraph.RootSource ();

                var result = BackHandler (sourceGraph).CreateSinkGraph (sourceGraph.Source);

                if (result != null) {
                    // souround with a view
                    var view = Activator.CreateInstance ((result as ISinkGraph<TItem, TEdge>).Sink.GetType ())
                        as IGraph<TItem,TEdge>;
                    return new SubGraph<TItem, TEdge> (result, view);
                }
            }
            return null;
        }

        #endregion
               
        IGraphSceneMeshBackHandler<TItem, TEdge> IGraphSceneMesh<TItem, TEdge>.BackHandler (IGraph<TItem, TEdge> graph) {
           return this.BackHandler(graph);
        }

        IGraphSceneMeshBackHandler<TItem, TSourceItem, TEdge, TSourceEdge> IGraphSceneMesh<TItem, TEdge>.BackHandler<TSourceItem, TSourceEdge> () {
            return this.BackHandler<TSourceItem, TSourceEdge> () as IGraphSceneMeshBackHandler<TItem, TSourceItem, TEdge, TSourceEdge>;
        }
    }

    public static class GraphSceneMeshExtensions {
        /// <summary>
        /// clears all Displays where scene's backend is the same
        /// </summary>
        /// <param name="scene"></param>
        public static void 
            ClearDisplaysOf<TItem, TEdge> (this IGraphSceneDisplayMesh<TItem, TEdge> mesh, IGraphScene<TItem, TEdge> scene) where TEdge : TItem, IEdge<TItem> {
            
            if (scene == null)
                return;

            mesh.DisplaysOfBackGraph (scene.Graph)
                               .ForEach (d => {
                                   if (d.Data != null) {
                                       d.Data.ClearView ();
                                   }
                                   d.Data = null;
                                   d.Perform ();
                               });

        }

        public static IEnumerable<IGraphSceneDisplay<TItem, TEdge>>
            DisplaysOfBackGraph<TItem, TEdge> (this IGraphSceneDisplayMesh<TItem, TEdge> mesh, IGraph<TItem, TEdge> graph) where TEdge : TItem, IEdge<TItem> {

            return mesh.Displays
                .Join (mesh.ScenesOfBackGraph (graph),
                d => d.Data,
                s => s, (d, s) => d);
        }

        public static IEnumerable<IGraphScene<TItem, TEdge>>
            ScenesOfBackGraph<TItem, TEdge> (this IGraphSceneDisplayMesh<TItem, TEdge> mesh, IGraph<TItem, TEdge> graph) where TEdge : TItem, IEdge<TItem> {

            var backMesh = mesh.BackHandler (graph);
            if (backMesh == null)
                return new IGraphScene<TItem, TEdge>[0];
            return backMesh.ScenesOfBackGraph (graph);
        }
    }
}


