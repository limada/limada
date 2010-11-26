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
    public class GCJohnBostonFoldingTest : FoldingControlerTest<GCJohnBostonGraphFactory> {
        protected override ICollection<IWidget> fullExpanded {
            get {
                return new IWidget[] {
                                         Mock.Factory.Node[1], // Person
                                         Mock.Factory.Node[2], // John
                                         Mock.Factory.Node[3], // City
                                         Mock.Factory.Node[4], // Boston
                                         Mock.Factory.Node[5], // Go
                                         Mock.Factory.Node[6], // Bus
                                         Mock.Factory.Link[1], // [Person->John]
                                         Mock.Factory.Link[2], // [City->Boston]
                                         Mock.Factory.Link[3], // [[Person->John]->Go]
                                         Mock.Factory.Link[4], // [[[Person->John]->Go]->[City->Boston]]
                                         Mock.Factory.Link[5], // [[[[Person->John]->Go]->[City->Boston]]->Bus]
                                     };
            }
        }


        [Test]
        public void Person() {
            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[1]; // Person
            Mock.Folding.CollapseToFocused();

            Assert.AreEqual(Mock.Scene.Graph.Count, 1);
            AreEquivalent(new IWidget[] { Mock.Factory.Node[1] }, Mock.Scene.Graph);

            Mock.Folding.Expand ();

            IWidget[] PersonExpanded = new IWidget[] {
                                                         Mock.Factory.Node[1], // Person
                                                         Mock.Factory.Node[2], // John
                                                         Mock.Factory.Node[5], // Go
                                                         Mock.Factory.Link[1], // [Person->John]
                                                         Mock.Factory.Link[3], // [[Person->John]->Go]
                                                     };
            AreEquivalent(PersonExpanded, Mock.Scene.Graph);

            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[5]; // Go
            Mock.Folding.Expand();

            AreEquivalent(fullExpanded, Mock.Scene.Graph);

            Mock.Folding.Collapse ();
            AreEquivalent(PersonExpanded, Mock.Scene.Graph);

            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Link[1]; // [Person->John]
            Mock.Folding.Expand();

            AreEquivalent(fullExpanded, Mock.Scene.Graph);

            this.ReportSummary();

        }
    }
}