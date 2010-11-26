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
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.Widget;
using Limaki.UnitTest;
using Limaki.Widgets;
using Limaki.Widgets.Layout;
using NUnit.Framework;

namespace Limaki.Tests.Graph.Wrappers {
    public abstract class FoldingControlerTest<TFactory> : TestBase 
        where TFactory : GenericGraphFactory<IGraphItem, IGraphEdge>, new() {
        public void AreEquivalent(ICollection<IWidget> widgets, IGraph<IWidget, IEdgeWidget> graph) {
            foreach (IWidget widget in widgets) {
                string s = widget.Data.ToString() + " not in graph";
                if (widget is IEdgeWidget) {
                    Assert.IsTrue(graph.Contains((IEdgeWidget)widget), s);
                } else {
                    Assert.IsTrue(graph.Contains(widget), s);
                }
            }
            foreach (IWidget widget in graph) {
                string s = widget.Data.ToString() + " not in list";
                Assert.IsTrue(widgets.Contains(widget), s);
            }
        }

        protected Mock<TFactory> _mock = null;
        public virtual Mock<TFactory> Mock {
            get {
                if (_mock == null) {
                    _mock = new Mock<TFactory>();
                }
                return _mock;
            }
            set { _mock = value; }
        }

        protected abstract ICollection<IWidget> fullExpanded { get; }

        [Test]
        public void FullExpanded() {
            Mock.Reset();
            Mock.Scene.Selected.Clear();
            AreEquivalent(fullExpanded, Mock.Scene.Graph);
            this.ReportSummary();
        }
    }

    public class Mock<TFactory>
    where TFactory : GenericGraphFactory<IGraphItem, IGraphEdge>, new() {

        protected Scene _scene;
        public virtual Scene Scene {
            get {
                if (_scene == null) {
                    _scene = this.Factory.Scene;
                }
                return _scene;
            }
            set { _scene = value; }
        }

        protected FoldingControler _folding;
        public virtual FoldingControler Folding {
            get {
                if (_folding == null) {
                    _folding = new FoldingControler();
                    _folding.Scene = this.Scene;
                    _folding.Layout = new GraphLayout<Scene, IWidget>(SceneHandler, StyleSheet.DefaultStyleSheet);
                }
                return _folding;
            }
            set { _folding = value; }
        }

        protected ISceneFactory _factory;
        public virtual ISceneFactory Factory {
            get {
                if (_factory == null) {
                    _factory = new SceneFactory<TFactory>();
                }
                return _factory;
            }
            set { _factory = value; }
        }

        public virtual void Reset() {
            _scene = null;
            _factory = null;
            _folding = null;
        }

        public virtual Scene SceneHandler() { return _scene; }
        
    }
}