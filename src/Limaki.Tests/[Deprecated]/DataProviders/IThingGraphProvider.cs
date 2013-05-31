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


using System.Collections.Generic;
using Limada.Model;
using Limaki.Data;
using Limaki.Graphs;

namespace Limada.Data {
    public interface IThingGraphProvider:IDataProvider<IThingGraph> {
        void ThingsOfView (IGraph<IThing, ILink> sourceView, ICollection<IThing> sink);
        void Export ( IGraph<IThing, ILink> view, IThingGraph target );
        void RawImport(IoInfo source, IDataProvider<IThingGraph> target);
    }
}