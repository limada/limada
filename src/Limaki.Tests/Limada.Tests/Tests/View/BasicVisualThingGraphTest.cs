/*
 * Limada 
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
using System.Text;
using Limada.Tests.Basic;
using Limaki.Graphs;
using Limada.Model;
using Limada.View.VisualThings;
using Limaki.UnitTest;
using Limada.Tests.Model;
using Limaki.View.Visuals;
using NUnit.Framework;
using Limaki.Tests.View.Visuals;

using Id = System.Int64;
using Limaki.Tests.Graph.GraphPair;
using Limaki.Tests.Graph.Basic;
using Limada.Schemata;



namespace Limada.Tests.View {

    public class BasicVisualThingGraphTest : BasicGraphPairTest<IVisual,IThing,IVisualEdge,ILink> {

        public override IGraph<IVisual, IVisualEdge> Graph {
            get {
                if (!(base.Graph is VisualThingGraph)) {
                    base.Graph = new VisualThingGraph ();
                }
                return base.Graph;
            }
            set {
                if (value is VisualThingGraph ) {
                    base.Graph = value;    
                }
            }
        }

        public override BasicGraphTestDataFactory<IVisual, IVisualEdge> GetFactory() {
            return new VisualGraphTestDataFactory();
        }

        public void ProveEdge(IThingGraph thingGraph, ILink link) {
            bool delete = false;
            var idLink = ((ILink<Id>)link);
            delete = idLink.Root == 0 || idLink.Leaf == 0;
            if (!delete) {
                var root = link.Root;
                var leaf = link.Leaf;
                var marker = link.Marker;
                delete = root == null || leaf == null || root.Id == 0 || leaf.Id == 0;
            }
            if (!delete) {
                thingGraph.Add(link);
            } else {
                thingGraph.Remove(link);
            }
        }

        public void ExpandAndSaveLinks(IGraph<IVisual, IVisualEdge> graph) {
            var thingGraph = graph.ThingGraph().Unwrap() as IThingGraph;

            if (thingGraph != null) {
                foreach(ILink link in thingGraph.Edges()) {
                    ProveEdge (thingGraph, link);
                }
                foreach (IThing thing in thingGraph.Edges()) {
                    foreach (ILink edge in thingGraph.Fork(thing)) {
                        ProveEdge (thingGraph, edge);
                    }
                }
            }
        }

        [Test]
        public void TestChangeData() {
            object newData = "changed";
            InitGraphTest("** ChangeData\t" +
              Data.One.ToString() + "\tto\t" + newData.ToString());

            //Data.Sink.Data = newData;
            Pair.DoChangeData(Data.One, newData);
            Assert.AreEqual(newData, Data.One.Data);

            Pair.OnGraphChange (Data.One, GraphEventType.Update);

            IThing thing = Pair.Get (Data.One);
            Assert.AreEqual (newData, thing.Data.ToString ());
        }
    }
}
