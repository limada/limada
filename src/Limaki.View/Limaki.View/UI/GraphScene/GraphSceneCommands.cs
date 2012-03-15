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


using System;
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Xwt;
using Limaki.View.Layout;

namespace Limaki.View.UI.GraphScene {
    
    public class DeleteCommand<TItem, TEdge> : Command<TItem, IGraphScene<TItem, TEdge>>, IDirtyCommand
    where TEdge : TItem, IEdge<TItem> {
        public DeleteCommand(TItem target, IGraphScene<TItem, TEdge> parameter) : base(target, parameter) { }
        public override void Execute() {
            this.Parameter.Graph.OnGraphChanged(Subject, GraphChangeType.Remove);
            this.Parameter.Remove(Subject);
        }
    }

    public class DeleteEdgeCommand<TItem, TEdge> : DeleteCommand<TItem, TEdge>
    where TEdge : TItem, IEdge<TItem> {
        public DeleteEdgeCommand(TItem target, IGraphScene<TItem, TEdge> parameter) : base(target, parameter) { }
        public override void Execute() {
            this.Parameter.Graph.OnGraphChanged(Subject, GraphChangeType.Remove);
            this.Parameter.RemoveBounds(Subject); // edges are deleted on scene.remove()!
        }
    }

    public class RemoveBoundsCommand<TItem, TEdge> : DeleteCommand<TItem, TEdge>
    where TEdge : TItem, IEdge<TItem> {
        public RemoveBoundsCommand(TItem target, IGraphScene<TItem, TEdge> parameter) : base(target, parameter) { }
        public override void Execute() {
            this.Parameter.RemoveBounds(Subject);
        }
    }

    public class StateChangeCommand<TItem> : Command<TItem, Pair<UiState>> {
        public StateChangeCommand(TItem target, Pair<UiState> param) : base(target, param) { }

        public override void Execute() { }

    }
}