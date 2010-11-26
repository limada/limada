namespace Limada.Model {
    public interface IThingFactory {
        IThing CreateThing ( IThingGraph graph, object data );
        IThing CreateThing ( object data );
        ILink CreateLink ( IThingGraph graph, object data );
        ILink CreateLink ( object data );
        ILink CreateLink(IThing root, IThing leaf, IThing marker);
    }
}