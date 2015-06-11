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



using Limada.Model;
using Limada.Schemata;
using Limada.View.VisualThings;
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;
using Limaki.View;
using Limaki.View.Visuals;

namespace Limada.Tests.View {

    public class SchemaViewFactory : SampleGraphFactory<IThing, ILink> {

        public override void Populate (IGraph<IThing, ILink> graph) {

        }

        public override void Populate (IGraph<IThing, ILink> graph, int start) {
            
        }
    }

    public class SchemaViewTestData<T> : ViusalThingSampleSceneFactory<SchemaViewFactory>
        where T : Schema, new () {

        public override Limaki.Graphs.IGraph<IThing, ILink> CreateSourceGraph () {
            return new T ().SchemaGraph;
        }

        public override GraphPair<IVisual, IThing, IVisualEdge, ILink> CreateGraphPair () {
            return new VisualThingGraph (Mapper.Sink, Mapper.Source as IThingGraph, Mapper.Transformer as VisualThingTransformer);
        }

    }
}
