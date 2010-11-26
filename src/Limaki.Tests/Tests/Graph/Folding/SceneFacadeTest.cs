/*
 * Limaki 
 * Version 0.081
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


using System.Collections.Generic;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.Widget;
using Limaki.UnitTest;
using Limaki.Widgets;
using Limaki.Widgets.Layout;
using NUnit.Framework;
using Limaki.Actions;
using Limaki.Common.Collections;

namespace Limaki.Tests.Graph.Wrappers {
    public abstract class SceneFacadeTest<TFactory> : DomainTest 
        where TFactory : GenericGraphFactory<IGraphItem, IGraphEdge>, new() {

        public void AreEquivalent(IEnumerable<IWidget> widgets, IGraph<IWidget, IEdgeWidget> graph) {
            
            foreach (IWidget widget in widgets) {
                string s = "graph.Contains( " + widget.Data.ToString() + " )";
                if (widget is IEdgeWidget) {
                    Assert.IsTrue(graph.Contains((IEdgeWidget)widget), s);
                } else {
                    Assert.IsTrue(graph.Contains(widget), s);
                }
            }
            ICollection<IWidget> widgetCollection = new List<IWidget>(widgets);
            foreach (IWidget widget in graph) {
                string s = "widgets.Contains( " + widget.Data.ToString() + " )";
                Assert.IsTrue(widgetCollection.Contains(widget), s);
            }
        }

        public bool AlwaysInvoke = false;
        bool invoked = false;

        public void CommandsExecute() {
             //if (!invoked) {
            //    Mock.SceneControler.Invoke ();
            //    if (!AlwaysInvoke)
            //        invoked = true;
            //}
            Mock.Control.CommandsExecute ();
            Mock.Scene.Commands.Clear ();           
        }

        public void TestShapes(Scene scene) {
            CommandsExecute ();
            ICollection<IWidget> indexList = new Set<IWidget>();
            foreach(IWidget widget in scene.SpatialIndex.Query()) {
                if (!indexList.Contains(widget)) {
                    indexList.Add (widget);
                } else {
                    Assert.Fail (widget +" two times in SpatialIndex");
                }
                bool found = false;
                if (widget is IEdgeWidget)
                    found = scene.Contains ((IEdgeWidget)widget);
                else
                    found = scene.Contains (widget);

                Assert.IsTrue (found,
                    "to much items in SpatialIndex: ! scene.Contains ( " + widget.ToString() + " ) of Spatialindex");
            }

            foreach(IWidget widget in scene.Graph) {
                Assert.IsTrue(indexList.Contains(widget),
                    "to less items in SpatialIndex: ! SpatialIndex.Contains ( " + widget.ToString() + " ) of scene.Graph");
            }
        }

        protected Mock<TFactory> _mock = null;
        public virtual Mock<TFactory> Mock {
            get {
                if (_mock == null) {
                    _mock = new Mock<TFactory>();
                }
                return _mock;
            }
            set { _mock = value; }
        }

        public abstract IEnumerable<IWidget> FullExpanded { get; }
        
        public virtual IEnumerable<IWidget> FirstNode {
            get { yield return Mock.Factory.Node[1]; }
        }

        [Test]
        public void InvokeTest() {
            CommandsExecute();
            Mock.SceneControler.Invoke ();
            TestShapes (Mock.Scene);
        }

        [Test]
        public virtual void FullExpandTest() {
            CommandsExecute();
            Mock.Reset();
            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[1];
            Mock.SceneFacade.Expand (true);
            AreEquivalent(FullExpanded, Mock.Scene.Graph);
            TestShapes(Mock.Scene);
            this.ReportSummary();
        }
    }
}