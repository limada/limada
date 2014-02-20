using System;
using System.Collections.Generic;
using System.Linq;
using Limaki.Common;
using Limaki.Common.Linqish;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.Graph.Wrappers;
using Limaki.Tests.Visuals;
using Limaki.View.Mesh;
using Limaki.View.Visuals.UI;
using Limaki.View.Visuals.Visualizers;
using Limaki.Visuals;
using NUnit.Framework;
using Xwt;

namespace Limaki.Tests.View.Visuals {

    public class GraphSceneMeshTest : DomainTest {

        IGraphSceneMesh<IVisual, IVisualEdge> _mesh = null;
        protected IGraphSceneMesh<IVisual, IVisualEdge> Mesh { get { return _mesh ?? (Registry.Pool.TryGetCreate<IGraphSceneMesh<IVisual, IVisualEdge>> ()); } }

        public IEnumerable<SceneFacadeTestWrapper<TFactory>> MeshTests<TFactory> (
            params SceneFacadeTestWrapper<TFactory>[] sources)
            where TFactory : SampleGraphFactoryBase<IGraphEntity, IGraphEdge>, new () {

            var source = sources[0];
            Mesh.AddDisplay (source.Display);

            foreach (var sink in sources.Skip (1)) {
                sink.Mock.Scene = Mesh.CreateSinkScene (source.Scene.Graph);

                Mesh.AddDisplay (sink.Display);

                ((SampleGraphPairFactory<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>)
                 sink.Mock.Factory).GraphPair =
                    // no, its not sink.Graph, and not sink.Scene.... but:
                    sink.Mock.Scene.Graph.Source<IVisual, IVisualEdge, IGraphEntity, IGraphEdge> ();
                ;

                // take the inner factorys:
                var sourceFactory = ((SampleGraphPairFactory<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>)
                                     source.Mock.Factory).Factory;

                var sinkFactory = ((SampleGraphPairFactory<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>)
                                   sink.Mock.Factory).Factory;

                // copy Nodes and Edges
                var i = 0;
                foreach (var n in sourceFactory.Nodes) sinkFactory.Nodes[i++] = n;
                i = 0;
                foreach (var n in sourceFactory.Edges) sinkFactory.Edges[i++] = n;

                sink.Reset ();
            }

            return sources;
        }

        [Test]
        public void EdgeAddAndChange () {

            var sources = MeshTests<ProgrammingLanguageFactory> (
                new ProgrammingGraphSceneTest ().Wraps (7));

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
                var backEdge = source.Graph.Get (sourceEdge) as IGraphEdge;

                // testing the sinkGraph
                Assert.IsTrue (sink.Graph.Source.Contains (backEdge));

                // adding to view
                source.SceneFacade.Add (sourceEdge, new Point (10, 10));
                source.Display.Perform ();

                // testing the sinkView
                var sinkEdge = sink.Graph.Get (backEdge) as IVisualEdge;
                Assert.IsNotNull (sinkEdge);

                sink.ProveViewContains (sinkEdge);

                // change the link in sink

                // not toggle with each test as the edge is deleted
                var iNewRoot = iJava; // i % 2 == 1 ? iJava : iNet;
                var iOldRoot = iNet; // i % 2 == 1 ? iNet : iJava;

                var sinkOldRoot = sink.ChangeLink (sinkEdge, sink.Nodes[iNewRoot], true);

                // test if changed 
                sink.ProoveChangedLink (sinkEdge, sink.Nodes[iNewRoot], sinkOldRoot, true);
                source.ProoveChangedLink (sourceEdge, source.Nodes[iNewRoot], source.Nodes[iOldRoot], true);

                sources.ForEach (t => {
                    var tEdge = t.Graph.Get (backEdge) as IVisualEdge;
                    t.ProoveChangedLink (tEdge, t.Nodes[iNewRoot], t.Nodes[iOldRoot], true,
                        // there could be a display without visible root and leaf
                        t.Scene.Contains (tEdge.Root) && t.Scene.Contains (tEdge.Leaf)
                        );
                });

                // remove in sink
                sink.RemoveEdge (sinkEdge);

                sink.ProveViewNotContains (sinkEdge);
                sink.ProveNotContains (sink.Graph, sinkEdge);
                source.ProveViewNotContains (sourceEdge);

                Assert.IsFalse (sink.Graph.Source.Contains (backEdge));

                sources.ForEach (t => {
                    var tEdge = t.Graph.Get (backEdge) as IVisualEdge;
                    Assert.IsNull (tEdge);
                    Assert.IsFalse (t.Graph.Any (e => e.Data == sourceEdge.Data));
                });

                sink.Expand (sink.Nodes[iProgramming], true);
                sink.ProveViewNotContains (sinkEdge);
                sink.SetFocused (sink.Nodes[iNet]);
                sink.SceneFacade.CollapseToFocused ();
            }
        }

        [Test]
        public void RegisterMesh () {

            var sources = MeshTests<ProgrammingLanguageFactory> (new ProgrammingGraphSceneTest ().Wraps (5));
            var source = sources.First ();
            foreach (var sink in sources.Skip (1)) {
                Assert.AreSame (source.Scene, source.Display.Data);
                Assert.AreSame (sink.Scene, sink.Display.Data);
                Assert.AreSame (source.Graph.Source, sink.Graph.Source);

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

            var source = new ProgrammingGraphSceneTest ().Wrap ();
            EdgeAddChangeRemove (source, iProgramming, iNet, iJava);
            EdgeAddChangeRemove (source, iProgramming, iNet, iJava);
            EdgeAddChangeRemove (source, iLibraries, iLanguage, iJava);
           
        }

        /// <summary>
        /// this tests if a link is added, changed and removed 
        /// </summary>

        public void EdgeAddChangeRemove<TFactory> (SceneFacadeTestWrapper<TFactory> source, int iOne, int iTwo, int iThree)
            where TFactory : SampleGraphFactoryBase<IGraphEntity, IGraphEdge>, new () {

            source.EnsureShape (source.Nodes[iThree]);

            source.SetFocused (source.Nodes[iOne]);
            source.SceneFacade.CollapseToFocused ();

            source.Expand (source.Nodes[iTwo], false);

            var sourceEdge = source.AddEdge (source.Nodes[iOne], source.Nodes[iTwo]);

            source.ChangeLink (sourceEdge, source.Nodes[iThree], true);

            source.RemoveEdge (sourceEdge);

            source.Expand (source.Nodes[iTwo], true);

            source.ProveViewNotContains (sourceEdge);

        }
    }
}