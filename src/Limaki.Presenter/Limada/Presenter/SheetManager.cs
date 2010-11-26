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

namespace Limada.Presenter {

    public class SheetManager : ISheetManager {

        #region SheetInfo
        private IDictionary<Id, SheetInfo> _sheets = null;
        /// <summary>
        /// names to thing.ids of loaded and saved sheets
        /// </summary>
        protected IDictionary<Id, SheetInfo> sheets {
            get {
                if (_sheets == null) {
                    _sheets = new Dictionary<Id, SheetInfo>();
                }
                return _sheets;
            }
            set { _sheets = value; }
        }

        public SheetInfo RegisterSheet(Id id, string name) {
            SheetInfo result = default( SheetInfo );
            if (id !=0 && sheets.ContainsKey(id)) {
                result = sheets[id];
                result.Name = name;
            } else {
                if (id == 0) {
                    id = Isaac.Long;
                }
                result = new SheetInfo() { Id = id, Name = name };
                sheets[id] = result;
            }
            
            return result;
        }

        public void Clear() {
            sheets = null;
        }


        public SheetInfo GetSheetInfo(Id id) {
            SheetInfo result = new SheetInfo() { Id = id };
            sheets.TryGetValue (id, out result);
            return result;
        }
        #endregion
     

        #region save sheet to thing
        
        private IThingGraph GetThingGraph(Scene scene) {
            var thingGraph = WidgetThingGraphExtension.GetThingGraph(scene.Graph);
            if (thingGraph == null) {
                throw new ArgumentException("Sheet works only on ThingGraphs");
            }

            if (thingGraph is SchemaThingGraph) {
                thingGraph = ((SchemaThingGraph)thingGraph).Source as IThingGraph;
            }

            return thingGraph;
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

        public SheetInfo SaveToThing(Scene scene, IGraphLayout<IWidget,IEdgeWidget> layout, SheetInfo info) {
            var graph = GetThingGraph(scene);
            IThing sheetThing = GetSheetThing(graph, info.Id);
            return SaveToThing (scene, layout, sheetThing,info.Name);
        }



        public SheetInfo SaveToThing(Scene scene, IGraphLayout<IWidget,IEdgeWidget> layout, IThing thing, string name) {
            var result = default( SheetInfo );
            if (thing is IStreamThing || thing == null) {
                
                StreamInfo<Stream> streamInfo = new StreamInfo<Stream>(
                    new MemoryStream(), CompressionType.bZip2, StreamTypes.LimadaSheet);

                
                Sheet sheet = new Sheet(scene, layout);
                sheet.Save(streamInfo.Data);
                streamInfo.Data.Position = 0;
                streamInfo.Description = name;
                
                thing = new WidgetThingStreamHelper().SetStream(scene.Graph, thing, streamInfo);
                result = RegisterSheet(thing.Id, name);
                result.Persistent = true;

            } else {
                throw new ArgumentException("thing must be a StreamThing");
            }
            return result;
        }

        public bool IsSaveable(Scene scene) {
            return scene != null && WidgetThingGraphExtension.GetThingGraph(scene.Graph) != null;
        }



        #endregion

        #region load sheet
        

        

        public void LoadSheet(Scene scene, IGraphLayout<IWidget,IEdgeWidget> layout, Stream stream) {
            SceneTools.CleanScene(scene);
            using (Sheet sheet = new Sheet(scene, layout)) {
                sheet.Read(stream);
            } 
        }

        public SheetInfo LoadSheet(Scene scene, IGraphLayout<IWidget,IEdgeWidget> layout, StreamInfo<Stream> info) {
            var result = default(SheetInfo);
            try {
                string name = string.Empty;
                if (info.Description != null) {
                    name = info.Description.ToString();
                }

                Id id = 0;
                if (info.Source is Id) {
                    id = (Id)info.Source;
                }

                LoadSheet(scene, layout, info.Data);

                result = RegisterSheet(id, name);
                result.Persistent = true;
            } finally {
                
            }
            return result;
        }

        public SheetInfo LoadSheet(Scene scene, IGraphLayout<IWidget,IEdgeWidget> layout, IStreamThing thing) {
            var result = default(SheetInfo);
            thing.DeCompress();
            try {
                var info = new StreamInfo<Stream>();
                info.Description = WidgetThingGraphExtension.GetDescription(scene.Graph, thing);

                info.Data = thing.Data;
                info.Source = thing.Id;

                result = LoadSheet(scene, layout, info);
            } finally {
                thing.ClearRealSubject ();
            }
            return result;
        }

        #endregion

 
    }


}