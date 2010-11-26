/*
 * Limaki 
 * Version 0.063
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

namespace Limaki.Tests.Graph {

    public abstract class EdgeData<TItem, TEdge>
        where TEdge : IEdge<TItem> {

        public EdgeData() {
            InitData ();
        }

        public TItem One = default(TItem);
        public TItem Two = default(TItem);
        public TItem Three = default(TItem);
        public TItem Aside = default(TItem);
        public TItem Single = default(TItem);

        public TItem Nothing = default(TItem);

        public TEdge OneAside = default(TEdge);
        public TEdge OneTwo = default(TEdge);
        public TEdge TwoThree = default(TEdge);

        public TEdge TwoThree_One = default(TEdge);
        public TEdge reserved1 = default(TEdge);
        public TEdge reserved2 = default(TEdge);

        protected abstract void InitItems();

        protected virtual TEdge NewEdge(TItem root, TItem leaf) {
            TEdge result = default(TEdge);
            result = (TEdge)Activator.CreateInstance(typeof(TEdge), new object[] { root, leaf });
            return result;
        }
        protected virtual void InitData() {
            InitItems ();
            OneAside = NewEdge(One, Aside);
            OneTwo = NewEdge(One, Two);
            TwoThree = NewEdge(Two, Three);
        }

        public virtual IEnumerable<TEdge> List {
            get {
                yield return OneTwo;
                yield return TwoThree;

                yield return OneAside;
                yield return OneAside;
            }
        }
        }
}