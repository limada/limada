/*
 * Limaki
 * Version 0.07
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
using Limaki.Common;
using Limaki.Common.Collections;

namespace Limaki.Graphs {
    /// <summary>
    /// Converts Graph One into Graph Two
    /// </summary>
    /// <typeparam name="TItemOne"></typeparam>
    /// <typeparam name="TItemTwo"></typeparam>
    /// <typeparam name="TEdgeOne"></typeparam>
    /// <typeparam name="TEdgeTwo"></typeparam>
    public class GraphUniConverter<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo>
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

        public Act<TItemOne, TItemTwo> RegisterPair = null;

        #region One2Two

        public virtual TItemTwo Convert( TItemOne one ) {
            TItemTwo result = default(TItemTwo);
            if (!Dict.TryGetValue(one, out result)) {
                if (!( one is TEdgeOne )) {
                    TItemTwo two = CreateItem(one);
                    Two.Add(two);
                    RegisterPair(one, two);
                    result = two;
                } else {
                    TEdgeOne edgeOne = (TEdgeOne)one;

                    TItemTwo root = Convert(edgeOne.Root);
                    TItemTwo leaf = Convert(edgeOne.Leaf);
                    done.Add(edgeOne.Root);
                    done.Add(edgeOne.Leaf);

                    if (!Dict.TryGetValue(one, out result)) {
                        TEdgeTwo edgeTwo = CreateEdge(edgeOne);
                        edgeTwo.Root = root;
                        edgeTwo.Leaf = leaf;

                        RegisterPair(edgeOne, edgeTwo);

                        Two.Add(edgeTwo);

                        RegisterEdge(edgeOne, edgeTwo);
                        result = edgeTwo;

                    }
                }
            }

            return result;
        }

        Set<TItemOne> done = new Set<TItemOne>();

        public virtual void Convert() {
            done.Clear();
            foreach (TItemOne a in One) {
                if (!done.Contains(a)) {
                    Convert(a);
                    done.Add(a);
                }
                foreach (TEdgeOne edge in One.DepthFirstTwig(a)) {
                    if (!done.Contains(edge)) {
                        Convert(edge);
                        done.Add(edge);
                    }
                }
            }
            done.Clear();
        }

        #endregion

        #region Factory-Methods

        public Converter<TItemOne, TItemTwo> CreateItem = null;
        public Converter<TEdgeOne, TEdgeTwo> CreateEdge = null;
        public Act<TEdgeOne, TEdgeTwo> RegisterEdge = null;


        #endregion
        }
}