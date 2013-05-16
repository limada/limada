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


using System;
using Limaki.Graphs;

namespace Limaki.Model {

    public interface IGraphEntity {
        object Data { get;set; }
    }

    public class GraphEntity<T> : IGraphEntity {
        public GraphEntity( T data ) {
            this.Data = data;
        }
        protected T _data = default( T );
        public virtual T Data {
            get { return _data; }
            set { _data = value; }
        }

        public override string ToString() {
            return Data.ToString();
        }

        #region IGraphEntity Member

        object IGraphEntity.Data {
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

    public interface IGraphEdge : IGraphEntity, IEdge<IGraphEntity> { }

    public class GraphEdge : IGraphEdge {
        public GraphEdge() { }
        public GraphEdge( IGraphEntity root, IGraphEntity leaf ) {
            this.Root = root;
            this.Leaf = leaf;
        }

        #region IEdge<IGraphEntity> Member
        IGraphEntity _Root = null;
        public virtual IGraphEntity Root {
            get { return _Root; }
            set { _Root = value; }
        }
        IGraphEntity _Leaf = null;
        public virtual IGraphEntity Leaf {
            get { return _Leaf; }
            set { _Leaf = value; }
        }

        #endregion
        public override string ToString() {
            return GraphExtensions.EdgeString<IGraphEntity, IGraphEdge>(this);
        }

        #region IGraphEntity Member

        object IGraphEntity.Data {
            get { return new Common.Empty(); }
            set { }
        }

        #endregion
    }
}