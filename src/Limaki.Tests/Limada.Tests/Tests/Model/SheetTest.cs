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

using System.IO;
using Limada.Model;
using Limaki.Contents;
using Limaki.Drawing.Styles;
using Limaki.Graphs;
using Limada.View;
using Limada.View.Vidgets;
using Limada.View.VisualThings;
using Limaki.Tests;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.Graph.GraphPair;
using Limaki.View;
using Limaki.View.GraphScene;
using Limaki.View.Visuals;
using Limaki.View.Viz.Visuals;
using NUnit.Framework;
using Id = System.Int64;
using Limaki.Model;
using Limaki.Drawing;
using System.Runtime.Serialization;
using System;
using Limaki.Common;
using System.Collections.Generic;
using System.Xml;
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

            ThingXmlIdSerializer serializer = new ThingXmlIdSerializer ();
            serializer.Graph = graph;
            serializer.Things = graph;

            foreach(IThing t in graph) {
                Assert.IsTrue (serializer.Things.Contains (t));
            }

            Stream s = new MemoryStream ();
            serializer.Write (s);
            s.Position = 0;
            
            serializer = new ThingXmlIdSerializer();
            serializer.Graph = graph;
            serializer.Read (s);
            foreach (IThing t in graph) {
                Assert.IsTrue(serializer.Things.Contains(t));
            }


        }


        public IGraphSceneLayout<IVisual,IVisualEdge> CreateLayout() {
            var styleSheet = StyleSheet.CreateDefaultStyleSheet ();
            var result = new VisualsSceneLayout<IVisual, IVisualEdge>(null,styleSheet);
            return result;
        }

        Stream SaveSheet (IGraphScene<IVisual, IVisualEdge> scene, IGraphSceneLayout<IVisual, IVisualEdge> layout) {
            var sheet = new Sheet(scene, layout);
            sheet.Layout.DataHandler = delegate() { return sheet.Scene; };

            var s = new MemoryStream();
            sheet.Save(s);
            s.Position = 0;
            return s;
        }

        [Test]
        public void TestSheet() {
            var sourceGraph =
                ModelHelper.GetSourceGraph<ProgrammingLanguageFactory<IGraphEntity, IGraphEdge>> ();

            var scene = new Scene();
            scene.Graph = sourceGraph;

            var thingGraph = sourceGraph.Source as IThingGraph;
            
            var layout = this.CreateLayout();
            layout.DataHandler = () => scene;

            new GraphSceneFacade<IVisual, IVisualEdge> (() => scene, layout)
                   .Add (scene.Graph, true, false);

            var s = SaveSheet (scene, layout);
            
            var reader = new StreamReader (s);
            ReportDetail (reader.ReadToEnd ());
            s.Position = 0;

            var sheet = new Sheet(new Scene(), layout);
            sheet.Layout.DataHandler = delegate() { return sheet.Scene; };

            var targetGraph = new VisualThingGraph(new VisualGraph(), thingGraph);
            sheet.Scene.Graph = targetGraph;
            s.Position = 0;
            sheet.Read (s);

            foreach(var target in targetGraph) {
                var thing = targetGraph.Get (target);
                var source = sourceGraph.Get (thing);

                Assert.AreEqual (target.Location, source.Location);
                Assert.AreEqual(target.Size, source.Size);

            }

            foreach (var source in sourceGraph) {
                var thing = sourceGraph.Get(source);
                var target = targetGraph.Get(thing);

                Assert.AreEqual(target.Location, source.Location);
                Assert.AreEqual(target.Size, source.Size);

            }
        }


        void TestScene (IGraphScene<IVisual, IVisualEdge> scene, IGraphSceneLayout<IVisual, IVisualEdge> layout, Stream s) {
            
            
            var sheetManager = new SheetManager();
            scene.CleanScene();

            using (var sheet = new Sheet(scene, layout)) {
                s.Position = 0;
                sheet.Read(s);
            }

            var visualThingGraph = scene.Graph.Source<IVisual, IVisualEdge, IThing, ILink>();

            foreach(var visual in scene.Elements) {
                var thing = visualThingGraph.Get (visual);
                Assert.IsNotNull (thing);
            }
        }

        [Test]
        public void TestSheetManager() {
            var scene = new Scene();

            scene.Graph = ModelHelper.GetSourceGraph<EntityProgrammingLanguageFactory>();
            if (!(scene.Graph is SubGraph<IVisual,IVisualEdge>)){
                scene.Graph = new SubGraph<IVisual, IVisualEdge> (new VisualGraph (), scene.Graph);
            }

            var layout = this.CreateLayout();

            var s = SaveSheet(scene, layout);

            TestScene(scene, layout,s);

            TestScene(scene, layout, s);

            TestScene(scene, layout, s);

        }

        [Test]
        public void TestDataContractSerializer() {
            var sourceGraph = ModelHelper.GetSourceGraph<EntityProgrammingLanguageFactory>(1);
            var thingGraph = sourceGraph.Source as IThingGraph;

            var s = new MemoryStream();

            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            
            var writer =
                //System.Runtime.Serialization.Json.JsonReaderWriterFactory.CreateJsonWriter (s);
                XmlDictionaryWriter.CreateBinaryWriter(s);
                //XmlDictionaryWriter.CreateDictionaryWriter (XmlWriter.Create (s, settings));


            writer.WriteStartElement ("root");
            var ser = new DataContractSerializer(factory.Clazz<IThing>(), factory.KnownClasses);
            
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
            var sourceGraph = ModelHelper.GetSourceGraph<EntityProgrammingLanguageFactory>(10);
            var thingGraph = sourceGraph.Source as IThingGraph;
            
            var dataStream = new StreamWriter (new MemoryStream ());
            var streamContent = "This is the streamcontent";
            dataStream.Write(streamContent);
            dataStream.Flush ();

            var streamThing = factory.CreateItem<Stream>(null) as IStreamThing;
            thingGraph.Add(streamThing);

            var streamId = streamThing.Id;
            streamThing.StreamType = ContentTypes.Text;
            streamThing.Compression = CompressionType.bZip2;
            streamThing.Data = dataStream.BaseStream;


            streamThing.Flush();
            streamThing.ClearRealSubject ();

            var s = new MemoryStream();

            var serializer = new ThingXmlSerializer();
            serializer.Graph = thingGraph;
            
            int thingCount = 0;
            foreach (var thing in thingGraph) {
                serializer.Things.Add (thing);
                thingCount++;
            }

            Assert.AreEqual (thingCount, serializer.Things.Count);
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
            var reader = new StreamReader(s);
            this.ReportDetail(reader.ReadToEnd());

            s.Position = 0;
            serializer = new ThingXmlSerializer();
            serializer.Graph = new ThingGraph();
            serializer.Read (s);

            Assert.AreEqual(thingCount, serializer.Things.Count);

            serializer.Graph.AddRange(serializer.Things);

            var data = serializer.Graph.ContentContainer.GetById(streamId);
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

            scene.Graph = ModelHelper.GetSourceGraph<EntityProgrammingLanguageFactory>();
            if (!(scene.Graph is SubGraph<IVisual, IVisualEdge>)) {
                scene.Graph = new SubGraph<IVisual, IVisualEdge>(scene.Graph,new VisualGraph());
            }

            var layout = this.CreateLayout();
            var thingGraph = scene.Graph.ThingGraph();

            var info = new SceneInfo { Name = "TestFavoriteManagerAddToSheets" };
            var sheetManager = new SheetManager();
            sheetManager.SaveInGraph(scene, layout, info);
            Assert.IsTrue(info.State.Clean);
            var sheetThing = thingGraph.GetById(info.Id);
            Assert.IsNotNull(sheetThing);
            
            new FavoriteManager().AddToSheets(scene.Graph, info.Id);

            var sheets = thingGraph.GetById(TopicSchema.Sheets.Id);
            Assert.IsNotNull(sheets);
        }
    }
}
