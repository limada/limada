/*
 * Limada
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

using System;
using System.Collections.Generic;
using System.Text;
using Limaki.Graphs.Extensions;
using Limada.Schemata;

namespace Limada.Model {
    public class ThingGraphUtils {
        public static void DeepCopy(IThingGraph source, IEnumerable<IThing> items, IThingGraph target) {
            if (source != null && target != null) {
                Walker<IThing, ILink> walker = new Walker<IThing, ILink>(source);
                Queue<IThing> queue = new Queue<IThing>(items);
                while (queue.Count != 0) {
                    IThing item = queue.Dequeue();
                    if (!walker.visited.Contains(item)) {

                        if (item is ILink)
                            target.Add((ILink)item);
                        else
                            target.Add(item);

                        foreach (LevelItem<IThing> levelItem in walker.DeepWalk(item, 0)) {

                            if (levelItem.Node is ILink) {
                                target.Add((ILink)levelItem.Node);

                                queue.Enqueue(((ILink)levelItem.Node).Marker);

                            } else {
                                target.Add(levelItem.Node);
                            }
                        }
                    }
                }
            }
        }

        public static object GetDescription(IThingGraph source, IThing thing) {
            object name = null;
            var thingGraph = source as SchemaThingGraph;

            if (thingGraph != null) {
                IThing desc = thingGraph.ThingToDisplay(thing);
                if (desc != null && desc != thing) {
                    name = desc.Data;
                }
            } else {
                throw new ArgumentException("source must be a SchemaThingGraph");
            }
            return name;
        }
    }
}
