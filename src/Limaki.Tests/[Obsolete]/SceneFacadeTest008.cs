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


using System.Collections.Generic;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.Visuals;
using Limaki.UnitTest;
using Limaki.Visuals;
using NUnit.Framework;
using Limaki.Actions;
using Limaki.Common.Collections;
using System;

namespace Limaki.Tests.Graph.Wrappers {
    [Obsolete]
    public abstract class SceneFacadeTest008<TFactory> : DomainTest 
        where TFactory : TestGraphFactory<IGraphEntity, IGraphEdge>, new() {

        public void AreEquivalent(IEnumerable<IVisual> visuals, IGraph<IVisual, IVisualEdge> graph) {
            
            foreach (var visual in visuals) {
                string s = "graph.Contains( " + visual.Data.ToString() + " )";
                if (visual is IVisualEdge) {
                    Assert.IsTrue(graph.Contains((IVisualEdge)visual), s);
                } else {
                    Assert.IsTrue(graph.Contains(visual), s);
                }
            }
            var visualsCollection = new List<IVisual>(visuals);
            foreach (var visual in graph) {
                var s = "visuals.Contains( " + visual.Data.ToString() + " )";
                Assert.IsTrue(visualsCollection.Contains(visual), s);
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
            Mock.Display.Perform ();
            Mock.Scene.Requests.Clear ();           
        }

        public void TestShapes (IGraphScene<IVisual, IVisualEdge> scene) {
            CommandsExecute ();
            var indexList = new Set<IVisual>();
            foreach(var visual in scene.SpatialIndex.Query()) {
                if (!indexList.Contains(visual)) {
                    indexList.Add (visual);
                } else {
                    Assert.Fail (visual +" two times in SpatialIndex");
                }
                bool found = false;
                if (visual is IVisualEdge)
                    found = scene.Contains ((IVisualEdge)visual);
                else
                    found = scene.Contains (visual);

                Assert.IsTrue (found,
                    "to much items in SpatialIndex: ! scene.Contains ( " + visual.ToString() + " ) of Spatialindex");
            }

            foreach(var visual in scene.Graph) {
                if (visual.Shape != null)
                Assert.IsTrue(indexList.Contains(visual),
                    "to less items in SpatialIndex: ! SpatialIndex.Contains ( " + visual.ToString() + " ) of scene.Graph");
            }
        }

        protected Mock008<TFactory> _mock = null;
        public virtual Mock008<TFactory> Mock {
            get {
                if (_mock == null) {
                    _mock = new Mock008<TFactory>();
                }
                return _mock;
            }
            set { _mock = value; }
        }

        public abstract IEnumerable<IVisual> FullExpanded { get; }
        
        public virtual IEnumerable<IVisual> FirstNode {
            get { yield return Mock.Factory.Nodes[1]; }
        }

        [Test]
        public void InvokeTest() {
            CommandsExecute();
            Mock.Display.Reset ();
            TestShapes (Mock.Scene);
        }

        [Test]
        public virtual void FullExpandTest() {
            CommandsExecute();
            Mock.Reset();
            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Nodes[1];
            Mock.SceneFacade.Expand (true);
            AreEquivalent(FullExpanded, Mock.Scene.Graph);
            TestShapes(Mock.Scene);
            this.ReportSummary();
        }
    }
}