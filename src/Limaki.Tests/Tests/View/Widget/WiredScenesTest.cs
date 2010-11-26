/*
 * Limaki 
 * Version 0.08
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

using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.Graph.Wrappers;
using Limaki.Drawing.UI;
using Limaki.Widgets;
using Limaki.Winform.Displays;
using NUnit.Framework;

namespace Limaki.Tests.View.Widget {
    public class WiredScenesTest:DomainTest {
        /// <summary>
        /// TODO: same as DragDropFacadeTest-PrepareTest???
        /// </summary>
        /// <typeparam name="TFactory"></typeparam>
        /// <param name="sourceTest"></param>
        /// <param name="targetTest"></param>
        public void PrepareTests<TFactory>(
            SceneFacadeTest<TFactory> sourceTest, SceneFacadeTest<TFactory> targetTest) 
            where TFactory : GenericGraphFactory<IGraphItem, IGraphEdge>, new() {

            WiredDisplays facade = new WiredDisplays();

            targetTest.Mock.Scene = facade.CreateTargetScene(sourceTest.Mock.Scene);
            facade.WireScene(targetTest.Mock.Control, sourceTest.Mock.Scene, targetTest.Mock.Scene); 
            facade.WireScene(sourceTest.Mock.Control, targetTest.Mock.Scene, sourceTest.Mock.Scene);

            var targetGraph = 
                new GraphPairFacade<IWidget, IEdgeWidget>().Source(targetTest.Mock.Scene.Graph).Two
                   as IGraphPair<IWidget, IGraphItem, IEdgeWidget, IGraphEdge>;

            ((GenericBiGraphFactory<IWidget, IGraphItem, IEdgeWidget, IGraphEdge>)
              targetTest.Mock.Factory).GraphPair = targetGraph;

            var sourceInnerFactory =
                ((GenericBiGraphFactory<IWidget, IGraphItem, IEdgeWidget, IGraphEdge>)
                  sourceTest.Mock.Factory).Factory;

            var targetInnerFactory =
                ((GenericBiGraphFactory<IWidget, IGraphItem, IEdgeWidget, IGraphEdge>)
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
            ProgrammingLanguageFoldingTest sourceTest = new ProgrammingLanguageFoldingTest();
            ProgrammingLanguageFoldingTest targetTest = new ProgrammingLanguageFoldingTest();

            PrepareTests<ProgrammingLanguageFactory> (sourceTest, targetTest);

            targetTest.Net();

            var sourceGraph =
                new GraphPairFacade<IWidget, IEdgeWidget>().Source(sourceTest.Mock.Scene.Graph).Two
                   as IGraphPair<IWidget, IGraphItem, IEdgeWidget, IGraphEdge>;

            var sourceView =
                sourceTest.Mock.Scene.Graph as IGraphPair<IWidget, IWidget, IEdgeWidget, IEdgeWidget>;

            var targetGraph =
                new GraphPairFacade<IWidget, IEdgeWidget>().Source(targetTest.Mock.Scene.Graph).Two
                   as IGraphPair<IWidget, IGraphItem, IEdgeWidget, IGraphEdge>;


            var targetView =
                targetTest.Mock.Scene.Graph as IGraphPair<IWidget, IWidget, IEdgeWidget, IEdgeWidget>;


            sourceTest.Mock.Scene.Selected.Clear();
            sourceTest.Mock.Scene.Focused = sourceTest.Mock.Factory.Node[1]; // Programming
            sourceTest.Mock.SceneFacade.Expand(true);
            sourceTest.Mock.Control.CommandsExecute();

            targetTest.Mock.Scene.Selected.Clear();
            targetTest.Mock.Scene.Focused = targetTest.Mock.Factory.Node[1]; // Programming
            targetTest.Mock.SceneFacade.Expand (false);
            targetTest.Mock.Control.CommandsExecute();

            // make a new link, add it to source, look if in targetGraph.Two
            IEdgeWidget sourceEdge =
                new EdgeWidget<string>(".Net->Programming",
                sourceTest.Mock.Factory.Node[4], // .NET
                sourceTest.Mock.Factory.Node[1]); // Programming);

            sourceGraph.Add (sourceEdge);
            sourceView.OnGraphChanged (sourceEdge, GraphChangeType.Add);

            sourceTest.Mock.SceneFacade.Add (sourceEdge, new PointI (10, 10));
            sourceTest.Mock.Control.CommandsExecute();

            // testing the data source
            IGraphEdge graphEdge = sourceGraph.Get(sourceEdge) as IGraphEdge;
            Assert.IsTrue(targetGraph.Two.Contains(graphEdge));

            // testing the targetTest
            IEdgeWidget targetEdge = targetGraph.Get (graphEdge) as IEdgeWidget;
            Assert.IsNotNull (targetEdge);

            Assert.IsTrue (targetView.One.Contains(targetEdge));


            // change the link in targetTest
            IWidget targetNewRoot = targetTest.Mock.Factory.Node[3]; // java
            IWidget targetOldRoot = targetEdge.Root;
            targetTest.Mock.Scene.ChangeEdge(targetEdge, targetNewRoot, true);
            targetTest.Mock.Scene.Graph.OnGraphChanged(targetEdge, GraphChangeType.Update);

            targetTest.Mock.Scene.Commands.Add(new LayoutCommand<IWidget>(targetEdge, LayoutActionType.Justify));
            foreach (IWidget widget in targetTest.Mock.Scene.Twig(targetEdge)) {
                targetTest.Mock.Scene.Commands.Add(new LayoutCommand<IWidget>(widget, LayoutActionType.Justify));
            }

            targetTest.Mock.Control.CommandsExecute ();

            // test in target
            Assert.AreSame (targetEdge.Root, targetNewRoot);
            Assert.AreNotSame (targetEdge.Root, targetOldRoot);
            Assert.IsTrue (targetView.Edges (targetNewRoot).Contains (targetEdge));
            Assert.IsFalse(targetView.Edges(targetOldRoot).Contains(targetEdge));
            
            // test in source
            IWidget sourceNewRoot = sourceTest.Mock.Factory.Node[3];
            IWidget sourceOldRoot = sourceTest.Mock.Factory.Node[4];
            Assert.AreSame(sourceEdge.Root, sourceNewRoot);
            Assert.AreNotSame(sourceEdge.Root, sourceOldRoot);
            Assert.IsTrue(sourceView.Edges(sourceNewRoot).Contains(sourceEdge));
            Assert.IsFalse(sourceView.Edges(sourceOldRoot).Contains(sourceEdge));
        }
    }
}
