/*
 * Limada 
 * Version 0.09
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://limada.sourceforge.net
 */


using System;
using System.Collections.Generic;
using System.IO;
using Limada.Common;
using Limada.Model;
using Limada.Schemata;
using Limada.View;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Model.Streams;
using Limaki.Presenter.Visuals.UI;
using Limaki.Visuals;
using Id = System.Int64;
using Limaki.Graphs.Extensions;

namespace Limada.Presenter {

    public class SheetManager : ISheetManager {

        #region SheetInfo
        private IDictionary<Id, SceneInfo> _sheets = null;
        /// <summary>
        /// names to thing.ids of loaded and saved sheets
        /// </summary>
        protected IDictionary<Id, SceneInfo> Sheets {
            get {
                if (_sheets == null) {
                    _sheets = new Dictionary<Id, SceneInfo>();
                }
                return _sheets;
            }
            set { _sheets = value; }
        }

        private IDictionary<Id, Stream> _sheetsStreams = null;
        /// <summary>
        /// streams of sheets
        /// </summary>
        protected IDictionary<Id, Stream> SheetStreams {
            get { return _sheetsStreams ?? (_sheetsStreams = new Dictionary<Id, Stream>()); }
            set { _sheetsStreams = value; }
        }

        public Action<SceneInfo> SheetRegistered { get; set; }

        public void RegisterSheet(SceneInfo info) {
            if (info.Id == 0) {
                info.Id = Isaac.Long;
            }
            Sheets[info.Id] = info;
        }

        public SceneInfo RegisterSheet(Id id, string name) {
            var result = default( SceneInfo );
            if (id !=0 && Sheets.ContainsKey(id)) {
                result = Sheets[id];
                if (!string.IsNullOrEmpty(name))
                    result.Name = name;
            } else {
                if (id == 0) {
                    id = Isaac.Long;
                }
                result = new SceneInfo() { Id = id, Name = name };
                result.State.Hollow = true;
                Sheets[id] = result;
                if (SheetRegistered != null)
                    SheetRegistered(result);
            }
            
            return result;
        }

        public void Clear() {
            Sheets = null;
            SheetStreams = null;
        }

        public SceneInfo GetSheetInfo(Id id) {
            var result = default(SceneInfo);
            Sheets.TryGetValue(id, out result);
            return result;
        }
        #endregion
     

        #region save sheet to thing
        private IThingGraph GetThingGraph(IGraph<IVisual, IVisualEdge> graph) {
            var thingGraph = graph.ThingGraph();
            if (thingGraph == null) {
                throw new ArgumentException("Sheet works only on ThingGraphs");
            }

            if (thingGraph is SchemaThingGraph) {
                thingGraph = ((SchemaThingGraph)thingGraph).Source as IThingGraph;
            }

            return thingGraph;
        }

        private IThingGraph GetThingGraph(IGraphScene<IVisual, IVisualEdge> scene) {
            return GetThingGraph(scene.Graph);
        }

        IThing GetSheetThing(IThingGraph thingGraph, Id id) {
            if (id == -1)
                return null;
            IThing result = thingGraph.GetById(id);
            if (result != null) {
                if (!(result is IStreamThing && 
                    ((IStreamThing)result).StreamType == StreamTypes.LimadaSheet)) {
                    Registry.Pool.TryGetCreate<IExceptionHandler>().Catch(
                        new ArgumentException("This id does not belong to a sheet")
                        , MessageType.OK);
                }
            }
            return result;
        }

        public SceneInfo CreateSheet(IGraphScene<IVisual, IVisualEdge> scene) {
            SceneExtensions.CleanScene(scene);
            var result = new SceneInfo();
            result.State.Hollow = true;
            result.Id = Isaac.Long;
            result.Name = string.Empty;
            return result;
        }

        public bool SaveStreamInGraph(Stream source, IGraph<IVisual, IVisualEdge> target, SceneInfo info) {
            var thingGraph = GetThingGraph(target);
            var thing = GetSheetThing(thingGraph, info.Id) as IStreamThing;
            
            var content = new Content<Stream>(source, CompressionType.bZip2, StreamTypes.LimadaSheet);
            content.Description = info.Name;

            if (thing is IStreamThing || thing == null){
                var factory = target.ThingFactory();
                var result =  new ThingStreamFacade(factory).SetStream(thingGraph, thing, content)!=null;
                info.State.Hollow = false;
                info.State.Clean = true;
                return result;
            }
            
            return false;
        }

        
        public void SaveInGraph(IGraphScene<IVisual, IVisualEdge> scene, IGraphLayout<IVisual, IVisualEdge> layout, SceneInfo info) {
            var graph = GetThingGraph(scene);
            var sheetThing = GetSheetThing(graph, info.Id);
            var saved = SaveToThing(scene, layout, sheetThing, info.Name);
            info.Id = saved.Id;
            info.Name = saved.Name;
            var sheetVisual = scene.Graph.VisualOf(sheetThing);
            if (sheetVisual != null) {
                //sheetVisual.Data = info.Name;
                scene.Graph.OnChangeData(sheetVisual,info.Name);
                scene.Graph.OnDataChanged(sheetVisual);
            }
            saved.State.CopyTo(info.State);
        }

        public SceneInfo SaveToThing(IGraphScene<IVisual, IVisualEdge> scene, IGraphLayout<IVisual, IVisualEdge> layout, IThing thing, string name) {
            var result = default( SceneInfo );
            if (thing is IStreamThing || thing == null) {
                
                Content<Stream> content = new Content<Stream>(
                    new MemoryStream(), CompressionType.bZip2, StreamTypes.LimadaSheet);

                var sheet = new Sheet(scene, layout);
                sheet.Save(content.Data);
                content.Data.Position = 0;
                content.Description = name;
                
                thing = new VisualThingStreamHelper().SetStream(scene.Graph, thing, content);
                
                result = RegisterSheet(thing.Id, name);
                result.State.Hollow = false;
                result.State.Clean = true;
                result.State.CopyTo(scene.State);
                
            } else {
                throw new ArgumentException("thing must be a StreamThing");
            }
            return result;
        }

        public bool IsSaveable(IGraphScene<IVisual, IVisualEdge> scene) {
            return scene != null && scene.Graph.ThingGraph() != null;
        }



        #endregion

        #region load sheet
        public bool Load(IGraphScene<IVisual, IVisualEdge> scene, IGraphLayout<IVisual, IVisualEdge> layout, Id id) {
            var result = LoadFromStore(scene, layout, id);
            try {
                if (!result) {
                    var graph = scene.Graph.ThingGraph();

                    if (graph != null) {
                        var thing = GetSheetThing(graph, id) as IStreamThing;
                        var info = LoadFromThing(thing, scene, layout);
                        RegisterSheet(info);
                    }
                }
                return result;
            } catch {
                return false;
            }
        }

        protected void LoadFromStream(Stream source, IGraphScene<IVisual, IVisualEdge> target, IGraphLayout<IVisual, IVisualEdge> layout) {
            SceneExtensions.CleanScene(target);
            source.Position = 0;
            using (var sheet = new Sheet(target, layout)) {
                sheet.Read(source);
            } 
        }

        public SceneInfo LoadFromContent(Content<Stream> source, IGraphScene<IVisual, IVisualEdge> target, IGraphLayout<IVisual, IVisualEdge> layout) {
            var result = default(SceneInfo);
            try {
                string name = string.Empty;
                if (source.Description != null) {
                    name = source.Description.ToString();
                }

                Id id = 0;
                if (source.Source is Id) {
                    id = (Id)source.Source;
                }

                LoadFromStream(source.Data, target, layout);

                result = RegisterSheet(id, name);
                result.State.Clean = true;
                result.State.Hollow = false;
                result.State.CopyTo(target.State);
                
            } finally {
                
            }
            return result;
        }

        public SceneInfo LoadFromThing(IStreamThing source, IGraphScene<IVisual, IVisualEdge> target, IGraphLayout<IVisual, IVisualEdge> layout) {
            var result = default(SceneInfo);
            source.DeCompress();
            try {
                var content = new Content<Stream>();
                content.Description = target.Graph.Description(source);

                content.Data = source.Data;
                content.Source = source.Id;

                result = LoadFromContent(content, target, layout);

                
            } finally {
                source.ClearRealSubject ();
            }
            return result;
        }

       
        #endregion

        public virtual void VisitRegisteredSheets(Action<SceneInfo> visitor){
            foreach (var sheetInfo in Sheets.Values){
                visitor(sheetInfo);
            }
        }

        #region SheetStreams

        public bool SaveInStore(IGraphScene<IVisual, IVisualEdge> scene, IGraphLayout<IVisual, IVisualEdge> layout, Id id) {
            if (scene.Graph.Count > 0) {
                var stream = new MemoryStream();
                new Sheet(scene, layout).Save(stream);
                stream.Position = 0;

                SheetStreams[id] = stream;
                
                return true;
            } else {
                SheetStreams.Remove(id);
                return false;
            }
            
        }
        public Stream GetFromStore(Id id) {
            Stream stream = null;
            if (SheetStreams.TryGetValue(id, out stream)) {
                stream.Position = 0;
            }
            return stream;

        }

        public bool StoreContains(Id id) {
            return SheetStreams.ContainsKey(id);
        }

        public bool LoadFromStore(IGraphScene<IVisual, IVisualEdge> scene, IGraphLayout<IVisual, IVisualEdge> layout, Id id) {
            Stream stream = null;
            if (SheetStreams.TryGetValue(id, out stream)){
                LoadFromStream(stream, scene, layout);
                stream.Position = 0;
                return true;
            }
            return false;
        }

        #endregion
    }


}