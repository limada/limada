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
using System.Collections.Generic;
using System.Linq;
using Limaki.Common;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;

namespace Limaki.Presenter.Layout {
    public class Alligner<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {

        public Alligner(IGraphScene<TItem, TEdge> data, IGraphLayout<TItem, TEdge> layout) {
            this.Data = data;
            this.Layout = layout;

        }
        public IGraphLayout<TItem, TEdge> Layout { get; protected set; }
        public IGraphScene<TItem, TEdge> Data { get; protected set; }
        public IGraph<TItem, TEdge> Graph {
            get { return Data.Graph; }
        }

        protected IShapeProxy<TItem, TEdge> _proxy = null;
        public IShapeProxy<TItem, TEdge> Proxy {
            get { return _proxy ?? (_proxy = CreateProxy(this.Layout)); }
            set { _proxy = value; }
        }

        protected virtual IShapeProxy<TItem, TEdge> CreateProxy(IGraphLayout<TItem, TEdge> layout) {
            return new GraphItemShapeProxy<TItem, TEdge>(layout);
        }

        public virtual void Allign(IEnumerable<TItem> items, VerticalAlignement alignement) {
        }
    }

    public enum HorizontalAlignement {
        Left,Center,Right
    }

    public enum VerticalAlignement {
        Top, Center, Bottom
    }

}