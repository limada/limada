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


using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Limaki.Graphs.Extensions;
using Limaki.Model;
using Limaki.View;
using NUnit.Framework;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Winform.DragDrop;
using Limaki.UnitTest;
using Limaki.Winform;
using Limaki.Actions;
using Limaki.Widgets;
using Limaki.Tests.Graph.Wrappers;
using Limaki.Tests.Graph.Model;
using Limaki.Graphs;
using Limaki.Tests.Widget;

namespace Limaki.Tests.View.Display {
    public class DragDropFacadeTest:DomainTest {

        [Test]
        public void ProgrammingLanguageTest() {
            var sourceTest = new ProgrammingLanguageFoldingTest();
            var targetTest = new ProgrammingLanguageFoldingTest();
            
            PrepareTest<ProgrammingLanguageFactory> (sourceTest, targetTest);

            IEnumerable<IWidget> sourceItems = sourceTest.FirstNode;
            IEnumerable<IWidget> targetItems = targetTest.FirstNode;

            DoDragDrop<ProgrammingLanguageFactory> (sourceTest, targetTest, sourceItems, targetItems);

            sourceTest = new ProgrammingLanguageFoldingTest();
            targetTest = new ProgrammingLanguageFoldingTest();

            PrepareTest<ProgrammingLanguageFactory>(sourceTest, targetTest);

            DoDragDrop<ProgrammingLanguageFactory>(sourceTest, targetTest, 
                sourceTest.ProgrammingLanguageNet, 
                targetTest.NetCollapsed);

        }



        [Test]
        public void JohnBostonTest() {
            GCJohnBostonFoldingTest sourceTest = new GCJohnBostonFoldingTest();
            GCJohnBostonFoldingTest targetTest = new GCJohnBostonFoldingTest();

            PrepareTest<GCJohnBostonGraphFactory> (sourceTest, targetTest);

            
            DoDragDrop<GCJohnBostonGraphFactory>(sourceTest, targetTest, 
                sourceTest.JohnGoBostonNodes, 
                targetTest.JohnGoBoston);

            targetTest.Mock.Scene.Selected.Clear ();
            targetTest.Mock.Scene.Focused = targetTest.Mock.Factory.Node[5]; //Go
            targetTest.Mock.SceneFacade.Toggle ();
            targetTest.TestShapes (targetTest.Mock.Scene);
            targetTest.Mock.SceneFacade.Toggle();
            targetTest.TestShapes(targetTest.Mock.Scene);
        }

        public virtual void DoDragDrop<TFactory> (
            SceneFacadeTest<TFactory> sourceTest, 
            SceneFacadeTest<TFactory> targetTest,
            IEnumerable<IWidget> sourceItems,
            IEnumerable<IWidget> expectedTargetResult)
        where TFactory : GenericGraphFactory<IGraphItem, IGraphEdge>, new() {

            DragDropFacade facade = new DragDropFacade();
            foreach (IWidget item in sourceItems) {
                IDataObject dataObject =
                    facade.SetWidget (sourceTest.Mock.Control, sourceTest.Mock.Scene.Graph,item);

                facade.DoDragDrop (
                    targetTest.Mock.Scene,
                    targetTest.Mock.Control,
                    dataObject,
                    targetTest.Mock.Layout,
                    new PointI (),
                    5);
            }

            targetTest.AreEquivalent(expectedTargetResult, targetTest.Mock.Scene.Graph);
            targetTest.TestShapes(targetTest.Mock.Scene);
    
        }

        public virtual void PrepareTest<TFactory>(SceneFacadeTest<TFactory> sourceTest, SceneFacadeTest<TFactory> targetTest)
        where TFactory : GenericGraphFactory<IGraphItem, IGraphEdge>, new() {
            Mock<TFactory> targetMock = new Mock<TFactory>();

            var facade = new GraphPairFacade<IWidget, IEdgeWidget>();
            IGraphPair<IWidget, IWidget, IEdgeWidget, IEdgeWidget> sourcePair = 
                sourceTest.Mock.Scene.Graph as GraphView<IWidget, IEdgeWidget>;

            if (sourcePair != null) {
                sourcePair = facade.Source(sourcePair);
                var sourceGraph = 
                    sourcePair.Two as IGraphPair<IWidget,IGraphItem,IEdgeWidget,IGraphEdge>;

                Assert.IsNotNull (sourceGraph, "sourceGraph is null");

                var targetGraph =
                        new LiveGraphPair<IWidget, IGraphItem, IEdgeWidget, IGraphEdge>(
                        new WidgetGraph(), sourceGraph.Two,
                        new GraphItem2WidgetAdapter().ReverseAdapter());

                Scene targetScene = new Scene ();
                targetScene.Graph = new GraphView<IWidget, IEdgeWidget>(targetGraph, new WidgetGraph());
                targetMock.Scene = targetScene;

                var targetFactory = targetTest.Mock.Factory as SceneFactory<TFactory>;
                var  sourceFactory = sourceTest.Mock.Factory as SceneFactory<TFactory>;


                targetFactory.GraphPair = targetGraph;
                targetTest.Mock.Scene = targetScene;

                for (int i = 0; i < targetFactory.Factory.Edge.Count; i++)
                    targetFactory.Factory.Edge[i] = sourceFactory.Factory.Edge[i];

                for (int i = 0; i < targetFactory.Factory.Node.Count; i++)
                    targetFactory.Factory.Node[i] = sourceFactory.Factory.Node[i];

            }
            

        }
    }
}
