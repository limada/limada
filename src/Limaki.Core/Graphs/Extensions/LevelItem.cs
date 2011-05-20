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


namespace Limaki.Graphs.Extensions {
    public class LevelItem<TItem> {
        public LevelItem() {}

        public LevelItem(TItem node, TItem path, int level) {
            this.Node = node;
            this.Path = path;
            this.Level = level;
        }
        public TItem Node;
        public TItem Path;
        public int Level;
    }
}