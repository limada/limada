/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2014 Lytico
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

    public class MeshGraphVisitor<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> 
        where TSinkEdge : IEdge<TSinkItem>, TSinkItem
        where TSourceEdge : IEdge<TSourceItem>, TSourceItem
    {

        public IGraphSceneMesh<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> Mesh { get; private set; }
        public IGraph<TSinkItem, TSinkEdge> SourceGraph { get; private set; }
        public TSinkItem SourceItem { get; private set; }
        public IGraphSceneDisplay<TSinkItem, TSinkEdge> SourceDisplay { get; private set; }
        public IGraphScene<TSinkItem, TSinkEdge> SourceScene { get; private set; }
        public IGraph<TSourceItem, TSourceEdge> BackGraph { get; private set; }
        public TSourceItem BackItem { get; private set; }

        public IEnumerable<IGraphScene<TSinkItem, TSinkEdge>> SinkScenes {
            get {
                return Mesh.ScenesOfBackGraph (BackGraph).Where (s => s != SourceScene);
            }
        }

        public MeshGraphVisitor (IGraphSceneMesh<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> mesh, IGraph<TSinkItem, TSinkEdge> graph, TSinkItem item) {
            this.Mesh = mesh;
            this.SourceGraph = graph;
            this.SourceItem = item;
            this.SourceDisplay = Mesh.Displays.FirstOrDefault (d => d.Data.Graph == SourceGraph);
            this.SourceScene = Mesh.Scenes.FirstOrDefault (s => s.Graph == SourceGraph);
            this.BackGraph = BackGraphOf (SourceGraph);
            this.BackItem =  BackItemOf (SourceGraph,item);
        }

        public void ChangeDataVisit (Action<TSinkItem, TSinkItem, IGraphScene<TSinkItem, TSinkEdge>, IGraphSceneDisplay<TSinkItem, TSinkEdge>> visitor) {
            foreach (var sinkScene in SinkScenes) {
                var sinkGraph = sinkScene.Graph;
                if (ContainsVisualOf (sinkGraph, BackItem)) {
                    var sinkItem = VisualOf (sinkGraph, BackItem);
                    var sinkDisplay = Mesh.Displays.FirstOrDefault (d => d != SourceDisplay && d.Data == sinkScene);
                    visitor (SourceItem, sinkItem, sinkScene, sinkDisplay);
                }
            }
        }

        public void GraphChangedVisit (
            Action<IGraph<TSinkItem, TSinkEdge>, TSinkItem,
                    TSinkItem, IGraphScene<TSinkItem, TSinkEdge>,
                    IGraphSceneDisplay<TSinkItem, TSinkEdge>, GraphEventType
            > visitor, GraphEventType eventType) {

            foreach (var sinkScene in SinkScenes) {
                var sinkGraph = sinkScene.Graph;
                if (ContainsVisualOf (sinkGraph.RootSource (), BackItem) || eventType == GraphEventType.Add) {
                    var sinkItem = VisualOf (sinkGraph, BackItem);
                    var sinkDisplay = Mesh.Displays.FirstOrDefault (d => d != SourceDisplay && d.Data == sinkScene);
                    visitor (SourceGraph, SourceItem, sinkItem, sinkScene, sinkDisplay, eventType);
                }
            }
        }

        public bool IsVisible (IGraph<TSinkItem, TSinkEdge> sinkGraph, TSourceItem backItem) {
            return ContainsVisualOf (sinkGraph, backItem);
        }

        public TSinkItem VisualOf (IGraph<TSinkItem, TSinkEdge> sinkGraph, TSourceItem backItem) {
            var graph = sinkGraph.Source<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> ();
            if (backItem != null && graph != null) {
                return graph.Get (backItem);
            }
            return default(TSinkItem);
        }

        public bool ContainsVisualOf (IGraph<TSinkItem, TSinkEdge> sinkGraph, TSourceItem backItem) {
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