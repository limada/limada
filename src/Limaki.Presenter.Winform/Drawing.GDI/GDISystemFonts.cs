/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

namespace Limaki.Drawing.GDI {
    public class GDISystemFonts:ISystemFonts {
        public Font CaptionFont {
            get { return new GDIFont(System.Drawing.SystemFonts.CaptionFont); }
        }

        public Font DefaultFont {
            get { return new GDIFont(System.Drawing.SystemFonts.DefaultFont); }
        }

        public Font DialogFont {
            get { return new GDIFont(System.Drawing.SystemFonts.DialogFont); }
        }

        public Font IconTitleFont {
            get { return new GDIFont(System.Drawing.SystemFonts.IconTitleFont); }
        }

        public Font MenuFont {
            get { return new GDIFont(System.Drawing.SystemFonts.MenuFont); }
        }

        public Font MessageBoxFont {
            get { return new GDIFont(System.Drawing.SystemFonts.MessageBoxFont); }
        }

        public Font SmallCaptionFont {
            get { return new GDIFont(System.Drawing.SystemFonts.SmallCaptionFont); }
        }

        public Font StatusFont {
            get { return new GDIFont(System.Drawing.SystemFonts.StatusFont); }
        }


    }
}