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
using Limada.Model;
using Limada.VisualThings;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.View.Visualizers;
using Limaki.Visuals;

namespace Limaki.View.Visuals.UI {

    public class GraphSceneMeshVisitor<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> : IGraphSceneMeshVisitor<TSinkItem, TSinkEdge>
        where TSinkEdge : IEdge<TSinkItem>, TSinkItem
        where TSourceEdge : IEdge<TSourceItem>, TSourceItem {

        public IGraphSceneMesh<TSinkItem, TSinkEdge> Mesh { get; private set; }
        public GraphSceneMeshBackHandler<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> BackHandler { get; private set; }
        public IGraph<TSinkItem, TSinkEdge> SourceGraph { get; private set; }
        public TSinkItem SourceItem { get; private set; }

        protected IGraphSceneDisplay<TSinkItem, TSinkEdge> SourceDisplay { get; private set; }
        protected IGraphScene<TSinkItem, TSinkEdge> SourceScene { get; private set; }
        protected IGraph<TSourceItem, TSourceEdge> BackGraph { get; private set; }
        protected TSourceItem BackItem { get; private set; }

        public GraphSceneMeshVisitor (
                IGraphSceneMesh<TSinkItem, TSinkEdge> mesh, 
                IGraphSceneMeshBackHandler<TSinkItem, TSinkEdge> backHandler,
                IGraph<TSinkItem, TSinkEdge> graph, 
                TSinkItem item) {

            this.Mesh = mesh;
            this.BackHandler = backHandler as GraphSceneMeshBackHandler<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>;
            this.SourceGraph = graph;
            this.SourceItem = item;

            this.SourceDisplay = Mesh.Displays.FirstOrDefault (d => d.Data.Graph == SourceGraph);
            this.SourceScene = Mesh.Scenes.FirstOrDefault (s => s.Graph == SourceGraph);
            this.BackGraph = BackGraphOf (SourceGraph);
            this.BackItem =  BackItemOf (SourceGraph,item);

        }

        protected IEnumerable<IGraphScene<TSinkItem, TSinkEdge>> SinkScenes {
            get {
                if (BackGraph == null)
                    return new IGraphScene<TSinkItem, TSinkEdge>[0];
                return Mesh.Scenes.Where (s => s.Graph != SourceGraph && BackGraphOf (s.Graph) == BackGraph);
            }
        }

        public void ChangeDataVisit (Action<TSinkItem, TSinkItem, IGraphScene<TSinkItem, TSinkEdge>, IGraphSceneDisplay<TSinkItem, TSinkEdge>> visit) {
            foreach (var sinkScene in SinkScenes) {
                var sinkGraph = sinkScene.Graph;
                if (ContainsSourceOf (sinkGraph, BackItem)) {
                    var sinkItem = SourceOf (sinkGraph, BackItem);
                    var sinkDisplay = Mesh.Displays.FirstOrDefault (d => d != SourceDisplay && d.Data == sinkScene);

                    visit (SourceItem, sinkItem, sinkScene, sinkDisplay);
                }
            }
        }

        public void GraphChangedVisit (Action<
                                            IGraph<TSinkItem, TSinkEdge>, TSinkItem,
                                            TSinkItem, IGraphScene<TSinkItem, TSinkEdge>,
                                            IGraphSceneDisplay<TSinkItem, TSinkEdge>, GraphEventType
                                        > visit, GraphEventType eventType) {

            foreach (var sinkScene in SinkScenes) {
                var sinkGraph = sinkScene.Graph;
                if (ContainsSourceOf (sinkGraph.RootSource (), BackItem) || eventType == GraphEventType.Add) {
                    var sinkItem = SourceOf (sinkGraph, BackItem);
                    var sinkDisplay = Mesh.Displays.FirstOrDefault (d => d != SourceDisplay && d.Data == sinkScene);

                    visit (SourceGraph, SourceItem, sinkItem, sinkScene, sinkDisplay, eventType);
                }
            }
        }

        public bool IsVisible (IGraph<TSinkItem, TSinkEdge> sinkGraph, TSourceItem backItem) {
            return ContainsSourceOf (sinkGraph, backItem);
        }

        public TSinkItem SourceOf (IGraph<TSinkItem, TSinkEdge> sinkGraph, TSourceItem backItem) {
            var graph = sinkGraph.Source<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> ();
            if (backItem != null && graph != null) {
                return graph.Get (backItem);
            }
            return default(TSinkItem);
        }

        public bool ContainsSourceOf (IGraph<TSinkItem, TSinkEdge> sinkGraph, TSourceItem backItem) {
            var graph = sinkGraph.Source<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> ();
            if (backItem != null && graph != null) {
                return graph.Source2Sink.ContainsKey (backItem);
            }
            return false;
        }

        public static TSourceItem BackItemOf (IGraph<TSinkItem, TSinkEdge> sinkGraph, TSinkItem sinkItem) {
            var graph = sinkGraph.Source<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> ();
            if (sinkItem != null && graph != null) {
                return graph.Get (sinkItem);
            }
            return default(TSourceItem);
        }

        public IGraph<TSourceItem, TSourceEdge> BackGraphOf (IGraph<TSinkItem, TSinkEdge> sinkGraph) {
            var sourceGraph = sinkGraph.Source<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> ();
            if (sourceGraph != null ) {
                return sourceGraph.Source;
            }
            return null;
        }
    }
}