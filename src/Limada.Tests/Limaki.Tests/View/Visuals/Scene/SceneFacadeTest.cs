using System;
using System.Collections.Generic;
using Limaki.Common;
using Limaki.Common.Linqish;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.View.Visuals;
using Limaki.View.Visuals;
using NUnit.Framework;

namespace Limaki.Tests.View.Visuals {

    public abstract class SceneFacadeTest<TItem, TEdge, TFactory> : DomainTest
        where TEdge : IEdge<TItem>, TItem
        where TFactory : ISampleGraphFactory<TItem, TEdge>, new () {

        public abstract IEnumerable<IVisual> FullExpanded { get; }

        public bool AlwaysInvoke = false;
        bool invoked = false;

        protected SceneTestEnvironment<TItem, TEdge, TFactory> _mock = null;
        public virtual SceneTestEnvironment<TItem, TEdge, TFactory> Mock {
            get { return _mock ?? (_mock = new SceneTestEnvironment<TItem, TEdge, TFactory> ()); }
            set { _mock = value; }
        }

        [Test]
        public void InvokeTest () {
            Mock.CommandsPerform ();
            Mock.Display.Reset ();
            Mock.ProveShapes (Mock.Scene);
        }

        [Test]
        public virtual void FullExpandTest () {
            Mock.CommandsPerform ();
            Mock.Clear ();
            Mock.Scene.Selected.Clear ();
            Mock.SetFocused (Mock.SampleFactory.Nodes[1]);
            Mock.SceneFacade.Expand (true);
            Mock.AreEquivalent (FullExpanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);
            this.ReportSummary ();
        }

        
    }
}