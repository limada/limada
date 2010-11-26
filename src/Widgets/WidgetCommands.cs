/*
 * Limaki 
 * Version 0.064
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
}
