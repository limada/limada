/*
 * Limaki 
 * Version 0.071
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

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Limaki.Common;
using Limaki.Actions;

namespace Limaki.Widgets {

    public class MoveCommand:Command<IWidget,Point>{
        public MoveCommand(IWidget target, Point param) : base(target, param) { }
        public override void Execute() {
           this.Target.Shape.Location = this.Parameter;
        }
    }
    public class MoveByCommand : Command<IWidget, Size> {
        public MoveByCommand(IWidget target, Size param) : base(target, param) { }
        public override void Execute() {
            this.Target.Shape.Location -= this.Parameter;
        }
    }
    public class ResizeCommand : Command<IWidget, Rectangle> {
        public ResizeCommand(IWidget target, Rectangle param) : base(target, param) { }

        public override void Execute() {
            this.Target.Shape.Location = this.Parameter.Location;
            this.Target.Shape.Size = this.Parameter.Size;
        }

    }

    public class DeleteCommand : Command<IWidget, Scene> {
        public DeleteCommand(IWidget target, Scene parameter) : base(target, parameter) { }
        public override void Execute() {
            this.Parameter.Remove(Target);
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
