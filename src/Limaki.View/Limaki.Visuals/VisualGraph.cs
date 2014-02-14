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
 * 
 */


using Limaki.Graphs;

namespace Limaki.Visuals {

    public class VisualGraph:Graph<IVisual,IVisualEdge> {
        public override void DoChangeData (Limaki.Visuals.IVisual item, object data) {
            base.DoChangeData(item, data);
            EnsureChangeData (item, data);
        }

        public bool EnsureChangeData (IVisual item, object data) {
            if (item == null)
                return false;
            if (!object.Equals(item.Data, data)) {
                item.Data = data;
                this.Add(item);
                return true;
            }
            return false;
        }
    }
}