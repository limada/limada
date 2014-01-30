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

using Limaki.View.UI;
using Size = Xwt.Size;


namespace Limaki.View.GtkBackends {
    public class GtkSystemInformation : IUISystemInformation {
        public Size DragSize {
            get { return new Size(Gtk.Settings.Default.DndDragThreshold, Gtk.Settings.Default.DndDragThreshold); }
        }

        public int DoubleClickTime {
            get { return Gtk.Settings.Default.DoubleClickTime; }
        }

        public int VerticalScrollBarWidth {
            get { return 10; }
        }

        public int HorizontalScrollBarHeight {
            get { return 10; }
        }
    }
}