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

using Limaki.Drawing;

namespace Limaki.Widgets {
    public class Renderer {

        Renderer Parent = null;
        public Renderer(Renderer parent) {
            this.Parent = parent;
        }

        Scene _scene = null;
        public virtual Scene Scene {
            get {
                if ((_scene == null) && (Parent != null)) {
                    return Parent.Scene;
                }
                return _scene;
            }
            set { _scene = value; }
        }

        private ILayout<Scene,IWidget> _layout = null;
        public virtual ILayout<Scene, IWidget> Layout {
            get {
                if ((_layout == null) && (Parent != null)) {
                    return Parent.Layout;
                }
                return _layout;
            }
            set { _layout = value; }
        }



    }
}