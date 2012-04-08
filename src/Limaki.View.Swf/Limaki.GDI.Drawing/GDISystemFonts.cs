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
 * http://limada.sourceforge.net
 * 
 */

using Xwt.Drawing;
using Xwt.Engine;

namespace Limaki.Drawing.Gdi {

    public class GdiSystemFonts:ISystemFonts {

        public Font CaptionFont {
            get { return WidgetRegistry.CreateFrontend<Font>(System.Drawing.SystemFonts.CaptionFont); }
        }

        public Font DefaultFont {
            get { return WidgetRegistry.CreateFrontend<Font>(System.Drawing.SystemFonts.DefaultFont); }
        }

        public Font DialogFont {
            get { return WidgetRegistry.CreateFrontend<Font>(System.Drawing.SystemFonts.DialogFont); }
        }

        public Font IconTitleFont {
            get { return WidgetRegistry.CreateFrontend<Font>(System.Drawing.SystemFonts.IconTitleFont); }
        }

        public Font MenuFont {
            get { return WidgetRegistry.CreateFrontend<Font>(System.Drawing.SystemFonts.MenuFont); }
        }

        public Font MessageBoxFont {
            get { return WidgetRegistry.CreateFrontend<Font>(System.Drawing.SystemFonts.MessageBoxFont); }
        }

        public Font SmallCaptionFont {
            get { return WidgetRegistry.CreateFrontend<Font>(System.Drawing.SystemFonts.SmallCaptionFont); }
        }

        public Font StatusFont {
            get { return WidgetRegistry.CreateFrontend<Font>(System.Drawing.SystemFonts.StatusFont); }
        }


    }
}