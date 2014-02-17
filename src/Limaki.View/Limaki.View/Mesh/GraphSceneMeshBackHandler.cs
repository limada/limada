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
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Visuals;

namespace Limaki.View.Visuals.UI {

    public class GraphSceneMeshBackHandler<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> : IGraphSceneMeshBackHandler<TSinkItem, TSinkEdge>
        where TSinkEdge : IEdge<TSinkItem>, TSinkItem
        where TSourceEdge : IEdge<TSourceItem>, TSourceItem {

        ICollection<IGraph<TSourceItem, TSourceEdge>> _backGraphs = new HashSet<IGraph<TSourceItem, TSourceEdge>> ();
        public ICollection<IGraph<TSourceItem, TSourceEdge>> BackGraphs { get { return _backGraphs; } }

        public Func<ICollection<IGraphScene<TSinkItem, TSinkEdge>>> Scenes { get; set; }

        public void RegisterBackGraph (IGraph<TSinkItem, TSinkEdge> graph) {
            RegisterBackGraph (BackGraphOf (graph));
        }

        public void UnregisterBackGraph (IGraph<TSinkItem, TSinkEdge> graph) {
            UnregisterBackGraph (BackGraphOf (graph));
        }

        public void RegisterBackGraph (IGraph<TSourceItem, TSourceEdge> root) {
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

        public void UnregisterBackGraph (IGraph<TSourceItem, TSourceEdge> root) {
            if (BackGraphs.Contains (root) && !ScenesOfBackGraph (root).Any ()) {
                root.GraphChanged -= this.BackGraphChanged;
                root.ChangeData -= this.BackGraphChangeData;
                root.DataChanged -= this.BackGraphDataChanged;
                BackGraphs.Remove (root);

            }
        }

        public IGraph<TSourceItem, TSourceEdge> BackGraphOf (IGraph<TSinkItem, TSinkEdge> graph) {
            var source = graph.Source<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> ();
            if (source != null)
                return source.Source;
            return null;
        }

        public IEnumerable<IGraphScene<TSinkItem, TSinkEdge>> ScenesOfBackGraph (IGraph<TSourceItem, TSourceEdge> backGraph) {
            if (backGraph == null)
                return new IGraphScene<TSinkItem, TSinkEdge>[0];

            return Scenes().Where (s => BackGraphOf (s.Graph) == backGraph);
        }

        #region BackGraphEvents

        private void BackGraphChangeData (IGraph<TSourceItem, TSourceEdge> graph, TSourceItem backItem, object data) { }

        private void BackGraphDataChanged (IGraph<TSourceItem, TSourceEdge> graph, TSourceItem backItem) { }

        private void BackGraphChanged (IGraph<TSourceItem, TSourceEdge> graph, TSourceItem backItem, GraphEventType eventType) { }

        #endregion
        
    }
}