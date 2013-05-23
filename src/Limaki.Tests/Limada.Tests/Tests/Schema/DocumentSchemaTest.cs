/*
 * Limaki 
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

using Limada.Model;
using Limada.Schemata;
using Limaki.Tests.Graph;
using NUnit.Framework;
using System.IO;
using System.Linq;
using Limaki.Common;
using Limaki.Model.Content;
using System;
using Limaki;
using Limaki.Tests;
using Limaki.Data;
using Limaki.Common.Linqish;
using Limaki.Model.Content.IO;

namespace Limada.Tests.Schemata {
    [TestFixture]
    public class DocumentSchemaTest : SchemaTest {
        public override Schema Schema {
            get { return new DocumentSchema(); }
        }
        [TestFixtureSetUp]
        public override void Setup() {
            base.Setup();
        }
        [Test]
        public override void TestSchema() {
            base.TestSchema();
        }

        #region Title
        public void ValidateTitle(DocumentSchema schema, IThingGraph graph, IThing thing, IThing title) {
            Assert.AreSame(schema.Title, title);

            this.ReportDetail(GraphTestUtils.ReportGraph<IThing, ILink>(graph, "* Title added"));
            bool found = false;
            bool firstMarker = false;
            foreach (ILink link in graph.Edges(thing)) {
                if (link.Marker == DocumentSchema.DocumentTitle) {
                    Assert.IsFalse(firstMarker, "second title found");
                    firstMarker = true;
                }
                if (link.Leaf == schema.Title && link.Marker == DocumentSchema.DocumentTitle
                    && link.Leaf == title) {
                    Assert.IsFalse(found);
                    found = true;
                }
            }
            Assert.IsTrue(found, "Title not found");
        }

        [Test]
        public virtual void TestTitle() {
            this.ReportDetail("**** TestTitle");
            IThingGraph graph = new ThingGraph();
            IThing thing = Factory.CreateItem();
            graph.Add(thing);

            IThing title = Factory.CreateItem("Title1");
            DocumentSchema schema = new DocumentSchema(graph, thing);

            // test the new description:
            schema.Title = title;
            ValidateTitle(schema, graph, thing, title);
            ValidateTitle(schema, graph, thing, title);

            // add same description again:
            schema.Title = title;
            ValidateTitle(schema, graph, thing, title);

            // the first description will be an orphan:
            IThing orphan = title;

            // make a new description:
            title = Factory.CreateItem("Title2");
            schema.Title = title;
            ValidateTitle(schema, graph, thing, title);

            // test if orphan is deleted:
            Assert.IsFalse(graph.Contains(orphan), "Orphan not deleted");

            // take a new schema:
            schema = new DocumentSchema(graph, thing);
            ValidateTitle(schema, graph, thing, title);

        }
        #endregion

        

        [Test]
        public virtual void TestDocumentWithPages() {
            this.ReportDetail("**** TestDocumentWithPages");
            var factory = new TestDocumentFactory();
            var graph = new SchemaThingGraph(new ThingGraph());
            var root = DocumentSchema.DocumentsRoot;
            factory.CreateDocuments(graph, root, 1);
            var docs = graph.Edges(root).Where(l => l.Marker == DocumentSchema.Document).Select(l => l.Leaf);
            foreach(var doc in docs) {
                var title = graph.ThingToDisplay(doc);
                this.ReportDetail(title.ToString());
                var pages = graph.Edges(doc).Where(l => l.Marker == DocumentSchema.DocumentPage).Select(l => l.Leaf);
                foreach(var page in pages) {
                    var number = graph.ThingToDisplay(page);
                    Assert.IsNotNull(number);
                    this.ReportDetail(number.ToString());
                }

            }
            
        }

        [Test]
        public virtual void TestDocumentWithTestData() {
            
            IThingGraph thingGraph = new ThingGraph();
            var prov = new Limada.Data.Db4oThingGraphProvider();
            prov.Open(IoInfo.FromFileName(TestLocations.GraphtestDir + "DocumentTest.limo"));
            thingGraph = prov.Data;



            this.ReportDetail("**** TestDocumentWithTestData");
            var factory = new TestDocumentFactory();
            var docSchema = new DocumentSchema();
            var graph = new SchemaThingGraph(thingGraph);
            Limada.Schemata.Schema.IdentityGraph.ForEach(s => graph.Add(s));

            var root = DocumentSchema.DocumentsRoot;
            var path = TestLocations.BlobSource;

            var document = docSchema.CreateDocument(graph, path);
            graph.Add(Factory.CreateEdge(root, document, DocumentSchema.Document));

            factory.ReadPagesFromDir(graph, document, path);

            var docs = graph.Edges(root).Where(l => l.Marker == DocumentSchema.Document).Select(l => l.Leaf);
            foreach(var doc in docs) {
                var title = graph.ThingToDisplay(doc);
                this.ReportDetail(title.ToString());
                var pages = graph.Edges(doc).Where(l => l.Marker == DocumentSchema.DocumentPage).Select(l => l.Leaf);
                foreach(var page in pages) {
                    var number = graph.ThingToDisplay(page);
                    Assert.IsNotNull(number);
                    this.ReportDetail(number.ToString());
                }

            }
            prov.Save();
        }
    }

    public class TestDocumentFactory {
        private IThingFactory _factory = null;
        public IThingFactory Factory { get { return _factory ?? (_factory = Registry.Factory.Create<IThingFactory>()); } }

        public void CreateDocuments(IThingGraph graph, IThing root, int count) {
            var docSchema = new DocumentSchema();
            for (int i = 0; i < count; i++) {

                var document = docSchema.CreateDocument(graph, null);
                docSchema.CreatePage(graph, document, null, 1);
                graph.Add(Factory.CreateEdge(root, document, DocumentSchema.Document));

               
            }

        }

        public void ReadPagesFromDir(IThingGraph graph, IThing document, string path) {
            var docSchema = new DocumentSchema();
            var imageStreamProvider = new ImageContentInStream();
            var nr = 1;
            
            foreach (var file in Directory.GetFiles(path).OrderBy(f => f)) {
                if (imageStreamProvider.InfoSink.Supports(Path.GetExtension(file))) {
                    var stream = imageStreamProvider.Read(IOUtils.UriFromFileName(file));
                    if (stream != null && stream.Data != null) {
                        docSchema.CreatePage(graph, document, stream, nr);
                        nr++;
                    }
                }
            }
        }
    }
}