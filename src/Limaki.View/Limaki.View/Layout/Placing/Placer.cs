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

using Limaki.Drawing;
using Limaki.Graphs;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Limaki.View.Layout {

    public class Placer<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {

        public Placer (IGraphScene<TItem, TEdge> scene, IShaper<TItem> layout) {
            this.GraphScene = scene;
            this.Shaper = layout;
        }

        public Placer (IGraphScene<TItem, TEdge> scene, IShaper<TItem> shaper, Action<Placer<TItem, TEdge>> call): this(scene, shaper) {
            call(this);
            Commit();
        }

        protected IGraphSceneLocator<TItem, TEdge> _locator = null;
        public virtual IGraphSceneLocator<TItem, TEdge> Locator {
            get { return _locator ?? (_locator = CreateLocator(this.Shaper)); }
            set { _locator = value; }
        }
        
        protected virtual IGraphSceneLocator<TItem, TEdge> CreateLocator(IShaper<TItem> shaper) {
            return new GraphSceneLocator<TItem, TEdge>(shaper);
        }
     
        public IShaper<TItem> Shaper { get; protected set; }
        public IGraphScene<TItem, TEdge> GraphScene { get; protected set; }

        protected IGraph<TItem, TEdge> Graph {
            get { return GraphScene.Graph; }
        }

        /// <summary>
        /// testing if too much visits
        /// </summary>
        internal int visits = 0;

        public virtual void VisitItems (IEnumerable<TItem> items, Action<TItem> visitor) {
            foreach (var item in items.Where (i => !(i is TEdge))) {
                visits++;
                visitor (item);
            }
        }
        
        public void Commit() {
            Locator.Commit(this.GraphScene.Requests);
        }
    }
}