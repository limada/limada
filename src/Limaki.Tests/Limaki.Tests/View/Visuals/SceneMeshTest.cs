/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014-2015 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Common.Linqish;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.View.Visuals;
using NUnit.Framework;
using System.Linq;
using Xwt;
using System.Collections.Generic;

namespace Limaki.Tests.View.Visuals {

    public class SceneMeshTest<TItem, TEdge> : SceneMeshTestBase<TItem, TEdge>
    where TEdge : IEdge<TItem>, TItem {

        int iProgramming = 1; 
        int iLanguage = 2; 
        int iJava = 3; 
        int iNet = 4; 
        int iLibraries = 5;
        int iCollections = 6;

        int iProgrammingLanguage = 1;
        int iProgrammingLanguageJava = 2;
        int iProgrammingLanguageNet = 3;

        [Test]
        public void EdgeAddAndChange () {

            var sources = MeshTests (
                SceneTestEnvironment<TItem, TEdge>.Create<ProgrammingLanguageFactory<TItem, TEdge>> (7));

            var source = sources.First ();

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
                var backEdge = (TEdge) (object) source.Source.Get (sourceEdge);

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
                    Assert.IsFalse (t.Source.OfType<IVisualEdge>().Any (e => e.Data == sourceEdge.Data));
                });

                sink.Expand (sink.Nodes[iProgramming], true);
                sink.ProveViewNotContains (sinkEdge);
                sink.SetFocused (sink.Nodes[iNet]);
                sink.SceneFacade.CollapseToFocused ();
            }
        }

        [Test]
        public void RegisterMesh () {

            var sources = MeshTests ( SceneTestEnvironment<TItem, TEdge>
                .Create<ProgrammingLanguageFactory<TItem, TEdge>> (5));

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

            var source = new SceneTestEnvironment<TItem, TEdge, ProgrammingLanguageFactory<TItem, TEdge>> ();
            EdgeAddChangeRemove (source, iProgramming, iNet, iJava);
            EdgeAddChangeRemove (source, iProgramming, iNet, iJava);
            EdgeAddChangeRemove (source, iLibraries, iLanguage, iJava);

        }

        [Test]
        public void RemoveLibraries1 () {
            var test = new ProgrammingSceneTest<TItem, TEdge>();

            var sources = MeshTests (SceneTestEnvironment<TItem, TEdge>
                .Create<ProgrammingLanguageFactory<TItem, TEdge>> (3));
            test.Mock = sources.First () as SceneTestEnvironment<TItem, TEdge, ProgrammingLanguageFactory<TItem, TEdge>>;
            
            var source1 = test.Mock;
            var source2 = sources.Skip (1).First ();

            source2.Expand (source2.Nodes[iProgramming], false);
            test.RemoveLibraries ();

            var librariesRemoved = new IVisual[] {
                source2.Nodes[iProgramming], 
                source2.Nodes[iLanguage], 
                source2.Edges[iProgrammingLanguage], 
                source2.Nodes[iJava], 
                source2.Nodes[iNet], 
                source2.Nodes[iCollections], 
                source2.Edges[iProgrammingLanguageJava], 
                source2.Edges[iProgrammingLanguageNet], 
            };


            source2.AreEquivalent (librariesRemoved, source2.Scene.Graph);

            source2.SetFocused (source2.Nodes[iLanguage]);
            source2.SceneFacade.CollapseToFocused ();
            source2.CommandsPerform ();
            source2.SceneFacade.Delete ();
            source2.CommandsPerform ();

            var languageRemoved = new IVisual[] {
                source1.Nodes[iProgramming], 
                source1.Nodes[iCollections], 
                source1.Nodes[iJava], 
                source1.Nodes[iNet], 
            };

            source1.AreEquivalent (languageRemoved, source1.Scene.Graph);

            source1.ProveShapes ();
            source2.ProveShapes ();
        }

        [Test]
        public void RemoveLibraries2 () {
            var test = new ProgrammingSceneTest<TItem, TEdge> (); 

            var sources = MeshTests (SceneTestEnvironment<TItem, TEdge>
                .Create<ProgrammingLanguageFactory<TItem, TEdge>> (3));
            test.Mock = sources.First () as SceneTestEnvironment<TItem, TEdge, ProgrammingLanguageFactory<TItem, TEdge>>;

            var source1 = test.Mock;
            var source2 = sources.Skip (1).First ();

            source1.Expand (source1.Nodes[iProgramming], false);

            source2.SetFocused (source2.Nodes[iLibraries]);
            source2.CommandsPerform ();

            source2.SceneFacade.Delete ();
            source2.CommandsPerform ();

            var librariesRemoved = new IVisual[] {
                source1.Nodes[iProgramming], 
                source1.Nodes[iLanguage], 
                source1.Edges[iProgrammingLanguage], 
                source1.Nodes[iJava], 
                source1.Nodes[iNet], 
                source1.Nodes[iCollections], 
                source1.Edges[iProgrammingLanguageJava], 
                source1.Edges[iProgrammingLanguageNet], 
            };

            source1.AreEquivalent (librariesRemoved, source1.Scene.Graph);
            source1.ProveShapes ();
            source2.ProveShapes ();
        }
    }
}