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

namespace Limaki.Drawing {
    public interface ISystemFonts {
        Font CaptionFont { get; }
        Font DefaultFont {get; }
        Font DialogFont {get; }
        Font IconTitleFont {get; }
        Font MenuFont {get; }
        Font MessageBoxFont {get; }
        Font SmallCaptionFont {get; }
        Font StatusFont { get; }
    }
}