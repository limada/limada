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
using Limada.Model;
using Limaki.View.Visualizers;
using Limaki.Common;
using System;
using System.Linq;
using Limaki.Graphs.Extensions;


namespace Limaki.View.Visuals.UI {

    /// <summary>
    /// a central place to register GraphChange Events
    /// </summary>
    /// 
    public abstract class GraphSceneMesh<IVisual, IVisualEdge> : IGraphSceneMesh<IVisual, IVisualEdge> where IVisualEdge : IEdge<IVisual>, IVisual {

        private ICollection<IGraphScene<IVisual, IVisualEdge>> _scenes = new HashSet<IGraphScene<IVisual, IVisualEdge>>();
        public ICollection<IGraphScene<IVisual, IVisualEdge>> Scenes { get { return _scenes; } }

        private ICollection<IGraphSceneDisplay<IVisual, IVisualEdge>> _displays = new HashSet<IGraphSceneDisplay<IVisual, IVisualEdge>>();
        public ICollection<IGraphSceneDisplay<IVisual, IVisualEdge>> Displays { get { return _displays; } }


        public void AddScene (IGraphScene<IVisual, IVisualEdge> scene) {
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


        public void RemoveScene (IGraphScene<IVisual, IVisualEdge> scene) {
            if (scene != null) {
                var graph = scene.Graph;
                graph.GraphChanged -= this.VisualGraphChanged;
                graph.ChangeData -= this.VisualGraphChangeData;
                graph.DataChanged -= this.VisualGraphDataChanged;
                Scenes.Remove (scene);
                UnregisterBackGraph (graph);

            }
        }

        public void AddDisplay (IGraphSceneDisplay<IVisual, IVisualEdge> display) {
            if (display != null) {
                Displays.Add (display);
            }
        }

        public void RemoveDisplay (IGraphSceneDisplay<IVisual, IVisualEdge> display) {
            if (display != null) {
                Displays.Remove (display);
            }
        }


        #region BackGraph handling

        private IDictionary<Tuple<Type, Type>, IGraphSceneMeshBackHandler<IVisual, IVisualEdge>> _backHandlers = new Dictionary<Tuple<Type, Type>, IGraphSceneMeshBackHandler<IVisual, IVisualEdge>>();

        protected virtual IGraphSceneMeshBackHandler<IVisual, IVisualEdge> BackHandler (IGraph<IVisual, IVisualEdge> graph) {
            return BackHandler (graph.RootSink().GraphPairTypes());
        }

        protected virtual IGraphSceneMeshBackHandler<IVisual, IVisualEdge> BackHandler (Type[] types) {
            if (types != null) {
                var key = Tuple.Create (types[1], types[3]);
                IGraphSceneMeshBackHandler<IVisual, IVisualEdge> backHandler = null;
                if (!_backHandlers.TryGetValue (key, out backHandler)) {
                    backHandler = Activator.CreateInstance (typeof (GraphSceneMeshBackHandler<,,,>).MakeGenericType (types)) as IGraphSceneMeshBackHandler<IVisual, IVisualEdge>;
                    _backHandlers.Add (key, backHandler);
                    backHandler.Scenes = () => this.Scenes;
                }

                return backHandler;
            }
            return null;
        }

        private void RegisterBackGraph (IGraph<IVisual, IVisualEdge> graph) {
            var backMesh = BackHandler (graph);
            backMesh.RegisterBackGraph (graph);
        }

        private void UnregisterBackGraph (IGraph<IVisual, IVisualEdge> graph) {
            var backMesh = BackHandler (graph);
            backMesh.UnregisterBackGraph (graph);
        }

        #endregion

        #region VisualGraphEvents

        private void VisualGraphChangeData (IGraph<IVisual, IVisualEdge> graph, IVisual visual, object data) {
        }

        protected virtual IGraphSceneMeshVisitor<IVisual, IVisualEdge> CreateVisitor (IGraph<IVisual, IVisualEdge> graph, IVisual item) {
            var types = graph.RootSink().GraphPairTypes();
            var backMesh = BackHandler (types);
            return Activator.CreateInstance (typeof (GraphSceneMeshVisitor<,,,>).MakeGenericType (types), this, backMesh, graph, item)
                   as IGraphSceneMeshVisitor<IVisual, IVisualEdge>;
        }

        protected abstract IGraphSceneEvents<IVisual, IVisualEdge> CreateSceneEvents();

        protected virtual void VisualGraphDataChanged (IGraph<IVisual, IVisualEdge> graph, IVisual visual) {
            var visitor = CreateVisitor (graph, visual);
            var sceneEvents = CreateSceneEvents ();
            visitor.ChangeDataVisit (sceneEvents.GraphDataChanged);
        }

        protected void VisualGraphChanged (IGraph<IVisual, IVisualEdge> graph, IVisual visual, GraphEventType eventType) {
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
        public virtual void CopyDisplayProperties (IGraphSceneDisplay<IVisual, IVisualEdge> sourceDisplay, IGraphSceneDisplay<IVisual, IVisualEdge> targetDisplay) {
            targetDisplay.BackColor = sourceDisplay.BackColor;
            targetDisplay.ZoomState = sourceDisplay.ZoomState;
            targetDisplay.SelectAction.Enabled = sourceDisplay.SelectAction.Enabled;
            targetDisplay.MouseScrollAction.Enabled = sourceDisplay.MouseScrollAction.Enabled;
        }

        public abstract IGraphScene<IVisual, IVisualEdge> CreateTargetScene (IGraph<IVisual, IVisualEdge> sourceGraph);
        public abstract IGraph<IVisual, IVisualEdge> CreateTargetGraph (IGraph<IVisual, IVisualEdge> source);



        #endregion
    }
}


