/*
 * Limada
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

using System;
using System.Collections.Generic;
using System.Text;
using Limaki.Graphs.Extensions;
using Limada.Schemata;
using Limaki.Common.Collections;
using System.Linq;
using Limaki.Graphs;
using Id = System.Int64;
using Limaki.Common;

namespace Limada.Model {

    public static class ThingGraphExtensions {

        public static void DeepCopy(this IThingGraph source, IEnumerable<IThing> items, IThingGraph target) {

            if (source != null && target != null) {
                Walker<IThing, ILink> walker = new Walker<IThing, ILink>(source);
                Queue<IThing> queue = new Queue<IThing>(items);
                while (queue.Count != 0) {
                    IThing item = queue.Dequeue();
                    if (!walker.Visited.Contains(item)) {
                        if (item is ILink)
                            target.Add((ILink)item);
                        else
                            target.Add(item);

                        foreach (LevelItem<IThing> levelItem in walker.DeepWalk(item, 0)) {
                            ILink link = levelItem.Node as ILink;
                            if (link != null) {
                                target.Add(link);
                                queue.Enqueue(link.Marker);

                            } else {
                                target.Add(levelItem.Node);
                            }
                        }
                    }
                }
            }
        }

        public static void AddRange(this IThingGraph thingGraph, IEnumerable<IThing> things) {

            var links = new Set<ILink>();
            foreach (var thing in things) {
                if (thing is ILink) {
                    links.Add((ILink)thing);
                } else {
                    thingGraph.Add(thing);
                }
            }
            int linkDepth = 100;
            int depth = 0;
            while (links.Count > 0 && depth < linkDepth) {
                depth++;
                foreach (var link in links.ToArray()) {
                    var idLink = (ILink<Id>)link;
                    if (link.Leaf == null) {
                        var thing = thingGraph.GetById(idLink.Leaf);
                        if (thing != null) {
                            link.Leaf = thing;
                        }
                    }
                    if (link.Root == null) {
                        var thing = thingGraph.GetById(idLink.Root);
                        if (thing != null) {
                            link.Root = thing;
                        }
                    }
                    if (link.Marker == null) {
                        var thing = thingGraph.GetById(idLink.Marker);
                        if (thing != null) {
                            link.Marker = thing;
                        }
                    }
                    if (link.Root != null && link.Leaf != null && link.Marker != null) {
                        thingGraph.Add(link);
                        links.Remove(link);
                    }
                }
            }
        }


        public static object Description(this IThingGraph source, IThing thing) {
            object name = null;
            var thingGraph = source as SchemaThingGraph;

            if (thingGraph != null) {
                var desc = thingGraph.ThingToDisplay(thing);
                if (desc != null && desc != thing) {
                    name = desc.Data;
                }
            } else {
                throw new ArgumentException("source must be a SchemaThingGraph");
            }
            return name;
        }

        public static void SetDescription (this IThingGraph source, IThing thing, object name) {
            var thingGraph = source as SchemaThingGraph;

            if (thingGraph != null) {
                var desc = thingGraph.ThingToDisplay(thing);
                if (desc != null) {
                    if (desc != thing) {
                        source.DoChangeData (desc, name);
                    }
                } else {
                    var factory = Registry.Pool.TryGetCreate<IThingFactory>();
                    new Schema(source, thing).SetTheLeaf(CommonSchema.DescriptionMarker,factory.CreateItem(source,name));
                }
            } else {
                throw new ArgumentException("source must be a SchemaThingGraph");
            }

        }

        public static object Source(this IThingGraph source, IThing thing) {
            object name = null;
            if (source != null) {
                var desc = new Schema(source, thing).GetTheLeaf(CommonSchema.SourceMarker);
                if (desc != null && desc != thing) {
                    name = desc.Data;
                }
            }
            return name;
        }

        public static void SetSource (this IThingGraph source, IThing thing, object name) {
            if (source != null) {
                var schema = new CommonSchema(source, thing);
                var desc = schema.GetTheLeaf(CommonSchema.SourceMarker);
                if (desc != null) {
                    if (desc != thing) {
                        source.DoChangeData (desc, name);
                    }
                } else {
                    var factory = Registry.Pool.TryGetCreate<IThingFactory>();
                    schema.SetTheLeaf(CommonSchema.SourceMarker, factory.CreateItem(source, name));
                }
            }
        }

        /// <summary>
        /// gets a complete list of things 
        /// as: link.leaf, link.marker, link.root
        /// if graph is SchemaGraph:
        /// thing.description, thing.pathtodescrition
        /// </summary>
        /// <param name="source"></param>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static IEnumerable<IThing> CompletedThings(this IEnumerable<IThing> things, IGraph<IThing,ILink> graph) {
            if (things == null)
                yield break;

            var sourceGraph = graph;
            if (graph is SchemaThingGraph)
                sourceGraph = ( (SchemaThingGraph) graph ).Source;

            var stack = new Queue<IThing> ();
            var done = new Set<IThing> ();
            Action<IThing> addThing = null;
            Action<ILink> addLink = (link) => {
                if (!done.Contains(link)) {
                    var walker = new Walker<IThing, ILink>(sourceGraph);
                    if (!done.Contains(link.Marker)) {
                        foreach (var thing in walker.Walk (link.Marker, 0))
                            addThing (thing.Node);
                    }
                    addThing(link.Root);
                    addThing(link.Leaf);

                    stack.Enqueue(link);
                    done.Add(link);
                }
            };

            addThing = (thing) => {
                if (!done.Contains(thing)) {
                    if (thing is ILink) {
                        foreach (var vein in graph.Vein((ILink)thing)) {
                            addLink(vein);
                        }
                    } else {
                        stack.Enqueue(thing);
                        done.Add(thing);
                    }
                }
            };

            foreach (var thing in things) {
                addThing (thing);

                var schemaGraph = graph as SchemaThingGraph;
                if (schemaGraph != null) {
                    addThing (schemaGraph.ThingToDisplay (thing));
                    foreach (var link in schemaGraph.ThingToDisplayPath (thing)) {
                        addThing (link);
                    }
                }

                Registry.Pool.TryGetCreate<GraphDepencencies<IThing, ILink>> ().VisitItems (
                    GraphCursor.Create (graph, thing), d => addThing (d), GraphEventType.Add);
            }

            foreach (var item in stack)
                yield return item;
        }


        /// <summary>
        /// Search for name
        /// if something is found,
        /// get the described thing for it
        /// </summary>
        /// <param name="thingGraph"></param>
        /// <param name="name"></param>
        /// <param name="exact"></param>
        /// <returns></returns>
        public static IEnumerable<IThing> Search(this IThingGraph thingGraph, object name, bool exact) {
            var schemaGraph = thingGraph as SchemaThingGraph;
            bool isSchemaGraph = schemaGraph != null;

            var schema = new CommonSchema();

            foreach (var thing in thingGraph.GetByData(name, exact)) {
                IThing described = null;
                if (isSchemaGraph) {
                    described = schemaGraph.DescribedThing(thing);
                } else {
                    described = schema.GetTheRoot(thingGraph, thing, CommonSchema.DescriptionMarker);
                }
                if (described != null) {
                    yield return described;
                } else {
                    yield return thing;
                }
            }
        }

        public static bool EnsureChangeData (this IThingGraph thingGraph, IThing item, object data) {
            if (item == null)
                return false;
            var result = false;

            var link = item as ILink;
            if (link != null) {
                var marker = data as IThing;
                if (marker != null && link.Marker != marker) {
                    link.Marker = marker;
                    result = true;
                }
                return false;
            } else if (!object.Equals (item.Data, data)) {
                item.Data = data;
                result = true;
            }
            if (result) {
                thingGraph.Add (item);
            }
            return result;
        }

        public static IEnumerable<IThing> MergeThing (this IThingGraph thingGraph, IThing source, IThing sink) {

            foreach (var link in thingGraph.Edges (source)) {
                if (link.Root == source)
                    link.Root = sink;
                if (link.Leaf == source)
                    link.Leaf = sink;
                if (link.Marker == source)
                    link.Marker = sink;
                thingGraph.Add (link);
                yield return link;
            }

            foreach (var link in thingGraph.Where<ILink> (l => l.Marker!=null && l.Marker.Id == source.Id)) {
                link.Marker = sink;
                thingGraph.Add (link);
                yield return link;
            }

           
        }


    }
}
