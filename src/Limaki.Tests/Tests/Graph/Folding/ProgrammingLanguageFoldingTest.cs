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
using Limaki.Drawing;
using Limaki.Tests.Graph.Model;
using Limaki.Widgets;
using NUnit.Framework;

namespace Limaki.Tests.Graph.Wrappers {

    public class ProgrammingLanguageFoldingTest : SceneFacadeTest<ProgrammingLanguageFactory> {
        public override IEnumerable<IWidget> FullExpanded {
            get {
                yield return Mock.Factory.Node[1]; // Programming
                yield return Mock.Factory.Node[2]; // Language
                yield return Mock.Factory.Node[3]; // Java
                yield return Mock.Factory.Node[4]; // .NET
                yield return Mock.Factory.Node[5]; // Libraries
                yield return Mock.Factory.Node[6]; // Collections
                yield return Mock.Factory.Node[7]; // List
                yield return Mock.Factory.Node[8]; // IList
                yield return Mock.Factory.Edge[1]; // [Programming->Language]
                yield return Mock.Factory.Edge[2]; // [[Programming->Language]->Java]
                yield return Mock.Factory.Edge[3]; // [[Programming->Language]->.NET]
                yield return Mock.Factory.Edge[4]; // [Programming->Libraries]
                yield return Mock.Factory.Edge[5]; // [[Programming->Libraries]->Collections]
                yield return Mock.Factory.Edge[6]; // [[[Programming->Libraries]->Collections]->List]
                yield return Mock.Factory.Edge[7]; // [[[Programming->Libraries]->Collections]->IList]
                yield return Mock.Factory.Edge[8]; // [[[Programming->Language]->.NET]->IList]
                yield return Mock.Factory.Edge[9];  // [[[Programming->Language]->Java]->List]

            }
        }

        public virtual IEnumerable<IWidget> ProgrammingExpanded {
            get {
                yield return Mock.Factory.Node[1]; // Programming
                yield return Mock.Factory.Node[2]; // Language
                yield return Mock.Factory.Node[3]; // Java
                yield return Mock.Factory.Node[4]; // .NET
                yield return Mock.Factory.Node[5]; // Libraries
                yield return Mock.Factory.Node[6]; // Collections
                yield return Mock.Factory.Edge[1]; // [Programming->Language]
                yield return Mock.Factory.Edge[2]; // [[Programming->Language]->Java]
                yield return Mock.Factory.Edge[3]; // [[Programming->Language]->.NET]
                yield return Mock.Factory.Edge[4]; // [Programming->Libraries]
                yield return Mock.Factory.Edge[5]; // [[Programming->Libraries]->Collections]
            }
        }

        public IEnumerable<IWidget> NetExpanded {
            get {
                yield return Mock.Factory.Node[1]; // Programming
                yield return Mock.Factory.Node[2]; // Language
                yield return Mock.Factory.Node[3]; // Java
                yield return Mock.Factory.Node[4]; // .NET
                yield return Mock.Factory.Node[5]; // Libraries
                yield return Mock.Factory.Node[6]; // Collections
                yield return Mock.Factory.Node[8]; // IList
                yield return Mock.Factory.Edge[1]; // [Programming->Language]
                yield return Mock.Factory.Edge[2]; // [[Programming->Language]->Java]
                yield return Mock.Factory.Edge[3]; // [[Programming->Language]->.NET]
                yield return Mock.Factory.Edge[4]; // [Programming->Libraries]
                yield return Mock.Factory.Edge[5]; // [[Programming->Libraries]->Collections]
                yield return Mock.Factory.Edge[7]; // [[[Programming->Libraries]->Collections]->IList]
                yield return Mock.Factory.Edge[8]; // [[[Programming->Language]->.NET]->IList]
            }
        }

        public IEnumerable<IWidget> NetCollapsed {
            get {
                yield return Mock.Factory.Node[1]; // Programming
                yield return Mock.Factory.Node[2]; // Language
                yield return Mock.Factory.Node[4]; // .NET
                yield return Mock.Factory.Edge[1]; //[Programming->Language]
                yield return Mock.Factory.Edge[3]; //[[Programming->Language]->.NET]
            }
        }

        public IEnumerable<IWidget> ProgrammingLanguageNet {
            get {
                yield return Mock.Factory.Node[1]; // Programming
                yield return Mock.Factory.Node[2]; // Language
                yield return Mock.Factory.Node[4]; // .NET
            }
        }


        public IEnumerable<IWidget> languageCollapsed {
            get {
                yield return Mock.Factory.Node[1];
                yield return Mock.Factory.Node[2];
                yield return Mock.Factory.Node[5];
                yield return Mock.Factory.Node[6];
                yield return Mock.Factory.Node[7];
                yield return Mock.Factory.Node[8];
                yield return Mock.Factory.Edge[1];
                yield return Mock.Factory.Edge[4];
                yield return Mock.Factory.Edge[5];
                yield return Mock.Factory.Edge[6];
                yield return Mock.Factory.Edge[7];
            }
        }

        public IEnumerable<IWidget> javaCollapsed {
            get {
                yield return Mock.Factory.Node[1];
                yield return Mock.Factory.Node[2];
                yield return Mock.Factory.Node[3];
                yield return Mock.Factory.Node[4];
                yield return Mock.Factory.Node[5];
                yield return Mock.Factory.Node[6];
                yield return Mock.Factory.Node[8];
                yield return Mock.Factory.Edge[1];
                yield return Mock.Factory.Edge[2];
                yield return Mock.Factory.Edge[3];
                yield return Mock.Factory.Edge[4];
                yield return Mock.Factory.Edge[5];
                yield return Mock.Factory.Edge[7];
                yield return Mock.Factory.Edge[8];
            }
        }

        [Test]
        public void AlwaysInvoked() {
            Net ();
            Programming ();
        }

        [Test]
        public void Programming() {
            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[1]; // Programming
            Mock.SceneFacade.CollapseToFocused();

            Assert.AreEqual(Mock.Scene.Graph.Count, 1);
            AreEquivalent(new IWidget[] { Mock.Factory.Node[1] }, Mock.Scene.Graph);
            TestShapes(Mock.Scene);


            // 1 Collapse - Expand - Cycle
            Mock.SceneFacade.Expand(false);
            AreEquivalent(ProgrammingExpanded, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            // 2 Collapse - Expand - Cycle
            Mock.SceneFacade.Collapse();
            AreEquivalent(new IWidget[] { Mock.Factory.Node[1] }, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            Mock.SceneFacade.Expand(false);
            AreEquivalent(ProgrammingExpanded, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            // 3 Collapse - Expand - Cycle
            Mock.SceneFacade.Collapse();
            AreEquivalent(new IWidget[] { Mock.Factory.Node[1] }, Mock.Scene.Graph);
            TestShapes(Mock.Scene);
                        
            Mock.SceneFacade.Expand(false);
            AreEquivalent(ProgrammingExpanded, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[5];//"Library"
            Mock.SceneFacade.Expand(false);

            AreEquivalent(ProgrammingExpanded, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[6]; //"Collections"

            Mock.SceneFacade.Expand(false);
            AreEquivalent(FullExpanded, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            Mock.SceneFacade.Collapse();
            AreEquivalent(ProgrammingExpanded, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[4]; //"Net"


            Mock.SceneFacade.Expand(false);
            AreEquivalent(NetExpanded, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            Mock.SceneFacade.Collapse();
            AreEquivalent(ProgrammingExpanded, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            Mock.SceneFacade.Expand(false);
            AreEquivalent(NetExpanded, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[6]; //"Collections"

            Mock.SceneFacade.Collapse();
            AreEquivalent(ProgrammingExpanded, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            Mock.SceneFacade.Expand(false);
            AreEquivalent(FullExpanded, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[2]; //"Language"



            Mock.SceneFacade.Collapse();
            AreEquivalent(languageCollapsed, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            Mock.SceneFacade.Expand(false);
            AreEquivalent(FullExpanded, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[3]; //"Java"



            Mock.SceneFacade.Collapse();
            AreEquivalent(javaCollapsed, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            Mock.SceneFacade.Expand(false);
            AreEquivalent(FullExpanded, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            this.ReportSummary();
        }



        [Test]
        public void Net() {
            Mock.Scene.Selected.Clear();

            Mock.Scene.Focused = Mock.Factory.Node[4]; // .NET
            Mock.SceneFacade.CollapseToFocused();

            Assert.AreEqual(1,Mock.Scene.Graph.Count);
            AreEquivalent(new IWidget[] { Mock.Factory.Node[4] }, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            IEnumerable<IWidget> netExpanded2 = new IWidget[] {
                                                      Mock.Factory.Node[1], // Programming
                                                      Mock.Factory.Node[2], // Language
                                                      Mock.Factory.Node[4], // .NET
                                                      Mock.Factory.Node[8], // IList
                                                      Mock.Factory.Edge[1],//[Programming->Language]
                                                      Mock.Factory.Edge[3],//[[Programming->Language]->.NET]
                                                      Mock.Factory.Edge[8],//[[[Programming->Language]->.NET]->IList]
                                                  };

            Mock.SceneFacade.Expand(false);
            AreEquivalent(netExpanded2, Mock.Scene.Graph);
            TestShapes(Mock.Scene);


            Mock.SceneFacade.Collapse();
            AreEquivalent(NetCollapsed, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            Mock.SceneFacade.Expand(false);
            AreEquivalent(netExpanded2, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[8];//"IList"
            Mock.SceneFacade.Expand(false);

            IEnumerable<IWidget> iListExpanded = new IWidget[] {
                                                        Mock.Factory.Node[1], // Programming
                                                        Mock.Factory.Node[2], // Language
                                                        Mock.Factory.Node[4], // .NET
                                                        Mock.Factory.Node[5], // Libraries
                                                        Mock.Factory.Node[6], // Collections
                                                        Mock.Factory.Node[8], // IList
                                                        Mock.Factory.Edge[1], // [Programming->Language]
                                                        Mock.Factory.Edge[3], // [[Programming->Language]->.NET]
                                                        Mock.Factory.Edge[4], // [Programming->Libraries]
                                                        Mock.Factory.Edge[5], // [[Programming->Libraries]->Collections]
                                                        Mock.Factory.Edge[7], // [[[Programming->Libraries]->Collections]->IList]
                                                        Mock.Factory.Edge[8], // [[[Programming->Language]->.NET]->IList]
                                                    };

            AreEquivalent(iListExpanded, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            Mock.SceneFacade.Collapse();
            AreEquivalent(iListExpanded, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Edge[8]; // [[[Programming->Language]->.NET]->IList]
            Mock.SceneFacade.CollapseToFocused();

            AreEquivalent(netExpanded2, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            this.ReportSummary();
        }


        [Test]
        public void ProgrammingLanguageEdge() {
            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Edge[1]; // Programming
            Mock.SceneFacade.CollapseToFocused();

            IEnumerable<IWidget> programmingLanguageEdge = new IWidget[] {
                                                                  Mock.Factory.Node[1], // Programming
                                                                  Mock.Factory.Node[2], // Language
                                                                  Mock.Factory.Edge[1], //[Programming->Language]
                                                              };

            Assert.AreEqual(Mock.Scene.Graph.Count, 3);
            AreEquivalent(programmingLanguageEdge, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            IEnumerable<IWidget> ProgrammingLanguageExpanded = new IWidget[] {
                                                                      Mock.Factory.Node[1], // Programming
                                                                      Mock.Factory.Node[2], // Language
                                                                      Mock.Factory.Edge[1], //[Programming->Language]
                                                                      Mock.Factory.Node[3], // Java
                                                                      Mock.Factory.Node[4], // .NET
                                                                      Mock.Factory.Edge[2], //[[Programming->Language]->Java]
                                                                      Mock.Factory.Edge[3], //[[Programming->Language]->.NET]
                                                                      Mock.Factory.Node[7], // List
                                                                      Mock.Factory.Node[8], // IList
                                                                      Mock.Factory.Edge[8], //[[[Programming->Language]->.NET]->IList]
                                                                      Mock.Factory.Edge[9]  //[[[Programming->Language]->Java]->List]
                                                                  };

            Mock.SceneFacade.Expand(false);
            AreEquivalent(ProgrammingLanguageExpanded, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            Mock.SceneFacade.Collapse();
            AreEquivalent(programmingLanguageEdge, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            this.ReportSummary();
        }


        [Test]
        public void SingleAddProgrammingLanguageNet() {
            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[1]; // Programming
            Mock.SceneFacade.CollapseToFocused();
            TestShapes(Mock.Scene);

            Mock.SceneFacade.Add(Mock.Factory.Node[2], PointI.Empty);// Language
            TestShapes(Mock.Scene);

            Mock.SceneFacade.Add(Mock.Factory.Node[4], PointI.Empty);// .NET

            AreEquivalent(NetCollapsed, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            this.ReportSummary();
        }
    }
}