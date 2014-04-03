/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Windows;
using Xwt;
using Xwt.Drawing;
using Point = System.Windows.Point;
using Size = System.Windows.Size;
using SWM = System.Windows.Media;
using SWMI = System.Windows.Media.Imaging;

namespace Limaki.Drawing.WpfBackend {
    public static class DrawingConverter {

        public static Point[] ToWpf (this Xwt.Point[] value) {
            var result = new Point[value.Length];
            for (int i = 0; i < value.Length; i++) {
                result[i] = new Point (value[i].X, value[i].Y);
            }
            return result;
        }

        public static Rect ToWpf (this Rectangle value) {
            return new Rect (value.X, value.Y, value.Width, value.Height);
        }

        public static Rectangle ToXwt (this Rect value) {
            return new Rectangle((int)value.X, (int)value.Y, (int)value.Width, (int)value.Height);
        }
        public static Point ToWpf (this Xwt.Point value) {
            return new Point (value.X, value.Y);
        }
        public static Xwt.Point ToXwt (this Point value) {
            return new Xwt.Point(value.X, value.Y);
        }

        public static Color ToXwt(this System.Windows.Media.Color value) {
            return Xwt.WPFBackend.DataConverter.ToXwtColor(value);
        }
        public static System.Windows.Media.Color ToWpf (this Color value) {
            return Xwt.WPFBackend.DataConverter.ToWpfColor(value);
        }

        public static Size ToWpf (this Xwt.Size value) {
            return new Size (value.Width, value.Height);
        }
        public static Xwt.Size ToXwt (this Size value) {
            return new Xwt.Size (value.Width, value.Height);
        }

        public static SWM.ImageSource ToWpf (this  Xwt.Drawing.Image value) {
            if (value == null)
                return null;
            return Toolkit.CurrentEngine.GetNativeImage (value) as SWM.ImageSource;
        }
    }
}
