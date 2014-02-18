using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.Graph.Wrappers;
using Limaki.View.Layout;
using Limaki.View.Mesh;
using Limaki.View.Visuals.UI;
using Limaki.View.Visuals.Visualizers;
using Limaki.Visuals;
using NUnit.Framework;
using Xwt;

namespace Limaki.Tests.View.Visuals {

    public class GraphSceneMeshTest : DomainTest {
        /// <summary>
        /// TODO: same as DragDropFacadeTest-PrepareTest???
        /// </summary>
        /// <typeparam name="TFactory"></typeparam>
        /// <param name="sourceTest"></param>
        /// <param name="sinkTest"></param>
        public void PrepareTests<TFactory> (
            SceneFacadeTest<TFactory> sourceTest, 
            SceneFacadeTest<TFactory> sinkTest)
            where TFactory : GenericGraphFactory<IGraphEntity, IGraphEdge>, new () {

            var mesh = new VisualGraphSceneMesh ();

            sinkTest.Mock.Scene = mesh.CreateSinkScene (sourceTest.Mock.Scene.Graph);
            mesh.AddDisplay (sourceTest.Mock.Display);
            mesh.AddDisplay (sinkTest.Mock.Display);

            ((GenericBiGraphFactory<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>)
             sinkTest.Mock.Factory).GraphPair =
                    sinkTest.Mock.Scene.Graph.Source<IVisual, IVisualEdge, IGraphEntity, IGraphEdge> ();
        }

        [Test]
        public void EdgeAddAndChangeTest () {
            var sourceTest = new ProgrammingLanguageFoldingTest ();
            var sinkTest = new ProgrammingLanguageFoldingTest ();

            PrepareTests<ProgrammingLanguageFactory> (sourceTest, sinkTest);

            sinkTest.Net ();

            var sourceGraph =
                sourceTest.Mock.Scene.Graph.RootSource ().Source
                as IGraphPair<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>;

            var sourceView =
                sourceTest.Mock.Scene.Graph as IGraphPair<IVisual, IVisual, IVisualEdge, IVisualEdge>;

            var targetGraph =
                sinkTest.Mock.Scene.Graph.RootSource ().Source
                as IGraphPair<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>;


            var targetView =
                sinkTest.Mock.Scene.Graph as IGraphPair<IVisual, IVisual, IVisualEdge, IVisualEdge>;


            sourceTest.Mock.Scene.Selected.Clear ();
            sourceTest.Mock.SetFocused (sourceTest.Mock.Factory.Nodes[1]); // Programming
            sourceTest.Mock.SceneFacade.Expand (true);
            sourceTest.Mock.Display.Perform ();

            sinkTest.Mock.Scene.Selected.Clear ();
            sinkTest.Mock.SetFocused (sinkTest.Mock.Factory.Nodes[1]); // Programming
            sinkTest.Mock.SceneFacade.Expand (false);
            sinkTest.Mock.Display.Perform ();

            // make a new link, add it to source, look if in targetGraph.Source
            var sourceEdge =
                new VisualEdge<string> (".Net->Programming",
                                        sourceTest.Mock.Factory.Nodes[4], // .NET
                                        sourceTest.Mock.Factory.Nodes[1]); // Programming);

            sourceGraph.Add (sourceEdge);
            sourceView.OnGraphChanged (sourceEdge, GraphEventType.Add);

            sourceTest.Mock.SceneFacade.Add (sourceEdge, new Point (10, 10));
            sourceTest.Mock.Display.Perform ();

            // testing the data source
            var graphEdge = sourceGraph.Get (sourceEdge) as IGraphEdge;
            Assert.IsTrue (targetGraph.Source.Contains (graphEdge));

            // testing the targetTest
            IVisualEdge targetEdge = targetGraph.Get (graphEdge) as IVisualEdge;
            Assert.IsNotNull (targetEdge);

            Assert.IsTrue (targetView.Sink.Contains (targetEdge));


            // change the link in targetTest
            var targetNewRoot = sinkTest.Mock.Factory.Nodes[3]; // java
            var targetOldRoot = targetEdge.Root;
            sinkTest.Mock.Scene.ChangeEdge (targetEdge, targetNewRoot, true);
            sinkTest.Mock.Scene.Graph.OnGraphChanged (targetEdge, GraphEventType.Update);

            sinkTest.Mock.Scene.Requests.Add (new LayoutCommand<IVisual> (targetEdge, LayoutActionType.Justify));
            foreach (var visualEdge in sinkTest.Mock.Scene.Twig (targetEdge)) {
                sinkTest.Mock.Scene.Requests.Add (new LayoutCommand<IVisual> (visualEdge, LayoutActionType.Justify));
            }

            sinkTest.Mock.Display.Perform ();

            // test in target
            Assert.AreSame (targetEdge.Root, targetNewRoot);
            Assert.AreNotSame (targetEdge.Root, targetOldRoot);
            Assert.IsTrue (targetView.Edges (targetNewRoot).Contains (targetEdge));
            Assert.IsFalse (targetView.Edges (targetOldRoot).Contains (targetEdge));

            // test in source
            var sourceNewRoot = sourceTest.Mock.Factory.Nodes[3];
            var sourceOldRoot = sourceTest.Mock.Factory.Nodes[4];
            Assert.AreSame (sourceEdge.Root, sourceNewRoot);
            Assert.AreNotSame (sourceEdge.Root, sourceOldRoot);
            Assert.IsTrue (sourceView.Edges (sourceNewRoot).Contains (sourceEdge));
            Assert.IsFalse (sourceView.Edges (sourceOldRoot).Contains (sourceEdge));
        }
    }
}