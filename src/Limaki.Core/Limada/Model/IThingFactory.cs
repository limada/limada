using Limaki.Graphs;
namespace Limada.Model {
    public interface IThingFactory:IGraphModelFactory<IThing,ILink> {
        IThing CreateItem();
        IThing CreateItem ( IThingGraph graph, object data );
        ILink CreateEdge ( IThingGraph graph, object data );
    }
}