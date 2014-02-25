/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */

using Xwt.Backends;
using Xwt.Drawing;
using Xwt.Gdi.Backend;

namespace Limaki.Drawing.Gdi {

    public class GdiSystemFontBackend : SystemFontBackend {
        public override Font CaptionFont { get { return System.Drawing.SystemFonts.CaptionFont.ToXwt(); } }
                
        public override Font DefaultFont { get { return System.Drawing.SystemFonts.DefaultFont.ToXwt(); } }
                
        public override Font DialogFont { get { return System.Drawing.SystemFonts.DialogFont.ToXwt(); } }
                
        public override Font IconTitleFont { get { return System.Drawing.SystemFonts.IconTitleFont.ToXwt(); } }
                
        public override Font MenuFont { get { return System.Drawing.SystemFonts.MenuFont.ToXwt(); } }
                
        public override Font MessageBoxFont { get { return System.Drawing.SystemFonts.MessageBoxFont.ToXwt(); } }
                
        public override Font SmallCaptionFont { get { return System.Drawing.SystemFonts.SmallCaptionFont.ToXwt(); } }
                
        public override Font StatusFont { get { return System.Drawing.SystemFonts.StatusFont.ToXwt(); } }


    }
}