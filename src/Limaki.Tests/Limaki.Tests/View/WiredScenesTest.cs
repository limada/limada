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

using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Model;
using Limaki.View.Visuals.Visualizers;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.Graph.Wrappers;
using Limaki.Visuals;
using NUnit.Framework;
using Limaki.View.Visuals;
using Limaki.View.Layout;
using Xwt;


namespace Limaki.Tests.View.Visuals {
    public class WiredScenesTest:DomainTest {
        /// <summary>
        /// TODO: same as DragDropFacadeTest-PrepareTest???
        /// </summary>
        /// <typeparam name="TFactory"></typeparam>
        /// <param name="sourceTest"></param>
        /// <param name="targetTest"></param>
        public void PrepareTests<TFactory>(
            SceneFacadeTest<TFactory> sourceTest, SceneFacadeTest<TFactory> targetTest) 
            where TFactory : GenericGraphFactory<IGraphEntity, IGraphEdge>, new() {

            WiredDisplays facade = new WiredDisplays();

            targetTest.Mock.Scene = facade.CreateTargetScene(sourceTest.Mock.Scene);
            facade.WireScene(sourceTest.Mock.Display, sourceTest.Mock.Scene, targetTest.Mock.Scene);
            facade.WireScene(sourceTest.Mock.Display, targetTest.Mock.Scene, sourceTest.Mock.Scene);

            var targetGraph = 
                targetTest.Mock.Scene.Graph.RootSource().Two
                   as IGraphPair<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>;

            ((GenericBiGraphFactory<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>)
              targetTest.Mock.Factory).GraphPair = targetGraph;

            var sourceInnerFactory =
                ((GenericBiGraphFactory<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>)
                  sourceTest.Mock.Factory).Factory;

            var targetInnerFactory =
                ((GenericBiGraphFactory<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>)
                  targetTest.Mock.Factory).Factory;

            for (int i = 0; i < sourceInnerFactory.Node.Count; i++) {
                targetInnerFactory.Node[i] = sourceInnerFactory.Node[i];
            }

            for (int i = 0; i < sourceInnerFactory.Edge.Count; i++) {
                targetInnerFactory.Edge[i] = sourceInnerFactory.Edge[i];
            }
            
        }

        [Test]
        public void EdgeAddAndChangeTest() {
            var sourceTest = new ProgrammingLanguageFoldingTest();
            var targetTest = new ProgrammingLanguageFoldingTest();

            PrepareTests<ProgrammingLanguageFactory> (sourceTest, targetTest);

            targetTest.Net();

            var sourceGraph =
                sourceTest.Mock.Scene.Graph.RootSource().Two
                   as IGraphPair<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>;

            var sourceView =
                sourceTest.Mock.Scene.Graph as IGraphPair<IVisual, IVisual, IVisualEdge, IVisualEdge>;

            var targetGraph =
                targetTest.Mock.Scene.Graph.RootSource().Two
                   as IGraphPair<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>;


            var targetView =
                targetTest.Mock.Scene.Graph as IGraphPair<IVisual, IVisual, IVisualEdge, IVisualEdge>;


            sourceTest.Mock.Scene.Selected.Clear();
            sourceTest.Mock.Scene.Focused = sourceTest.Mock.Factory.Node[1]; // Programming
            sourceTest.Mock.SceneFacade.Expand(true);
            sourceTest.Mock.Display.Execute();

            targetTest.Mock.Scene.Selected.Clear();
            targetTest.Mock.Scene.Focused = targetTest.Mock.Factory.Node[1]; // Programming
            targetTest.Mock.SceneFacade.Expand (false);
            targetTest.Mock.Display.Execute();

            // make a new link, add it to source, look if in targetGraph.Two
            var sourceEdge =
                new VisualEdge<string>(".Net->Programming",
                sourceTest.Mock.Factory.Node[4], // .NET
                sourceTest.Mock.Factory.Node[1]); // Programming);

            sourceGraph.Add (sourceEdge);
            sourceView.OnGraphChanged (sourceEdge, GraphChangeType.Add);

            sourceTest.Mock.SceneFacade.Add (sourceEdge, new Point (10, 10));
            sourceTest.Mock.Display.Execute();

            // testing the data source
            var graphEdge = sourceGraph.Get(sourceEdge) as IGraphEdge;
            Assert.IsTrue(targetGraph.Two.Contains(graphEdge));

            // testing the targetTest
            IVisualEdge targetEdge = targetGraph.Get (graphEdge) as IVisualEdge;
            Assert.IsNotNull (targetEdge);

            Assert.IsTrue (targetView.One.Contains(targetEdge));


            // change the link in targetTest
            var targetNewRoot = targetTest.Mock.Factory.Node[3]; // java
            var targetOldRoot = targetEdge.Root;
            targetTest.Mock.Scene.ChangeEdge(targetEdge, targetNewRoot, true);
            targetTest.Mock.Scene.Graph.OnGraphChanged(targetEdge, GraphChangeType.Update);

            targetTest.Mock.Scene.Requests.Add(new LayoutCommand<IVisual>(targetEdge, LayoutActionType.Justify));
            foreach (var visualEdge in targetTest.Mock.Scene.Twig(targetEdge)) {
                targetTest.Mock.Scene.Requests.Add(new LayoutCommand<IVisual>(visualEdge, LayoutActionType.Justify));
            }

            targetTest.Mock.Display.Execute();

            // test in target
            Assert.AreSame (targetEdge.Root, targetNewRoot);
            Assert.AreNotSame (targetEdge.Root, targetOldRoot);
            Assert.IsTrue (targetView.Edges (targetNewRoot).Contains (targetEdge));
            Assert.IsFalse(targetView.Edges(targetOldRoot).Contains(targetEdge));
            
            // test in source
            var sourceNewRoot = sourceTest.Mock.Factory.Node[3];
            var sourceOldRoot = sourceTest.Mock.Factory.Node[4];
            Assert.AreSame(sourceEdge.Root, sourceNewRoot);
            Assert.AreNotSame(sourceEdge.Root, sourceOldRoot);
            Assert.IsTrue(sourceView.Edges(sourceNewRoot).Contains(sourceEdge));
            Assert.IsFalse(sourceView.Edges(sourceOldRoot).Contains(sourceEdge));
        }
    }
}
