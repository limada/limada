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
using Limaki.Contents.IO;

namespace Limada.Tests.Schemata {
    [TestFixture]
    public class DigidocTest : SchemaTest {
        public override Schema Schema {
            get { return new DigidocSchema(); }
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
        public void ValidateTitle(DigidocSchema schema, IThingGraph graph, IThing thing, IThing title) {
            Assert.AreSame(schema.Title, title);

            this.ReportDetail(GraphTestUtils.ReportGraph<IThing, ILink>(graph, "* Title added"));
            bool found = false;
            bool firstMarker = false;
            foreach (ILink link in graph.Edges(thing)) {
                if (link.Marker == DigidocSchema.DocumentTitle) {
                    Assert.IsFalse(firstMarker, "second title found");
                    firstMarker = true;
                }
                if (link.Leaf == schema.Title && link.Marker == DigidocSchema.DocumentTitle
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
            var digidoc = new DigidocSchema(graph, thing);

            // test the new description:
            digidoc.Title = title;
            ValidateTitle(digidoc, graph, thing, title);
            ValidateTitle(digidoc, graph, thing, title);

            // add same description again:
            digidoc.Title = title;
            ValidateTitle(digidoc, graph, thing, title);

            // the first description will be an orphan:
            IThing orphan = title;

            // make a new description:
            title = Factory.CreateItem("Title2");
            digidoc.Title = title;
            ValidateTitle(digidoc, graph, thing, title);

            // test if orphan is deleted:
            Assert.IsFalse(graph.Contains(orphan), "Orphan not deleted");

            // take a new schema:
            digidoc = new DigidocSchema(graph, thing);
            ValidateTitle(digidoc, graph, thing, title);

        }
        #endregion

        

        [Test]
        public virtual void TestDocumentWithPages() {
            this.ReportDetail("**** TestDocumentWithPages");
            var factory = new TestDocumentFactory();
            var graph = new SchemaThingGraph(new ThingGraph());
            var root = DigidocSchema.DocumentsRoot;
            factory.CreateDocuments(graph, root, 1);
            var docs = graph.Edges(root).Where(l => l.Marker == DigidocSchema.Document).Select(l => l.Leaf);
            foreach(var doc in docs) {
                var title = graph.ThingToDisplay(doc);
                this.ReportDetail(title.ToString());
                var pages = graph.Edges(doc).Where(l => l.Marker == DigidocSchema.DocumentPage).Select(l => l.Leaf);
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
            var prov = new Limada.Data.Db4oThingGraphIo();
            var d = prov.Open(Iori.FromFileName(TestLocations.GraphtestDir + "DocumentTest.limo"));
            thingGraph = d.Data;



            this.ReportDetail("**** TestDocumentWithTestData");
            var factory = new TestDocumentFactory();
            var digidoc = new DigidocSchema();
            var graph = new SchemaThingGraph(thingGraph);
            Limada.Schemata.Schema.IdentityGraph.ForEach(s => graph.Add(s));

            var root = DigidocSchema.DocumentsRoot;
            var path = TestLocations.BlobSource;

            var document = digidoc.CreateDocument(graph, path);
            graph.Add(Factory.CreateEdge(root, document, DigidocSchema.Document));

            factory.ReadPagesFromDir(graph, document, path);

            var docs = graph.Edges(root).Where(l => l.Marker == DigidocSchema.Document).Select(l => l.Leaf);
            foreach(var doc in docs) {
                var title = graph.ThingToDisplay(doc);
                this.ReportDetail(title.ToString());
                var pages = graph.Edges(doc).Where(l => l.Marker == DigidocSchema.DocumentPage).Select(l => l.Leaf);
                foreach(var page in pages) {
                    var number = graph.ThingToDisplay(page);
                    Assert.IsNotNull(number);
                    this.ReportDetail(number.ToString());
                }

            }
            prov.Flush(d);
        }
    }

    public class TestDocumentFactory {
        private IThingFactory _factory = null;
        public IThingFactory Factory { get { return _factory ?? (_factory = Registry.Factory.Create<IThingFactory>()); } }

        public void CreateDocuments(IThingGraph graph, IThing root, int count) {
            var digidoc = new DigidocSchema();
            for (int i = 0; i < count; i++) {

                var document = digidoc.CreateDocument(graph, null);
                digidoc.CreatePage(graph, document, null, 1);
                graph.Add(Factory.CreateEdge(root, document, DigidocSchema.Document));

               
            }

        }

        public void ReadPagesFromDir(IThingGraph graph, IThing document, string path) {
            var digidoc = new DigidocSchema();
            var imageStreamProvider = new ImageStreamContentIo();
            var nr = 1;
            
            foreach (var file in Directory.GetFiles(path).OrderBy(f => f)) {
                if (imageStreamProvider.Detector.Supports(Path.GetExtension(file))) {
                    var stream = imageStreamProvider.ReadContent(IoUtils.UriFromFileName(file));
                    if (stream != null && stream.Data != null) {
                        digidoc.CreatePage(graph, document, stream, nr);
                        nr++;
                    }
                }
            }
        }
    }
}