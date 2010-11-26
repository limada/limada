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
 */

using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;

namespace Limaki.Widgets {

    public class MoveCommand:Command<IWidget,PointI>{
        public MoveCommand(IWidget target, PointI param) : base(target, param) { }
        public override void Execute() {
           this.Target.Shape.Location = this.Parameter;
        }
    }
    public class MoveByCommand : Command<IWidget, SizeI> {
        public MoveByCommand(IWidget target, SizeI param) : base(target, param) { }
        public override void Execute() {
            this.Target.Shape.Location -= this.Parameter;
        }
    }
    public class ResizeCommand : Command<IWidget, RectangleI> {
        public ResizeCommand(IWidget target, RectangleI param) : base(target, param) { }

        public override void Execute() {
            this.Target.Shape.Location = this.Parameter.Location;
            this.Target.Shape.Size = this.Parameter.Size;
        }

    }

    
    public class DeleteCommand : Command<IWidget, Scene> {
        public DeleteCommand(IWidget target, Scene parameter) : base(target, parameter) { }
        public override void Execute() {
            this.Parameter.Graph.OnGraphChanged(Target, GraphChangeType.Remove);
            this.Parameter.Remove(Target);
        }
    }

    public class DeleteEdgeCommand : DeleteCommand {
        public DeleteEdgeCommand(IWidget target, Scene parameter) : base(target, parameter) { }
        public override void Execute() {
            this.Parameter.Graph.OnGraphChanged(Target, GraphChangeType.Remove);
            this.Parameter.RemoveBounds(Target); // edges are deleted on scene.remove()!
        }
    }

    public class RemoveBoundsCommand : DeleteCommand {
        public RemoveBoundsCommand(IWidget target, Scene parameter) : base(target, parameter) { }
        public override void Execute() {
            this.Parameter.RemoveBounds(Target);
        }
    }

    public class StateChangeCommand : Command<IWidget, Pair<UiState>> {
        public StateChangeCommand(IWidget target, Pair<UiState> param) : base(target, param) { }

        public override void Execute() {}

    }
}
