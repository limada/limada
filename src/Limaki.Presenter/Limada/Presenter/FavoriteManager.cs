/*
 * Limada 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.Linq;
using Limada.Model;
using Limada.Schemata;
using Limada.View;

using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Model.Streams;
using Limaki.Presenter;
using Limaki.Presenter.Widgets.Layout;
using Limaki.Widgets;

using Id = System.Int64;
using Limada.Common;
using Limaki.Presenter.Display;
using System.Collections.Generic;
using Limaki.UseCases.Viewers;
using Limaki.Presenter.Widgets.UI;

namespace Limada.Presenter {
    public class FavoriteManager {

        public FavoriteManager(){}
        public FavoriteManager(IGraphSceneDisplay<IWidget, IEdgeWidget> display): this() {
            this.Display = display;
            
        }

        public ISheetManager SheetManager { get; set; }

        public void AddToFavorites(IGraphScene<IWidget, IEdgeWidget> scene) {
            AddToFavorites(scene, TopicSchema.TopicMarker, false);
        }

        public void ViewOnOpen(IGraphScene<IWidget, IEdgeWidget> scene) {
            AddToFavorites(scene, TopicSchema.AutoViewMarker, true);
        }

        protected virtual IThing TryAddSomeThing(IThingGraph thingGraph, IThing something) {
            var thing = thingGraph.GetById(something.Id);
            if (thing == null) {
                thing = something;
                thingGraph.Add(thing);
            }
            return thing;
        }

        public void AddToFavorites(IGraphScene<IWidget, IEdgeWidget> scene, IThing marker, bool oneAndOnly) {
            if (scene == null || scene.Focused == null)
                return;
            
            var graph = GraphPairExtension<IWidget, IEdgeWidget>
                .Source<IThing, ILink>(scene.Graph);

            if (graph == null)
                return;

            var thingGraph = graph.Two as IThingGraph;
            if (graph != null && thingGraph != null && scene.Focused != null) {
                var thing = graph.Get(scene.Focused);
                if (thing != null) {
                    var topics = TryAddSomeThing(thingGraph, TopicSchema.Topics);
                    var factory = graph.ThingFactory();
                    ILink link = null;
                    if (oneAndOnly) {
                        var schema = new Schema();
                        link = schema.SetTheLeaf(thingGraph, topics, marker, thing);
                    } else {
                        link = factory.CreateEdge(topics, thing, marker);
                        thingGraph.Add(link);
                    }
                    if (link != null) {
                        var edge = graph.Get(link);
                        scene.Graph.OnGraphChanged(edge, Limaki.Graphs.GraphChangeType.Add);
                    }
                }
            }
        }

        private IGraphSceneDisplay<IWidget, IEdgeWidget> _display = null;
        public IGraphSceneDisplay<IWidget, IEdgeWidget> Display {
            get { return _display; }
            set { _display = value; }
        }


        protected virtual bool DisplaySheet(IGraphSceneDisplay<IWidget, IEdgeWidget> display, IThing thing, IThingGraph thingGraph ) {
            var streamThing = thing as IStreamThing;
            try {
                if (streamThing != null && streamThing.StreamType == StreamTypes.LimadaSheet) {
                    var streamInfo = ThingStreamFacade.GetStreamInfo(thingGraph, streamThing);
                    streamInfo.Source = streamThing.Id;
                    var info = SheetManager.LoadFromStreamInfo(streamInfo, display.Data as Scene, display.Layout);
                    display.DataId = info.Id;
                    display.Text = info.Name;

                    display.Execute();
                    info.State.CopyTo(display.State);
                    return true;
                }
            } catch (Exception e) { }
            return false;
        }

        private Id HomeId = Isaac.Long;
        public virtual void GoHome(IGraphSceneDisplay<IWidget, IEdgeWidget> display, bool doAutoView) {
            if (display == null)
                return;
            if (display.Data == null)
                return;

            this.Display = display;

            display.DataId = HomeId;
            display.Text = "Favorites";
            new State {Hollow = true}.CopyTo(display.State);

            var view = display.Data.Graph as GraphView<IWidget, IEdgeWidget>;

            var graph = GraphPairExtension<IWidget, IEdgeWidget>
                            .Source<IThing, ILink>(view) as WidgetThingGraph;
            IThingGraph thingGraph = null;
            if (graph != null) 
                thingGraph = graph.Two as IThingGraph;
            
            var done = false;

            if (graph != null && view != null && thingGraph != null) {
                var topic = thingGraph.GetById(TopicSchema.Topics.Id);
                var topicsCount = thingGraph.Edges(topic).Count;
                var showTopic = topic != null && (topicsCount > 0);

                // look if there is only one sheet:
                var sheets = thingGraph.GetById(TopicSchema.Sheets.Id);
                var sheetsCount = thingGraph.Edges(sheets).Where(l => l.Marker.Id == TopicSchema.SheetMarker.Id).Count();
                
                if (doAutoView && sheets != null && sheetsCount==1 && topicsCount <= 1) {
                    var autoView = thingGraph.Edges(sheets)
                        .Where(link => link.Marker.Id == TopicSchema.SheetMarker.Id)
                        .Select(link => thingGraph.Adjacent(link, sheets))
                        .FirstOrDefault();

                    done = DisplaySheet(display, autoView, thingGraph);
                }

                if (! done && showTopic && doAutoView) {
                    var autoView = thingGraph.Edges(topic)
                        .Where(link => link.Marker.Id == TopicSchema.AutoViewMarker.Id)
                        .Select(link => thingGraph.Adjacent(link, topic))
                        .FirstOrDefault();

                    done = DisplaySheet(display, autoView, thingGraph);

                }

                if (! done && showTopic) {
                    var topicWidget = graph.Get(topic);
                    view.Add(topicWidget);
                    display.Data.Focused = topicWidget;
                    new GraphSceneFacade<IWidget,IEdgeWidget>(() => { return display.Data as Scene; }, display.Layout).Expand(false);
                    display.Invoke();
                    done = true;

                } 

                if (!done) {
                    foreach (IThing item in GraphPairExtension<IThing, ILink>.FindRoots(thingGraph, null)) {
                        if (!thingGraph.IsMarker(item))
                            view.Add(graph.Get(item));
                    }
                    display.Invoke();
                    done = true;
                }

            } 
            if (! done && view != null) {
                // it seems not to be a ThingGraph based Scene:
                foreach (var item in  GraphPairExtension<IWidget, IEdgeWidget>.FindRoots(view.Two, null)) {
                    view.Add(item);
                }
                display.Invoke();
            }
        }


        public virtual bool AddToSheets(IThingGraph graph, Id sheetId) {
            var thing = graph.GetById(sheetId) as IStreamThing;
                if (thing != null && thing.StreamType == StreamTypes.LimadaSheet) {
                    var add = graph.Edges(thing).Where(l => l.Marker.Id != CommonSchema.DescriptionMarker.Id).Count() == 0;
                    if(add) {
                        var sheets = TryAddSomeThing(graph, TopicSchema.Sheets);
                        var sheetMarker = TryAddSomeThing(graph, TopicSchema.SheetMarker);
                        TryAddSomeThing(graph, TopicSchema.TopicToSheetsLink);
                        var factory = Registry.Factory.Create<IThingFactory>();
                        var link = factory.CreateEdge(sheets, thing, sheetMarker);
                        graph.Add(link);
                    }
                    return add;
                }
            return false;
        }

        public virtual bool AddToSheets(IGraph<IWidget, IEdgeWidget> graph, Id sheetId) {
            var thingGraph = graph.ThingGraph();
            if (thingGraph != null) {
                return AddToSheets(thingGraph, sheetId);
            }
            return false;
        }

        public void SaveChanges(IEnumerable<IGraphSceneDisplay<IWidget, IEdgeWidget>> displays) {
            IGraphSceneDisplay<IWidget, IEdgeWidget> display = displays.First();
            IGraph<IWidget, IEdgeWidget> graph = graph = display.Data.Graph;
            if (graph.Count == 0)
                return;

            var thingGraph = graph.ThingGraph();
            if (thingGraph != null) {
                var topic = thingGraph.GetById(TopicSchema.Topics.Id);
                if(topic == null) {
                    var info = SheetManager.GetSheetInfo(display.DataId) ?? new SheetInfo { Id = display.DataId };
                    info = SheetManager.SaveInGraph(display.Data,display.Layout,info);
                    AddToSheets(thingGraph, info.Id);
                }
            }
        }
    }
}