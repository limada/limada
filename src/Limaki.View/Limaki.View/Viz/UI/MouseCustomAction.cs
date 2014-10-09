/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.View.Vidgets;

namespace Limaki.View.Viz.UI {

    public class MouseCustomAction : MouseActionBase, IMouseAction {

        public MouseCustomAction():base(){}

        public MouseCustomAction (EventHandler<MouseActionEventArgs> mouseDown) : base () {
            this.MouseDown += mouseDown;
        }

        public event EventHandler<MouseActionEventArgs> MouseDown;
        public event EventHandler<MouseActionEventArgs> MouseMove;
        public event EventHandler<MouseActionEventArgs> MouseHover;
        public event EventHandler<MouseActionEventArgs> MouseUp;

        public override void OnMouseDown (MouseActionEventArgs e) {
            base.OnMouseDown (e);
            if (MouseDown != null)
                MouseDown (this, e);
        }

        public override void OnMouseMove (MouseActionEventArgs e) {
            if (MouseMove != null)
                MouseMove (this, e);
        }

        public override void OnMouseHover (MouseActionEventArgs e) {
            base.OnMouseHover (e);
            if (MouseHover != null)
                MouseHover (this, e);
        }

        public override void OnMouseUp (MouseActionEventArgs e) {
            base.OnMouseUp (e);
            if (MouseUp != null)
                MouseUp (this, e);
        }
    }
}