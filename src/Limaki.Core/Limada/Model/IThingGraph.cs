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
using Limaki.Data;
using Limaki.Graphs;
using Id = System.Int64;
using System.Linq.Expressions;
using System;

namespace Limada.Model {
    public interface IThingGraph:IGraph<IThing,ILink> {
        IThing GetById ( Id id );
        bool IsMarker ( IThing thing );
        ICollection<IThing> Markers();
        void AddMarker ( IThing marker );
        IEnumerable<IThing> GetByData ( object data );
        IEnumerable<IThing> GetByData ( object data, bool exact );
        IDataContainer<Id> DataContainer { get;set;}
        IThing UniqueThing ( IThing thing );
        IEnumerable<T> Where<T>(Expression<Func<T,bool>> predicate) where T:IThing;
    }
}