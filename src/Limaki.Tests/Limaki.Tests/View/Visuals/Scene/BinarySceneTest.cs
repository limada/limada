using System.Collections.Generic;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.Visuals;
using NUnit.Framework;
using Limaki.Graphs;

namespace Limaki.Tests.View.Visuals {

    public class BinarySceneTest<TItem, TEdge> : SceneFacadeTest<TItem, TEdge, BinaryGraphFactory<TItem, TEdge>>
         where TEdge : IEdge<TItem>, TItem {

        public override IEnumerable<IVisual> FullExpanded {
            get {
                yield return Mock.SampleFactory.Nodes[1]; // 1
                yield return Mock.SampleFactory.Nodes[2]; // 2
                yield return Mock.SampleFactory.Nodes[3]; // 3
                yield return Mock.SampleFactory.Nodes[4]; // 4
                yield return Mock.SampleFactory.Nodes[5]; // 5
                yield return Mock.SampleFactory.Nodes[6]; // 6
                yield return Mock.SampleFactory.Nodes[7]; // 7
                yield return Mock.SampleFactory.Nodes[8]; // 8
                yield return Mock.SampleFactory.Nodes[9]; // 9
                yield return Mock.SampleFactory.Edges[1]; // [1->2]
                yield return Mock.SampleFactory.Edges[2]; // [4->3]
                yield return Mock.SampleFactory.Edges[3]; // [1->[4->3]]
                yield return Mock.SampleFactory.Edges[4]; // [5->8]
                yield return Mock.SampleFactory.Edges[5]; // [5->6]
                yield return Mock.SampleFactory.Edges[6]; //[5->7]
                yield return Mock.SampleFactory.Edges[7]; //[8->9]
                yield return Mock.SampleFactory.Edges[8]; //[[4->3]->[5->8]]
            }
        }


        [Test]
        public void FullExandAndN1 () {
            this.FullExpandTest ();
            this.N1 ();
        }

        [Test]
        public void N1 () {
            Mock.Scene.Selected.Clear ();
            Mock.SetFocused ( Mock.SampleFactory.Nodes[1]); // 1
            Mock.SceneFacade.CollapseToFocused ();

            Assert.AreEqual (Mock.Scene.Graph.Count, 1);
            Mock.AreEquivalent (new IVisual[] { Mock.SampleFactory.Nodes[1] }, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            var N1Expanded = new IVisual[] {
                Mock.SampleFactory.Nodes[1], // 1
                Mock.SampleFactory.Nodes[2], // 2
                Mock.SampleFactory.Nodes[3], // 3
                Mock.SampleFactory.Nodes[4], // 4
                Mock.SampleFactory.Edges[1], // [1->2]
                Mock.SampleFactory.Edges[2], // [4->3]
                Mock.SampleFactory.Edges[3], // [1->[4->3]]
            };

            Mock.SceneFacade.Expand (false);
            Mock.AreEquivalent (N1Expanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.Scene.Selected.Clear ();
            Mock.SetFocused ( Mock.SampleFactory.Nodes[4]); // 4
            Mock.SceneFacade.Expand (false);

            var N4Expanded = new IVisual[] {
                Mock.SampleFactory.Nodes[1], // 1
                Mock.SampleFactory.Nodes[2], // 2
                Mock.SampleFactory.Nodes[3], // 3
                Mock.SampleFactory.Nodes[4], // 4
                Mock.SampleFactory.Nodes[5], // 3
                Mock.SampleFactory.Nodes[8], // 4
                Mock.SampleFactory.Edges[1], // [1->2]
                Mock.SampleFactory.Edges[2], // [4->3]
                Mock.SampleFactory.Edges[3], // [1->[4->3]]
                Mock.SampleFactory.Edges[4], // [5->8]
                Mock.SampleFactory.Edges[8], //[[4->3]->[5->8]]
            };

            Mock.AreEquivalent (N4Expanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.Scene.Selected.Clear ();
            Mock.SetFocused (Mock.SampleFactory.Nodes[5]);
            Mock.SceneFacade.Expand (false);

            var N5Expanded = new IVisual[] {
                Mock.SampleFactory.Nodes[1], // 1
                Mock.SampleFactory.Nodes[2], // 2
                Mock.SampleFactory.Nodes[3], // 3
                Mock.SampleFactory.Nodes[4], // 4
                Mock.SampleFactory.Nodes[5], // 5
                Mock.SampleFactory.Nodes[6], // 6
                Mock.SampleFactory.Nodes[7], // 7
                Mock.SampleFactory.Nodes[8], // 8
                Mock.SampleFactory.Edges[1], // [1->2]
                Mock.SampleFactory.Edges[2], // [4->3]
                Mock.SampleFactory.Edges[3], // [1->[4->3]]
                Mock.SampleFactory.Edges[4], // [5->8]
                Mock.SampleFactory.Edges[5], // [5->6]
                Mock.SampleFactory.Edges[6], //[5->7]
                Mock.SampleFactory.Edges[8], //[[4->3]->[5->8]]
            };

            Mock.AreEquivalent (N5Expanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            var N5Collapsed = new IVisual[] {
                Mock.SampleFactory.Nodes[1], // 1
                Mock.SampleFactory.Nodes[2], // 2
                Mock.SampleFactory.Nodes[3], // 3
                Mock.SampleFactory.Nodes[4], // 4
                Mock.SampleFactory.Nodes[5], // 5
                Mock.SampleFactory.Edges[1], // [1->2]
                Mock.SampleFactory.Edges[2], // [4->3]
                Mock.SampleFactory.Edges[3], // [1->[4->3]]
            };

            Mock.SceneFacade.Collapse ();
            Mock.AreEquivalent (N5Collapsed, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.SceneFacade.Expand (false);
            Mock.AreEquivalent (N5Expanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.Scene.Selected.Clear ();
            Mock.SetFocused (Mock.SampleFactory.Nodes[8]);
            Mock.SceneFacade.Expand (false);

            Mock.AreEquivalent (FullExpanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.SceneFacade.Collapse ();

            Mock.AreEquivalent (N5Expanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            this.ReportSummary ();
        }
    }
}