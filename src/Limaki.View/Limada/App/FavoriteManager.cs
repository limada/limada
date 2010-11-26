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
using Limaki.Drawing.UI;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Model.Streams;
using Limaki.Widgets;
using Limaki.Widgets.Layout;

namespace Limada.App {
    public class FavoriteManager {

        public FavoriteManager(){}
        public FavoriteManager(IDataDisplay<Scene> display): this() {
            this.Display = display;
            
        }

        public void AddToFavorites(Scene scene) {
            AddToFavorites(scene, TopicSchema.TopicMarker, false);
        }

        public void ViewOnOpen(Scene scene) {
            AddToFavorites(scene, TopicSchema.AutoViewMarker, true);
        }

        protected virtual IThing TryAddTopic(IThingGraph thingGraph) {
            var topics = thingGraph.GetById(TopicSchema.Topics.Id);
            if (topics == null) {
                topics = TopicSchema.Topics;
                thingGraph.Add(topics);
            }
            return topics;
        }

        public void AddToFavorites(Scene scene, IThing marker, bool oneAndOnly) {
            if (scene == null)
                return;
            var graph = new GraphPairFacade<IWidget, IEdgeWidget>()
                .Source<IThing, ILink>(scene.Graph);

            var thingGraph = graph.Two as IThingGraph;
            if (graph != null && thingGraph != null && scene.Focused != null) {
                var thing = graph.Get(scene.Focused);
                if (thing != null) {
                    var topics = TryAddTopic(thingGraph);
                    var factory = WidgetThingGraphExtension.GetThingFactory(graph);
                    ILink link = null;
                    if (oneAndOnly) {
                        var schema = new Schema();
                        link = schema.SetTheLeaf(thingGraph, topics, marker, thing);
                    } else {
                        link = factory.CreateLink(topics, thing, marker);
                        thingGraph.Add(link);
                    }
                    if (link != null) {
                        var edge = graph.Get(link);
                        scene.Graph.OnGraphChanged(edge, Limaki.Graphs.GraphChangeType.Add);
                    }
                }
            }
        }

        private IDataDisplay<Scene> _display = null;
        public IDataDisplay<Scene> Display {
            get { return _display; }
            set { _display = value; }
        }

        ILayout<Scene, IWidget> _layout = null;
        protected virtual ILayout<Scene, IWidget> Layout {
            get {
                if (_layout == null) {
                    _layout = new GraphLayout<Scene, IWidget>(
                        () => { return Display.Data; },
                        Registry.Pool.TryGetCreate<StyleSheets>().DefaultStyleSheet);
                }
                return _layout;
            }
            set { _layout = value; }
        }

        public virtual void ShowRoots(IDataDisplay<Scene> display, bool doAutoView) {
            if (display == null)
                return;
            if (display.Data == null)
                return;

            this.Display = display;

            var view = display.Data.Graph as GraphView<IWidget, IEdgeWidget>;

            var graph = new GraphPairFacade<IWidget, IEdgeWidget>()
                            .Source<IThing, ILink>(view) as WidgetThingGraph;

            if (graph != null) {
                IThingGraph source = graph.Two as IThingGraph;

                if (graph != null && view != null && source != null) {
                    IThing topic = source.GetById (TopicSchema.Topics.Id);
                    if (topic != null && ( source.Edges (topic).Count > 0 )) {
                        if (doAutoView) {
                            try {
                                var autoView = ( 
                                    from link in source.Edges (topic)
                                    where link.Marker.Id == TopicSchema.AutoViewMarker.Id
                                    select source.Adjacent (link, topic)).
                                    FirstOrDefault ();

                                var streamThing = autoView as IStreamThing;
                                if (streamThing != null && streamThing.StreamType == StreamTypes.LimadaSheet) {
                                    var sheetManager = new SheetManager ();
                                    sheetManager.LoadSheet (display.Data, Layout, streamThing);
                                    topic = null;
                                    display.CommandsExecute ();
                                }
                            } catch (Exception e) {}
                        }
                        if (topic != null) {
                            var topicWidget = graph.Get (topic);
                            view.Add (topicWidget);
                            display.Data.Focused = topicWidget;
                            new SceneFacade (( ) => {return display.Data;},Layout).Expand (false);
                            display.CommandsInvoke ();
                        }
                    } else {
                        foreach (IThing item in new GraphPairFacade<IThing, ILink> ().FindRoots (source, null)) {
                            if (!source.IsMarker (item))
                                view.Add (graph.Get (item));
                        }
                        display.CommandsInvoke ();
                    }
                }
            } else if (view !=null) {
                // it seems not to be a ThingGraph based Scene:
                foreach (var item in new GraphPairFacade<IWidget, IEdgeWidget>().FindRoots(view.Two, null)) {
                    view.Add(item);
                }
                display.CommandsInvoke();
            }
        }


    }
}