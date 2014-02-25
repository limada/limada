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

using Xwt.Backends;
using Xwt.Drawing;

namespace Limaki.View.XwtBackend {

    public class XwtSystemFontBackend : SystemFontBackend {
        public Font CreateFont(string family, double size) {
            return Xwt.Drawing.Font.FromName(family).WithSize(size);
        }
        public override Font CaptionFont { get { return Font.SystemFont; } }
                
        public override Font DefaultFont { get { return Font.SystemFont; } }
                
        public override Font DialogFont { get { return Font.SystemFont; } }
                
        public override Font IconTitleFont { get { return Font.SystemFont; } }
                
        public override Font MenuFont { get { return Font.SystemFont; } }
                
        public override Font MessageBoxFont { get { return Font.SystemFont; } }
                
        public override Font SmallCaptionFont { get { return Font.SystemFont; } }
                
        public override Font StatusFont { get { return Font.SystemFont; } }

    }

}
