/*
 * Limaki 
 * Version 0.071
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
    public class BinaryGraphFoldingTest : FoldingControlerTest<BinaryGraphFactory> {
        protected override ICollection<IWidget> fullExpanded {
            get {
                return new IWidget[] {
                                         Mock.Factory.Node[1], // 1
                                         Mock.Factory.Node[2], // 2
                                         Mock.Factory.Node[3], // 3
                                         Mock.Factory.Node[4], // 4
                                         Mock.Factory.Node[5], // 5
                                         Mock.Factory.Node[6], // 6
                                         Mock.Factory.Node[7], // 7
                                         Mock.Factory.Node[8], // 8
                                         Mock.Factory.Node[9], // 9
                                         Mock.Factory.Link[1], // [1->2]
                                         Mock.Factory.Link[2], // [4->3]
                                         Mock.Factory.Link[3], // [1->[4->3]]
                                         Mock.Factory.Link[4], // [5->8]
                                         Mock.Factory.Link[5], // [5->6]
                                         Mock.Factory.Link[6], //[5->7]
                                         Mock.Factory.Link[7], //[8->9]
                                         Mock.Factory.Link[8], //[[4->3]->[5->8]]
                                     };
            }
        }



        [Test]
        public void N1() {
            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[1]; // 1
            Mock.Folding.CollapseToFocused();

            Assert.AreEqual(Mock.Scene.Graph.Count, 1);
            AreEquivalent(new IWidget[] { Mock.Factory.Node[1] }, Mock.Scene.Graph);

            IWidget[] N1Expanded= new IWidget[] {
                                                    Mock.Factory.Node[1], // 1
                                                    Mock.Factory.Node[2], // 2
                                                    Mock.Factory.Node[3], // 3
                                                    Mock.Factory.Node[4], // 4
                                                    Mock.Factory.Link[1], // [1->2]
                                                    Mock.Factory.Link[2], // [4->3]
                                                    Mock.Factory.Link[3], // [1->[4->3]]
                                                };

            Mock.Folding.Expand ();
            AreEquivalent(N1Expanded, Mock.Scene.Graph);

            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[4]; // 4
            Mock.Folding.Expand();
            
            IWidget[] N4Expanded = new IWidget[] {
                                                     Mock.Factory.Node[1], // 1
                                                     Mock.Factory.Node[2], // 2
                                                     Mock.Factory.Node[3], // 3
                                                     Mock.Factory.Node[4], // 4
                                                     Mock.Factory.Node[5], // 3
                                                     Mock.Factory.Node[8], // 4
                                                     Mock.Factory.Link[1], // [1->2]
                                                     Mock.Factory.Link[2], // [4->3]
                                                     Mock.Factory.Link[3], // [1->[4->3]]
                                                     Mock.Factory.Link[4], // [5->8]
                                                     Mock.Factory.Link[8], //[[4->3]->[5->8]]
                                                 };

            AreEquivalent(N4Expanded, Mock.Scene.Graph);

            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[5]; 
            Mock.Folding.Expand();

            IWidget[] N5Expanded = new IWidget[] {
                                                     Mock.Factory.Node[1], // 1
                                                     Mock.Factory.Node[2], // 2
                                                     Mock.Factory.Node[3], // 3
                                                     Mock.Factory.Node[4], // 4
                                                     Mock.Factory.Node[5], // 5
                                                     Mock.Factory.Node[6], // 6
                                                     Mock.Factory.Node[7], // 7
                                                     Mock.Factory.Node[8], // 8
                                                     Mock.Factory.Link[1], // [1->2]
                                                     Mock.Factory.Link[2], // [4->3]
                                                     Mock.Factory.Link[3], // [1->[4->3]]
                                                     Mock.Factory.Link[4], // [5->8]
                                                     Mock.Factory.Link[5], // [5->6]
                                                     Mock.Factory.Link[6], //[5->7]
                                                     Mock.Factory.Link[8], //[[4->3]->[5->8]]
                                                 };

            AreEquivalent(N5Expanded, Mock.Scene.Graph);

            IWidget[] N5Collapsed = new IWidget[] {
                                                      Mock.Factory.Node[1], // 1
                                                      Mock.Factory.Node[2], // 2
                                                      Mock.Factory.Node[3], // 3
                                                      Mock.Factory.Node[4], // 4
                                                      Mock.Factory.Node[5], // 5
                                                      Mock.Factory.Link[1], // [1->2]
                                                      Mock.Factory.Link[2], // [4->3]
                                                      Mock.Factory.Link[3], // [1->[4->3]]
                                                  };

            Mock.Folding.Collapse ();
            AreEquivalent(N5Collapsed, Mock.Scene.Graph);
            
            Mock.Folding.Expand();
            AreEquivalent(N5Expanded, Mock.Scene.Graph);

            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[8];
            Mock.Folding.Expand();

            AreEquivalent(fullExpanded, Mock.Scene.Graph);

            Mock.Folding.Collapse ();

            AreEquivalent(N5Expanded, Mock.Scene.Graph);
            this.ReportSummary();

        }
    }
}