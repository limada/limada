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
using System.Collections.Generic;
using Limaki.Graphs;

namespace Limaki.Tests.Graph.Basic {

    public abstract class BasicGraphTestDataFactory<TItem, TEdge>
        where TEdge : IEdge<TItem> {

        public BasicGraphTestDataFactory() {
            CreateEdges ();
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

        protected abstract void CreateItems();

        protected virtual void CreateEdges() {
            CreateItems ();
            OneAside = CreateEdge(One, Aside);
            OneTwo = CreateEdge(One, Two);
            TwoThree = CreateEdge(Two, Three);
            if (Graph<TItem,TEdge>.EdgeIsItemClazz) {
                TwoThree_One = CreateEdge ((TItem)(object)TwoThree, One);
            }
        }

        protected virtual TEdge CreateEdge(TItem root, TItem leaf) {
            TEdge result = default(TEdge);
            result = (TEdge)Activator.CreateInstance(typeof(TEdge), new object[] { root, leaf });
            return result;
        }


        public virtual IEnumerable<TEdge> Edges {
            get {
                yield return OneTwo;
                yield return TwoThree;

                yield return OneAside;
                yield return OneAside;

                if (Graph<TItem, TEdge>.EdgeIsItemClazz) {
                    yield return TwoThree_One;
                }
                //var list = new TEdge[] {
                //              OneTwo,
                //              TwoThree,
                //              OneAside,
                //              OneAside,
                //            };
                //return list;
            }
        }

        public virtual IEnumerable<TItem> Items {
            get {
                yield return One;
                yield return Two;
                yield return Three;
                yield return Single;
                yield return Aside;
                yield return Nothing;
            }
        }
    }
}