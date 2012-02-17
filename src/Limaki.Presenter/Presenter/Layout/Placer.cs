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
using Limaki.Common;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using System.Collections.Generic;

namespace Limaki.Presenter.Layout {
    /// <summary>
    /// this is to replace Arranger
    /// refactoring: break up arranger into smaller, coupled classes
    /// call arranger with a AllignerChain or something
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    public class Placer<TItem, TEdge> : PlacerBase<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {

        public Placer(IGraphScene<TItem, TEdge> data, IGraphLayout<TItem, TEdge> layout) {
            this.Data = data;
            this.Layout = layout;
        }
        public Placer(PlacerBase<TItem, TEdge> aligner):base(aligner) {}

        protected IShapeProxy<TItem, TEdge> _proxy = null;
        public override IShapeProxy<TItem, TEdge> Proxy {
            get { return _proxy ?? (_proxy = CreateProxy(this.Layout)); }
            set { _proxy = value; }
        }


        /// <summary>
        /// testing if too much visits
        /// </summary>
        internal int visits = 0;

        protected virtual IShapeProxy<TItem, TEdge> CreateProxy(IGraphLayout<TItem, TEdge> layout) {
            return new GraphItemShapeProxy<TItem, TEdge>(layout);
        }

        public virtual void VisitItems(IEnumerable<TItem> items, Action<TItem> visitor) {
            foreach (var item in items.Where(i=> !(i is TEdge))) {
                visits++;
                visitor(item);
            }
        }
    }

    

}