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
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.View.UI.GraphScene;
using Limaki.View.Visualizers;
using Limaki.Common.Linqish;

namespace Limaki.View.Mesh {

    /// <summary>
    /// handles the backing Graphs of Scenes
    /// that is the <see cref="IGraphPair{TSinkItem, TSourceItem, TSinkEdge, TSourceEdge}.Source"/>
    /// containing the entities
    /// </summary>
    /// <typeparam name="TSinkItem"></typeparam>
    /// <typeparam name="TSourceItem"></typeparam>
    /// <typeparam name="TSinkEdge"></typeparam>
    /// <typeparam name="TSourceEdge"></typeparam>
    public class GraphSceneMeshBackHandler<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> : IGraphSceneMeshBackHandler<TSinkItem, TSinkEdge>
        where TSinkEdge : IEdge<TSinkItem>, TSinkItem
        where TSourceEdge : IEdge<TSourceItem>, TSourceItem {

        ICollection<IGraph<TSourceItem, TSourceEdge>> _backGraphs = new HashSet<IGraph<TSourceItem, TSourceEdge>> ();
        public ICollection<IGraph<TSourceItem, TSourceEdge>> BackGraphs { get { return _backGraphs; } }

        public Func<ICollection<IGraphScene<TSinkItem, TSinkEdge>>> Scenes { get; set; }
        public Func<ICollection<IGraphSceneDisplay<TSinkItem, TSinkEdge>>> Displays { get; set; }

        public void RegisterBackGraph (IGraph<TSinkItem, TSinkEdge> graph) {
            RegisterBackGraph (BackGraphOf (graph));
        }

        public void UnregisterBackGraph (IGraph<TSinkItem, TSinkEdge> graph) {
            UnregisterBackGraph (BackGraphOf (graph));
        }

        public void RegisterBackGraph (IGraph<TSourceItem, TSourceEdge> root) {
            if (!BackGraphs.Contains (root)) {
                BackGraphs.Add (root);
                root.GraphChange -= this.BackGraphChange;
                root.GraphChange += this.BackGraphChange;
                root.ChangeData -= this.BackGraphChangeData;
                root.ChangeData += this.BackGraphChangeData;
                root.DataChanged -= this.BackGraphDataChanged;
                root.DataChanged += this.BackGraphDataChanged;
            }

        }

        public void UnregisterBackGraph (IGraph<TSourceItem, TSourceEdge> root) {
            if (BackGraphs.Contains (root) && !ScenesOfBackGraph (root).Any ()) {
                root.GraphChange -= this.BackGraphChange;
                root.ChangeData -= this.BackGraphChangeData;
                root.DataChanged -= this.BackGraphDataChanged;
                BackGraphs.Remove (root);

            }
        }

        /// <summary>
        /// returns the backing Graph
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public IGraph<TSourceItem, TSourceEdge> BackGraphOf (IGraph<TSinkItem, TSinkEdge> graph) {
            var source = graph.Source<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> ();
            if (source != null)
                return source.Source;
            return null;
        }

        public IEnumerable<IGraphScene<TSinkItem, TSinkEdge>> ScenesOfBackGraph (IGraph<TSourceItem, TSourceEdge> backGraph) {
            if (backGraph == null)
                return new IGraphScene<TSinkItem, TSinkEdge>[0];

            return Scenes ().Where (s => BackGraphOf (s.Graph) == backGraph);
        }

        public IGraphSceneDisplay<TSinkItem, TSinkEdge> DisplayOf (IGraphScene<TSinkItem, TSinkEdge> scene) {
            return Displays ().Where (d => d.Data == scene).FirstOrDefault ();
        }

        #region BackGraphEvents

        private void BackGraphChangeData (IGraph<TSourceItem, TSourceEdge> graph, TSourceItem backItem, object data) { }

        private void BackGraphDataChanged (IGraph<TSourceItem, TSourceEdge> graph, TSourceItem backItem) { }

        protected ICollection<Tuple<IGraph<TSourceItem, TSourceEdge>, TSourceItem, GraphEventType>> changing = new HashSet<Tuple<IGraph<TSourceItem, TSourceEdge>, TSourceItem, GraphEventType>> ();

        protected virtual void BackGraphChange (IGraph<TSourceItem, TSourceEdge> graph, TSourceItem backItem, GraphEventType eventType) {

            var change = Tuple.Create (graph, backItem, eventType);
            if (changing.Contains (change))
                return;

            try {
                changing.Add (change);

                var toPerform = new HashSet<IGraphSceneDisplay<TSinkItem, TSinkEdge>> ();
                var dependencies = Registry.Pool.TryGetCreate<GraphDepencencies<TSourceItem, TSourceEdge>> ();
                dependencies.VisitItems (GraphCursor.Create (graph, backItem),
                    sourceItem => {
                        foreach (var scene in ScenesOfBackGraph (graph)) {

                            var sg = scene.Graph.Source<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> ();

                            var sinkItem = default (TSinkItem);
                            if (sg.Count == 0 || !sg.Source2Sink.TryGetValue (sourceItem, out sinkItem))
                                continue;

                            var visible = scene.Contains (sinkItem);
                            if (eventType == GraphEventType.Remove) {
                                if (visible &&
                                    !scene.Requests
                                         .OfType<DeleteCommand<TSinkItem, TSinkEdge>> ()
                                         .Any (r => sinkItem.Equals (r.Subject))) {

                                    scene.RequestDelete (sinkItem, null);
                                    toPerform.Add (DisplayOf (scene));
                                }
                            }

                            if (!visible)
                                foreach (var dg in scene.Graph.Graphs ())
                                    if (eventType == GraphEventType.Remove) {
                                        dg.OnGraphChange (sinkItem, eventType);
                                        dg.Remove (sinkItem);
                                    }

                        }
                    }
                    , eventType);

                toPerform.ForEach (display => display.Perform ());

            } finally {
                changing.Remove (change);
            }

        }

        #endregion

    }
}