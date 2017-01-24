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

namespace Limaki.View.GtkBackend {

    public class ToolbarButtonBackend : ToolbarButtonBackendBase<Gtk.ToolItem> {
    }

    public class ToolbarButtonBackend1 : ToolbarButtonBackendBase<GtkToolButton> {
        public override Xwt.Drawing.Image Image {
            get { return base.Image;
            }
            set {
                base.Image = value;
                Widget.IconWidget = this.ImageWidget;
            }
        }
    }
}
