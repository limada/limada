/*
 * Limaki 
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


using System;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Common;
using Limaki.Widgets;
using Limaki.Presenter.Layout;

namespace Limaki.Presenter.Widgets.Layout {
    public class ArrangerLayout<TItem, TEdge> : WidgetLayout<TItem, TEdge>
        where TItem : IWidget
        where TEdge : IEdge<TItem>, TItem {

        public ArrangerLayout(Get<IGraphScene<TItem,TEdge>> handler, IStyleSheet stylesheet) : base(handler, stylesheet) { }

        public override void Invoke() {
            var data = Data;
            if (data != null) {
                var arranger = new Arranger<TItem, TEdge>(data, this);

                arranger.ArrangeDeepWalk (
                    GraphPairExtension<TItem, TEdge>.FindRoots(data.Graph, data.Focused),
                    true,
                    (PointI) Distance);

                arranger.Commit();
            }
        }

    }


}

