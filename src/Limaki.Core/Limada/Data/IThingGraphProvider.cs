/*
 * Limada 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */


using System.Collections.Generic;
using Limada.Model;
using Limaki.Data;
using Limaki.Graphs;

namespace Limada.Data {
    public interface IThingGraphProvider:IDataProvider<IThingGraph> {
        void ReadIntoList ( ICollection<IThing> things, IGraph<IThing, ILink> view );
        void Export ( IGraph<IThing, ILink> view, IThingGraph target );
    }
}