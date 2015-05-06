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
using System;
using LVV = Limaki.View.Vidgets;
using SW = System.Windows;

namespace Limaki.View.WpfBackend {

    public class ToolStripBackend00 : ToolBar, IToolStripBackend {

        #region IVidgetBackend

        public VidgetApplicationContext ApplicationContext { get; set; }

        public ToolStrip Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            ApplicationContext = context;
            this.Frontend = (ToolStrip)frontend;
        }

        IVidget IVidgetBackend.Frontend { get { return this.Frontend; } }

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
                overflowGrid.Visibility = SW.Visibility.Collapsed;
            }

            var mainPanelBorder = this.Template.FindName ("MainPanelBorder", this) as FrameworkElement;
            if (mainPanelBorder != null) {
                mainPanelBorder.Margin = new Thickness (0);
            }
        }

        protected virtual void Compose () { }

        [Obsolete]
        public void AddItems (params UIElement[] items) {
            foreach (var item in items)
                Items.Add (item);

        }

        public void InsertItem (int index, IToolStripItemBackend item) {
            Items.Insert (index, item);
        }

        public void RemoveItem (IToolStripItemBackend item) {
            Items.Remove (item);
        }

        public void SetVisibility (LVV.Visibility value) {
            if (value == LVV.Visibility.Visible)
                this.Visibility = SW.Visibility.Visible;
            else if (value == LVV.Visibility.Hidden)
                this.Visibility = SW.Visibility.Hidden;
            else if (value == LVV.Visibility.Collapsed)
                this.Visibility = SW.Visibility.Collapsed;
        }
    }
}
