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


using Limada.Model;
using Limaki.Graphs;
using Limaki.View.Visuals;

namespace Limada.View.VisualThings {

    public class VisualThingGraph : LiveGraphPair<IVisual, IThing, IVisualEdge, ILink> {

        public VisualThingGraph( IGraph<IVisual, IVisualEdge> sink, IThingGraph source):
            this(sink, source, new VisualThingTransformer()) { }

        public VisualThingGraph (): this(null, null, new VisualThingTransformer()) {
            this.Sink = new VisualGraph ();
            this.Source = new ThingGraph ();
        }

        public VisualThingGraph (IGraph<IVisual, IVisualEdge> sink, IThingGraph source, VisualThingTransformer transformer ) : base(sink, source, transformer) {}
   
    }
}