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
using Limada.Schemata;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using System;
using Limaki.Common;

namespace Limada.Tests.Model {

   public class GraphItem2ThingTransformer : GraphItemTransformer<IGraphEntity, IThing, IGraphEdge, ILink> {
       private IThingFactory _factory = null;
       public IThingFactory Factory { get { return _factory ?? (_factory = Registry.Factory.Create<IThingFactory>()); } }
 
       public override IGraphEntity CreateSinkItem(IGraph<IThing, ILink> source,
            IGraph<IGraphEntity, IGraphEdge> sink, IThing item) {
            return new GraphEntity<string>(item.ToString());
        }

       public override IGraphEdge CreateSinkEdge(IGraph<IThing, ILink> source, 
           IGraph<IGraphEntity, IGraphEdge> sink,ILink item) {
            return new GraphEdge();
        }

       public override IThing CreateSourceItem(IGraph<IGraphEntity, IGraphEdge> sink,
           IGraph<IThing, ILink> source, IGraphEntity item) {
            return Factory.CreateItem(item.Data);
        }

       public override ILink CreateSourceEdge(IGraph<IGraphEntity, IGraphEdge> sink,
           IGraph<IThing, ILink> source, IGraphEdge item) {
           return Factory.CreateEdge(CommonSchema.EmptyMarker);
        }
       
       public override void ChangeData(IGraph<IThing, ILink> source, IThing item, object data) {
           throw new Exception("The method or operation is not implemented.");
       }

       public override void ChangeData(IGraph<IGraphEntity, IGraphEdge> sink, IGraphEntity item, object data) {
           IGraphPair<IGraphEntity, IThing, IGraphEdge, ILink> graph =
               sink as IGraphPair<IGraphEntity, IThing, IGraphEdge, ILink>;
           if (graph !=null) {
               var thing = graph.Get (item);
               thing.Data = data;
               item.Data = data;
           }
       }

       public override void UpdateSinkItem (IGraph<IThing, ILink> source, IGraph<IGraphEntity, IGraphEdge> sink, IThing sourceItem, IGraphEntity sinkItem) {
           sinkItem.Data = sourceItem.Data;
       }
   }
}