/*
 * Limada 
 * Version 0.08
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
using System.Text;
using Limada.Model;
using Limaki.Model;
using Limaki.Tests;
using Limaki.UnitTest;
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;
using NUnit.Framework;
using Limada.Schemata;
using Limaki.Common.Collections;
using Limaki.Tests.Graph;
using System.IO;
using Limaki.Common;

namespace Limada.Tests.Model {
    public class MapperTester<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> 
        where TEdgeOne : IEdge<TItemOne>, TItemOne
        where TEdgeTwo : IEdge<TItemTwo>, TItemTwo {
        
        public bool TargetContainsSource(
                     IGraph<TItemOne, TEdgeOne> source,
                     IGraph<TItemTwo, TEdgeTwo> target,
                     Func<TItemOne,TItemTwo> getItem,
                     ICollection<TItemOne> notContained ) {
            bool result = true;

            if (notContained == null) {
                notContained = new EmptyCollection<TItemOne>();
            }


            foreach (TItemOne item in source) {
                if (!( item is TEdgeOne )) {
                    bool contains = getItem(item) != null;
                    if (!contains) {
                        notContained.Add(item);
                    }
                    result = result && contains;
                }
            }


            foreach (TEdgeOne edge in source.Edges()) {
                bool contains = getItem(edge) != null;
                if (!contains) {
                    notContained.Add(edge);
                }
                result = result && contains;
            }
            return result;
        }

        public void ProveConversion(
                IGraph<TItemOne, TEdgeOne> source,
                IGraph<TItemTwo, TEdgeTwo> target,
                Func<TItemOne,TItemTwo> getItem
            ) {

            ICollection<TItemOne> failedItems = new Set<TItemOne>();

            bool contains =
                TargetContainsSource(source,target,getItem, failedItems);

            if (!contains) {
                StringWriter message = new StringWriter();
                foreach (TItemOne item in failedItems) {
                    message.WriteLine(item.ToString() + "\tnot in " + target.GetType().Name);
                }
                Assert.Fail(message.ToString());
            }
        }

    }

    public class GraphMapperTest:DomainTest {
        public void TestMapper<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo>(
            IGraph<TItemOne, TEdgeOne> source, IGraph<TItemTwo, TEdgeTwo> target,
            GraphMapper<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> mapper)
              where TEdgeOne:IEdge<TItemOne>,TItemOne
              where TEdgeTwo:IEdge<TItemTwo>,TItemTwo{

            MapperTester<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo>
                testerOne = new MapperTester<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo>();
            mapper.ConvertOneTwo();

            testerOne.ProveConversion(source, target, mapper.Get);

            //mapper.Clear();
            mapper.ConvertTwoOne();
            MapperTester<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne>
                testerTwo = new MapperTester<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne>();

            testerTwo.ProveConversion(target, source, mapper.Get);


        }

        public void TestBinaryData( IGraphFactory<IGraphItem, IGraphEdge> data ) {
            data.Count = 10;
            data.Populate();
            this.ReportDetail(data.GetType().FullName + "\t"+data.Count);

            ThingGraph thingGraph = new ThingGraph();
            GraphMapper<IGraphItem,IThing,IGraphEdge,ILink> mapper =
                new GraphMapper<IGraphItem, IThing, IGraphEdge, ILink>(
                data.Graph, thingGraph, new GraphItem2ThingAdapter());
            TestMapper<IGraphItem,IThing,IGraphEdge,ILink>(data.Graph,thingGraph,mapper);
        }

        [Test]
        public void TestBinaryData() {
            TestBinaryData(new BinaryTreeFactory());
            TestBinaryData(new BinaryGraphFactory());
            ReportSummary();
        }

    }
}
