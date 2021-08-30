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
using Limaki.Graphs;
using Limaki.View;
using Limaki.View.Viz;
using Limaki.View.Viz.Mapping;

namespace Limaki.View.Viz.Mapping {

    public class GraphSceneDisplayMapVisitor<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> : IGraphSceneDisplayMapVisitor<TSinkItem, TSinkEdge>
        where TSinkEdge : IEdge<TSinkItem>, TSinkItem
        where TSourceEdge : IEdge<TSourceItem>, TSourceItem {

        public IGraphSceneMapDisplayOrganizer<TSinkItem, TSinkEdge> Organizer { get; private set; }
        public GraphSceneDisplayMapInteractor<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> BackHandler { get; private set; }
        public IGraph<TSinkItem, TSinkEdge> SourceGraph { get; private set; }
        public TSinkItem SourceItem { get; private set; }

        protected IGraphSceneDisplay<TSinkItem, TSinkEdge> SourceDisplay { get; private set; }
        protected IGraphScene<TSinkItem, TSinkEdge> SourceScene { get; private set; }
        protected IGraph<TSourceItem, TSourceEdge> BackGraph { get; private set; }
        protected TSourceItem BackItem { get; private set; }

        public GraphSceneDisplayMapVisitor (
                IGraphSceneMapDisplayOrganizer<TSinkItem, TSinkEdge> organizer, 
                IGraphSceneDisplayMapInteractor<TSinkItem, TSinkEdge> backHandler,
                IGraph<TSinkItem, TSinkEdge> graph, 
                TSinkItem item) {

            this.Organizer = organizer;
            this.BackHandler = backHandler as GraphSceneDisplayMapInteractor<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>;
            this.SourceGraph = graph;
            this.SourceItem = item;

            this.SourceDisplay = Organizer.Displays.FirstOrDefault (d => d.Data.Graph == SourceGraph);
            this.SourceScene = Organizer.Scenes.FirstOrDefault (s => s.Graph == SourceGraph);
            this.BackGraph = BackGraphOf (SourceGraph);
            this.BackItem =  BackItemOf (SourceGraph,item);

        }

        protected IEnumerable<IGraphScene<TSinkItem, TSinkEdge>> SinkScenes {
            get {
                if (BackGraph == null)
                    return new IGraphScene<TSinkItem, TSinkEdge>[0];
                return Organizer.Scenes.Where (s => s.Graph != SourceGraph && BackGraphOf (s.Graph) == BackGraph);
            }
        }

        public void ChangeDataVisit (Action<TSinkItem, TSinkItem, IGraphScene<TSinkItem, TSinkEdge>, IGraphSceneDisplay<TSinkItem, TSinkEdge>> visit) {
            foreach (var sinkScene in SinkScenes) {
                var sinkGraph = sinkScene.Graph;
                if (ContainsSinkOf (sinkGraph, BackItem)) {
                    var sinkItem = SinkOf (sinkGraph, BackItem);
                    var sinkDisplay = Organizer.Displays.FirstOrDefault (d => d != SourceDisplay && d.Data == sinkScene);

                    visit (SourceItem, sinkItem, sinkScene, sinkDisplay);
                }
            }
        }

        public void GraphChangedVisit (Action<
                                            object,
                                            GraphChangeArgs<TSinkItem, TSinkEdge>,
                                            TSinkItem, IGraphScene<TSinkItem, TSinkEdge>,
                                            IGraphSceneDisplay<TSinkItem, TSinkEdge>
                                        > visit, object sender, GraphChangeArgs<TSinkItem, TSinkEdge> args) {

            var eventType = args.EventType;

            foreach (var sinkScene in SinkScenes) {
                var sinkGraph = sinkScene.Graph;
                if (ContainsSinkOf (sinkGraph.RootSource (), BackItem) || eventType == GraphEventType.Add) {
                    var sinkItem = SinkOf (sinkGraph, BackItem);
                    if (sinkItem == null)
                        continue;

                    var sinkDisplay = Organizer.Displays.FirstOrDefault (d => d != SourceDisplay && d.Data == sinkScene);

                    visit (sender, args, sinkItem, sinkScene, sinkDisplay);
                }
            }
        }

        public static bool IsVisible (IGraph<TSinkItem, TSinkEdge> sinkGraph, TSourceItem backItem) {
            return ContainsSinkOf (sinkGraph, backItem);
        }

        public static TSinkItem SinkOf (IGraph<TSinkItem, TSinkEdge> sinkGraph, TSourceItem backItem) {
            return sinkGraph.SinkItemOf<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> (backItem);
        }

        public static bool ContainsSinkOf (IGraph<TSinkItem, TSinkEdge> sinkGraph, TSourceItem backItem) {
            return sinkGraph.ContainsSinkItemOf<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> (backItem);
        }

        public static TSourceItem BackItemOf (IGraph<TSinkItem, TSinkEdge> sinkGraph, TSinkItem sinkItem) {
            return sinkGraph.SourceItemOf<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> (sinkItem);
        }

        public static IGraph<TSourceItem, TSourceEdge> BackGraphOf (IGraph<TSinkItem, TSinkEdge> sinkGraph) {
            var sourceGraph = sinkGraph.Source<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> ();
            if (sourceGraph != null ) {
                return sourceGraph.Source;
            }
            return null;
        }
    }
}