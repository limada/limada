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
 */

namespace Limaki.Actions {
    public class ActionPriorities {
        const int minPriority = 1000;
        public const int DataLayerPriority = minPriority + 1000;
        public const int ToolsLayerPriority = DataLayerPriority + 1000;
        public const int SelectionPriority = ToolsLayerPriority + 1000;
        public const int DragActionPriority = SelectionPriority + 1000;
        public const int ScrollActionPriority = DragActionPriority + 1000;
        public const int ZoomActionPriority = ScrollActionPriority + 1000;
    }
}