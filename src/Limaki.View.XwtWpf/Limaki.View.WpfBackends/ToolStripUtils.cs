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

namespace Limaki.View.WpfBackend {

    public class ToolStripUtils {

        public static void SetCommand (IToolStripCommandItem item, ref ToolStripCommand _command, ToolStripCommand value) {
            if (_command != value) {
                try {
                    if (_command != null)
                        _command.DeAttach (item);
                    _command = value;
                    _command.Attach (item);
                } finally {
                }
            }
        }

        public static void SetSize (IToolStripCommandItem item, Xwt.Size value) {
            var button = item as Button;
            if (button != null) {
                button.RenderSize = value.ToWpf ();
            }
        }

        public static FixedBitmap WpfImage (Xwt.Drawing.Image value) {
            return new FixedBitmap { Source = value.ToWpf () as System.Windows.Media.Imaging.BitmapSource };
        }
    }



}