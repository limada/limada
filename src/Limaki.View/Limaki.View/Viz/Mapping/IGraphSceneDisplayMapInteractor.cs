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
using Limaki.Graphs;
using Limaki.View;
using Limaki.View.GraphScene;

namespace Limaki.View.Viz.Mapping {
	
    public interface IGraphSceneDisplayMapInteractor<TSinkItem, TSinkEdge>: IGraphSceneMapInteractor<TSinkItem, TSinkEdge>
        where TSinkEdge : IEdge<TSinkItem>, TSinkItem {

        Func<ICollection<IGraphSceneDisplay<TSinkItem, TSinkEdge>>> Displays { get; set; }

    }
}