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
using Limaki.Graphs.Extensions;
using Limaki.Visuals;
using Limada.Model;
using System.Linq;
using Limaki.View.Visualizers;
using Limaki.Common;

namespace Limaki.View.Visuals.UI {


    /// <summary>
    /// a central place to register GraphChange Events
    /// 
    /// <remarks>
    /// current implementation works only on ThingGraph-backed scenes
    /// eg. test-examples are NOT working as they are GraphEntity-backed
    /// </remarks>
    /// </summary>
    public class VisualThingSceneMesh : IGraphSceneMesh<IVisual, IThing, IVisualEdge, ILink> {

        ICollection<IGraphScene<IVisual, IVisualEdge>> _scenes = new HashSet<IGraphScene<IVisual, IVisualEdge>> ();
        public ICollection<IGraphScene<IVisual, IVisualEdge>> Scenes { get { return _scenes; } }

        ICollection<IGraphSceneDisplay<IVisual, IVisualEdge>> _displays = new HashSet<IGraphSceneDisplay<IVisual, IVisualEdge>> ();
        public ICollection<IGraphSceneDisplay<IVisual, IVisualEdge>> Displays { get { return _displays; } }

        ICollection<IGraph<IThing, ILink>> _backGraphs = new HashSet<IGraph<IThing, ILink>> ();
        public ICollection<IGraph<IThing, ILink>> BackGraphs { get { return _backGraphs; } }

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

        protected void RegisterBackGraph (IGraph<IVisual, IVisualEdge> graph) {
            var source = graph.Source<IVisual, IVisualEdge, IThing, ILink> ();
            if (source != null) {
                var root = source.Source;
                if (!BackGraphs.Contains (root)) {
                    BackGraphs.Add (root);
                    root.GraphChanged -= this.BackGraphChanged;
                    root.GraphChanged += this.BackGraphChanged;
                    root.ChangeData -= this.BackGraphChangeData;
                    root.ChangeData += this.BackGraphChangeData;
                    root.DataChanged -= this.BackGraphDataChanged;
                    root.DataChanged += this.BackGraphDataChanged;
                }
            }
        }

        protected void UnregisterBackGraph (IGraph<IVisual, IVisualEdge> graph) {
            var root = BackGraphOf (graph);
            if (BackGraphs.Contains (root) && !ScenesOfBackGraph (root).Any ()) {
                root.GraphChanged -= this.BackGraphChanged;
                root.ChangeData -= this.BackGraphChangeData;
                root.DataChanged -= this.BackGraphDataChanged;
                BackGraphs.Remove (root);

            }
        }

        protected IGraph<IThing, ILink> BackGraphOf (IGraph<IVisual, IVisualEdge> graph) {
            var source = graph.Source<IVisual, IVisualEdge, IThing, ILink> ();
            if (source != null)
                return source.Source;
            return null;
        }

        public IEnumerable<IGraphScene<IVisual, IVisualEdge>> ScenesOfBackGraph (IGraph<IThing, ILink> backGraph) {
            if (backGraph == null)
                return new IGraphScene<IVisual, IVisualEdge>[0];

            return Scenes.Where (s => BackGraphOf (s.Graph) == backGraph);
        }

        #region VisualGraphEvents

        private void VisualGraphChangeData (IGraph<IVisual, IVisualEdge> graph, IVisual visual, object data) { }

        protected virtual void VisualGraphDataChanged (IGraph<IVisual, IVisualEdge> graph, IVisual visual) {
            var visitor = new MeshGraphVisitor<IVisual, IThing, IVisualEdge, ILink> (this, graph, visual);
            var sceneEvents = new VisualSceneEvents ();
            visitor.ChangeDataVisit (sceneEvents.GraphDataChanged);
        }

        protected void VisualGraphChanged (IGraph<IVisual, IVisualEdge> graph, IVisual visual, GraphEventType eventType) {
            var visitor = new MeshGraphVisitor<IVisual, IThing, IVisualEdge, ILink> (this, graph, visual);
            var sceneEvents = new VisualSceneEvents ();
            visitor.GraphChangedVisit (sceneEvents.GraphChanged, eventType);
        }

        #endregion

        #region BackGraphEvents 

        private void BackGraphChangeData (IGraph<IThing, ILink> graph, IThing backItem, object data) { }

        private void BackGraphDataChanged (IGraph<IThing, ILink> graph, IThing backItem) { }

        private void BackGraphChanged (IGraph<IThing, ILink> graph, IThing backItem, GraphEventType eventType) { }

        #endregion

       
    }
}


