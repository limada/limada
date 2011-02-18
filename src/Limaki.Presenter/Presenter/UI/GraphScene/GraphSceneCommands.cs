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
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;

namespace Limaki.Presenter.UI {
    public interface IDirtyCommand {}
    public class MoveCommand<TItem> : Command<TItem, Func<TItem,IShape>, PointI>,IDirtyCommand {
        public MoveCommand(TItem target, Func<TItem, IShape> shape, PointI param) : base(target, shape, param) { }
        public override void Execute() {
            var shape = this.Parameter (this.Subject);
            shape.Location = this.Parameter2;
        }
    }
    public class MoveByCommand<TItem> : Command<TItem, Func<TItem, IShape>, SizeI>,IDirtyCommand {
        public MoveByCommand(TItem target, Func<TItem, IShape> shape, SizeI param) : base(target, shape, param) { }
        public override void Execute() {
            var shape = this.Parameter(Subject);
            shape.Location -= this.Parameter2;
        }
    }

    public class ResizeCommand<TItem> : Command<TItem, Func<TItem, IShape>, RectangleI>, IDirtyCommand {
        public ResizeCommand(TItem target, Func<TItem, IShape> shape, RectangleI param) : base(target, shape, param) { }

        public override void Execute() {
            var shape = this.Parameter (Subject);
            shape.Location = this.Parameter2.Location;
            shape.Size = this.Parameter2.Size;
        }

    }

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