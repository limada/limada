/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2013 Lytico
 *
 * http://www.limada.org
 */


using Limaki.Graphs;
using Limaki.View;
using System.Collections.Generic;

namespace Limaki.View.GraphScene {

    public interface ISceneViz<TSinkItem, TSinkEdge>
        where TSinkEdge : TSinkItem, IEdge<TSinkItem> {
        
        void Flush (IGraphScene<TSinkItem, TSinkEdge> scene);
        
        void MergeVisual (IGraphScene<TSinkItem, TSinkEdge> scene);
        void RevertEdges (IGraphScene<TSinkItem, TSinkEdge> scene);
    }

    public interface ISceneViz<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>: ISceneViz<TSinkItem, TSinkEdge>
        where TSinkEdge : TSinkItem, IEdge<TSinkItem>
		where TSourceEdge : TSourceItem, IEdge<TSourceItem> 
	{

		IGraph<TSourceItem, TSourceEdge> WrapGraph (IGraph<TSourceItem, TSourceEdge> source);

		IGraph<TSinkItem, TSinkEdge> CreateSinkGraph (IGraph<TSourceItem, TSourceEdge> source);

		IGraphScene<TSinkItem, TSinkEdge> CreateScene (IGraph<TSourceItem, TSourceEdge> source);

	    SubGraph<TSourceItem, TSourceEdge> CreateThingsView (IGraphScene<TSinkItem, TSinkEdge> scene);

        IEnumerable<TSourceItem> SelectedThings (IGraphScene<TSinkItem, TSinkEdge> scene);

        void SetDescription (IGraphScene<TSinkItem, TSinkEdge> scene, TSourceItem thing, string description);

	}

}