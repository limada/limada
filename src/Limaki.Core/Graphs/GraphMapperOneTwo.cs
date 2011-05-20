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
using Limaki.Common.Collections;

namespace Limaki.Graphs {
    public interface IFactoryListener<T> {
        Action<T> ItemCreated { get; set; }
    }

    /// <summary>
    /// Converts Graph One into Graph Two
    /// </summary>
    /// <typeparam name="TItemOne"></typeparam>
    /// <typeparam name="TItemTwo"></typeparam>
    /// <typeparam name="TEdgeOne"></typeparam>
    /// <typeparam name="TEdgeTwo"></typeparam>
    public class GraphMapperOneTwo<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo>:IFactoryListener<TItemTwo>
        where TEdgeOne : IEdge<TItemOne>, TItemOne
        where TEdgeTwo : IEdge<TItemTwo>, TItemTwo {

        protected IGraph<TItemOne, TEdgeOne> _one = null;
        public virtual IGraph<TItemOne, TEdgeOne> One {
            get {
                if (_one == null) {
                    _one = new Graph<TItemOne, TEdgeOne>();
                }
                return _one;

            }
            set { _one = value; }
        }

        protected IGraph<TItemTwo, TEdgeTwo> _two = null;
        public virtual IGraph<TItemTwo, TEdgeTwo> Two {
            get {
                if (_two == null) {
                    _two = new Graph<TItemTwo, TEdgeTwo>();
                }
                return _two;

            }
            set { _two = value; }
        }

        private IDictionary<TItemOne, TItemTwo> _dict = null;
        public IDictionary<TItemOne, TItemTwo> Dict {
            get {
                if (_dict == null) {
                    _dict = new Dictionary<TItemOne, TItemTwo>();
                }
                return _dict;

            }
            set { _dict = value; }
        }



        #region One2Two

        public virtual TItemTwo TryGetCreate(TItemOne one) {
            TItemTwo result = default(TItemTwo);
            if (!Dict.TryGetValue(one, out result)) {
                if (!(one is TEdgeOne)) {
                    TItemTwo two = CreateItem(this.One,this.Two,one);
                    if (ItemCreated != null) {
                        ItemCreated (two);
                    }
                    Two.Add(two);
                    RegisterPair(one, two);
                    result = two;
                } else {
                    TEdgeOne edgeOne = (TEdgeOne)one;

                    TItemTwo root = TryGetCreate(edgeOne.Root);
                    TItemTwo leaf = TryGetCreate(edgeOne.Leaf);
                    Done.Add(edgeOne.Root);
                    Done.Add(edgeOne.Leaf);

                    if (!Dict.TryGetValue(one, out result)) {
                        TEdgeTwo edgeTwo = CreateEdge(this.One,this.Two,edgeOne);

                        edgeTwo.Root = root;
                        edgeTwo.Leaf = leaf;

                        if (ItemCreated != null) {
                            ItemCreated(edgeTwo);
                        }

                        RegisterPair(edgeOne, edgeTwo);

                        Two.Add(edgeTwo);

                        EdgeCreated(edgeOne, edgeTwo);
                        result = edgeTwo;

                    }
                }
            }

            return result;
        }

        public ICollection<TItemOne> _done = null;
        public ICollection<TItemOne> Done {
            get { return _done ?? (_done = new Set<TItemOne>());}
            set { _done = value; }
        }


        public virtual void Convert() {
            Done.Clear();
            foreach (TItemOne a in One) {
                if (!Done.Contains(a)) {
                    TryGetCreate(a);
                    Done.Add(a);
                }

                foreach (TEdgeOne edge in One.DepthFirstTwig(a)) {
                    if (!Done.Contains(edge)) {
                        TryGetCreate(edge);
                        Done.Add(edge);
                    }
                }
            }
            Done.Clear();
        }

        #endregion

        #region Factory-Methods


        public Func<IGraph<TItemOne,TEdgeOne>,IGraph<TItemTwo,TEdgeTwo>,TItemOne, TItemTwo> 
            CreateItem = null;
        public Func<IGraph<TItemOne, TEdgeOne>, IGraph<TItemTwo, TEdgeTwo>, TEdgeOne, TEdgeTwo> 
            CreateEdge = null;
        public Action<TEdgeOne, TEdgeTwo> EdgeCreated = null;

        public Action<TItemOne, TItemTwo> RegisterPair = null;
        

        private Action<TItemTwo> _itemCreated = null;
        public Action<TItemTwo> ItemCreated {
            get { return _itemCreated; }
            set { _itemCreated = value; }
        }
       
        #endregion
    }


}