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
using Limaki.View.Viz.Visualizers.ToolStrips;

namespace Limaki.View.GtkBackend {

    public class ToolStripBackend : VidgetBackend<Gtk.Toolbar>, IToolStripBackend {

        public VidgetApplicationContext ApplicationContext { get; set; }

        public new ToolStrip Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            ApplicationContext = context;
            this.Frontend = (ToolStrip)frontend;
        }


        protected override void Compose () {
            base.Compose ();
            Widget.Orientation = Gtk.Orientation.Horizontal;
            Widget.ToolbarStyle = Gtk.ToolbarStyle.Icons;
        }

        [Obsolete]
        public void AddItems (params Gtk.ToolItem[] items) {
            var ic = Widget.NItems;
            for (int i = 0; i < items.Length; i++)
                Widget.Insert (items[i], ic + i);

        }

        public void InsertItem (int index, IToolStripItemBackend item) {
            Widget.Insert (item.ToGtk(), index);
        }

        public void RemoveItem (IToolStripItemBackend item) {
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