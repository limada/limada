/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;
using Limaki.Common.Linqish;
using System.Linq;

namespace Limaki.Tests.View.Visuals {

    public abstract class SampleGraphSceneFactory<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge, TFactory> : 
        SampleGraphPairFactory<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>

        where TSinkEdge : IEdge<TSinkItem>, TSinkItem
        where TSourceEdge : IEdge<TSourceItem>, TSourceItem
        where TFactory : ISampleGraphFactory<TSourceItem, TSourceEdge>, new() {

        public SampleGraphSceneFactory () : base (new TFactory (), null) { }

        public override ISampleGraphFactory<TSourceItem, TSourceEdge> Factory {
            get { return _factory ?? (_factory = new TFactory ()); }
        }

        protected abstract IGraphScene<TSinkItem, TSinkEdge> CreateScene();

        /// <summary>
        /// Creates a new scene and populates it
        /// </summary>
        public virtual IGraphScene<TSinkItem, TSinkEdge> Scene {
            get {
                var result = CreateScene ();
                Populate ();
                result.Graph = this.Graph;
                return result;
            }
        }

        public virtual void PopulateScene (IGraphScene<TSinkItem, TSinkEdge> scene) {
            Populate (scene.Graph);
            scene.ClearSpatialIndex ();
        }

        public void EnsureShapes (IGraphSceneLayout<TSinkItem, TSinkEdge> layout) {
            Nodes.Where (n => n != null).ForEach (n => layout.Perform (n));
            Edges.Where (n => n != null).ForEach (n => layout.Perform (n));
        }
    }
}