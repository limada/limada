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


using Limada.Model;
using Limaki.Graphs;
using Limaki.Visuals;

namespace Limada.View {
    public class VisualThingGraph : LiveGraphPair<IVisual, IThing, IVisualEdge, ILink> {
        public VisualThingGraph( IGraph<IVisual, IVisualEdge> one, IThingGraph two):
            this(one, two, new VisualThingAdapter()) { }

        public VisualThingGraph ()
            : this(null, null, new VisualThingAdapter()) {
            this.One = new VisualGraph ();
            this.Two = new ThingGraph ();
        }

        public VisualThingGraph (
            IGraph<IVisual, IVisualEdge> one, IThingGraph two,
            VisualThingAdapter adapter )
            : base(one, two, adapter) {
        }
    }
}