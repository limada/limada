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

using System.Collections.Generic;

namespace Limaki.Graphs {
    /// <summary>
    /// GraphPair where mapper is not filled automatically
    /// needs call of Mapper.ConvertSinkSource () or similar stuff to fill the Mapper
    /// </summary>
    /// <typeparam name="TSinkItem"></typeparam>
    /// <typeparam name="TSourceItem"></typeparam>
    /// <typeparam name="TSinkEdge"></typeparam>
    /// <typeparam name="TSourceEdge"></typeparam>
    public class HollowGraphPair<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> : GraphPair<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>
        where TSinkEdge : IEdge<TSinkItem>, TSinkItem
        where TSourceEdge : IEdge<TSourceItem>, TSourceItem {

        public HollowGraphPair (IGraph<TSinkItem, TSinkEdge> sink, IGraph<TSourceItem, TSourceEdge> source,
            GraphItemTransformer<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> Transformer)
            : base(sink, source, Transformer) { }

        public override TSinkItem Get (TSourceItem a) {
            return Mapper.Get (a);
        }

        public override ICollection<TSinkEdge> Edges (TSinkItem item) {
            return Sink.Edges (item);
        }

        public override int EdgeCount (TSinkItem item) {
            return Sink.EdgeCount (item);
        }

        public override IEnumerator<TSinkItem> GetEnumerator () {
            return Sink.GetEnumerator ();
        }
    }
}