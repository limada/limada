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

using Limaki.Common.Linqish;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.View.Visuals;
using NUnit.Framework;
using System.Linq;
using Xwt;

namespace Limaki.Tests.View.Visuals {

    public class EntitySceneMeshTest : SceneMeshTest<IGraphEntity, IGraphEdge> {

        [Test]
        public void EdgeAddAndChange () {

            var sources = MeshTests (
                SceneTestEnvironment<IGraphEntity, IGraphEdge>.Create<ProgrammingLanguageFactory<IGraphEntity, IGraphEdge>> (7));

            var source = sources.First ();

            var iNet = 4; // .Net
            var iProgramming = 1; // Programming
            var iJava = 3; // Java 

            var iSink = 0;

            // expand Programming
            source.Expand (source.Nodes[iProgramming], true);

            foreach (var sink in sources.Skip (1)) {
                iSink++;

                sink.SetFocused (sink.Nodes[iNet]); 
                sink.SceneFacade.CollapseToFocused ();

                // add and expand Programming
                sink.Expand (sink.Nodes[iProgramming], false);

                // test if .Net and Programming in sinkView
                sink.ProveViewContains (sink.Nodes[iNet], sink.Nodes[iProgramming]);

                // make a new link, add it to source
            
                var sourceEdge = source.AddEdge (source.Nodes[iNet], source.Nodes[iProgramming]);
                source.ProveViewContains (sourceEdge);
                var backEdge = source.Source.Get (sourceEdge) as IGraphEdge;

                // testing the sinkGraph
                Assert.IsTrue (sink.Source.Source.Contains (backEdge));

                // adding to view
                source.SceneFacade.Add (sourceEdge, new Point (10, 10));
                source.Display.Perform ();

                // testing the sinkView
                var sinkEdge = sink.Source.Get (backEdge) as IVisualEdge;
                Assert.IsNotNull (sinkEdge);

                sink.ProveViewContains (sinkEdge);

                // change the link in sink

                // not toggle with each test as the edge is deleted
                var iNewRoot = iJava; // i % 2 == 1 ? iJava : iNet;
                var iOldRoot = iNet; // i % 2 == 1 ? iNet : iJava;

                var sinkOldRoot = sink.ChangeLink (sinkEdge, sink.Nodes[iNewRoot], true);

                // test if changed 
                sink.ProveChangedEdge (sinkEdge, sink.Nodes[iNewRoot], sinkOldRoot, true);
                source.ProveChangedEdge (sourceEdge, source.Nodes[iNewRoot], source.Nodes[iOldRoot], true);

                sources.ForEach (t => {
                    var tEdge = t.Source.Get (backEdge) as IVisualEdge;
                    t.ProveChangedEdge (tEdge, t.Nodes[iNewRoot], t.Nodes[iOldRoot], true,
                        // there could be a display without visible root and leaf
                        t.Scene.Contains (tEdge.Root) && t.Scene.Contains (tEdge.Leaf)
                        );
                });

                // remove in sink
                sink.RemoveEdge (sinkEdge);

                sink.ProveViewNotContains (sinkEdge);
                sink.ProveNotContains (sink.Source, sinkEdge);
                source.ProveViewNotContains (sourceEdge);

                Assert.IsFalse (sink.Source.Source.Contains (backEdge));

                sources.ForEach (t => {
                    var tEdge = t.Source.Get (backEdge) as IVisualEdge;
                    Assert.IsNull (tEdge);
                    Assert.IsFalse (t.Source.Any (e => e.Data == sourceEdge.Data));
                });

                sink.Expand (sink.Nodes[iProgramming], true);
                sink.ProveViewNotContains (sinkEdge);
                sink.SetFocused (sink.Nodes[iNet]);
                sink.SceneFacade.CollapseToFocused ();
            }
        }

        [Test]
        public void RegisterMesh () {

            var sources = MeshTests ( SceneTestEnvironment<IGraphEntity, IGraphEdge>
                .Create<ProgrammingLanguageFactory<IGraphEntity, IGraphEdge>> (5));

            var source = sources.First ();
            foreach (var sink in sources.Skip (1)) {
                Assert.AreSame (source.Scene, source.Display.Data);
                Assert.AreSame (sink.Scene, sink.Display.Data);
                Assert.AreSame (source.Source.Source, sink.Source.Source);

                Assert.IsTrue (Mesh.Scenes.Contains (source.Scene));
                Assert.IsTrue (Mesh.Scenes.Contains (sink.Scene));

                Assert.AreEqual (source.Nodes.Count, sink.Nodes.Count);
                Assert.AreEqual (source.Edges.Count, sink.Edges.Count);
                source = sink;
            }
        }


        [Test]
        public void EdgeAddChangeRemove () {
            var iNet = 4; // .Net
            var iProgramming = 1; // Programming
            var iLanguage = 2; // Language
            var iLibraries = 5; // Libraries
            var iJava = 3; // Java 

            var Programming2Language = 101;  //[Programming->Language]
            var Programming2Libraries = 104; //[Programming->Libraries]

            var source = new SceneTestEnvironment<IGraphEntity, IGraphEdge, ProgrammingLanguageFactory<IGraphEntity, IGraphEdge>> ();
            EdgeAddChangeRemove (source, iProgramming, iNet, iJava);
            EdgeAddChangeRemove (source, iProgramming, iNet, iJava);
            EdgeAddChangeRemove (source, iLibraries, iLanguage, iJava);
           
        }

         }
}