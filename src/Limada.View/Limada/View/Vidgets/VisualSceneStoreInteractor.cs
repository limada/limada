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
 */

using System;
using System.Collections.Generic;
using System.IO;
using Limada.Model;
using Limada.View.VisualThings;
using Limaki.Common;
using Limaki.Contents;
using Limaki.Graphs;
using Limaki.View;
using Limaki.View.GraphScene;
using Limaki.View.Visuals;
using Limaki.View.Viz.Mapping;

namespace Limada.View.Vidgets {

    /// <summary>
     /// </summary>
    public class VisualSceneStoreInteractor : IVisualSceneStoreInteractor {

        public SheetStore SheetStore { get; set; } = new SheetStore ();

        IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge> _organizer = null;
        public IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge> Organizer { get { return _organizer ?? (_organizer = Registry.Pooled<IGraphSceneMapDisplayOrganizer<IVisual, IVisualEdge>> ()); } }

        protected IThingGraph GetThingGraph (IGraph<IVisual, IVisualEdge> graph) {
            var thingGraph = graph.ThingGraph ();
            if (thingGraph == null) {
                throw new ArgumentException ("Sheet works only on ThingGraphs");
            }
            thingGraph = thingGraph.Unwrap () as IThingGraph;
            return thingGraph;
        }

        protected IThingGraph GetThingGraph (IGraphScene<IVisual, IVisualEdge> scene) {
            return GetThingGraph (scene.Graph);
        }

        protected IThing GetSheetThing (IThingGraph thingGraph, long id) {
            if (id == -1)
                return null;
            var result = thingGraph.GetById (id);
            if (result != null) {
                if (!(result is IStreamThing &&
                    ((IStreamThing)result).StreamType == ContentTypes.LimadaSheet)) {
                    Registry.Pooled<IExceptionHandler> ().Catch (
                        new ArgumentException ($"id {id:X16} does not belong to a sheet")
                        , MessageType.OK);
                }
            }
            return result;
        }

        public bool SaveStreamInGraph (Stream source, IGraph<IVisual, IVisualEdge> target, SceneInfo info) {

            var thingGraph = GetThingGraph (target);
            var thing = GetSheetThing (thingGraph, info.Id) as IStreamThing;

            var content = new Content<Stream> (source, CompressionType.bZip2, ContentTypes.LimadaSheet) { Description = info.Name };

            if (thing is IStreamThing || thing == null) {
                var factory = target.ThingFactory ();
                var result = new ThingContentFacade (factory).AssignContent (thingGraph, thing, content) != null;
                info.State.Hollow = false;
                info.State.Clean = true;
                return result;
            }

            return false;
        }

        public bool SaveInStore (IGraphScene<IVisual, IVisualEdge> scene, IGraphSceneLayout<IVisual, IVisualEdge> layout, long id) {
            
            if (scene.Graph.Count > 0) {
                var stream = new MemoryStream ();
                new SheetSerializer ().Save (stream, scene.Graph, layout);
                stream.Position = 0;

                SheetStore.Add (id, stream.GetBuffer ());

                return true;
            }

            SheetStore.Remove (id);
            return false;
        }

        public Stream StreamFromStore (long id) {
            byte [] buffer = null;
            try {
                if (SheetStore.TryGetValue (id, out buffer)) {
                    return new MemoryStream (buffer);
                }
            } catch (Exception ex) {
                // TODO: stream-closed-error should never happen.Try to get reread the source  
                Registry.Pooled<IExceptionHandler> ().Catch (ex, MessageType.OK);
            }
            return null;

        }

        public bool IsSaveable (IGraphScene<IVisual, IVisualEdge> scene) {
            return scene != null && scene.Graph.ThingGraph () != null;
        }

        public void SaveInGraph (IGraphScene<IVisual, IVisualEdge> scene, IGraphSceneLayout<IVisual, IVisualEdge> layout, SceneInfo info) {
            var graph = GetThingGraph (scene);
            var sheetThing = GetSheetThing (graph, info.Id);
            var saved = SaveToThing (scene, layout, sheetThing, info.Name);
            info.Id = saved.Id;
            info.Name = saved.Name;
            saved.State.CopyTo (info.State);
            saved.State.CopyTo (scene.State);
            var sheetVisual = scene.Graph.VisualOf (sheetThing);
            if (sheetVisual != null) {
                //sheetVisual.Data = info.Name;
                scene.Graph.DoChangeData (sheetVisual, info.Name);
                scene.Graph.OnGraphChange (sheetVisual, GraphEventType.Update);
            }
        }

        protected SceneInfo SaveToThing (IGraphScene<IVisual, IVisualEdge> scene, IGraphSceneLayout<IVisual, IVisualEdge> layout, IThing thing, string name) {

            var result = default (SceneInfo);
            if (thing is IStreamThing || thing == null) {

                var content = new Content<Stream> (
                    new MemoryStream (), CompressionType.bZip2, ContentTypes.LimadaSheet);

                var serializer = new SheetSerializer ();
                serializer.Save (content.Data, scene.Graph, layout);
                content.Data.Position = 0;
                content.Description = name;

                thing = new VisualThingsContentInteractor ().AssignContent (scene.Graph, thing, content);

                result = SheetStore.RegisterSceneInfo (thing.Id, name);
                result.State.Hollow = false;
                result.State.Clean = true;
                result.State.CopyTo (scene.State);

            } else {
                throw new ArgumentException ("thing must be a StreamThing");
            }
            return result;
        }

        public IGraphScene<IVisual, IVisualEdge> Load (IGraph<IVisual, IVisualEdge> source, IGraphSceneLayout<IVisual, IVisualEdge> layout, Int64 id) {
            try {
                var result = LoadFromStore (source, layout, id);
                if (result == null) {
                    var graph = source.ThingGraph ();

                    if (graph != null) {
                        var thing = GetSheetThing (graph, id) as IStreamThing;
                        result = LoadFromThing (thing, source, layout);
                    }
                }
                return result;
            } catch {
                return null;
            }
        }

        public IGraphScene<IVisual, IVisualEdge> LoadFromStore (IGraph<IVisual, IVisualEdge> source, IGraphSceneLayout<IVisual, IVisualEdge> layout, Int64 id) {
            byte [] buffer = null;
            try {
                if (SheetStore.TryGetValue (id, out buffer)) {

                    // save state of store:
                    var info = SheetStore.GetSheetInfo (id);
                    var state = info.State.Clone ();

                    var result = LoadFromStream (new MemoryStream (buffer), source, layout);

                    state.CopyTo (info.State);
                    state.CopyTo (result.State);

                    return result;
                }
            } catch (Exception ex) {
                // TODO: stream-closed-error should never happen.Try to get reread the source  
                Registry.Pooled<IExceptionHandler> ().Catch (ex, MessageType.OK);
            }
            return null;
        }


        protected IGraphScene<IVisual, IVisualEdge> LoadFromStream (Stream source, IGraph<IVisual, IVisualEdge> sourceGraph, IGraphSceneLayout<IVisual, IVisualEdge> layout) {

            if (source == null || sourceGraph == null)
                return null;
            
            try {

                sourceGraph.CheckLayout(layout);

                source.Position = 0;

                var scene = Organizer.CreateSinkScene (sourceGraph);
                layout = scene.CloneLayout (layout);

                var visuals = new SheetSerializer ().Read (source, scene.Graph, layout);

                new GraphSceneFacade<IVisual, IVisualEdge> (() => scene, layout)
                    .Add (visuals, true, false);

                source.Position = 0;

                return scene;

            } catch (Exception ex) {
                // TODO: stream-closed-error should never happen.Try to get reread the source  
                Registry.Pooled<IExceptionHandler> ().Catch (ex, MessageType.OK);
            }
            return null;
        }

        protected IGraphScene<IVisual, IVisualEdge> LoadFromThing (IStreamThing sourceThing, IGraph<IVisual, IVisualEdge> sourceGraph, IGraphSceneLayout<IVisual, IVisualEdge> layout) {

            var result = default (IGraphScene<IVisual, IVisualEdge>);
            var content = sourceGraph.ThingGraph().ContentOf (sourceThing);
            try {

                content.Source = sourceThing.Id;
                result = LoadFromContent (content, sourceGraph, layout);

            } finally {
                sourceThing.ClearRealSubject ();
            }
            return result;
        }

        public IGraphScene<IVisual, IVisualEdge> LoadFromContent (Content<Stream> source, IGraph<IVisual, IVisualEdge> sourceGraph, IGraphSceneLayout<IVisual, IVisualEdge> layout) {
            var result = default (IGraphScene<IVisual, IVisualEdge>);
            try {
                var name = string.Empty;
                if (source.Description != null) {
                    name = source.Description.ToString ();
                }

                Int64 id = 0;
                if (source.Source is Int64) {
                    id = (Int64)source.Source;
                }

                result = LoadFromStream (source.Data, sourceGraph, layout);
                if (result != null) {
                    result.State.Clean = true;
                    result.State.Hollow = false;
                    var info = SheetStore.RegisterSceneInfo (id, name);
                    result.State.CopyTo (info.State);
                }

            } finally {

            }
            return result;
        }

        public void Clear () {
            SheetStore.Clear ();
        }
    }
}