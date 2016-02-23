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


using Limaki.Common;
using Limaki.Common.IOC;
using System;

namespace Limaki.Graphs {
    /// <summary>
    /// encapsulates operations on GraphPairs
    /// where the type of TItemTwo and TEdgeTwo are unknown
    /// </summary>
    [Obsolete("use Mesh")]
    public abstract class GraphMapping : IGraphMapping {
        
        private static IGraphMapping _graphMapping = null;
        public static IGraphMapping Mapping {
            get {
                if (_graphMapping == null) {
                    _graphMapping = Registry.Pooled<IGraphMapping>();
                }
                return _graphMapping;
            }
        }

        private IGraphMapping _next;

        public IGraphMapping Next {
            get { return _next; }
            set { _next = value; }
        }

        public abstract IGraph<TItem, TEdge> CloneGraphPair<TItem, TEdge>(IGraph<TItem, TEdge> source)
            where TEdge : IEdge<TItem>;

        [Obsolete ("use Mesh")]
        public abstract TItem LookUp<TItem, TEdge>(IGraphPair<TItem, TItem, TEdge, TEdge> sourceGraph,
                                                   IGraphPair<TItem, TItem, TEdge, TEdge> targetGraph,
                                                   TItem sourceitem) where TEdge : IEdge<TItem>, TItem;

        public TItem LookUp<TItem, TEdge> (IGraph<TItem, TEdge> sourceGraph, IGraph<TItem, TEdge> targetGraph, TItem sourceitem)
            where TEdge : IEdge<TItem>, TItem {
            return LookUp(sourceGraph as IGraphPair<TItem, TItem, TEdge, TEdge>, targetGraph as IGraphPair<TItem, TItem, TEdge, TEdge>, sourceitem);
        }

        public static void ChainGraphMapping<TMapping>(IApplicationContext context) where TMapping : IGraphMapping {
            IGraphMapping currentmapping = null;

            if (context.Factory.Contains<IGraphMapping>()) {
                currentmapping = context.Pooled<IGraphMapping>();
                context.Pool.Remove<IGraphMapping>();
            }

            context.Factory.Add<IGraphMapping, TMapping>();

            if (currentmapping != null && !(currentmapping is TMapping)) {
                IGraphMapping mapping = context.Pooled<IGraphMapping>();
                mapping.Next = currentmapping;
            }
        }
    }
}