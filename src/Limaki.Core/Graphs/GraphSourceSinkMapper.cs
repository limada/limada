/*
 * Limaki
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using Limaki.Common.Collections;

namespace Limaki.Graphs {

    public interface IFactoryListener<T> {
        Action<T> ItemCreated { get; set; }
    }

    /// <summary>
    /// Converts Graph Source into Graph Sink
    /// </summary>
    /// <typeparam name="TSourceItem"></typeparam>
    /// <typeparam name="TSinkItem"></typeparam>
    /// <typeparam name="TSourceEdge"></typeparam>
    /// <typeparam name="TSinkEdge"></typeparam>
    public class GraphSourceSinkMapper<TSourceItem, TSinkItem, TSourceEdge, TSinkEdge>:IFactoryListener<TSinkItem>
        where TSourceEdge : IEdge<TSourceItem>, TSourceItem
        where TSinkEdge : IEdge<TSinkItem>, TSinkItem {

        protected IGraph<TSourceItem, TSourceEdge> _source = null;
        public virtual IGraph<TSourceItem, TSourceEdge> Source {
            get { return _source ?? (_source = new Graph<TSourceItem, TSourceEdge>()); }
            set { _source = value; }
        }

        protected IGraph<TSinkItem, TSinkEdge> _sink = null;
        public virtual IGraph<TSinkItem, TSinkEdge> Sink {
            get { return _sink ?? (_sink = new Graph<TSinkItem, TSinkEdge>()); }
            set { _sink = value; }
        }

        protected IDictionary<TSourceItem, TSinkItem> _dict = null;
        public IDictionary<TSourceItem, TSinkItem> Dict {
            get { return _dict ?? (_dict = new Dictionary<TSourceItem, TSinkItem>()); }
            set { _dict = value; }
        }



        #region Sink2Source

        public virtual TSinkItem TryGetCreate(TSourceItem source) {
            var result = default(TSinkItem);
            if (!Dict.TryGetValue(source, out result)) {
                if (!(source is TSourceEdge)) {
                    var sink = CreateItem(this.Source,this.Sink,source);
                    if (ItemCreated != null) {
                        ItemCreated (sink);
                    }
                    Sink.Add(sink);
                    RegisterPair(source, sink);
                    result = sink;
                } else {
                    var sourceEdge = (TSourceEdge)source;

                    var root = TryGetCreate(sourceEdge.Root);
                    var leaf = TryGetCreate(sourceEdge.Leaf);
                    Done.Add(sourceEdge.Root);
                    Done.Add(sourceEdge.Leaf);

                    if (!Dict.TryGetValue(source, out result)) {
                        var sinkEdge = CreateEdge(this.Source,this.Sink,sourceEdge);

                        sinkEdge.Root = root;
                        sinkEdge.Leaf = leaf;

                        if (ItemCreated != null) {
                            ItemCreated(sinkEdge);
                        }

                        RegisterPair(sourceEdge, sinkEdge);

                        Sink.Add(sinkEdge);

                        EdgeCreated(sourceEdge, sinkEdge);
                        result = sinkEdge;

                    }
                }
            }

            return result;
        }

        public ICollection<TSourceItem> _done = null;
        public ICollection<TSourceItem> Done {
            get { return _done ?? (_done = new Set<TSourceItem>());}
            set { _done = value; }
        }


        public virtual void Convert() {
            Done.Clear();
            foreach (var source in Source) {
                if (!Done.Contains(source)) {
                    TryGetCreate(source);
                    Done.Add(source);
                }

                foreach (var sourceEdge in Source.DepthFirstTwig(source)) {
                    if (!Done.Contains(sourceEdge)) {
                        TryGetCreate(sourceEdge);
                        Done.Add(sourceEdge);
                    }
                }
            }
            Done.Clear();
        }

        #endregion

        #region Factory-Methods


        public Func<IGraph<TSourceItem, TSourceEdge>, IGraph<TSinkItem, TSinkEdge>, TSourceItem, TSinkItem> CreateItem { get; set; }

        public Func<IGraph<TSourceItem, TSourceEdge>, IGraph<TSinkItem, TSinkEdge>, TSourceEdge, TSinkEdge> CreateEdge { get; set; }

        public Action<TSourceEdge, TSinkEdge> EdgeCreated { get; set; }

        public Action<TSourceItem, TSinkItem> RegisterPair { get; set; }

        public Action<TSinkItem> ItemCreated { get; set; }

        #endregion
    }


}