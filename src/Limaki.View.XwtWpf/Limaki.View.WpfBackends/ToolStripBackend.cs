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

using System.Linq;
using System.Text;
using System.Windows.Controls;
using Limaki.View.Vidgets;
using Limaki.View.Viz.Visualizers.ToolStrips;
using System.Windows;

namespace Limaki.View.WpfBackend {

    public abstract class ToolStripBackend : ToolBar, IDisplayToolStripBackend {

        #region IVidgetBackend

        public abstract void InitializeBackend (IVidget frontend, VidgetApplicationContext context);

        public Xwt.Size Size { get { return this.VidgetBackendSize(); } }

        public void Update () { this.VidgetBackendUpdate(); }

        public void Invalidate () { this.VidgetBackendInvalidate(); }

        public void SetFocus() { this.VidgetBackendSetFocus (); }

        public void Invalidate (Xwt.Rectangle rect) { this.VidgetBackendInvalidate(rect); }

        #endregion

        public void Dispose () { }

        protected override void OnRenderSizeChanged (SizeChangedInfo sizeInfo) {

            // hide overflow
            base.OnRenderSizeChanged (sizeInfo);
            var overflowGrid = this.Template.FindName ("OverflowGrid", this) as FrameworkElement;
            if (overflowGrid != null) {
                overflowGrid.Visibility = Visibility.Collapsed;
            }

            var mainPanelBorder = this.Template.FindName ("MainPanelBorder", this) as FrameworkElement;
            if (mainPanelBorder != null) {
                mainPanelBorder.Margin = new Thickness (0);
            }
        }

        protected virtual void Compose () { }

        public void AddItems (params UIElement[] items) {
            foreach (var item in items)
                Items.Add (item);

        }
    }
}
