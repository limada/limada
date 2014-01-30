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

using Limaki.Drawing;
using Xwt.Drawing;

namespace Limaki.View.XwtBackend {

    public class XwtSystemFonts : ISystemFonts {
        public Font CreateFont(string family, double size) {
            return Xwt.Drawing.Font.FromName(family).WithSize(size);
        }
        public Font CaptionFont { get { return Font.SystemFont; } }

        public Font DefaultFont { get { return Font.SystemFont; } }

        public Font DialogFont { get { return Font.SystemFont; } }

        public Font IconTitleFont { get { return Font.SystemFont; } }

        public Font MenuFont { get { return Font.SystemFont; } }

        public Font MessageBoxFont { get { return Font.SystemFont; } }

        public Font SmallCaptionFont { get { return Font.SystemFont; } }

        public Font StatusFont { get { return Font.SystemFont; } }

    }

}
