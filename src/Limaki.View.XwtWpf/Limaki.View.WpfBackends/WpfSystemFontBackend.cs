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


using Xwt.Drawing;
using Xwt.WPFBackend;
using System.Globalization;
using SW = System.Windows;
using SystemFonts = System.Windows.SystemFonts;

namespace Limaki.Drawing.WpfBackend {

    public class WpfSystemFontBackend : Xwt.Backends.SystemFontBackend {
#if ! SILVERLIGHT
        public const double PixelToPoint = 1.5;

        private Font CreateFont (System.Windows.Media.FontFamily family, double size) {
            return Font.FromName (family.Source + " " + (size / PixelToPoint).ToString (CultureInfo.InvariantCulture));
        }

        public override Font CaptionFont { get { return CreateFont (SW.SystemFonts.CaptionFontFamily, SW.SystemFonts.CaptionFontSize); } }
                
        public override Font DefaultFont { get { return CreateFont (SW.SystemFonts.CaptionFontFamily, SW.SystemFonts.CaptionFontSize); } }
                
        public override Font DialogFont { get { return CreateFont (SW.SystemFonts.MessageFontFamily, SW.SystemFonts.MessageFontSize); } }
                
        public override Font IconTitleFont { get { return CreateFont (SW.SystemFonts.IconFontFamily, SW.SystemFonts.IconFontSize); } }
                
        public override Font MenuFont { get { return CreateFont (SW.SystemFonts.MenuFontFamily, SW.SystemFonts.MenuFontSize); } }
                
        public override Font MessageBoxFont { get { return CreateFont (SW.SystemFonts.MessageFontFamily, SW.SystemFonts.MessageFontSize); } }
                
        public override Font SmallCaptionFont { get { return CreateFont (SW.SystemFonts.SmallCaptionFontFamily, SW.SystemFonts.SmallCaptionFontSize); } }
                
        public override Font StatusFont { get { return CreateFont (SW.SystemFonts.StatusFontFamily, SW.SystemFonts.StatusFontSize); } }

#else
        public Font CaptionFont {
            get { return new Font("Tahoma", 12); }
        }

        public Font DefaultFont {
            get { return new Font("Tahoma", 12); }
        }

        public Font DialogFont {
            get { return new Font("Tahoma", 12); }
        }

        public Font IconTitleFont {
            get { return new Font("Tahoma", 12); }
        }

        public Font MenuFont {
            get { return new Font("Tahoma", 12); }
        }

        public Font MessageBoxFont {
            get { return new Font("Tahoma", 12); }
        }

        public Font SmallCaptionFont {
            get { return new Font("Tahoma", 12); }
        }

        public Font StatusFont {
            get { return new Font("Tahoma", 12); }
        }
#endif

    }
}