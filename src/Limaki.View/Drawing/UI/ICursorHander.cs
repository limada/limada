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

namespace Limaki.Drawing.UI {
    public interface ICursorHander {
        void SetCursor ( IControl control, Anchor anchor, bool hasHit);
        void SetEdgeCursor(IControl control, Anchor anchor);
        void SaveCursor( IControl control);
        void RestoreCursor ( IControl control );
    }
}