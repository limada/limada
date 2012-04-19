/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */


using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Common;
using Limaki.View.Layout;
using Limaki.Visuals;
using Xwt;

namespace Limaki.View.Visuals.Layout {

    public class VisualsSceneArrangerLayout<TItem, TEdge> : VisualsSceneLayout<TItem, TEdge>
        where TItem : IVisual
        where TEdge : IEdge<TItem>, TItem {

        public VisualsSceneArrangerLayout(Get<IGraphScene<TItem, TEdge>> handler, IStyleSheet stylesheet) : base(handler, stylesheet) { }

        public override void Invoke() {
            var data = Data;
            if (data != null) {
                var arranger = new Arranger0<TItem, TEdge>(data, this);

                arranger.ArrangeDeepWalk (
                    data.Graph.FindRoots(data.Focused),
                    true,
                    (Point) Border);

                arranger.Commit();
            }
        }

    }


}

