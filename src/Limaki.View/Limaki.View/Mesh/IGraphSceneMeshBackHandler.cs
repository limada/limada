/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using Limaki.Drawing;
using Limaki.Graphs;

namespace Limaki.View.Mesh {

    /// <summary>
    /// the sink side of a
    /// <see cref="GraphSceneMeshBackHandler{TSinkItem, TSourceItem, TSinkEdge, TSourceEdge}"/>
    /// </summary>
    /// <typeparam name="TSinkItem"></typeparam>
    /// <typeparam name="TSinkEdge"></typeparam>
    public interface IGraphSceneMeshBackHandler<TSinkItem, TSinkEdge> where TSinkEdge : IEdge<TSinkItem>, TSinkItem {

        void RegisterBackGraph (IGraph<TSinkItem, TSinkEdge> graph);
        void UnregisterBackGraph (IGraph<TSinkItem, TSinkEdge> graph);

        Func<ICollection<IGraphScene<TSinkItem, TSinkEdge>>> Scenes { get; set; }
    }
}