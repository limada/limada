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

using System.Runtime.InteropServices;
using System.Windows;
using Limaki.View.UI;
using Size = Xwt.Size;

namespace Limaki.View.WpfBackends {

    public class WpfSystemInformation : IUISystemInformation {
        public Size DragSize {
            get {
                return new Size(SystemParameters.MinimumHorizontalDragDistance, SystemParameters.MinimumHorizontalDragDistance);
            }
        }

        [DllImport("user32.dll")]
        static extern uint GetDoubleClickTime ();
        public int DoubleClickTime {
            get {
                return (int) GetDoubleClickTime();
            }
        }

        public int VerticalScrollBarWidth {
            get {
                return (int)SystemParameters.VerticalScrollBarWidth;
            }
        }

        public int HorizontalScrollBarHeight {
            get {
                return (int)SystemParameters.HorizontalScrollBarHeight;
            }
        }

        public Size ScreenResolution {
            get {
                var ps = PresentationSource.FromVisual(Application.Current.MainWindow);
                var m = ps.CompositionTarget.TransformToDevice;
                var result = new Size(m.M11, m.M22);
                var screenHeight = SystemParameters.PrimaryScreenHeight * result.Height;
                var screenWidth = SystemParameters.PrimaryScreenWidth * result.Width;
                return result;
            }
        }
    }
}