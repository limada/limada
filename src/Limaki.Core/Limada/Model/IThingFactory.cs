using Limaki.Graphs;
using Limaki.Common;
using Id = System.Int64;
using System;

namespace Limada.Model {

    public interface IThingFactory : IGraphModelFactory<IThing, ILink>, IFactory {
        
        IThing CreateItem();
        IThing CreateIdItem(Id id);
        IThing CreateItem<T>(Id id, T data);
        IThing CreateItem(IThingGraph graph, object data);
        IThing CreateItem(Id id, IThingGraph graph, object data);

        ILink CreateEdge ( IThingGraph graph, object data );
        ILink CreateEdge(Id id, IThingGraph graph, object data);
        ILink CreateEdge<T>(Id id, T data);
        ILink CreateEdge(Id id, IThing root, IThing leaf, IThing marker);

    }
}