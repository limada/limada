/*
 * Limada 
 * Version 0.09
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 */


using System;
using System.Collections.Generic;
using System.IO;
using Limada.Common;
using Limada.Model;
using Limada.Schemata;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Model.Streams;
using Limaki.Widgets;
using Id = System.Int64;
using Limaki.Presenter.Widgets.UI;
using Limada.View;
using Limaki.Graphs;

namespace Limada.Presenter {

    public class SheetManager : ISheetManager {

        #region SheetInfo
        private IDictionary<Id, SheetInfo> _sheets = null;
        /// <summary>
        /// names to thing.ids of loaded and saved sheets
        /// </summary>
        protected IDictionary<Id, SheetInfo> Sheets {
            get {
                if (_sheets == null) {
                    _sheets = new Dictionary<Id, SheetInfo>();
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

        public SheetInfo RegisterSheet(Id id, string name) {
            SheetInfo result = default( SheetInfo );
            if (id !=0 && Sheets.ContainsKey(id)) {
                result = Sheets[id];
                if (!string.IsNullOrEmpty(name))
                    result.Name = name;
            } else {
                if (id == 0) {
                    id = Isaac.Long;
                }
                result = new SheetInfo() { Id = id, Name = name };
                result.State.Hollow = true;
                Sheets[id] = result;
            }
            
            return result;
        }

        public void Clear() {
            Sheets = null;
            SheetStreams = null;
        }

        public SheetInfo GetSheetInfo(Id id) {
            var result = default(SheetInfo);
            Sheets.TryGetValue(id, out result);
            return result;
        }
        #endregion
     

        #region save sheet to thing
        private IThingGraph GetThingGraph(IGraph<IWidget, IEdgeWidget> graph) {
            var thingGraph = graph.ThingGraph();
            if (thingGraph == null) {
                throw new ArgumentException("Sheet works only on ThingGraphs");
            }

            if (thingGraph is SchemaThingGraph) {
                thingGraph = ((SchemaThingGraph)thingGraph).Source as IThingGraph;
            }

            return thingGraph;
        }

        private IThingGraph GetThingGraph(IGraphScene<IWidget, IEdgeWidget> scene) {
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

        public bool SaveStreamInGraph(Stream source, IGraph<IWidget, IEdgeWidget> target, SheetInfo info) {
            var thingGraph = GetThingGraph(target);
            var thing = GetSheetThing(thingGraph, info.Id) as IStreamThing;
            StreamInfo<Stream> streamInfo = new StreamInfo<Stream>(
                    source, CompressionType.bZip2, StreamTypes.LimadaSheet);
            streamInfo.Description = info.Name;
            if (thing is IStreamThing || thing == null){
                var factory = target.ThingFactory();
                var result =  new ThingStreamFacade(factory).SetStream(thingGraph, thing, streamInfo)!=null;
                info.State.Hollow = false;
                info.State.Clean = true;
                return result;
            }
            
            return false;
        }

        public SheetInfo SaveInGraph(IGraphScene<IWidget, IEdgeWidget> scene, IGraphLayout<IWidget, IEdgeWidget> layout, SheetInfo info) {
            var graph = GetThingGraph(scene);
            IThing sheetThing = GetSheetThing(graph, info.Id);
            return SaveToThing (scene, layout, sheetThing,info.Name);
        }



        public SheetInfo SaveToThing(IGraphScene<IWidget, IEdgeWidget> scene, IGraphLayout<IWidget, IEdgeWidget> layout, IThing thing, string name) {
            var result = default( SheetInfo );
            if (thing is IStreamThing || thing == null) {
                
                StreamInfo<Stream> streamInfo = new StreamInfo<Stream>(
                    new MemoryStream(), CompressionType.bZip2, StreamTypes.LimadaSheet);

                var sheet = new Sheet(scene, layout);
                sheet.Save(streamInfo.Data);
                streamInfo.Data.Position = 0;
                streamInfo.Description = name;
                
                thing = new WidgetThingStreamHelper().SetStream(scene.Graph, thing, streamInfo);
                
                result = RegisterSheet(thing.Id, name);
                result.State.Hollow = false;
                result.State.Clean = true;
                result.State.CopyTo(scene.State);
                
            } else {
                throw new ArgumentException("thing must be a StreamThing");
            }
            return result;
        }

        public bool IsSaveable(IGraphScene<IWidget, IEdgeWidget> scene) {
            return scene != null && scene.Graph.ThingGraph() != null;
        }



        #endregion

        #region load sheet

        public void LoadFromStream(Stream source, IGraphScene<IWidget, IEdgeWidget> target, IGraphLayout<IWidget, IEdgeWidget> layout) {
            SceneTools.CleanScene(target);
            using (Sheet sheet = new Sheet(target, layout)) {
                sheet.Read(source);
            } 
        }

        public SheetInfo LoadFromStreamInfo(StreamInfo<Stream> source, IGraphScene<IWidget, IEdgeWidget> target, IGraphLayout<IWidget, IEdgeWidget> layout) {
            var result = default(SheetInfo);
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

        public SheetInfo LoadFromThing(IStreamThing source, IGraphScene<IWidget, IEdgeWidget> target, IGraphLayout<IWidget, IEdgeWidget> layout) {
            var result = default(SheetInfo);
            source.DeCompress();
            try {
                var info = new StreamInfo<Stream>();
                info.Description = target.Graph.Description(source);

                info.Data = source.Data;
                info.Source = source.Id;

                result = LoadFromStreamInfo(info, target, layout);

                
            } finally {
                source.ClearRealSubject ();
            }
            return result;
        }

       
        #endregion

        public virtual void VisitRegisteredSheets(Action<SheetInfo> visitor){
            foreach (var sheetInfo in Sheets.Values){
                visitor(sheetInfo);
            }
        }

        #region SheetStreams
        public bool StoreInStreams(IGraphScene<IWidget, IEdgeWidget> scene, IGraphLayout<IWidget, IEdgeWidget> layout, Id id) {
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
        public Stream GetFromStreams(Id id) {
            Stream stream = null;
            if(SheetStreams.TryGetValue(id, out stream)){
                stream.Position = 0;
            }
            return stream;

        }
        public bool LoadFromStreams(IGraphScene<IWidget, IEdgeWidget> scene, IGraphLayout<IWidget, IEdgeWidget> layout, Id id) {
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