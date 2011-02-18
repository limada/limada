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

using System.IO;
using Limada.Model;
using Limada.View;
using Limaki.Graphs;
using Limaki.Tests;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.Graph.Wrappers;
using Limaki.Widgets;
using NUnit.Framework;
using Id = System.Int64;
using Limaki.Model;
using Limaki.Drawing;
using Limaki.Graphs.Extensions;
using System.Runtime.Serialization;
using System;
using Limaki.Common;
using System.Collections.Generic;
using System.Xml;
using Limaki.Model.Streams;
using Limaki.Presenter.Layout;
using Limada.Presenter;
using Limaki.Presenter.Widgets.UI;
using Limaki.Presenter.Widgets.Layout;
using Limada.Schemata;

namespace Limada.Tests.Model {
    public class SheetTest : DomainTest {
        private IThingFactory _factory = null;
        public IThingFactory factory { get { return _factory ?? (_factory = Registry.Factory.Create<IThingFactory>()); } }

        [Test]
        public void ThingIdSerializerTest() {
            
            IThingGraph graph = new ThingGraph ();
            IThing thing = factory.CreateItem();
            graph.Add (thing);
            graph.Add (factory.CreateItem());

            ThingIdSerializer serializer = new ThingIdSerializer ();
            serializer.Graph = graph;
            serializer.ThingCollection = graph;

            foreach(IThing t in graph) {
                Assert.IsTrue (serializer.ThingCollection.Contains (t));
            }

            Stream s = new MemoryStream ();
            serializer.Write (s);
            s.Position = 0;
            
            serializer = new ThingIdSerializer();
            serializer.Graph = graph;
            serializer.Read (s);
            foreach (IThing t in graph) {
                Assert.IsTrue(serializer.ThingCollection.Contains(t));
            }


        }


        public IGraphLayout<IWidget,IEdgeWidget> GetLayout() {
            var styleSheet = StyleSheet.CreateDefaultStyleSheet ();
            var result = new WidgetLayout<IWidget, IEdgeWidget>(null,styleSheet);
            return result;
        }
        
        Stream SaveSheet(Scene scene, IGraphLayout<IWidget,IEdgeWidget> layout) {
            Sheet sheet = new Sheet(scene, layout);
            sheet.Layout.DataHandler = delegate() { return sheet.Scene; };

            Stream s = new MemoryStream();
            sheet.Save(s);
            s.Position = 0;
            return s;
        }

        [Test]
        public void TestSheet() {
            WidgetThingGraph sourceGraph = 
                ModelHelper.GetSourceGraph<ProgrammingLanguageFactory> ();

            Scene scene = new Scene();
            scene.Graph = sourceGraph;

            IThingGraph thingGraph = sourceGraph.Two as IThingGraph;
            
            IGraphLayout<IWidget,IEdgeWidget> layout = this.GetLayout();

            Stream s = SaveSheet (scene, layout);


            StreamReader reader = new StreamReader (s);
            ReportDetail (reader.ReadToEnd ());
            s.Position = 0;

            Sheet sheet = new Sheet(new Scene(), layout);
            sheet.Layout.DataHandler = delegate() { return sheet.Scene; };

            WidgetThingGraph targetGraph = new WidgetThingGraph(new WidgetGraph(), thingGraph);
            sheet.Scene.Graph = targetGraph;
            s.Position = 0;
            sheet.Read (s);

            foreach(IWidget target in targetGraph) {
                IThing thing = targetGraph.Get (target);
                IWidget source = sourceGraph.Get (thing);

                Assert.AreEqual (target.Location, source.Location);
                Assert.AreEqual(target.Size, source.Size);

            }

            foreach (IWidget source in sourceGraph) {
                IThing thing = sourceGraph.Get(source);
                IWidget target = targetGraph.Get(thing);

                Assert.AreEqual(target.Location, source.Location);
                Assert.AreEqual(target.Size, source.Size);

            }
        }


        void TestScene(Scene scene, IGraphLayout<IWidget,IEdgeWidget> layout, Stream s) {
            
            
            SheetManager sheetManager = new SheetManager();
            SceneTools.CleanScene(scene);

            using (Sheet sheet = new Sheet(scene, layout)) {
                s.Position = 0;
                sheet.Read(s);
            }
            
            IGraphPair<IWidget, IThing, IEdgeWidget, ILink> widgetThingGraph =
                new GraphPairFacade<IWidget, IEdgeWidget>().Source<IThing, ILink>(scene.Graph);

            foreach(IWidget widget in scene.Elements) {
                IThing thing = widgetThingGraph.Get (widget);
                Assert.IsNotNull (thing);
            }
        }

        [Test]
        public void TestSheetManager() {
            Scene scene = new Scene();

            scene.Graph = ModelHelper.GetSourceGraph<ProgrammingLanguageFactory>();
            if (!(scene.Graph is GraphView<IWidget,IEdgeWidget>)){
                scene.Graph = new GraphView<IWidget, IEdgeWidget> (new WidgetGraph (), scene.Graph);
            }

            IGraphLayout<IWidget,IEdgeWidget> layout = this.GetLayout();

            Stream s = SaveSheet(scene, layout);

            TestScene(scene, layout,s);

            TestScene(scene, layout, s);

            TestScene(scene, layout, s);

        }

        [Test]
        public void TestDataContractSerializer() {
            WidgetThingGraph sourceGraph =
                ModelHelper.GetSourceGraph<ProgrammingLanguageFactory>(1);
            IThingGraph thingGraph = sourceGraph.Two as IThingGraph;

            Stream s = new MemoryStream();

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            
            var writer =
                //System.Runtime.Serialization.Json.JsonReaderWriterFactory.CreateJsonWriter (s);
                XmlDictionaryWriter.CreateBinaryWriter(s);
                //XmlDictionaryWriter.CreateDictionaryWriter (XmlWriter.Create (s, settings));


            writer.WriteStartElement ("root");
            DataContractSerializer ser = 
                new DataContractSerializer(factory.Clazz<IThing>(), factory.KnownClasses);
            
            int thingCount = 0;
            foreach (var thing in thingGraph) {
                //ser.WriteStartObject (writer, thing);
                //ser.WriteObjectContent (writer, thing);

                    ser.WriteObject (writer, thing);
                    thingCount++;
                
            }



            writer.WriteEndElement ();
            writer.Flush ();

            s.Position = 0;
            //StreamReader reader = new StreamReader (s);
            //this.ReportDetail(reader.ReadToEnd ());
            this.ReportDetail (string.Format("Stream.Length={0}",s.Length));
            this.ReportDetail(string.Format("Thing.Count={0}", thingCount));

            s.Position = 0;
            thingCount = 0;
            var xreader =
                //System.Runtime.Serialization.Json.JsonReaderWriterFactory.CreateJsonReader (s,XmlDictionaryReaderQuotas.Max);
                XmlDictionaryReader.CreateBinaryReader(s, XmlDictionaryReaderQuotas.Max);
                //XmlDictionaryReader.CreateTextReader(s, XmlDictionaryReaderQuotas.Max);

            xreader.ReadStartElement ();
           
            while (ser.IsStartObject(xreader)) {
                IThing thing = ser.ReadObject (xreader) as IThing;
                thingCount++;
            }
            this.ReportDetail(string.Format("Thing.Count={0}", thingCount));
        }


        [Test]
        public void TestThingSerializer() {
            WidgetThingGraph sourceGraph =
                ModelHelper.GetSourceGraph<ProgrammingLanguageFactory>(10);

            IThingGraph thingGraph = sourceGraph.Two as IThingGraph;

            
            var dataStream = new StreamWriter (new MemoryStream ());
            var streamContent = "This is the streamcontent";
            dataStream.Write(streamContent);
            dataStream.Flush ();

            IStreamThing streamThing = factory.CreateItem<Stream>(null) as IStreamThing;
            thingGraph.Add(streamThing);

            var streamId = streamThing.Id;
            streamThing.StreamType = StreamTypes.ASCII;
            streamThing.Compression = CompressionType.bZip2;
            streamThing.Data = dataStream.BaseStream;


            streamThing.Flush();
            streamThing.ClearRealSubject ();

            Stream s = new MemoryStream();

            ThingSerializer serializer = new ThingSerializer();
            serializer.Graph = thingGraph;
            
            int thingCount = 0;
            foreach (var thing in thingGraph) {
                serializer.ThingCollection.Add (thing);
                thingCount++;
            }

            Assert.AreEqual (thingCount, serializer.ThingCollection.Count);
            serializer.Write (s);
            this.ReportDetail(string.Format("Stream.Length={0}", s.Length));
            this.ReportDetail(string.Format("Thing.Count={0}", thingCount));
            thingGraph.Clear();
            thingGraph = null;
            serializer = null;

            var compressionWorker = Registry.Factory.Create<ICompressionWorker>();
            var compressed = compressionWorker.Compress(s, CompressionType.bZip2);
            this.ReportDetail(string.Format("CompressedStream.Length={0}", compressed.Length));

            compressed.Dispose ();
            compressed = null;

            // readtest


            s.Position = 0;
            StreamReader reader = new StreamReader(s);
            this.ReportDetail(reader.ReadToEnd());

            s.Position = 0;
            serializer = new ThingSerializer();
            serializer.Graph = new ThingGraph();
            serializer.Read (s);

            Assert.AreEqual(thingCount, serializer.ThingCollection.Count);

            serializer.Graph.AddRange(serializer.ThingCollection);

            var data = serializer.Graph.DataContainer.GetById(streamId);
            Assert.IsNotNull (data);

            streamThing = serializer.Graph.GetById (streamId) as IStreamThing;
            Assert.IsNotNull(streamThing);
            streamThing.DeCompress ();

            using (var streamReader = new StreamReader(streamThing.Data)) {
                var resultContent = streamReader.ReadToEnd ();
                Assert.AreEqual(resultContent, streamContent);
            }

            streamThing.ClearRealSubject ();
        }
        [Test]
        public void TestFavoriteManagerAddToSheets() {
            var scene = new Scene();

            scene.Graph = ModelHelper.GetSourceGraph<ProgrammingLanguageFactory>();
            if (!(scene.Graph is GraphView<IWidget, IEdgeWidget>)) {
                scene.Graph = new GraphView<IWidget, IEdgeWidget>(scene.Graph,new WidgetGraph());
            }

            var layout = this.GetLayout();
            var thingGraph = scene.Graph.ThingGraph();

            var info = new SheetManager().SaveInGraph(scene, layout, new SheetInfo {Name = "TestFavoriteManagerAddToSheets"});
            var sheetThing = thingGraph.GetById(info.Id);
            Assert.IsNotNull(sheetThing);
            
            new FavoriteManager().AddToSheets(scene.Graph, info.Id);

            var sheets = thingGraph.GetById(TopicSchema.Sheets.Id);
            Assert.IsNotNull(sheets);
        }
    }
}
