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
 * http://limada.sourceforge.net
 * 
 */

using Limada.Model;
using Limada.Schemata;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using System;
using Limaki.Common;

namespace Limada.Tests.Model {
   public class GraphItem2ThingAdapter : GraphModelAdapter<IGraphItem, IThing, IGraphEdge, ILink> {
       private IThingFactory _factory = null;
       public IThingFactory Factory { get { return _factory ?? (_factory = Registry.Factory.Create<IThingFactory>()); } }
 
       public override IGraphItem CreateItemOne(IGraph<IThing, ILink> sender,
            IGraph<IGraphItem, IGraphEdge> target, IThing item) {
            return new GraphItem<string>(item.ToString());
        }

       public override IGraphEdge CreateEdgeOne(IGraph<IThing, ILink> sender, 
           IGraph<IGraphItem, IGraphEdge> target,ILink item) {
            return new GraphEdge();
        }

       public override IThing CreateItemTwo(IGraph<IGraphItem, IGraphEdge> sender,
           IGraph<IThing, ILink> target, IGraphItem item) {
            return Factory.CreateItem(item.Data);
        }

       public override ILink CreateEdgeTwo(IGraph<IGraphItem, IGraphEdge> sender,
           IGraph<IThing, ILink> target, IGraphEdge item) {
           return Factory.CreateEdge(CommonSchema.EmptyMarker);
        }
       
       public override void ChangeData(IGraph<IThing, ILink> sender, IThing item, object data) {
           throw new Exception("The method or operation is not implemented.");
       }

       public override void ChangeData(IGraph<IGraphItem, IGraphEdge> sender, IGraphItem item, object data) {
           IGraphPair<IGraphItem, IThing, IGraphEdge, ILink> graph =
               sender as IGraphPair<IGraphItem, IThing, IGraphEdge, ILink>;
           if (graph !=null) {
               IThing thing = graph.Get (item);
               thing.Data = data;
               item.Data = data;
           }
       }
    }
}