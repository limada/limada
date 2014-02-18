using System.Collections.Generic;
using Limaki.Tests.Graph.Model;
using Limaki.Visuals;
using NUnit.Framework;

namespace Limaki.Tests.Graph.Wrappers {

    public class BinaryGraphFoldingTest : SceneFacadeTest<BinaryGraphFactory> {
        public override IEnumerable<IVisual> FullExpanded {
            get {
                yield return Mock.Factory.Nodes[1]; // 1
                yield return Mock.Factory.Nodes[2]; // 2
                yield return Mock.Factory.Nodes[3]; // 3
                yield return Mock.Factory.Nodes[4]; // 4
                yield return Mock.Factory.Nodes[5]; // 5
                yield return Mock.Factory.Nodes[6]; // 6
                yield return Mock.Factory.Nodes[7]; // 7
                yield return Mock.Factory.Nodes[8]; // 8
                yield return Mock.Factory.Nodes[9]; // 9
                yield return Mock.Factory.Edges[1]; // [1->2]
                yield return Mock.Factory.Edges[2]; // [4->3]
                yield return Mock.Factory.Edges[3]; // [1->[4->3]]
                yield return Mock.Factory.Edges[4]; // [5->8]
                yield return Mock.Factory.Edges[5]; // [5->6]
                yield return Mock.Factory.Edges[6]; //[5->7]
                yield return Mock.Factory.Edges[7]; //[8->9]
                yield return Mock.Factory.Edges[8]; //[[4->3]->[5->8]]
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
            Mock.Scene.Focused = Mock.Factory.Node[1]; // 1
            Mock.SceneFacade.CollapseToFocused ();

            Assert.AreEqual (Mock.Scene.Graph.Count, 1);
            AreEquivalent (new IVisual[] { Mock.Factory.Nodes[1] }, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            var N1Expanded = new IVisual[] {
                Mock.Factory.Nodes[1], // 1
                Mock.Factory.Nodes[2], // 2
                Mock.Factory.Nodes[3], // 3
                Mock.Factory.Nodes[4], // 4
                Mock.Factory.Edges[1], // [1->2]
                Mock.Factory.Edges[2], // [4->3]
                Mock.Factory.Edges[3], // [1->[4->3]]
            };

            Mock.SceneFacade.Expand (false);
            AreEquivalent (N1Expanded, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            Mock.Scene.Selected.Clear ();
            Mock.Scene.Focused = Mock.Factory.Node[4]; // 4
            Mock.SceneFacade.Expand (false);

            var N4Expanded = new IVisual[] {
                Mock.Factory.Nodes[1], // 1
                Mock.Factory.Nodes[2], // 2
                Mock.Factory.Nodes[3], // 3
                Mock.Factory.Nodes[4], // 4
                Mock.Factory.Nodes[5], // 3
                Mock.Factory.Nodes[8], // 4
                Mock.Factory.Edges[1], // [1->2]
                Mock.Factory.Edges[2], // [4->3]
                Mock.Factory.Edges[3], // [1->[4->3]]
                Mock.Factory.Edges[4], // [5->8]
                Mock.Factory.Edges[8], //[[4->3]->[5->8]]
            };

            AreEquivalent (N4Expanded, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            Mock.Scene.Selected.Clear ();
            Mock.Scene.Focused = Mock.Factory.Node[5];
            Mock.SceneFacade.Expand (false);

            var N5Expanded = new IVisual[] {
                Mock.Factory.Nodes[1], // 1
                Mock.Factory.Nodes[2], // 2
                Mock.Factory.Nodes[3], // 3
                Mock.Factory.Nodes[4], // 4
                Mock.Factory.Nodes[5], // 5
                Mock.Factory.Nodes[6], // 6
                Mock.Factory.Nodes[7], // 7
                Mock.Factory.Nodes[8], // 8
                Mock.Factory.Edges[1], // [1->2]
                Mock.Factory.Edges[2], // [4->3]
                Mock.Factory.Edges[3], // [1->[4->3]]
                Mock.Factory.Edges[4], // [5->8]
                Mock.Factory.Edges[5], // [5->6]
                Mock.Factory.Edges[6], //[5->7]
                Mock.Factory.Edges[8], //[[4->3]->[5->8]]
            };

            AreEquivalent (N5Expanded, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            var N5Collapsed = new IVisual[] {
                Mock.Factory.Nodes[1], // 1
                Mock.Factory.Nodes[2], // 2
                Mock.Factory.Nodes[3], // 3
                Mock.Factory.Nodes[4], // 4
                Mock.Factory.Nodes[5], // 5
                Mock.Factory.Edges[1], // [1->2]
                Mock.Factory.Edges[2], // [4->3]
                Mock.Factory.Edges[3], // [1->[4->3]]
            };

            Mock.SceneFacade.Collapse ();
            AreEquivalent (N5Collapsed, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            Mock.SceneFacade.Expand (false);
            AreEquivalent (N5Expanded, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            Mock.Scene.Selected.Clear ();
            Mock.Scene.Focused = Mock.Factory.Node[8];
            Mock.SceneFacade.Expand (false);

            AreEquivalent (FullExpanded, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            Mock.SceneFacade.Collapse ();

            AreEquivalent (N5Expanded, Mock.Scene.Graph);
            TestShapes (Mock.Scene);

            this.ReportSummary ();
        }
    }
}