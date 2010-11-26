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
 */

namespace Limaki.Actions {
    public class ActionPriorities {
        private static int minPriority = 1000;
        public static int LayerPriority = minPriority + 1000;
        public static int SelectionPriority = LayerPriority + 1000;
        public static int DragActionPriority = SelectionPriority + 1000;
        public static int ScrollActionPriority = DragActionPriority + 1000;
        public static int ZoomActionPriority = ScrollActionPriority + 1000;
    }
}