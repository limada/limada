/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2010 Lytico
 *
 * http://www.limada.org
 * 
 */

namespace Limaki.Viewers.ToolStripViewers {
    public interface IDisplayTool {
        void Attach ( bool select, bool move, bool connect, bool add );
        void Detach();
    }


}