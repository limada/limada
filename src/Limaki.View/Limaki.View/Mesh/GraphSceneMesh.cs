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

using System.Collections.Generic;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.Visualizers;
using System;
using Limaki.Graphs.Extensions;

namespace Limaki.View.Mesh {

    /// <summary>
    /// a central place to register Displays and Scenes
    /// registered scenes and their backing 
    /// Graphs are notified of changes
    /// </summary>
    public abstract class GraphSceneMesh<TItem, TEdge> : IGraphSceneMesh<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {

        private ICollection<IGraphScene<TItem, TEdge>> _scenes = new HashSet<IGraphScene<TItem, TEdge>>();
        public ICollection<IGraphScene<TItem, TEdge>> Scenes { get { return _scenes; } }

        private ICollection<IGraphSceneDisplay<TItem, TEdge>> _displays = new HashSet<IGraphSceneDisplay<TItem, TEdge>>();
        public ICollection<IGraphSceneDisplay<TItem, TEdge>> Displays { get { return _displays; } }


        public void AddScene (IGraphScene<TItem, TEdge> scene) {
            if (scene != null) {
                Scenes.Add (scene);
                var graph = scene.Graph;
                RegisterBackGraph (graph);
                graph.GraphChanged -= this.VisualGraphChanged;
                graph.GraphChanged += this.VisualGraphChanged;
                graph.ChangeData -= this.VisualGraphChangeData;
                graph.ChangeData += this.VisualGraphChangeData;
                graph.DataChanged -= this.VisualGraphDataChanged;
                graph.DataChanged += this.VisualGraphDataChanged;
            }
        }


        public void RemoveScene (IGraphScene<TItem, TEdge> scene) {
            if (scene != null) {
                var graph = scene.Graph;
                graph.GraphChanged -= this.VisualGraphChanged;
                graph.ChangeData -= this.VisualGraphChangeData;
                graph.DataChanged -= this.VisualGraphDataChanged;
                Scenes.Remove (scene);
                UnregisterBackGraph (graph);

            }
        }

        public void AddDisplay (IGraphSceneDisplay<TItem, TEdge> display) {
            if (display != null) {
                Displays.Add (display);
            }
        }

        public void RemoveDisplay (IGraphSceneDisplay<TItem, TEdge> display) {
            if (display != null) {
                Displays.Remove (display);
            }
        }


        #region BackGraph handling

        private IDictionary<Tuple<Type, Type>, IGraphSceneMeshBackHandler<TItem, TEdge>> _backHandlers = new Dictionary<Tuple<Type, Type>, IGraphSceneMeshBackHandler<TItem, TEdge>>();

        protected virtual IGraphSceneMeshBackHandler<TItem, TEdge> BackHandler (IGraph<TItem, TEdge> graph) {
            return BackHandler (graph.RootSink().GraphPairTypes());
        }

        protected virtual IGraphSceneMeshBackHandler<TItem, TEdge> BackHandler (Type[] types) {
            if (types != null) {
                var key = Tuple.Create (types[1], types[3]);
                IGraphSceneMeshBackHandler<TItem, TEdge> backHandler = null;
                if (!_backHandlers.TryGetValue (key, out backHandler)) {
                    backHandler = Activator.CreateInstance (typeof (GraphSceneMeshBackHandler<,,,>).MakeGenericType (types)) as IGraphSceneMeshBackHandler<TItem, TEdge>;
                    _backHandlers.Add (key, backHandler);
                    backHandler.Scenes = () => this.Scenes;
                }

                return backHandler;
            }
            return null;
        }

        private void RegisterBackGraph (IGraph<TItem, TEdge> graph) {
            var backMesh = BackHandler (graph);
            backMesh.RegisterBackGraph (graph);
        }

        private void UnregisterBackGraph (IGraph<TItem, TEdge> graph) {
            var backMesh = BackHandler (graph);
            backMesh.UnregisterBackGraph (graph);
        }

        #endregion

        #region VisualGraphEvents

        private void VisualGraphChangeData (IGraph<TItem, TEdge> graph, TItem visual, object data) {
        }

        protected virtual IGraphSceneMeshVisitor<TItem, TEdge> CreateVisitor (IGraph<TItem, TEdge> graph, TItem item) {
            var types = graph.RootSink().GraphPairTypes();
            var backMesh = BackHandler (types);
            return Activator.CreateInstance (typeof (GraphSceneMeshVisitor<,,,>).MakeGenericType (types), this, backMesh, graph, item)
                   as IGraphSceneMeshVisitor<TItem, TEdge>;
        }

        protected abstract IGraphSceneEvents<TItem, TEdge> CreateSceneEvents();

        protected virtual void VisualGraphDataChanged (IGraph<TItem, TEdge> graph, TItem visual) {
            var visitor = CreateVisitor (graph, visual);
            var sceneEvents = CreateSceneEvents ();
            visitor.ChangeDataVisit (sceneEvents.GraphDataChanged);
        }

        protected void VisualGraphChanged (IGraph<TItem, TEdge> graph, TItem visual, GraphEventType eventType) {
            var visitor = CreateVisitor (graph, visual);
            var sceneEvents = CreateSceneEvents ();
            visitor.GraphChangedVisit (sceneEvents.GraphChanged, eventType);
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
        public abstract IGraph<TItem, TEdge> CreateTargetGraph (IGraph<TItem, TEdge> source);



        #endregion
    }
}


