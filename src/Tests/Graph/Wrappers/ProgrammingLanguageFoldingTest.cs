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

    public class ProgrammingLanguageFoldingTest : FoldingControlerTest<ProgrammingLanguageFactory> {
        protected override ICollection<IWidget> fullExpanded {
            get {
                return new IWidget[] {
                                         Mock.Factory.Node[1], // Programming
                                         Mock.Factory.Node[2], // Language
                                         Mock.Factory.Node[3], // Java
                                         Mock.Factory.Node[4], // .NET
                                         Mock.Factory.Node[5], // Libraries
                                         Mock.Factory.Node[6], // Collections
                                         Mock.Factory.Node[7], // List
                                         Mock.Factory.Node[8], // IList
                                         Mock.Factory.Link[1], // [Programming->Language]
                                         Mock.Factory.Link[2], // [[Programming->Language]->Java]
                                         Mock.Factory.Link[3], // [[Programming->Language]->.NET]
                                         Mock.Factory.Link[4], // [Programming->Libraries]
                                         Mock.Factory.Link[5], // [[Programming->Libraries]->Collections]
                                         Mock.Factory.Link[6], // [[[Programming->Libraries]->Collections]->List]
                                         Mock.Factory.Link[7], // [[[Programming->Libraries]->Collections]->IList]
                                         Mock.Factory.Link[8], // [[[Programming->Language]->.NET]->IList]
                                         Mock.Factory.Link[9]  // [[[Programming->Language]->Java]->List]
                                     };
            }
        }





        [Test]
        public void Programming() {
            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[1]; // Programming
            Mock.Folding.CollapseToFocused();

            Assert.AreEqual(Mock.Scene.Graph.Count, 1);
            AreEquivalent(new IWidget[] { Mock.Factory.Node[1] }, Mock.Scene.Graph);

            IWidget[] programmingExpanded = new IWidget[] {
                                                              Mock.Factory.Node[1],
                                                              Mock.Factory.Node[2],
                                                              Mock.Factory.Node[3],
                                                              Mock.Factory.Node[4],
                                                              Mock.Factory.Node[5],
                                                              Mock.Factory.Node[6],
                                                              Mock.Factory.Link[1],
                                                              Mock.Factory.Link[2],
                                                              Mock.Factory.Link[3],
                                                              Mock.Factory.Link[4],
                                                              Mock.Factory.Link[5]
                                                          };
            Mock.Folding.Expand();
            AreEquivalent(programmingExpanded, Mock.Scene.Graph);

            Mock.Folding.Collapse();
            AreEquivalent(new IWidget[] { Mock.Factory.Node[1] }, Mock.Scene.Graph);

            Mock.Folding.Expand();
            AreEquivalent(programmingExpanded, Mock.Scene.Graph);

            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[5];//"Library"
            Mock.Folding.Expand();

            AreEquivalent(programmingExpanded, Mock.Scene.Graph);


            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[6]; //"Collections"

            Mock.Folding.Expand();
            AreEquivalent(fullExpanded, Mock.Scene.Graph);

            Mock.Folding.Collapse();
            AreEquivalent(programmingExpanded, Mock.Scene.Graph);

            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[4]; //"Net"
            IWidget[] netExpanded = new IWidget[] {
                                                      Mock.Factory.Node[1],
                                                      Mock.Factory.Node[2],
                                                      Mock.Factory.Node[3],
                                                      Mock.Factory.Node[4],
                                                      Mock.Factory.Node[5],
                                                      Mock.Factory.Node[6],
                                                      Mock.Factory.Node[8],
                                                      Mock.Factory.Link[1],
                                                      Mock.Factory.Link[2],
                                                      Mock.Factory.Link[3],
                                                      Mock.Factory.Link[4],
                                                      Mock.Factory.Link[5],
                                                      Mock.Factory.Link[7],
                                                      Mock.Factory.Link[8],
                                                  };

            Mock.Folding.Expand();
            AreEquivalent(netExpanded, Mock.Scene.Graph);

            Mock.Folding.Collapse();
            AreEquivalent(programmingExpanded, Mock.Scene.Graph);

            Mock.Folding.Expand();
            AreEquivalent(netExpanded, Mock.Scene.Graph);

            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[6]; //"Collections"

            Mock.Folding.Collapse();
            AreEquivalent(programmingExpanded, Mock.Scene.Graph);

            Mock.Folding.Expand();
            AreEquivalent(fullExpanded, Mock.Scene.Graph);

            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[2]; //"Language"

            IWidget[] languageCollapsed = new IWidget[] {
                                                            Mock.Factory.Node[1],
                                                            Mock.Factory.Node[2],
                                                            Mock.Factory.Node[5],
                                                            Mock.Factory.Node[6],
                                                            Mock.Factory.Node[7],
                                                            Mock.Factory.Node[8],
                                                            Mock.Factory.Link[1],
                                                            Mock.Factory.Link[4],
                                                            Mock.Factory.Link[5],
                                                            Mock.Factory.Link[6],
                                                            Mock.Factory.Link[7]
                                                        };

            Mock.Folding.Collapse();
            AreEquivalent(languageCollapsed, Mock.Scene.Graph);

            Mock.Folding.Expand();
            AreEquivalent(fullExpanded, Mock.Scene.Graph);

            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[3]; //"Java"

            IWidget[] javaCollapsed = new IWidget[] {
                                                        Mock.Factory.Node[1],
                                                        Mock.Factory.Node[2],
                                                        Mock.Factory.Node[3],
                                                        Mock.Factory.Node[4],
                                                        Mock.Factory.Node[5],
                                                        Mock.Factory.Node[6],
                                                        Mock.Factory.Node[8],
                                                        Mock.Factory.Link[1],
                                                        Mock.Factory.Link[2],
                                                        Mock.Factory.Link[3],
                                                        Mock.Factory.Link[4],
                                                        Mock.Factory.Link[5],
                                                        Mock.Factory.Link[7],
                                                        Mock.Factory.Link[8],
                                                    };

            Mock.Folding.Collapse();
            AreEquivalent(javaCollapsed, Mock.Scene.Graph);

            Mock.Folding.Expand();
            AreEquivalent(fullExpanded, Mock.Scene.Graph);

            this.ReportSummary();
        }

        [Test]
        public void Net() {
            Mock.Scene.Selected.Clear();

            Mock.Scene.Focused = Mock.Factory.Node[4]; // .NET
            Mock.Folding.CollapseToFocused();

            Assert.AreEqual(Mock.Scene.Graph.Count, 1);
            AreEquivalent(new IWidget[] { Mock.Factory.Node[4] }, Mock.Scene.Graph);

            IWidget[] netExpanded = new IWidget[] {
                                                      Mock.Factory.Node[1], // Programming
                                                      Mock.Factory.Node[2], // Language
                                                      Mock.Factory.Node[4], // .NET
                                                      Mock.Factory.Node[8], // IList
                                                      Mock.Factory.Link[1],//[Programming->Language]
                                                      Mock.Factory.Link[3],//[[Programming->Language]->.NET]
                                                      Mock.Factory.Link[8],//[[[Programming->Language]->.NET]->IList]
                                                  };

            Mock.Folding.Expand();
            AreEquivalent(netExpanded, Mock.Scene.Graph);

            IWidget[] netCollapsed = new IWidget[] {
                                                       Mock.Factory.Node[1], // Programming
                                                       Mock.Factory.Node[2], // Language
                                                       Mock.Factory.Node[4], // .NET
                                                       Mock.Factory.Link[1],//[Programming->Language]
                                                       Mock.Factory.Link[3],//[[Programming->Language]->.NET]
                                     
                                                   };
            Mock.Folding.Collapse();
            AreEquivalent(netCollapsed, Mock.Scene.Graph);

            Mock.Folding.Expand();
            AreEquivalent(netExpanded, Mock.Scene.Graph);

            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[8];//"IList"
            Mock.Folding.Expand();

            IWidget[] iListExpanded = new IWidget[] {
                                                        Mock.Factory.Node[1], // Programming
                                                        Mock.Factory.Node[2], // Language
                                                        Mock.Factory.Node[4], // .NET
                                                        Mock.Factory.Node[5], // Libraries
                                                        Mock.Factory.Node[6], // Collections
                                                        Mock.Factory.Node[8], // IList
                                                        Mock.Factory.Link[1], // [Programming->Language]
                                                        Mock.Factory.Link[3], // [[Programming->Language]->.NET]
                                                        Mock.Factory.Link[4], // [Programming->Libraries]
                                                        Mock.Factory.Link[5], // [[Programming->Libraries]->Collections]
                                                        Mock.Factory.Link[7], // [[[Programming->Libraries]->Collections]->IList]
                                                        Mock.Factory.Link[8], // [[[Programming->Language]->.NET]->IList]
                                                    };

            AreEquivalent(iListExpanded, Mock.Scene.Graph);

            Mock.Folding.Collapse();
            AreEquivalent(iListExpanded, Mock.Scene.Graph);

            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Link[8]; // [[[Programming->Language]->.NET]->IList]
            Mock.Folding.CollapseToFocused();

            AreEquivalent(netExpanded, Mock.Scene.Graph);
            this.ReportSummary();
        }

        [Test]
        public void ProgrammingLanguageEdge() {
            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Link[1]; // .NET
            Mock.Folding.CollapseToFocused();

            IWidget[] programmingLanguageEdge = new IWidget[] {
                                                                  Mock.Factory.Node[1], // Programming
                                                                  Mock.Factory.Node[2], // Language
                                                                  Mock.Factory.Link[1], //[Programming->Language]
                                                              };

            Assert.AreEqual(Mock.Scene.Graph.Count, 3);
            AreEquivalent(programmingLanguageEdge, Mock.Scene.Graph);

            IWidget[] ProgrammingLanguageExpanded = new IWidget[] {
                                                                      Mock.Factory.Node[1], // Programming
                                                                      Mock.Factory.Node[2], // Language
                                                                      Mock.Factory.Link[1], //[Programming->Language]
                                                                      Mock.Factory.Node[3], // Java
                                                                      Mock.Factory.Node[4], // .NET
                                                                      Mock.Factory.Link[2], //[[Programming->Language]->Java]
                                                                      Mock.Factory.Link[3], //[[Programming->Language]->.NET]
                                                                      Mock.Factory.Node[7], // List
                                                                      Mock.Factory.Node[8], // IList
                                                                      Mock.Factory.Link[8], //[[[Programming->Language]->.NET]->IList]
                                                                      Mock.Factory.Link[9]  //[[[Programming->Language]->Java]->List]
                                                                  };

            Mock.Folding.Expand();
            AreEquivalent(ProgrammingLanguageExpanded, Mock.Scene.Graph);

            Mock.Folding.Collapse();
            AreEquivalent(programmingLanguageEdge, Mock.Scene.Graph);
            this.ReportSummary();
        }
    }
}