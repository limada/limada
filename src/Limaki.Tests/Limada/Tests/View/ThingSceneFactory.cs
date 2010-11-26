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


using Limada.Model;
using Limada.View;
using Limaki.Graphs;
using Limaki.Tests.Widget;
using Limaki.Tests.Graph.Model;
using Limaki.Widgets;
using System.Collections.Generic;

namespace Limada.Tests.View {
    public abstract class ThingSceneFactory:ISceneFactory {
        public ThingSceneFactory() { }

        #region ISceneFactory Member

        IGraph<IWidget, IEdgeWidget> _graph = null;
        public virtual IGraph<IWidget, IEdgeWidget> Graph {
            get {
                if ( _graph == null ) {
                    _graph = new Graph<IWidget, IEdgeWidget>();
                }
                return _graph;

            }
            set { _graph = value; }
        }

        protected IThingGraph _thingGraph = null;
        public abstract IThingGraph ThingGraph { get; set; }

        public virtual Scene Scene {
            get {
                Scene result = new Scene();
                Populate(result);
                return result;
            }
        }

        private int _count = 1;
        public int Count {
            get { return _count; }
            set { _count = value; }
        }

        public string Name { get { return this.GetType().Name; } }

        public void Populate( Scene scene ) {
            if (ThingGraph != null) {
                this.Graph = new WidgetThingGraph (new WidgetGraph (), this.ThingGraph);
                scene.Graph = this.Graph;
            }
        }
        public void Populate() {
            Populate (this.Scene);
        }

        protected bool _seperateLattice = false;
        public bool SeperateLattice {
            get { return _seperateLattice; }
            set { _seperateLattice = value; }
        }

        protected bool _addDensity = false;
        public bool AddDensity {
            get { return _addDensity; }
            set { _addDensity = value; }
        }
        IList<IWidget> _node = null;
        public IList<IWidget> Node {
            get {
                if (_node == null) {
                    _node = new IWidget[11];
                }
                return _node;
            }
        }

        IList<IEdgeWidget> _link = null;
        public IList<IEdgeWidget> Edge {
            get {
                if (_link == null) {
                    _link = new IEdgeWidget[11];
                }
                return _link;
            }
        }
        #endregion

        #region IGraphFactory<IWidget,IEdgeWidget> Member


        public void Populate(IGraph<IWidget, IEdgeWidget> graph) {
            GraphModelAdapter<IThing, IWidget, ILink, IEdgeWidget> adapter =
                new WidgetThingAdapter().ReverseAdapter();

            GraphMapper<IThing, IWidget, ILink, IEdgeWidget> mapper =
                new GraphMapper<IThing, IWidget, ILink, IEdgeWidget>(
                ThingGraph, graph, adapter);
            mapper.ConvertOneTwo();
        }

        #endregion
    }
}
