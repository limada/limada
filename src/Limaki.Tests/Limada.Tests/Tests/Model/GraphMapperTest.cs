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


using Limada.Model;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests;
using Limaki.Tests.Graph.Model;
using NUnit.Framework;

namespace Limada.Tests.Model {
    public class GraphMapperTest:DomainTest {

        public void ProveMapper<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo>(
            IGraph<TItemOne, TEdgeOne> source, IGraph<TItemTwo, TEdgeTwo> target,
            GraphMapper<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> mapper)
              where TEdgeOne:IEdge<TItemOne>,TItemOne
              where TEdgeTwo:IEdge<TItemTwo>,TItemTwo{

            var testerOne = new MapperTester<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo>();
            mapper.ConvertSinkSource();

            testerOne.ProveConversion(source, target, mapper.Get);

            //mapper.Clear();
            mapper.ConvertSourceSink();
            var testerTwo = new MapperTester<TItemTwo, TItemOne, TEdgeTwo, TEdgeOne>();

            testerTwo.ProveConversion(target, source, mapper.Get);


        }

        public void ProveSample( ISampleGraphFactory<IGraphEntity, IGraphEdge> data ) {
            data.Count = 10;
            data.Populate();
            this.ReportDetail(data.GetType().FullName + "\t"+data.Count);

            var thingGraph = new ThingGraph();
            var mapper =
                new GraphMapper<IGraphEntity, IThing, IGraphEdge, ILink>(
                data.Graph, thingGraph, new GraphEntity2ThingTransformer());
            ProveMapper<IGraphEntity,IThing,IGraphEdge,ILink>(data.Graph,thingGraph,mapper);
        }

        [Test]
        public void TestBinaryData() {
            ProveSample(new EntityBinaryTreeFactory());
            ProveSample(new EntityBinaryGraphFactory());
            ReportSummary();
        }

    }
}
