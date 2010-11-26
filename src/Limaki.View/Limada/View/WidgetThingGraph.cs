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
using Limaki.Widgets;

namespace Limada.View {
    public class WidgetThingGraph : LiveGraphPair<IWidget, IThing, IEdgeWidget, ILink> {
        public WidgetThingGraph( IGraph<IWidget, IEdgeWidget> one, IThingGraph two):
            this(one, two, new WidgetThingAdapter()) { }

        public WidgetThingGraph ()
            : this(null, null, new WidgetThingAdapter()) {
            this.One = new WidgetGraph ();
            this.Two = new ThingGraph ();
        }

        public WidgetThingGraph (
            IGraph<IWidget, IEdgeWidget> one, IThingGraph two,
            WidgetThingAdapter adapter )
            : base(one, two, adapter) {
        }
    }
}