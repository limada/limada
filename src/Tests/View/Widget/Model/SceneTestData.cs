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
using Limaki.Tests.Graph.Model;

namespace Limaki.Tests.Widget {
    public abstract class SceneTestData : ISceneTestData {

        //public SceneTestData(GenericGraphFactory<IGraphItem, IGraphEdge> data) {
        //    _data = data;
        //}
        public virtual string Name {
            get { return Data.Name; }
        }

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

        protected GenericGraphFactory<IGraphItem, IGraphEdge> _data = null;
        public abstract GenericGraphFactory<IGraphItem, IGraphEdge> Data { get; }


        public virtual Scene Scene {
            get {
                Scene result = new Scene();
                result.Graph = this.Graph;

                Populate (result);

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
        public virtual void Populate ( Scene scene ) {
            Data.Count = this.Count;
            Data.AddDensity = this.addDensity;
            Data.SeperateLattice = this.seperateLattice;

            IGraphPair<IGraphItem, IWidget, IGraphEdge, IEdgeWidget> graphPair =
                new GraphPair<IGraphItem, IWidget, IGraphEdge, IEdgeWidget>(
                    new Graph<IGraphItem, IGraphEdge>(),
                    scene.Graph,
                    new GraphItem2WidgetConverter()
                );

            Data.Graph = graphPair;
            Data.Populate();

            MakeLinkStrings(scene);

            this.Node1 = graphPair.Converter.Get(Data.Node1);
            this.Node2 = graphPair.Converter.Get(Data.Node2);
            this.Node3 = graphPair.Converter.Get(Data.Node3);
            this.Node4 = graphPair.Converter.Get(Data.Node4);
            this.Node5 = graphPair.Converter.Get(Data.Node5);
            this.Node6 = graphPair.Converter.Get(Data.Node6);
            this.Node7 = graphPair.Converter.Get(Data.Node7);
            this.Node8 = graphPair.Converter.Get(Data.Node8);

            this.Link1 = graphPair.Converter.Get(Data.Link1) as IEdgeWidget;
            this.Link2 = graphPair.Converter.Get(Data.Link2) as IEdgeWidget;
            this.Link3 = graphPair.Converter.Get(Data.Link3) as IEdgeWidget;
            this.Link4 = graphPair.Converter.Get(Data.Link4) as IEdgeWidget;
            this.Link5 = graphPair.Converter.Get(Data.Link5) as IEdgeWidget;
            this.Link6 = graphPair.Converter.Get(Data.Link6) as IEdgeWidget;
            this.Link7 = graphPair.Converter.Get(Data.Link7) as IEdgeWidget;
            this.Link8 = graphPair.Converter.Get(Data.Link8) as IEdgeWidget;
        }

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