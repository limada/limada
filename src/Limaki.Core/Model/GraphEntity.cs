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

        public virtual T Data { get; set; }

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

        public virtual IGraphEntity Root { get; set; }

        public virtual IGraphEntity Leaf { get; set; }

        public override string ToString() {
            return GraphExtensions.EdgeString<IGraphEntity, IGraphEdge>(this);
        }

        object IGraphEntity.Data {
            get { return new Common.Empty(); }
            set { }
        }


    }

    public class GraphEntityFactory : IGraphModelFactory<IGraphEntity, IGraphEdge>, IGraphModelPropertyChanger<IGraphEntity, IGraphEdge> {

        public IGraph<IGraphEntity, IGraphEdge> Graph () {
            return new Graph<IGraphEntity, IGraphEdge> ();
        }

        public IGraphEdge CreateEdge (IGraphEntity root, IGraphEntity leaf) {
            return new GraphEdge (root, leaf);
        }

        public IGraphEdge CreateEdge () {
            return new GraphEdge ();
        }

        public IGraphEntity CreateItem<T> (T data) {
            return new GraphEntity<T> (data);
        }

        public IGraphEntity CreateItem<T> () {
            return new GraphEntity<T> (default (T));
        }

        public void SetProperty (IGraphEntity item, object data) {
            item.Data = data;
        }

        public object GetProperty (IGraphEntity item) {
            return item.Data;
        }

        public IGraphEdge CreateEdge<T> (T data) {
            return new GraphEdge ();
        }

        public IGraphEdge CreateEdge (IGraphEntity root, IGraphEntity leaf, object data) {
            return CreateEdge (root, leaf);
        }
    }
}