using System;
using System.Collections.Generic;
using Limaki.Graphs;

namespace Limaki.View.GraphScene {
    /// <summary>
    /// the sink side of a
    /// <see cref="IGraphSceneMeshBackHandler{TSinkItem,TSourceItem,TSinkEdge,TSourceEdge}"/>
    /// </summary>
    /// <typeparam name="TSinkItem"></typeparam>
    /// <typeparam name="TSinkEdge"></typeparam>
    public interface IGraphSceneMeshBackHandler<TSinkItem, TSinkEdge> where TSinkEdge : IEdge<TSinkItem>, TSinkItem {

        void RegisterBackGraph (IGraph<TSinkItem, TSinkEdge> graph);
        void UnregisterBackGraph (IGraph<TSinkItem, TSinkEdge> graph);

        void Clear ();

        Func<ICollection<IGraphScene<TSinkItem, TSinkEdge>>> Scenes { get; set; }

        IEnumerable<IGraphScene<TSinkItem, TSinkEdge>> ScenesOfBackGraph (IGraph<TSinkItem, TSinkEdge> graph);

        TSinkItem LookUp (IGraph<TSinkItem, TSinkEdge> sourceGraph, IGraph<TSinkItem, TSinkEdge> sinkGraph, TSinkItem lookItem);

        IGraph<TSinkItem, TSinkEdge> CreateSinkGraph (IGraph<TSinkItem, TSinkEdge> source);

    }

	public interface IGraphSceneMeshBackHandler<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>
	where TSinkEdge : IEdge<TSinkItem>, TSinkItem
	where TSourceEdge : IEdge<TSourceItem>, TSourceItem {

		ICollection<IGraph<TSourceItem, TSourceEdge>> BackGraphs { get; }
		IGraph<TSourceItem, TSourceEdge> BackGraphOf (IGraph<TSinkItem, TSinkEdge> graph);
		void RegisterBackGraph (IGraph<TSourceItem, TSourceEdge> root);
		void UnregisterBackGraph (IGraph<TSourceItem, TSourceEdge> root);
		void UnregisterBackGraph (IGraph<TSourceItem, TSourceEdge> root, bool forced);
		IEnumerable<IGraph<TSourceItem, TSourceEdge>> BackGraphsOf (IGraph<TSourceItem, TSourceEdge> graph);
		IEnumerable<IGraphScene<TSinkItem, TSinkEdge>> ScenesOfBackGraph (IGraph<TSourceItem, TSourceEdge> backGraph);
		IGraph<TSourceItem, TSourceEdge> WrapGraph (IGraph<TSourceItem, TSourceEdge> backGraph);
		IGraph<TSinkItem, TSinkEdge> CreateSinkGraph (IGraph<TSourceItem, TSourceEdge> backGraph);
		IGraphScene<TSinkItem, TSinkEdge> CreateScene (IGraph<TSourceItem, TSourceEdge> backGraph);
		IGraph<TSinkItem, TSinkEdge> CreateSinkGraph (IGraph<TSinkItem, TSinkEdge> source);
		TSinkItem LookUp (IGraph<TSinkItem, TSinkEdge> sourceGraph, IGraph<TSinkItem, TSinkEdge> sinkGraph, TSinkItem lookItem);

	}
}