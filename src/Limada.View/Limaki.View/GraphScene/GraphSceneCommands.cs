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
 * http://www.limada.org
 * 
 */


using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.Common;
using Limaki.View.Viz.Modelling;

namespace Limaki.View.GraphScene {
    
    public class DeleteCommand<TItem, TEdge> : Command<TItem, IGraphScene<TItem, TEdge>>, IDirtyCommand
    where TEdge : TItem, IEdge<TItem> {
        
        public DeleteCommand(TItem subject, IGraphScene<TItem, TEdge> parameter) : base(subject, parameter) { }
        
        public override void Execute() {
            this.Parameter.Graph.OnGraphChange(Subject, GraphEventType.Remove);
            this.Parameter.Remove(Subject);
        }
    }

    public class DeleteEdgeCommand<TItem, TEdge> : DeleteCommand<TItem, TEdge>
    where TEdge : TItem, IEdge<TItem> {

        public DeleteEdgeCommand(TItem subject, IGraphScene<TItem, TEdge> parameter) : base(subject, parameter) { }
        
        public override void Execute() {
            this.Parameter.Graph.OnGraphChange(Subject, GraphEventType.Remove);
            this.Parameter.RemoveBounds(Subject); // edges are deleted on scene.remove() of leaf or root (see above)
        }
    }

    public class RemoveBoundsCommand<TItem, TEdge> : DeleteCommand<TItem, TEdge>
    where TEdge : TItem, IEdge<TItem> {
        public RemoveBoundsCommand(TItem subject, IGraphScene<TItem, TEdge> parameter) : base(subject, parameter) { }
        public override void Execute() {
            this.Parameter.RemoveBounds(Subject);
        }
    }

    public class StateChangeCommand<TItem> : Command<TItem, Pair<UiState>> {
        public StateChangeCommand(TItem subject, Pair<UiState> param) : base(subject, param) { }

        public override void Execute() { }

    }
}