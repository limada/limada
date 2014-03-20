using System.Collections.Generic;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.View.Visuals;
using NUnit.Framework;
using Xwt;
using Limaki.Graphs;

namespace Limaki.Tests.View.Visuals {
   
    public class ProgrammingSceneTest<TItem, TEdge> : SceneFacadeTest<TItem, TEdge, ProgrammingLanguageFactory<TItem, TEdge>>
         where TEdge : IEdge<TItem>, TItem {

        public override IEnumerable<IVisual> FullExpanded {
            get {
                yield return Mock.SampleFactory.Nodes[1]; // Programming
                yield return Mock.SampleFactory.Nodes[2]; // Language
                yield return Mock.SampleFactory.Nodes[3]; // Java
                yield return Mock.SampleFactory.Nodes[4]; // .NET
                yield return Mock.SampleFactory.Nodes[5]; // Libraries
                yield return Mock.SampleFactory.Nodes[6]; // Collections
                yield return Mock.SampleFactory.Nodes[7]; // List
                yield return Mock.SampleFactory.Nodes[8]; // IList
                yield return Mock.SampleFactory.Edges[1]; // [Programming->Language]
                yield return Mock.SampleFactory.Edges[2]; // [[Programming->Language]->Java]
                yield return Mock.SampleFactory.Edges[3]; // [[Programming->Language]->.NET]
                yield return Mock.SampleFactory.Edges[4]; // [Programming->Libraries]
                yield return Mock.SampleFactory.Edges[5]; // [[Programming->Libraries]->Collections]
                yield return Mock.SampleFactory.Edges[6]; // [[[Programming->Libraries]->Collections]->List]
                yield return Mock.SampleFactory.Edges[7]; // [[[Programming->Libraries]->Collections]->IList]
                yield return Mock.SampleFactory.Edges[8]; // [[[Programming->Language]->.NET]->IList]
                yield return Mock.SampleFactory.Edges[9]; // [[[Programming->Language]->Java]->List]

            }
        }

        public virtual IEnumerable<IVisual> ProgrammingExpanded {
            get {
                yield return Mock.SampleFactory.Nodes[1]; // Programming
                yield return Mock.SampleFactory.Nodes[2]; // Language
                yield return Mock.SampleFactory.Nodes[3]; // Java
                yield return Mock.SampleFactory.Nodes[4]; // .NET
                yield return Mock.SampleFactory.Nodes[5]; // Libraries
                yield return Mock.SampleFactory.Nodes[6]; // Collections
                yield return Mock.SampleFactory.Edges[1]; // [Programming->Language]
                yield return Mock.SampleFactory.Edges[2]; // [[Programming->Language]->Java]
                yield return Mock.SampleFactory.Edges[3]; // [[Programming->Language]->.NET]
                yield return Mock.SampleFactory.Edges[4]; // [Programming->Libraries]
                yield return Mock.SampleFactory.Edges[5]; // [[Programming->Libraries]->Collections]
            }
        }

        public IEnumerable<IVisual> NetExpanded {
            get {
                yield return Mock.SampleFactory.Nodes[1]; // Programming
                yield return Mock.SampleFactory.Nodes[2]; // Language
                yield return Mock.SampleFactory.Nodes[3]; // Java
                yield return Mock.SampleFactory.Nodes[4]; // .NET
                yield return Mock.SampleFactory.Nodes[5]; // Libraries
                yield return Mock.SampleFactory.Nodes[6]; // Collections
                yield return Mock.SampleFactory.Nodes[8]; // IList
                yield return Mock.SampleFactory.Edges[1]; // [Programming->Language]
                yield return Mock.SampleFactory.Edges[2]; // [[Programming->Language]->Java]
                yield return Mock.SampleFactory.Edges[3]; // [[Programming->Language]->.NET]
                yield return Mock.SampleFactory.Edges[4]; // [Programming->Libraries]
                yield return Mock.SampleFactory.Edges[5]; // [[Programming->Libraries]->Collections]
                yield return Mock.SampleFactory.Edges[7]; // [[[Programming->Libraries]->Collections]->IList]
                yield return Mock.SampleFactory.Edges[8]; // [[[Programming->Language]->.NET]->IList]
            }
        }

        public IEnumerable<IVisual> NetCollapsed {
            get {
                yield return Mock.SampleFactory.Nodes[1]; // Programming
                yield return Mock.SampleFactory.Nodes[2]; // Language
                yield return Mock.SampleFactory.Nodes[4]; // .NET
                yield return Mock.SampleFactory.Edges[1]; //[Programming->Language]
                yield return Mock.SampleFactory.Edges[3]; //[[Programming->Language]->.NET]
            }
        }

        public IEnumerable<IVisual> ProgrammingLanguageNet {
            get {
                yield return Mock.SampleFactory.Nodes[1]; // Programming
                yield return Mock.SampleFactory.Nodes[2]; // Language
                yield return Mock.SampleFactory.Nodes[4]; // .NET
            }
        }


        public IEnumerable<IVisual> languageCollapsed {
            get {
                yield return Mock.SampleFactory.Nodes[1];
                yield return Mock.SampleFactory.Nodes[2];
                yield return Mock.SampleFactory.Nodes[5];
                yield return Mock.SampleFactory.Nodes[6];
                yield return Mock.SampleFactory.Nodes[7];
                yield return Mock.SampleFactory.Nodes[8];
                yield return Mock.SampleFactory.Edges[1];
                yield return Mock.SampleFactory.Edges[4];
                yield return Mock.SampleFactory.Edges[5];
                yield return Mock.SampleFactory.Edges[6];
                yield return Mock.SampleFactory.Edges[7];
            }
        }

        public IEnumerable<IVisual> javaCollapsed {
            get {
                yield return Mock.SampleFactory.Nodes[1];
                yield return Mock.SampleFactory.Nodes[2];
                yield return Mock.SampleFactory.Nodes[3];
                yield return Mock.SampleFactory.Nodes[4];
                yield return Mock.SampleFactory.Nodes[5];
                yield return Mock.SampleFactory.Nodes[6];
                yield return Mock.SampleFactory.Nodes[8];
                yield return Mock.SampleFactory.Edges[1];
                yield return Mock.SampleFactory.Edges[2];
                yield return Mock.SampleFactory.Edges[3];
                yield return Mock.SampleFactory.Edges[4];
                yield return Mock.SampleFactory.Edges[5];
                yield return Mock.SampleFactory.Edges[7];
                yield return Mock.SampleFactory.Edges[8];
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
            Mock.SetFocused (Mock.SampleFactory.Nodes[1]); // Programming
            Mock.Display.Layout.Perform (Mock.Scene.Focused);
            Mock.SceneFacade.CollapseToFocused ();

            Assert.AreEqual (Mock.Scene.Graph.Count, 1);
            Mock.AreEquivalent (new IVisual[] { Mock.SampleFactory.Nodes[1] }, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);


            // 1 Collapse - Expand - Cycle
            Mock.SceneFacade.Expand (false);
            Mock.AreEquivalent (ProgrammingExpanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            // 2 Collapse - Expand - Cycle
            Mock.SceneFacade.Collapse ();
            Mock.AreEquivalent (new IVisual[] { Mock.SampleFactory.Nodes[1] }, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.SceneFacade.Expand (false);
            Mock.AreEquivalent (ProgrammingExpanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            // 3 Collapse - Expand - Cycle
            Mock.SceneFacade.Collapse ();
            Mock.AreEquivalent (new IVisual[] { Mock.SampleFactory.Nodes[1] }, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.SceneFacade.Expand (false);
            Mock.AreEquivalent (ProgrammingExpanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.Scene.Selected.Clear ();
            Mock.SetFocused (Mock.SampleFactory.Nodes[5]);//"Library"
            Mock.Display.Layout.Perform (Mock.Scene.Focused);
            Mock.SceneFacade.Expand (false);

            Mock.AreEquivalent (ProgrammingExpanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.Scene.Selected.Clear ();
            Mock.SetFocused (Mock.SampleFactory.Nodes[6]); //"Collections"
            Mock.Display.Layout.Perform (Mock.Scene.Focused);

            Mock.SceneFacade.Expand (false);
            Mock.AreEquivalent (FullExpanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.SceneFacade.Collapse ();
            Mock.AreEquivalent (ProgrammingExpanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.Scene.Selected.Clear ();
            Mock.SetFocused (Mock.SampleFactory.Nodes[4]); //"Net"
            Mock.Display.Layout.Perform (Mock.Scene.Focused);

            Mock.SceneFacade.Expand (false);
            Mock.AreEquivalent (NetExpanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.SceneFacade.Collapse ();
            Mock.AreEquivalent (ProgrammingExpanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.SceneFacade.Expand (false);
            Mock.AreEquivalent (NetExpanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.Scene.Selected.Clear ();
            Mock.SetFocused (Mock.SampleFactory.Nodes[6]); //"Collections"
            Mock.Display.Layout.Perform (Mock.Scene.Focused);

            Mock.SceneFacade.Collapse ();
            Mock.AreEquivalent (ProgrammingExpanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.SceneFacade.Expand (false);
            Mock.AreEquivalent (FullExpanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.Scene.Selected.Clear ();
            Mock.SetFocused (Mock.SampleFactory.Nodes[2]); //"Language"
            Mock.Display.Layout.Perform (Mock.Scene.Focused);


            Mock.SceneFacade.Collapse ();
            Mock.AreEquivalent (languageCollapsed, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.SceneFacade.Expand (false);
            Mock.AreEquivalent (FullExpanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.Scene.Selected.Clear ();
            Mock.SetFocused (Mock.SampleFactory.Nodes[3]); //"Java"
            Mock.Display.Layout.Perform (Mock.Scene.Focused);


            Mock.SceneFacade.Collapse ();
            Mock.AreEquivalent (javaCollapsed, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.SceneFacade.Expand (false);
            Mock.AreEquivalent (FullExpanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            this.ReportSummary ();
        }



        [Test]
        public void Net () {
            Mock.Scene.Selected.Clear ();

            Mock.SetFocused (Mock.SampleFactory.Nodes[4]); // .NET
            Mock.SceneFacade.CollapseToFocused ();

            Assert.AreEqual (1, Mock.Scene.Graph.Count);
            Mock.AreEquivalent (new IVisual[] { Mock.SampleFactory.Nodes[4] }, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            var netExpanded2 = new IVisual[] {
                Mock.SampleFactory.Nodes[1], // Programming
                Mock.SampleFactory.Nodes[2], // Language
                Mock.SampleFactory.Nodes[4], // .NET
                Mock.SampleFactory.Nodes[8], // IList
                Mock.SampleFactory.Edges[1],//[Programming->Language]
                Mock.SampleFactory.Edges[3],//[[Programming->Language]->.NET]
                Mock.SampleFactory.Edges[8],//[[[Programming->Language]->.NET]->IList]
            };

            Mock.SceneFacade.Expand (false);
            Mock.AreEquivalent (netExpanded2, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.SceneFacade.Collapse ();
            Mock.AreEquivalent (NetCollapsed, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.SceneFacade.Expand (false);
            Mock.AreEquivalent (netExpanded2, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.Scene.Selected.Clear ();
            Mock.SetFocused (Mock.SampleFactory.Nodes[8]);//"IList"
            Mock.Display.Layout.Perform (Mock.Scene.Focused);
            Mock.SceneFacade.Expand (false);

            var iListExpanded = new IVisual[] {
                Mock.SampleFactory.Nodes[1], // Programming
                Mock.SampleFactory.Nodes[2], // Language
                Mock.SampleFactory.Nodes[4], // .NET
                Mock.SampleFactory.Nodes[5], // Libraries
                Mock.SampleFactory.Nodes[6], // Collections
                Mock.SampleFactory.Nodes[8], // IList
                Mock.SampleFactory.Edges[1], // [Programming->Language]
                Mock.SampleFactory.Edges[3], // [[Programming->Language]->.NET]
                Mock.SampleFactory.Edges[4], // [Programming->Libraries]
                Mock.SampleFactory.Edges[5], // [[Programming->Libraries]->Collections]
                Mock.SampleFactory.Edges[7], // [[[Programming->Libraries]->Collections]->IList]
                Mock.SampleFactory.Edges[8], // [[[Programming->Language]->.NET]->IList]
            };

            Mock.AreEquivalent (iListExpanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.SceneFacade.Collapse ();
            Mock.AreEquivalent (iListExpanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.Scene.Selected.Clear ();
            Mock.SetFocused (Mock.SampleFactory.Edges[8]); // [[[Programming->Language]->.NET]->IList]
            Mock.Display.Layout.Perform (Mock.Scene.Focused);
            Mock.SceneFacade.CollapseToFocused ();

            Mock.AreEquivalent (netExpanded2, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            this.ReportSummary ();
        }


        [Test]
        public void ProgrammingLanguageEdge () {
            Mock.Scene.Selected.Clear ();
            Mock.SetFocused (Mock.SampleFactory.Edges[1]); // Programming
            Mock.SceneFacade.CollapseToFocused ();

            var programmingLanguageEdge = new IVisual[] {
                Mock.SampleFactory.Nodes[1], // Programming
                Mock.SampleFactory.Nodes[2], // Language
                Mock.SampleFactory.Edges[1], //[Programming->Language]
            };

            Assert.AreEqual (Mock.Scene.Graph.Count, 3);
            Mock.AreEquivalent (programmingLanguageEdge, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            var programmingLanguageExpanded = new IVisual[] {
                Mock.SampleFactory.Nodes[1], // Programming
                Mock.SampleFactory.Nodes[2], // Language
                Mock.SampleFactory.Edges[1], //[Programming->Language]
                Mock.SampleFactory.Nodes[3], // Java
                Mock.SampleFactory.Nodes[4], // .NET
                Mock.SampleFactory.Edges[2], //[[Programming->Language]->Java]
                Mock.SampleFactory.Edges[3], //[[Programming->Language]->.NET]
                Mock.SampleFactory.Nodes[7], // List
                Mock.SampleFactory.Nodes[8], // IList
                Mock.SampleFactory.Edges[8], //[[[Programming->Language]->.NET]->IList]
                Mock.SampleFactory.Edges[9]  //[[[Programming->Language]->Java]->List]
            };

            Mock.SceneFacade.Expand (false);
            Mock.AreEquivalent (programmingLanguageExpanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.SceneFacade.Collapse ();
            Mock.AreEquivalent (programmingLanguageEdge, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            this.ReportSummary ();
        }


        [Test]
        public void SingleAddProgrammingLanguageNet () {
            Mock.Scene.Selected.Clear ();
            Mock.SetFocused (Mock.SampleFactory.Nodes[1]); // Programming
            Mock.Display.Layout.Perform (Mock.Scene.Focused);
            Mock.SceneFacade.CollapseToFocused ();
            Mock.ProveShapes (Mock.Scene);

            Mock.SceneFacade.Add (Mock.SampleFactory.Nodes[2], Point.Zero);// Language
            Mock.ProveShapes (Mock.Scene);

            Mock.SceneFacade.Add (Mock.SampleFactory.Nodes[4], Point.Zero);// .NET

            Mock.AreEquivalent (NetCollapsed, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            this.ReportSummary ();
        }
    }
}