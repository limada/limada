using System;
using System.Collections.Generic;
using Limaki.Graphs;

namespace Limaki.View.GraphScene {

    /// <summary>
    /// the sink side of a
    /// <see cref="IGraphSceneMapInteractor{TSinkItem,TSourceItem,TSinkEdge,TSourceEdge}"/>
    /// </summary>
    /// <typeparam name="TSinkItem"></typeparam>
    /// <typeparam name="TSinkEdge"></typeparam>
    public interface IGraphSceneMapInteractor<TSinkItem, TSinkEdge> where TSinkEdge : IEdge<TSinkItem>, TSinkItem {

        void RegisterBackGraph (IGraph<TSinkItem, TSinkEdge> graph);
        void UnregisterMappedGraph (IGraph<TSinkItem, TSinkEdge> graph);

        void Clear ();

        Func<ICollection<IGraphScene<TSinkItem, TSinkEdge>>> Scenes { get; set; }

        IEnumerable<IGraphScene<TSinkItem, TSinkEdge>> ScenesOfMappedGraph (IGraph<TSinkItem, TSinkEdge> graph);

        TSinkItem LookUp (IGraph<TSinkItem, TSinkEdge> sourceGraph, IGraph<TSinkItem, TSinkEdge> sinkGraph, TSinkItem lookItem);

        IGraph<TSinkItem, TSinkEdge> CreateSinkGraph (IGraph<TSinkItem, TSinkEdge> source);

    }

    /// <summary>
    /// handles mapping of <see cref="IGraph{TSinkItem, TSinkEdge}"/> to <see cref="IGraph{TSourceItem, TSourceEdge}"/>
    /// </summary>
	public interface IGraphSceneMapInteractor<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>
	where TSinkEdge : IEdge<TSinkItem>, TSinkItem
	where TSourceEdge : IEdge<TSourceItem>, TSourceItem {

		ICollection<IGraph<TSourceItem, TSourceEdge>> MappedGraphs { get; }

        void RegisterMappedGraph (IGraph<TSourceItem, TSourceEdge> root);
        void UnregisterMappedGraph (IGraph<TSourceItem, TSourceEdge> root);
        void UnregisterMappedGraph (IGraph<TSourceItem, TSourceEdge> root, bool forced);

		IGraph<TSourceItem, TSourceEdge> MappedGraphOf (IGraph<TSinkItem, TSinkEdge> graph);

		IEnumerable<IGraph<TSourceItem, TSourceEdge>> MappedGraphsOf (IGraph<TSourceItem, TSourceEdge> graph);

		IEnumerable<IGraphScene<TSinkItem, TSinkEdge>> ScenesOfMappedGraph (IGraph<TSourceItem, TSourceEdge> backGraph);

		IGraph<TSourceItem, TSourceEdge> WrapGraph (IGraph<TSourceItem, TSourceEdge> backGraph);

		IGraph<TSinkItem, TSinkEdge> CreateSinkGraph (IGraph<TSourceItem, TSourceEdge> backGraph);
		IGraphScene<TSinkItem, TSinkEdge> CreateScene (IGraph<TSourceItem, TSourceEdge> backGraph);
		IGraph<TSinkItem, TSinkEdge> CreateSinkGraph (IGraph<TSinkItem, TSinkEdge> source);

		TSinkItem LookUp (IGraph<TSinkItem, TSinkEdge> sourceGraph, IGraph<TSinkItem, TSinkEdge> sinkGraph, TSinkItem lookItem);

	}
}