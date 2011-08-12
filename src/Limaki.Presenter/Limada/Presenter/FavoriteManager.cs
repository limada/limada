/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
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
using Limaki.Presenter.Visuals.Layout;
using Limaki.Visuals;

using Id = System.Int64;
using Limada.Common;
using Limaki.Presenter.Display;
using System.Collections.Generic;
using Limaki.UseCases.Viewers;
using Limaki.Presenter.Visuals.UI;
using Limaki.Presenter.UI;
using System.Diagnostics;
using System.IO;

namespace Limada.Presenter {
    public class FavoriteManager {

        public FavoriteManager() {
            Trace.WriteLine(string.Format("HomeId\t{0}", HomeId));
        }

        public ISheetManager SheetManager { get; set; }

        public void AddToFavorites(IGraphScene<IVisual, IVisualEdge> scene) {
            AddToFavorites(scene, TopicSchema.TopicMarker, false);
        }

        public void SetAutoView(IGraphScene<IVisual, IVisualEdge> scene) {
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

        public void AddToFavorites(IGraphScene<IVisual, IVisualEdge> scene, IThing marker, bool oneAndOnly) {
            if (scene == null || scene.Focused == null)
                return;

            var graph = scene.Graph.Source<IVisual, IVisualEdge,IThing, ILink>();

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

        public IGraphSceneDisplay<IVisual, IVisualEdge> Display { get; set; }

        protected virtual void DisplaySheet(IGraphSceneDisplay<IVisual, IVisualEdge> display, Content<Stream> content) {
            var info = SheetManager.LoadFromContent(content, display.Data, display.Layout);
            display.Execute();
            display.Info = info;
        }

        protected virtual bool DisplaySheet(IGraphSceneDisplay<IVisual, IVisualEdge> display, IThing thing, IThingGraph thingGraph ) {
            var streamThing = thing as IStreamThing;
            try {
                if (streamThing != null && streamThing.StreamType == StreamTypes.LimadaSheet) {
                    var content = ThingStreamFacade.GetContent(thingGraph, streamThing);
                    content.Source = streamThing.Id;
                    DisplaySheet(display, content);
                    return true;
                }
            } catch (Exception e) {
                Trace.WriteLine("Error on displaying sheet {0}", e.Message);
                Debug.WriteLine(e.StackTrace);
            }
            return false;
        }

        [TODO("change homeId on opening database")]
        public Id HomeId = Isaac.Long;
        public virtual void GoHome(IGraphSceneDisplay<IVisual, IVisualEdge> display, bool doAutoView) {
            if (display == null)
                return;
            if (display.Data == null)
                return;

            var homeInfo = new SceneInfo {
                Id = HomeId,
                Name = "Favorites",
            };
            homeInfo.State.Hollow = true;            
            SceneTools.CleanScene(display.Data);
            display.DeviceRenderer.Render();
            display.Info = homeInfo;
            

            var view = display.Data.Graph as GraphView<IVisual, IVisualEdge>;

            var graph = view.Source<IVisual, IVisualEdge,IThing, ILink>() as VisualThingGraph;
            IThingGraph thingGraph = null;
            if (graph != null) 
                thingGraph = graph.Two as IThingGraph;
            
            var done = false;

            if (graph != null && view != null && thingGraph != null) {
                var topic = thingGraph.GetById(TopicSchema.Topics.Id);
                var topicsCount = thingGraph.Edges(topic).Count;
                var showTopic = topic != null && (topicsCount > 0);

                #region only one sheet
                // look if there is only one sheet:
                var sheets = thingGraph.GetById(TopicSchema.Sheets.Id);
                var sheetsCount = thingGraph.Edges(sheets).Where(l => l.Marker.Id == TopicSchema.SheetMarker.Id).Count();
                
                if (! done && doAutoView && sheets != null && sheetsCount==1 && topicsCount <= 1) {
                    var autoView = thingGraph.Edges(sheets)
                        .Where(link => link.Marker.Id == TopicSchema.SheetMarker.Id)
                        .Select(link => thingGraph.Adjacent(link, sheets))
                        .FirstOrDefault();

                    done = DisplaySheet(display, autoView, thingGraph);
                }
                #endregion

                #region Favorites sheet is in SheetManager
                if (! done) {
                    var info = SheetManager.GetSheetInfo(HomeId);
                    if (info != null) {
                        var sheet = SheetManager.GetFromStore(HomeId);
                        var content = new Content<Stream> {
                            Source = HomeId,
                            Description = homeInfo.Name,
                            Data = sheet,
                            StreamType = TopicSchema.SheetMarker.Id
                        };
                        DisplaySheet(display, content);
                        done = true;
                    }
                }
                #endregion

                #region AutoView
                if (! done && showTopic && doAutoView) {
                    var autoView = thingGraph.Edges(topic)
                        .Where(link => link.Marker.Id == TopicSchema.AutoViewMarker.Id)
                        .Select(link => thingGraph.Adjacent(link, topic))
                        .FirstOrDefault();

                    done = DisplaySheet(display, autoView, thingGraph);

                }
                #endregion

                #region show Topic
                if (! done && showTopic) {
                    var topicVisual = graph.Get(topic);
                    view.Add(topicVisual);
                    display.Data.Focused = topicVisual;
                    new GraphSceneFacade<IVisual,IVisualEdge>(() => display.Data,  display.Layout).Expand(false);
                    display.Invoke();
                    done = true;

                }
                #endregion

                #region show roots
                if (!done) {
                    foreach (var item in thingGraph.FindRoots(null)) {
                        if (!thingGraph.IsMarker(item))
                            view.Add(graph.Get(item));
                    }
                    display.Invoke();
                    done = true;
                }
                #endregion

            }

            #region no ThingGraph

            if (! done && view != null) {
                // it seems not to be a ThingGraph based Scene:
                foreach (var item in  view.Two.FindRoots(null)) {
                    view.Add(item);
                }
                display.Invoke();
            }

            #endregion
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

        public virtual bool AddToSheets(IGraph<IVisual, IVisualEdge> graph, Id sheetId) {
            var thingGraph = graph.ThingGraph();
            if (thingGraph != null) {
                return AddToSheets(thingGraph, sheetId);
            }
            return false;
        }

        /// <summary>
        /// saves scenes of displays with  AddToSheets
        /// </summary>
        /// <param name="displays"></param>
        public void SaveChanges(IEnumerable<IGraphSceneDisplay<IVisual, IVisualEdge>> displays) {
            IGraphSceneDisplay<IVisual, IVisualEdge> display = displays.First();
            IGraph<IVisual, IVisualEdge> graph = graph = display.Data.Graph;
            if (graph.Count == 0)
                return;

            var thingGraph = graph.ThingGraph();
            if (thingGraph != null) {
                var topic = thingGraph.GetById(TopicSchema.Topics.Id);
                if(topic == null) {
                    var info = display.Info;
                    SheetManager.SaveInGraph(display.Data,display.Layout,info);
                    display.Info = info;
                    AddToSheets(thingGraph, info.Id);
                }
            }
        }
    }
}