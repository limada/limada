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


namespace Limaki.Graphs {

    public interface IEdge<T> {
        T Root { get;set;}
        T Leaf { get;set;}
    }

    public interface IEdge:IEdge<object>{};

    public static class EdgeExtensions {

        public static bool RootIsEdge<TItem>  (this IEdge<TItem> edge) => edge != null && edge.Root is IEdge<TItem>;

        public static bool LeafIsEdge<TItem>  (this IEdge<TItem> edge) => edge != null && edge.Leaf is IEdge<TItem>;

        /// <summary>
        /// root or leaf is an edge
        /// </summary>
        /// <param name="edge">Edge</param>
        public static bool IsEdgeOfEdge<TItem> (this IEdge<TItem> edge) => edge !=null && (edge.Root is IEdge<TItem> || edge.Leaf is IEdge<TItem>);

        /// <summary>
        /// both root and leaf is an edge
        /// the edge connects two edges
        /// </summary>
        /// <param name="edge">Edge</param>
        public static bool IsEdgeOfEdges<TItem> (this IEdge<TItem> edge) => edge != null && edge.Root is IEdge<TItem> && edge.Leaf is IEdge<TItem>;

        /// <summary>
        /// edge links no edges
        /// </summary>
        public static bool IsEdgeOfItems<TItem> (this IEdge<TItem> edge) => edge != null && !(edge.Root is IEdge<TItem>) && !(edge.Leaf is IEdge<TItem>);

        public static TItem Adjacent<TItem> (this IEdge<TItem> edge, TItem item) {

            if (edge == null)
                return default (TItem);

            if (edge.Root.Equals (item))
                return edge.Leaf;
            
            if (edge.Leaf.Equals (item))
                return edge.Root;
            
            return default (TItem);

        }
    }
}
