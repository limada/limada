/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2012 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Limaki.Graphs;

namespace Limaki.View.Viz.Modelling {

    public class Placer<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {

        public Placer (IGraphScene<TItem, TEdge> scene, IShaper<TItem> shaper) {
            this.GraphScene = scene;
            this.Shaper = shaper;
        }

        public Placer (IGraphScene<TItem, TEdge> scene, IShaper<TItem> shaper, Action<Placer<TItem, TEdge>> call): this(scene, shaper) {
            call(this);
            Commit();
        }

        public IShaper<TItem> Shaper { get; protected set; }
        public IGraphScene<TItem, TEdge> GraphScene { get; protected set; }

        protected IGraphSceneLocator<TItem, TEdge> _locator = null;
        public virtual IGraphSceneLocator<TItem, TEdge> Locator {
            get { return _locator ?? (_locator = CreateLocator(this.Shaper)); }
            set { _locator = value; }
        }
        
        protected virtual IGraphSceneLocator<TItem, TEdge> CreateLocator(IShaper<TItem> shaper) {
            return new GraphSceneLocator<TItem, TEdge>(shaper);
        }
     
        protected IGraph<TItem, TEdge> Graph {
            get { return GraphScene.Graph; }
        }

        public virtual void VisitItems (IEnumerable<TItem> items, Action<TItem> visitor) {
            foreach (var item in items.Where (i => !(i is TEdge))) {
#if DEBUG
                visits++;
#endif
                visitor (item);
            }
        }
        
        public void Commit() {
            Locator.Commit(this.GraphScene.Requests);
        }


#if DEBUG
        /// <summary>
        /// testing if too much visits
        /// </summary>
        internal int visits = 0;
#endif

    }
}