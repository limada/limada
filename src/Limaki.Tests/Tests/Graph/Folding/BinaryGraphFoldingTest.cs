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

using System.Collections.Generic;
using Limaki.Tests.Graph.Model;
using Limaki.Widgets;
using NUnit.Framework;

namespace Limaki.Tests.Graph.Wrappers {
    public class BinaryGraphFoldingTest : SceneFacadeTest<BinaryGraphFactory> {
        public override IEnumerable<IWidget> FullExpanded {
            get {
                yield return Mock.Factory.Node[1]; // 1
                yield return Mock.Factory.Node[2]; // 2
                yield return Mock.Factory.Node[3]; // 3
                yield return Mock.Factory.Node[4]; // 4
                yield return Mock.Factory.Node[5]; // 5
                yield return Mock.Factory.Node[6]; // 6
                yield return Mock.Factory.Node[7]; // 7
                yield return Mock.Factory.Node[8]; // 8
                yield return Mock.Factory.Node[9]; // 9
                yield return Mock.Factory.Edge[1]; // [1->2]
                yield return Mock.Factory.Edge[2]; // [4->3]
                yield return Mock.Factory.Edge[3]; // [1->[4->3]]
                yield return Mock.Factory.Edge[4]; // [5->8]
                yield return Mock.Factory.Edge[5]; // [5->6]
                yield return Mock.Factory.Edge[6]; //[5->7]
                yield return Mock.Factory.Edge[7]; //[8->9]
                yield return Mock.Factory.Edge[8]; //[[4->3]->[5->8]]
            }
        }


        [Test]
        public void FullExandAndN1() {
            this.FullExpandTest ();
            this.N1 ();
        }

        [Test]
        public void N1() {
            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[1]; // 1
            Mock.SceneFacade.CollapseToFocused();

            Assert.AreEqual(Mock.Scene.Graph.Count, 1);
            AreEquivalent(new IWidget[] { Mock.Factory.Node[1] }, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            IEnumerable<IWidget> N1Expanded = new IWidget[] {
                                                    Mock.Factory.Node[1], // 1
                                                    Mock.Factory.Node[2], // 2
                                                    Mock.Factory.Node[3], // 3
                                                    Mock.Factory.Node[4], // 4
                                                    Mock.Factory.Edge[1], // [1->2]
                                                    Mock.Factory.Edge[2], // [4->3]
                                                    Mock.Factory.Edge[3], // [1->[4->3]]
                                                };

            Mock.SceneFacade.Expand (false);
            AreEquivalent(N1Expanded, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[4]; // 4
            Mock.SceneFacade.Expand(false);

            IEnumerable<IWidget> N4Expanded = new IWidget[] {
                                                     Mock.Factory.Node[1], // 1
                                                     Mock.Factory.Node[2], // 2
                                                     Mock.Factory.Node[3], // 3
                                                     Mock.Factory.Node[4], // 4
                                                     Mock.Factory.Node[5], // 3
                                                     Mock.Factory.Node[8], // 4
                                                     Mock.Factory.Edge[1], // [1->2]
                                                     Mock.Factory.Edge[2], // [4->3]
                                                     Mock.Factory.Edge[3], // [1->[4->3]]
                                                     Mock.Factory.Edge[4], // [5->8]
                                                     Mock.Factory.Edge[8], //[[4->3]->[5->8]]
                                                 };

            AreEquivalent(N4Expanded, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[5]; 
            Mock.SceneFacade.Expand(false);

            IEnumerable<IWidget> N5Expanded = new IWidget[] {
                                                     Mock.Factory.Node[1], // 1
                                                     Mock.Factory.Node[2], // 2
                                                     Mock.Factory.Node[3], // 3
                                                     Mock.Factory.Node[4], // 4
                                                     Mock.Factory.Node[5], // 5
                                                     Mock.Factory.Node[6], // 6
                                                     Mock.Factory.Node[7], // 7
                                                     Mock.Factory.Node[8], // 8
                                                     Mock.Factory.Edge[1], // [1->2]
                                                     Mock.Factory.Edge[2], // [4->3]
                                                     Mock.Factory.Edge[3], // [1->[4->3]]
                                                     Mock.Factory.Edge[4], // [5->8]
                                                     Mock.Factory.Edge[5], // [5->6]
                                                     Mock.Factory.Edge[6], //[5->7]
                                                     Mock.Factory.Edge[8], //[[4->3]->[5->8]]
                                                 };

            AreEquivalent(N5Expanded, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            IEnumerable<IWidget> N5Collapsed = new IWidget[] {
                                                      Mock.Factory.Node[1], // 1
                                                      Mock.Factory.Node[2], // 2
                                                      Mock.Factory.Node[3], // 3
                                                      Mock.Factory.Node[4], // 4
                                                      Mock.Factory.Node[5], // 5
                                                      Mock.Factory.Edge[1], // [1->2]
                                                      Mock.Factory.Edge[2], // [4->3]
                                                      Mock.Factory.Edge[3], // [1->[4->3]]
                                                  };

            Mock.SceneFacade.Collapse ();
            AreEquivalent(N5Collapsed, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            Mock.SceneFacade.Expand(false);
            AreEquivalent(N5Expanded, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[8];
            Mock.SceneFacade.Expand(false);

            AreEquivalent(FullExpanded, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            Mock.SceneFacade.Collapse ();

            AreEquivalent(N5Expanded, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            this.ReportSummary();
        }
    }
}