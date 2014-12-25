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

using System.Windows.Controls;
using Limaki.Drawing.WpfBackend;
using Limaki.View.Vidgets;
using SW = System.Windows;
using SWC = System.Windows.Controls;

namespace Limaki.View.WpfBackend {

    public class ToolStripUtils {

        public static FixedBitmap WpfImage (Xwt.Drawing.Image value) {
            return new FixedBitmap { Source = value.ToWpf () as System.Windows.Media.Imaging.BitmapSource };
        }

        public static SW.Style ToolbarItemStyle (SW.FrameworkElement value) {
            if (value is SWC.ComboBox)
                return (SW.Style)value.FindResource (SWC.ToolBar.ComboBoxStyleKey);
            else if (value is SWC.Primitives.ToggleButton)
                return (SW.Style)value.FindResource (SWC.ToolBar.ToggleButtonStyleKey);
            else if (value is SWC.Separator)
                return (SW.Style) value.FindResource (SWC.ToolBar.SeparatorStyleKey);
            return value.Style;
        }
    }



}