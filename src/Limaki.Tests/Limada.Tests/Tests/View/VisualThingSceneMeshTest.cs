
/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Linq;
using Limada.Model;
using Limada.Schemata;
using Limada.Tests.Model;
using Limaki.Common.Linqish;
using Limaki.Model;
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.View.Visuals;
using Limaki.View.UI.GraphScene;
using Limaki.Visuals;
using NUnit.Framework;
using Xwt;

namespace Limada.Tests.View {

    public class VisualThingSceneMeshTest : SceneMeshTest<IThing, ILink> {
        private const int iDocument = 1; // Document
        private const int iPage1 = 3; // first Page (stream)
        private const int iPageNr1 = 4; // page nr 1; invisible in SchemaGraph
        private const int iDoc2PageNr1 = 2; // link from Document to PageNr1

        private const int iPage2 = 5; // first Page (stream)

        [Test]
        public void TestDeleteDigidocPage () {

            var sources = MeshTests (
                SceneTestEnvironment<IThing, ILink>.Create<DigidocSampleFactory> (7));

            var source = sources.First ();
            Assert.IsTrue (source.Source.Source.Count > 0);

            // set a document in one view
            // add the first page
            source.Expand (source.Nodes[iDocument], true);
            source.ProveViewContains (source.Nodes[iDocument]);
            source.ProveViewNotContains (source.Nodes[iPage1]);

            source.Expand (source.Nodes[iPage1], false);
            source.ProveViewContains (source.Nodes[iDocument], source.Nodes[iPage1]);

            // view the page in another display
            // delete it 
            // prove if deleted in all displays
            var pageViewer = sources.Skip (1).First ();
            pageViewer.Expand (pageViewer.Nodes[iPage1], false);
            pageViewer.ProveViewNotContains (pageViewer.Nodes[iDocument]);
            pageViewer.ProveViewContains (pageViewer.Nodes[iPage1]);

            pageViewer.Scene.Delete (pageViewer.Nodes[iPage1], null);
            pageViewer.CommandsPerform ();

            source.ProveViewNotContains (source.Nodes[iPage1]);
            pageViewer.ProveViewNotContains (pageViewer.Nodes[iPage1]);
        }

        [Test]
        public void TestDeleteDigidocDependencies () {
            // test deleting of dependencies
            var sources = MeshTests (
                SceneTestEnvironment<IThing, ILink>.Create<DigidocSampleFactory> (7));

            var source = sources.First ();

            // set a document in one view
            // add the first page
            source.Expand (source.Nodes[iDocument], true);
            source.ProveViewContains (source.Nodes[iDocument]);
            source.ProveViewNotContains (source.Nodes[iPage1]);

            source.Expand (source.Nodes[iPage1], false);
            source.ProveViewContains (source.Nodes[iDocument], source.Nodes[iPage1]);


            // view page1 and 2 in other displays
            // hide page 1
            foreach (var pageViewer in sources.Skip (1)) {
                pageViewer.Expand (pageViewer.Nodes[iPage1], false);
                pageViewer.Expand (pageViewer.Nodes[iPage2], false);
                pageViewer.ProveViewNotContains (pageViewer.Nodes[iDocument]);
                pageViewer.ProveViewContains (pageViewer.Nodes[iPage1], pageViewer.Nodes[iPage2]);

                pageViewer.SetFocused (pageViewer.Nodes[iPage1]);
                pageViewer.SceneFacade.Hide ();
                pageViewer.CommandsPerform ();

                pageViewer.ProveViewNotContains (pageViewer.Nodes[iPage1]);
                pageViewer.ProveContains (pageViewer.Source, pageViewer.Nodes[iPage1]);
            }

            // delete the document in first view
            source.Scene.Delete (source.Nodes[iDocument], null);
            source.CommandsPerform ();

            // prove if deleted in all displays
            foreach (var pageViewer in sources) {
                pageViewer.ProveViewNotContains (pageViewer.Nodes.Where (n => n != null).ToArray ());
                pageViewer.ProveNotContains (pageViewer.Source, pageViewer.Nodes.Where (n => n != null).ToArray ());

                var inner = Innerfactory (pageViewer.SampleFactory);
                var thingGraph = pageViewer.Source.Source.Unwrap ();

                pageViewer.ProveNotContains (thingGraph, inner.Nodes.Where (n => n != null).ToArray ());
            }

        }
    }
}