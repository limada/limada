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

namespace Limaki.View.GtkBackend {

    public class ToolbarBackend : VidgetBackend<Gtk.Toolbar>, IToolbarBackend {

        public VidgetApplicationContext ApplicationContext { get; set; }

        public new Toolbar Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            ApplicationContext = context;
            this.Frontend = (Toolbar)frontend;
        }

        protected override void Compose () {
            base.Compose ();
            Widget.Orientation = Gtk.Orientation.Horizontal;
            Widget.ToolbarStyle = Gtk.ToolbarStyle.Icons;
        }

        public void InsertItem (int index, IToolbarItemBackend item) {
			var w = item.ToGtk();
            Widget.Insert (w, index);
        }

        public void RemoveItem (IToolbarItemBackend item) {
            Widget.Remove (item.ToGtk ());
        }

        public void SetVisibility (Visibility value) {
            if (value == Visibility.Visible && !Widget.Visible)
                Widget.Visible = true;
            else
                Widget.Visible = false;
        }
    }
}