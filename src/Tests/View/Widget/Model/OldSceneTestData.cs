/*
 * Limaki 
 * Version 0.07
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

using Limaki.Widgets;
using Limaki.Graphs;

namespace Limaki.Tests.Widget {
    public abstract class OldSceneTestData : ISceneTestData {
        public abstract string Name { get; }

        protected bool seperateLattice = false;
        protected bool addDensity = false;

        IGraph<IWidget, IEdgeWidget> _graph = null;
        public virtual IGraph<IWidget, IEdgeWidget> Graph {
            get {
                if (_graph == null) {
                    _graph = new Graph<IWidget, IEdgeWidget> ();
                } 
                return _graph;
                
            }
            set { _graph = value; }
        }
        public virtual Scene Scene {
            get {
                Scene result = new Scene();
                result.Graph = this.Graph;

                IWidget lastNode1 = null;
                IWidget lastNode2 = null;
                IWidget lastNode3 = null;
                for (int i = 0; i < Count; i++) {
                    if (i > 0) {
                        lastNode1 = Node1;
                        lastNode2 = Node5;
                        lastNode3 = Node8;
                    }
                    Populate(result);
                    if (i > 0) {
                        IEdgeWidget edgeWidget = new EdgeWidget<string>(string.Empty);
                        edgeWidget.Root = lastNode1;
                        edgeWidget.Leaf = Node1;
                        result.Add(edgeWidget);
                        if (seperateLattice) {
                            edgeWidget = new EdgeWidget<string>(string.Empty);
                            edgeWidget.Root = lastNode2;
                            edgeWidget.Leaf = Node5;
                            result.Add(edgeWidget);
                        }
                        if (addDensity) {
                            edgeWidget = new EdgeWidget<string>(string.Empty);
                            edgeWidget.Root = Node2;
                            edgeWidget.Leaf = lastNode3;
                            result.Add(edgeWidget);
                        }
                    }
                }

                MakeLinkStrings (result);

                return result;
            }
        }

        private int _count = 1;
        public int Count {
            get { return _count; }
            set { _count = value; }
        }

        protected string GetLinkString(IEdgeWidget edge) {
            return "[" + edge.Root.Data.ToString() + "->" + edge.Leaf.Data.ToString() + "]";
        }

        public virtual void MakeLinkStrings(Scene scene) {
            foreach (IWidget awidget in scene.Graph) {
                if (!(awidget is IEdgeWidget)) {
                    foreach (IEdgeWidget link in scene.Graph.Twig(awidget)) {
                        link.Data = GetLinkString(link);
                    }

                }
            }
        }
        public abstract void Populate ( Scene scene );

        public IWidget Node1;
        public IWidget Node2;
        public IWidget Node3;
        public IWidget Node4;
        public IWidget Node5;
        public IWidget Node6;
        public IWidget Node7;
        public IWidget Node8;
        public IWidget Node9;
        public IEdgeWidget Link1;
        public IEdgeWidget Link2;
        public IEdgeWidget Link3;
        public IEdgeWidget Link4;
        public IEdgeWidget Link5;
        public IEdgeWidget Link6;
        public IEdgeWidget Link7;
        public IEdgeWidget Link8;
    }
}