/*
 * Limaki 
 * Version 0.071
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


using System;
using System.Collections.Generic;
using Limaki.Graphs;

namespace Limaki.Tests.Graph.Model {
    public interface IGraphItem {
        object Data { get;set; }
    }

    public class GraphItem<T> : Item<T>, IGraphItem {
        public GraphItem( T data ) : base(data) { }

        #region IGraphItem Member

        object IGraphItem.Data {
            get  {return this.Data;}
            set {
                if (value is T) {
                    this.Data = (T)value;
                } else if (value != null) {
                    throw new ArgumentException();
                }
                
            }
        }

        #endregion
    }
    
    public interface IGraphEdge : IGraphItem, IEdge<IGraphItem> { }
    public class GraphEdge : IGraphEdge {
        public GraphEdge() { }
        public GraphEdge( IGraphItem root, IGraphItem leaf ) {
            this.Root = root;
            this.Leaf = leaf;
        }

        #region IEdge<GraphItem> Member
        IGraphItem _Root = null;
        public virtual IGraphItem Root {
            get { return _Root; }
            set { _Root = value; }
        }
        IGraphItem _Leaf = null;
        public virtual IGraphItem Leaf {
            get { return _Leaf; }
            set { _Leaf = value; }
        }

        #endregion
        public override string ToString() {
            return String.Format("[{0}->{1}]", this.Root, this.Leaf);
        }

        #region IGraphItem Member

        public object Data {
            get { return new Common.Empty(); }
            set { }
        }

        #endregion
    }
   
}