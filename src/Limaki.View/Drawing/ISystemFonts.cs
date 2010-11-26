/*
 * Limaki 
 * Version 0.08
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