using System.Collections.Generic;
using Limaki.Graphs;

namespace Limaki.View.GraphScene {
	
    public interface IGraphSceneMapOrganizer<TItem, TEdge> where TEdge : TItem, IEdge<TItem> {
		
        void AddScene (IGraphScene<TItem, TEdge> scene);
        void RemoveScene (IGraphScene<TItem, TEdge> scene);

        ICollection<IGraphScene<TItem, TEdge>> Scenes { get; }

        IGraphSceneMapInteractor<TItem, TEdge> MapInteractor (IGraph<TItem, TEdge> graph);
        IGraphSceneMapInteractor<TItem, TSourceItem, TEdge,TSourceEdge> MapInteractor<TSourceItem, TSourceEdge> () where TSourceEdge : TSourceItem, IEdge<TSourceItem>;

        IGraph<TItem, TEdge> CreateSinkGraph (IGraph<TItem, TEdge> source);
        IGraphScene<TItem, TEdge> CreateSinkScene (IGraph<TItem, TEdge> sourceGraph);
        TItem LookUp (IGraph<TItem, TEdge> sourceGraph, IGraph<TItem, TEdge> sinkGraph, TItem lookItem);

    }
}