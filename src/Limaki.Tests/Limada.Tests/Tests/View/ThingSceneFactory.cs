/*
 * Limaki 
 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */


using Limada.Model;
using Limaki.Graphs;
using Limada.View;
using Limada.VisualThings;
using Limaki.Tests.Visuals;
using Limaki.Tests.Graph.Model;
using Limaki.Visuals;
using System.Collections.Generic;
using Limaki.Drawing;

namespace Limada.Tests.View {
    public abstract class ThingSceneFactory:ISceneFactory {
        public ThingSceneFactory() { }

        #region ISceneFactory Member

        IGraph<IVisual, IVisualEdge> _graph = null;
        public virtual IGraph<IVisual, IVisualEdge> Graph {
            get {
                if ( _graph == null ) {
                    _graph = new Graph<IVisual, IVisualEdge>();
                }
                return _graph;

            }
            set { _graph = value; }
        }

        protected IThingGraph _thingGraph = null;
        public abstract IThingGraph ThingGraph { get; set; }

        public virtual IGraphScene<IVisual, IVisualEdge> Scene {
            get {
                var result = new Scene();
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

        public void Populate (IGraphScene<IVisual, IVisualEdge> scene) {
            if (ThingGraph != null) {
                this.Graph = new VisualThingGraph (new VisualGraph (), this.ThingGraph);
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
        IList<IVisual> _node = null;
        public IList<IVisual> Node {
            get {
                if (_node == null) {
                    _node = new IVisual[11];
                }
                return _node;
            }
        }

        IList<IVisualEdge> _link = null;
        public IList<IVisualEdge> Edge {
            get {
                if (_link == null) {
                    _link = new IVisualEdge[11];
                }
                return _link;
            }
        }
        #endregion

        #region IGraphFactory<IVisual, IVisualEdge> Member


        public void Populate(IGraph<IVisual, IVisualEdge> graph) {
            var adapter = new VisualThingAdapter().ReverseAdapter();
            var mapper = new GraphMapper<IThing, IVisual, ILink, IVisualEdge>(ThingGraph, graph, adapter);

            mapper.ConvertSinkSource();
        }

        #endregion
    }
}
