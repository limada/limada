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
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.Linq;
using Limaki.Drawing;
using Limaki.Graphs;
using System.Collections.Generic;

namespace Limaki.View.Layout {
    /// <summary>
    /// this is to replace Arranger
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    public class Placer<TItem, TEdge> : PlacerBase<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {

        public Placer(IGraphScene<TItem, TEdge> data, IGraphSceneLayout<TItem, TEdge> layout) {
            this.Data = data;
            this.Layout = layout;
        }

        public Placer(PlacerBase<TItem, TEdge> aligner):base(aligner) {}

        protected IShapeGraphProxy<TItem, TEdge> _proxy = null;
        public override IShapeGraphProxy<TItem, TEdge> Proxy {
            get { return _proxy ?? (_proxy = CreateProxy(this.Layout)); }
            set { _proxy = value; }
        }
        
        protected virtual IShapeGraphProxy<TItem, TEdge> CreateProxy(IGraphSceneLayout<TItem, TEdge> layout) {
            return new ShapeGraphProxy<TItem, TEdge>(layout);
        }

        
    }

    

}