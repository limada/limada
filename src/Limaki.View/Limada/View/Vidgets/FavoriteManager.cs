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
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Limada.Model;
using Limada.Schemata;
using Limada.View.VisualThings;
using Limaki.Common;
using Limaki.Contents;
using Limaki.Graphs;
using Limaki.View;
using Limaki.View.GraphScene;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.Mesh;
using Limaki.View.Viz.Visualizers;

namespace Limada.View.Vidgets {

    public class FavoriteManager {

        public FavoriteManager() {
			ResetHomeId();
        }

        public ISceneManager SceneManager { get; set; }
        public VisualsDisplayHistory VisualsDisplayHistory { get; set; }

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

            var thingGraph = graph.Source as IThingGraph;
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
                        scene.Graph.OnGraphChange(edge, Limaki.Graphs.GraphEventType.Add);
                    }
                }
            }
        }

        public IGraphSceneDisplay<IVisual, IVisualEdge> Display { get; set; }

        protected virtual void DisplaySheet(IGraphSceneDisplay<IVisual, IVisualEdge> display, Content<Stream> content) {
            SceneManager.Load (display, content);
            VisualsDisplayHistory.Store (display.Info);
        }

        protected virtual bool DisplaySheet(IGraphSceneDisplay<IVisual, IVisualEdge> display, IThing thing, IThingGraph thingGraph ) {
            var streamThing = thing as IStreamThing;
            try {
                if (streamThing != null && streamThing.StreamType == ContentTypes.LimadaSheet) {
                    var content = thingGraph.ContentOf(streamThing);
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

	
		Int64 _homeId = 0;
		public Int64 HomeId { 
			get { return _homeId; }
			set { 
				_homeId = value;
                Trace.WriteLine ($"{nameof (FavoriteManager)}.{nameof (HomeId)}={_homeId:X16}");
			} 
		}

		public void ResetHomeId() {
			HomeId = Isaac.Long;
		}

        IGraphSceneDisplayMesh<IVisual, IVisualEdge> _mesh = null;
        public IGraphSceneDisplayMesh<IVisual, IVisualEdge> Mesh { get { return _mesh ?? (_mesh = Registry.Pooled<IGraphSceneDisplayMesh<IVisual, IVisualEdge>> ()); } }

        public virtual void GoHome (IGraphSceneDisplay<IVisual, IVisualEdge> display, bool initialize) {
            if (display == null || display.Data == null)
                return;

            SceneInfo homeInfo = SceneManager.SheetStore.GetSheetInfo (HomeId);
            var homeIsStored = homeInfo != null;
            if (homeInfo == null) {
                homeInfo = SceneManager.SheetStore.CreateSceneInfo ("Favorites");
                homeInfo.Id = HomeId;
            }

            var scene = Mesh.CreateSinkScene (display.Data.Graph);

            Action showScene = () => {
                SceneManager.AssignScene (display, scene, homeInfo);
                display.Reset ();
            };

            var view = scene.Graph as SubGraph<IVisual, IVisualEdge>;
            var graph = view.Source<IVisual, IVisualEdge, IThing, ILink> () as VisualThingGraph;
            var thingGraph = graph.ThingGraph ();

            if (thingGraph == null) {
                // it seems not to be a ThingGraph based Scene:
                foreach (var item in view.Source.FindRoots (null)) {
                    view.Add (item);
                }
                showScene ();
                return;
            }

            var topic = thingGraph.GetById (TopicSchema.Topics.Id);
            var topicsCount = thingGraph.Edges (topic).Count;
            var showTopic = topic != null && (topicsCount > 0);


            var sheetsThing = thingGraph.GetById (TopicSchema.Sheets.Id);

            if (initialize && sheetsThing != null) {
                var sheetsCount = thingGraph.Edges (sheetsThing).Where (l => l.Marker.Id == TopicSchema.SheetMarker.Id).Count ();
                var sheetThings = thingGraph.Edges (sheetsThing)
                        .Where (link => link.Marker.Id == TopicSchema.SheetMarker.Id)
                        .Select (link => thingGraph.Adjacent (link, sheetsThing))
                                           .ToArray ();
                // register sheets
                foreach (var s in sheetThings) {
                    SceneManager.SheetStore.RegisterSceneInfo (s.Id, thingGraph.Description (s).ToString ());
                }

                // look if there is only one sheet; show it 
                if (sheetsCount == 1 && topicsCount <= 1) {
                    var autoView = sheetThings.FirstOrDefault ();

                    if (DisplaySheet (display, autoView, thingGraph))
                        return;
                }
            }
            // Favorites sheet could be in SheetManager
            if (homeIsStored) {
                var sheetStream = SceneManager.StreamFromStore (HomeId);
                if (sheetStream != null) {
                    var content = new Content<Stream> {
                        Source = HomeId,
                        Description = homeInfo.Name,
                        Data = sheetStream,
                        ContentType = ContentTypes.LimadaSheet,
                    };

                    DisplaySheet (display, content);
                    return;
                }
            }

            // show topic on start
            if (showTopic && initialize) {
                var autoView = thingGraph.Edges (topic)
                    .Where (link => link.Marker.Id == TopicSchema.AutoViewMarker.Id)
                    .Select (link => thingGraph.Adjacent (link, topic))
                    .FirstOrDefault ();

                if (DisplaySheet (display, autoView, thingGraph))
                    return;
            }


            // show Topic

            if (showTopic) {
                var topicVisual = graph.Get (topic);
                view.Add (topicVisual);
                scene.Focused = topicVisual;
                new GraphSceneFacade<IVisual, IVisualEdge> (() => scene, scene.CloneLayout (display.Layout)).Expand (false);
                showScene ();
                return;
            }

            // show roots
            foreach (var item in thingGraph.FindRoots (null)) {
                if (!thingGraph.IsMarker (item))
                    view.Add (graph.Get (item));
            }
            showScene ();
            return;

        }

        public virtual bool AddToSheets (IThingGraph graph, Int64 sheetId) {
            var thing = graph.GetById (sheetId) as IStreamThing;
            if (thing != null && thing.StreamType == ContentTypes.LimadaSheet) {
                var add = graph.Edges (thing).Where (l => l.Marker.Id != CommonSchema.DescriptionMarker.Id).Count () == 0;
                if (add) {
                    var sheets = TryAddSomeThing (graph, TopicSchema.Sheets);
                    var sheetMarker = TryAddSomeThing (graph, TopicSchema.SheetMarker);
                    TryAddSomeThing (graph, TopicSchema.TopicToSheetsLink);
                    var factory = Registry.Factory.Create<IThingFactory> ();
                    var link = factory.CreateEdge (sheets, thing, sheetMarker);
                    graph.Add (link);
                }
                return add;
            }
            return false;
        }

        public virtual bool AddToSheets(IGraph<IVisual, IVisualEdge> graph, Int64 sheetId) {
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
        public void SaveChanges (IEnumerable<IGraphSceneDisplay<IVisual, IVisualEdge>> displays) {

            foreach (var display in displays) {
                var graph = display.Data.Graph;
                if (graph.Count == 0)
                    return;

                var thingGraph = graph.ThingGraph ();
                if (thingGraph != null) {
                    var topic = thingGraph.GetById (TopicSchema.Topics.Id);
                    if (topic == null) {
                        SceneManager.SaveInGraph (display.Data, display.Layout, display.Info);

                        AddToSheets (thingGraph, display.Info.Id);
                    }
                }
            }
        }

		public void Clear() {
			ResetHomeId();
		}
       
    }
}