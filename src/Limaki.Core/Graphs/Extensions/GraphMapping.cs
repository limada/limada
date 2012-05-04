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

namespace Limaki.Graphs.Extensions {
    /// <summary>
    /// encapsulates operations on GraphPairs
    /// where the type of TItemTwo and TEdgeTwo are unknown
    /// </summary>
    public abstract class GraphMapping : IGraphMapping {
        
        private static IGraphMapping _graphMapping = null;
        public static IGraphMapping Mapping {
            get {
                if (_graphMapping == null) {
                    _graphMapping = Registry.Pool.TryGetCreate<IGraphMapping>();
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

        public abstract TItem LookUp<TItem, TEdge>(IGraphPair<TItem, TItem, TEdge, TEdge> sourceGraph,
                                                   IGraphPair<TItem, TItem, TEdge, TEdge> targetGraph,
                                                   TItem sourceitem) where TEdge : IEdge<TItem>, TItem;


        public static void ChainGraphMapping<TMapping>(IApplicationContext context) where TMapping : IGraphMapping {
            IGraphMapping currentmapping = null;

            if (context.Factory.Contains<IGraphMapping>()) {
                currentmapping = context.Pool.TryGetCreate<IGraphMapping>();
                context.Pool.Remove<IGraphMapping>();
            }

            context.Factory.Add<IGraphMapping, TMapping>();

            if (currentmapping != null && !(currentmapping is TMapping)) {
                IGraphMapping mapping = context.Pool.TryGetCreate<IGraphMapping>();
                mapping.Next = currentmapping;
            }
        }
    }
}