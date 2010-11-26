/*
 * Limaki 
 * Version 0.08
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

namespace Limaki.Graphs.Extensions {
    public interface IMarkerFacade<TOne, TEdgeOne> {
        string[] MarkersAsStrings();
        void ChangeMarkers ( IEnumerable<TOne> elements, object data );
        void ChangeAndAddMarker ( IEdge<TOne> edge, object data );
        IEdge<TOne> CreateDefaultEdge();
        object FittingMarker ( object data );
        object CreateMarker ( object data );
        object DefaultMarker { get;set;}
    }
}