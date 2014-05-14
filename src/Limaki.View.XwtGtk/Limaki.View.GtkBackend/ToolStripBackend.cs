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

using Limaki.View.Vidgets;
using Limaki.View.Viz.Visualizers.ToolStrips;

namespace Limaki.View.GtkBackend {

    public abstract class ToolStripBackend : Gtk.Toolbar, IDisplayToolStripBackend {

        #region IVidgetBackend

        public VidgetApplicationContext ApplicationContext { get; set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            ApplicationContext = context;
        }

        public Xwt.Size Size { get { return this.VidgetBackendSize(); } }

        public void Update () { this.VidgetBackendUpdate(); }

        public void Invalidate () { this.VidgetBackendInvalidate(); }

        public void SetFocus() { this.VidgetBackendSetFocus (); }

        public void Invalidate (Xwt.Rectangle rect) { this.VidgetBackendInvalidate(rect); }

        #endregion

        public void Dispose () { }

        protected virtual void Compose () {
            base.Orientation = Gtk.Orientation.Horizontal;
            base.ToolbarStyle = Gtk.ToolbarStyle.Icons;
        }

        public void AddItems (params Gtk.ToolItem[] items) {
            var ic = base.NItems;
            for (int i = 0; i < items.Length; i++)
                Insert (items[i], ic + i);

        }

       
    }
}