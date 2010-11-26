/*
 * Limaki 
 * Version 0.064
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
using NUnit.Framework;

namespace Limaki.Tests.Graph {
    public class Item<T> {
        public T Data = default( T );
        public Item(T data) {
            this.Data = data;
        }
        public override string ToString() {
            return Data.ToString();
        }
    }

    public class EdgeIsItem<T> : Item<T>, IEdge<Item<T>> {

        #region IEdge<Item> Member

        public EdgeIsItem(Item<T> root, Item<T> leaf):base(default(T)) {
            Root = root;
            Leaf = leaf;
        }


        public Item<T> _root = default(Item<T>);
        public Item<T> Root {
            get { return _root; }
            set {_root = value;}
        }

        public Item<T> _leaf = default(Item<T>);
        public Item<T> Leaf {
            get { return _leaf; }
            set { _leaf = value; }
        }
        public override string ToString() {
            return String.Format("[{0}->{1}]", this.Root, this.Leaf);
        }

        #endregion
    }

    public class EdgeIsItemData : EdgeData<Item<string>, EdgeIsItem<string>> {
        protected override void InitItems() {
            One = new Item<string>("One");
            Two = new Item<string>("Two");
            Three = new Item<string>("Three");
            Aside = new Item<string>("Aside");
            Single = new Item<string>("Single");
        }
        protected override void InitData() {
            base.InitData();
            TwoThree_One = new EdgeIsItem<string> (TwoThree, One);
        }
        public override IEnumerable<EdgeIsItem<string>> Edges {
            get {
                foreach (EdgeIsItem<string> edge in base.Edges)
                    yield return edge;

                yield return TwoThree_One;

            }
        } 
    }

    public class EdgeIsItemGraphTest : GraphTest<Item<string>, EdgeIsItem<string>> {
        public override EdgeData<Item<string>, EdgeIsItem<string>> createData() {
            return new EdgeIsItemData();
        }
        [Test]
        public override void AddNothing() {
            base.AddNothing();
        }
        [Test]
        public override void JustAddingData() {
            base.JustAddingData();
        }
        [Test]
        public override void RemoveLink() {
            base.RemoveLink();
        }
        [Test]
        public override void RemoveItem() {
            base.RemoveItem();
            InitGraphTest ("** remove "+data.One);
            Graph.Remove(data.One);
            FullReportGraph (Graph,"Removed:\t" + data.One);

        }
        [Test]
        public override void AddSingle() {
            base.AddSingle();
        }
    }
}