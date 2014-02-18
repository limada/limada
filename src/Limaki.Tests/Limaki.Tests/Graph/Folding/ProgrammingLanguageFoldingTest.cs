using System.Collections.Generic;
using Limaki.Tests.Graph.Model;
using Limaki.Visuals;
using NUnit.Framework;
using Xwt;

namespace Limaki.Tests.Graph.Wrappers {

    public class ProgrammingLanguageFoldingTest : SceneFacadeTest<ProgrammingLanguageFactory> {
        public override IEnumerable<IVisual> FullExpanded {
            get {
                yield return Mock.Factory.Nodes[1]; // Programming
                yield return Mock.Factory.Nodes[2]; // Language
                yield return Mock.Factory.Nodes[3]; // Java
                yield return Mock.Factory.Nodes[4]; // .NET
                yield return Mock.Factory.Nodes[5]; // Libraries
                yield return Mock.Factory.Nodes[6]; // Collections
                yield return Mock.Factory.Nodes[7]; // List
                yield return Mock.Factory.Nodes[8]; // IList
                yield return Mock.Factory.Edges[1]; // [Programming->Language]
                yield return Mock.Factory.Edges[2]; // [[Programming->Language]->Java]
                yield return Mock.Factory.Edges[3]; // [[Programming->Language]->.NET]
                yield return Mock.Factory.Edges[4]; // [Programming->Libraries]
                yield return Mock.Factory.Edges[5]; // [[Programming->Libraries]->Collections]
                yield return Mock.Factory.Edges[6]; // [[[Programming->Libraries]->Collections]->List]
                yield return Mock.Factory.Edges[7]; // [[[Programming->Libraries]->Collections]->IList]
                yield return Mock.Factory.Edges[8]; // [[[Programming->Language]->.NET]->IList]
                yield return Mock.Factory.Edges[9];  // [[[Programming->Language]->Java]->List]

            }
        }

        public virtual IEnumerable<IVisual> ProgrammingExpanded {
            get {
                yield return Mock.Factory.Nodes[1]; // Programming
                yield return Mock.Factory.Nodes[2]; // Language
                yield return Mock.Factory.Nodes[3]; // Java
                yield return Mock.Factory.Nodes[4]; // .NET
                yield return Mock.Factory.Nodes[5]; // Libraries
                yield return Mock.Factory.Nodes[6]; // Collections
                yield return Mock.Factory.Edges[1]; // [Programming->Language]
                yield return Mock.Factory.Edges[2]; // [[Programming->Language]->Java]
                yield return Mock.Factory.Edges[3]; // [[Programming->Language]->.NET]
                yield return Mock.Factory.Edges[4]; // [Programming->Libraries]
                yield return Mock.Factory.Edges[5]; // [[Programming->Libraries]->Collections]
            }
        }

        public IEnumerable<IVisual> NetExpanded {
            get {
                yield return Mock.Factory.Nodes[1]; // Programming
                yield return Mock.Factory.Nodes[2]; // Language
                yield return Mock.Factory.Nodes[3]; // Java
                yield return Mock.Factory.Nodes[4]; // .NET
                yield return Mock.Factory.Nodes[5]; // Libraries
                yield return Mock.Factory.Nodes[6]; // Collections
                yield return Mock.Factory.Nodes[8]; // IList
                yield return Mock.Factory.Edges[1]; // [Programming->Language]
                yield return Mock.Factory.Edges[2]; // [[Programming->Language]->Java]
                yield return Mock.Factory.Edges[3]; // [[Programming->Language]->.NET]
                yield return Mock.Factory.Edges[4]; // [Programming->Libraries]
                yield return Mock.Factory.Edges[5]; // [[Programming->Libraries]->Collections]
                yield return Mock.Factory.Edges[7]; // [[[Programming->Libraries]->Collections]->IList]
                yield return Mock.Factory.Edges[8]; // [[[Programming->Language]->.NET]->IList]
            }
        }

        public IEnumerable<IVisual> NetCollapsed {
            get {
                yield return Mock.Factory.Nodes[1]; // Programming
                yield return Mock.Factory.Nodes[2]; // Language
                yield return Mock.Factory.Nodes[4]; // .NET
                yield return Mock.Factory.Edges[1]; //[Programming->Language]
                yield return Mock.Factory.Edges[3]; //[[Programming->Language]->.NET]
            }
        }

        public IEnumerable<IVisual> ProgrammingLanguageNet {
            get {
                yield return Mock.Factory.Nodes[1]; // Programming
                yield return Mock.Factory.Nodes[2]; // Language
                yield return Mock.Factory.Nodes[4]; // .NET
            }
        }


        public IEnumerable<IVisual> languageCollapsed {
            get {
                yield return Mock.Factory.Nodes[1];
                yield return Mock.Factory.Nodes[2];
                yield return Mock.Factory.Nodes[5];
                yield return Mock.Factory.Nodes[6];
                yield return Mock.Factory.Nodes[7];
                yield return Mock.Factory.Nodes[8];
                yield return Mock.Factory.Edges[1];
                yield return Mock.Factory.Edges[4];
                yield return Mock.Factory.Edges[5];
                yield return Mock.Factory.Edges[6];
                yield return Mock.Factory.Edges[7];
            }
        }

        public IEnumerable<IVisual> javaCollapsed {
            get {
                yield return Mock.Factory.Nodes[1];
                yield return Mock.Factory.Nodes[2];
                yield return Mock.Factory.Nodes[3];
                yield return Mock.Factory.Nodes[4];
                yield return Mock.Factory.Nodes[5];
                yield return Mock.Factory.Nodes[6];
                yield return Mock.Factory.Nodes[8];
                yield return Mock.Factory.Edges[1];
                yield return Mock.Factory.Edges[2];
                yield return Mock.Factory.Edges[3];
                yield return Mock.Factory.Edges[4];
                yield return Mock.Factory.Edges[5];
                yield return Mock.Factory.Edges[7];
                yield return Mock.Factory.Edges[8];
            }
        }

        [Test]
        public void AlwaysInvoked () {
            Net ();
            Programming ();
        }

        [Test]
        public void Programming () {
            Mock.Scene.Selected.Clear ();
            Mock.Scene.Focused = Mock.Factory.Node[1]; // Programming
            Mock.Display.Layout.Perform (Mock.Scene.Focused);
            Mock.SceneFacade.CollapseToFocused ();

            Assert.AreEqual (Mock.Scene.Graph.Count, 1);
            AreEquivalent (new IVisual[] { Mock.Factory.Node[1] }, Mock.Scene.Graph);
            TestShapes (Mock.Scene);


            // 1 Collapse - Expand - Cycle
            Mock.SceneFacade.Expand (false);
            AreEquivalent (ProgrammingExpanded, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            // 2 Collapse - Expand - Cycle
            Mock.SceneFacade.Collapse ();
            AreEquivalent (new IVisual[] { Mock.Factory.Nodes[1] }, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            Mock.SceneFacade.Expand (false);
            AreEquivalent (ProgrammingExpanded, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            // 3 Collapse - Expand - Cycle
            Mock.SceneFacade.Collapse ();
            AreEquivalent (new IVisual[] { Mock.Factory.Node[1] }, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            Mock.SceneFacade.Expand (false);
            AreEquivalent (ProgrammingExpanded, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            Mock.Scene.Selected.Clear ();
            Mock.Scene.Focused = Mock.Factory.Node[5];//"Library"
            Mock.Display.Layout.Perform (Mock.Scene.Focused);
            Mock.SceneFacade.Expand (false);

            AreEquivalent (ProgrammingExpanded, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            Mock.Scene.Selected.Clear ();
            Mock.Scene.Focused = Mock.Factory.Node[6]; //"Collections"
            Mock.Display.Layout.Perform (Mock.Scene.Focused);

            Mock.SceneFacade.Expand (false);
            AreEquivalent (FullExpanded, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            Mock.SceneFacade.Collapse ();
            AreEquivalent (ProgrammingExpanded, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            Mock.Scene.Selected.Clear ();
            Mock.Scene.Focused = Mock.Factory.Node[4]; //"Net"
            Mock.Display.Layout.Perform (Mock.Scene.Focused);

            Mock.SceneFacade.Expand (false);
            AreEquivalent (NetExpanded, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            Mock.SceneFacade.Collapse ();
            AreEquivalent (ProgrammingExpanded, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            Mock.SceneFacade.Expand (false);
            AreEquivalent (NetExpanded, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            Mock.Scene.Selected.Clear ();
            Mock.Scene.Focused = Mock.Factory.Node[6]; //"Collections"
            Mock.Display.Layout.Perform (Mock.Scene.Focused);

            Mock.SceneFacade.Collapse ();
            AreEquivalent (ProgrammingExpanded, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            Mock.SceneFacade.Expand (false);
            AreEquivalent (FullExpanded, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            Mock.Scene.Selected.Clear ();
            Mock.Scene.Focused = Mock.Factory.Node[2]; //"Language"
            Mock.Display.Layout.Perform (Mock.Scene.Focused);


            Mock.SceneFacade.Collapse ();
            AreEquivalent (languageCollapsed, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            Mock.SceneFacade.Expand (false);
            AreEquivalent (FullExpanded, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            Mock.Scene.Selected.Clear ();
            Mock.Scene.Focused = Mock.Factory.Node[3]; //"Java"
            Mock.Display.Layout.Perform (Mock.Scene.Focused);


            Mock.SceneFacade.Collapse ();
            AreEquivalent (javaCollapsed, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            Mock.SceneFacade.Expand (false);
            AreEquivalent (FullExpanded, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            this.ReportSummary ();
        }



        [Test]
        public void Net () {
            Mock.Scene.Selected.Clear ();

            Mock.Scene.Focused = Mock.Factory.Node[4]; // .NET
            Mock.Display.Layout.Perform (Mock.Scene.Focused);
            Mock.SceneFacade.CollapseToFocused ();

            Assert.AreEqual (1, Mock.Scene.Graph.Count);
            AreEquivalent (new IVisual[] { Mock.Factory.Nodes[4] }, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            var netExpanded2 = new IVisual[] {
                Mock.Factory.Nodes[1], // Programming
                Mock.Factory.Nodes[2], // Language
                Mock.Factory.Nodes[4], // .NET
                Mock.Factory.Nodes[8], // IList
                Mock.Factory.Edges[1],//[Programming->Language]
                Mock.Factory.Edges[3],//[[Programming->Language]->.NET]
                Mock.Factory.Edges[8],//[[[Programming->Language]->.NET]->IList]
            };

            Mock.SceneFacade.Expand (false);
            AreEquivalent (netExpanded2, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            Mock.SceneFacade.Collapse ();
            AreEquivalent (NetCollapsed, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            Mock.SceneFacade.Expand (false);
            AreEquivalent (netExpanded2, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            Mock.Scene.Selected.Clear ();
            Mock.Scene.Focused = Mock.Factory.Node[8];//"IList"
            Mock.Display.Layout.Perform (Mock.Scene.Focused);
            Mock.SceneFacade.Expand (false);

            var iListExpanded = new IVisual[] {
                Mock.Factory.Nodes[1], // Programming
                Mock.Factory.Nodes[2], // Language
                Mock.Factory.Nodes[4], // .NET
                Mock.Factory.Nodes[5], // Libraries
                Mock.Factory.Nodes[6], // Collections
                Mock.Factory.Nodes[8], // IList
                Mock.Factory.Edges[1], // [Programming->Language]
                Mock.Factory.Edges[3], // [[Programming->Language]->.NET]
                Mock.Factory.Edges[4], // [Programming->Libraries]
                Mock.Factory.Edges[5], // [[Programming->Libraries]->Collections]
                Mock.Factory.Edges[7], // [[[Programming->Libraries]->Collections]->IList]
                Mock.Factory.Edges[8], // [[[Programming->Language]->.NET]->IList]
            };

            AreEquivalent (iListExpanded, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            Mock.SceneFacade.Collapse ();
            AreEquivalent (iListExpanded, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            Mock.Scene.Selected.Clear ();
            Mock.Scene.Focused = Mock.Factory.Edge[8]; // [[[Programming->Language]->.NET]->IList]
            Mock.Display.Layout.Perform (Mock.Scene.Focused);
            Mock.SceneFacade.CollapseToFocused ();

            AreEquivalent (netExpanded2, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            this.ReportSummary ();
        }


        [Test]
        public void ProgrammingLanguageEdge () {
            Mock.Scene.Selected.Clear ();
            Mock.SetFocused (Mock.Factory.Edges[1]); // Programming
            Mock.SceneFacade.CollapseToFocused ();

            var programmingLanguageEdge = new IVisual[] {
                Mock.Factory.Nodes[1], // Programming
                Mock.Factory.Nodes[2], // Language
                Mock.Factory.Edges[1], //[Programming->Language]
            };

            Assert.AreEqual (Mock.Scene.Graph.Count, 3);
            AreEquivalent (programmingLanguageEdge, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            var ProgrammingLanguageExpanded = new IVisual[] {
                Mock.Factory.Nodes[1], // Programming
                Mock.Factory.Nodes[2], // Language
                Mock.Factory.Edges[1], //[Programming->Language]
                Mock.Factory.Nodes[3], // Java
                Mock.Factory.Nodes[4], // .NET
                Mock.Factory.Edges[2], //[[Programming->Language]->Java]
                Mock.Factory.Edges[3], //[[Programming->Language]->.NET]
                Mock.Factory.Nodes[7], // List
                Mock.Factory.Nodes[8], // IList
                Mock.Factory.Edges[8], //[[[Programming->Language]->.NET]->IList]
                Mock.Factory.Edges[9]  //[[[Programming->Language]->Java]->List]
            };

            Mock.SceneFacade.Expand (false);
            AreEquivalent (ProgrammingLanguageExpanded, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            Mock.SceneFacade.Collapse ();
            AreEquivalent (programmingLanguageEdge, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            this.ReportSummary ();
        }


        [Test]
        public void SingleAddProgrammingLanguageNet () {
            Mock.Scene.Selected.Clear ();
            Mock.Scene.Focused = Mock.Factory.Node[1]; // Programming
            Mock.Display.Layout.Perform (Mock.Scene.Focused);
            Mock.SceneFacade.CollapseToFocused ();
            TestShapes (Mock.Scene);

            Mock.SceneFacade.Add (Mock.Factory.Node[2], Point.Zero);// Language
            TestShapes (Mock.Scene);

            Mock.SceneFacade.Add (Mock.Factory.Node[4], Point.Zero);// .NET

            AreEquivalent (NetCollapsed, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            this.ReportSummary ();
        }
    }
}