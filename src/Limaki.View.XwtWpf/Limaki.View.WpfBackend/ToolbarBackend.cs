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
using Limaki.View.Vidgets;
using System.Windows;
using System;
using LVV = Limaki.View.Vidgets;
using SW = System.Windows;
using SWC = System.Windows.Controls;

namespace Limaki.View.WpfBackend {

    public class ToolbarBackend : VidgetBackend<ToolbarBackend.ToolBar>, IToolbarBackend {

        public class ToolBar : SWC.ToolBar {

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
        }

        public override void Compose () {
            base.Compose ();
            Control.Focusable = true;
            Control.MouseDown += (s, e) => e.Handled = true;
        }

        public VidgetApplicationContext ApplicationContext { get; set; }

        public new Toolbar Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            ApplicationContext = context;
            this.Frontend = (Toolbar)frontend;
        }

        public void InsertItem (int index, IToolbarItemBackend item) {

            var value = item.ToWpf ();
            if (value != null) {
                value.Style = ToolbarUtils.ToolbarItemStyle (value);
                Control.Items.Insert (index, value);
            }
        }

        public void RemoveItem (IToolbarItemBackend item) {
            Control.Items.Remove (item.ToWpf ());
        }

        public void SetVisibility (LVV.Visibility value) {
            if (value == LVV.Visibility.Visible)
                Control.Visibility = SW.Visibility.Visible;
            else if (value == LVV.Visibility.Hidden)
                Control.Visibility = SW.Visibility.Hidden;
            else if (value == LVV.Visibility.Collapsed)
                Control.Visibility = SW.Visibility.Collapsed;
        }
    }
}
