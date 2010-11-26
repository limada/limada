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


using System.Collections.Generic;

namespace Limaki.Graphs.Extensions {
    public class Walker<TItem, TEdge> : WalkerBase<TItem,TEdge> 
        where TEdge : IEdge<TItem>, TItem {
        public Walker(IGraph<TItem, TEdge> graph) : base (graph) {}


        /// <summary>
        /// Maybe a Trail???
        /// Iterates over all Nodes and Vertices which are connected to each other
        /// starting with the start-item
        /// </summary>
        /// <param name="start"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public virtual IEnumerable<LevelItem<TItem>> DeepWalk(TItem start, int level) {
            if (!visited.Contains(start)) {
                visited.Add(start);
                Queue<LevelItem<TItem>> queue = new Queue<LevelItem<TItem>>();
                queue.Enqueue(new LevelItem<TItem>(start, default(TItem), level));
                while (queue.Count > 0) {
                    LevelItem<TItem> item = queue.Dequeue();
                    yield return item;

                    level = item.Level + 1;

                    if (item.Node is TEdge) {
                        TEdge edge = (TEdge)item.Node;

                        foreach (TEdge edge_edge in graph.Edges(edge)) {
                            // follow link:
                            if (!visited.Contains(edge_edge)) {
                                queue.Enqueue(new LevelItem<TItem>(edge_edge, edge, level));
                                visited.Add(edge_edge);
                            }
                        }

                        TItem adjacent = graph.Adjacent(edge, item.Path);

                        if (adjacent != null) {
                            // follow adjacent of node:
                            if (!visited.Contains(adjacent)) {
                                queue.Enqueue(new LevelItem<TItem>(adjacent, edge, level));
                                visited.Add(adjacent);
                            }
                        } else {
                            if (!visited.Contains(edge.Root)) {
                                queue.Enqueue(new LevelItem<TItem>(edge.Root, edge, level));
                                visited.Add(edge.Root);
                            }
                            if (!visited.Contains(edge.Leaf)) {
                                queue.Enqueue(new LevelItem<TItem>(edge.Leaf, edge, level));
                                visited.Add(edge.Leaf);
                            }
                        }
                    } else {
                        foreach (TEdge edge in graph.Edges(item.Node)) {
                            // follow link:
                            if (!visited.Contains(edge)) {
                                queue.Enqueue(new LevelItem<TItem>(edge, item.Node, level));
                                visited.Add(edge);
                            }
                        }
                    }
                }
            }

        }
        /// <summary>
        /// maybe same as deepwalk???
        /// iterates over all edges and nodes connected with start
        /// iterates over all edges of edges recursivly
        /// if edge.Root or edge.Leaf is an edge, it is iterated too
        /// </summary>
        /// <param name="start"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public virtual IEnumerable<LevelItem<TItem>> Walk(TItem start, int level) {
            if (!visited.Contains(start)) {
                visited.Add(start);
                Queue<LevelItem<TItem>> queue = new Queue<LevelItem<TItem>>();
                queue.Enqueue(new LevelItem<TItem>(start, default(TItem), level));
                while (queue.Count > 0) {
                    LevelItem<TItem> item = queue.Dequeue();
                    yield return item;

                    level = item.Level+1;
                    if (item.Node is TEdge) {

                        TEdge edge = (TEdge)item.Node;

                        // follow link of links
                        foreach (TEdge edge_edge in graph.Edges(edge)) { // Fork!?
                            if (!visited.Contains(edge_edge)) {
                                queue.Enqueue(new LevelItem<TItem>(edge_edge, edge, level ));
                                visited.Add(edge_edge);
                            }
                        }
                        TItem adjacent = graph.Adjacent(edge, item.Path);
                        if (adjacent != null) {
                            // follow adjacent of node:
                            if (!visited.Contains(adjacent)) {
                                LevelItem<TItem> result = new LevelItem<TItem>(adjacent, edge, level);
                                if (adjacent is TEdge)
                                    queue.Enqueue(result);
                                else
                                    yield return result;
                                visited.Add(adjacent);
                            }
                        } else {
                            if (!visited.Contains(edge.Root)) {
                                LevelItem<TItem> result = new LevelItem<TItem>(edge.Root, edge, level);
                                if (edge.Root is TEdge)
                                    queue.Enqueue(result);
                                else
                                    yield return result;
                                visited.Add(edge.Root);
                            }
                            if (!visited.Contains(edge.Leaf)) {
                                LevelItem<TItem> result = new LevelItem<TItem>(edge.Leaf, edge, level);
                                if (edge.Leaf is TEdge)
                                    queue.Enqueue(result);
                                else
                                    yield return result;
                                visited.Add(edge.Leaf);
                            }
                        }
                    } else {
                        foreach (TEdge edge in graph.Edges(item.Node)) {
                            // follow link:
                            if (!visited.Contains(edge)) {
                                queue.Enqueue(new LevelItem<TItem>(edge, item.Node, level ));
                                visited.Add(edge);
                            }
                        }
                    }
                }
            }


        }

        public virtual IEnumerable<LevelItem<TItem>> ExpandWalk(TItem start, int level) {
            if (!visited.Contains(start)) {
                visited.Add(start);
                Queue<LevelItem<TItem>> queue = new Queue<LevelItem<TItem>>();
                queue.Enqueue(new LevelItem<TItem>(start, default(TItem), level));
                while (queue.Count > 0) {
                    LevelItem<TItem> item = queue.Dequeue();
                    yield return item;
                    level = item.Level;
                    if (item.Node is TEdge) {

                        TEdge edge = (TEdge)item.Node;

                        TItem adjacent = graph.Adjacent(edge, start);
                        if (adjacent != null || (edge.Equals(start)) || (graph.RootIsEdge(edge) && graph.LeafIsEdge(edge))) {
                            // follow link of links
                            foreach (TEdge edge_edge in graph.Edges(edge)) {
                                // Fork!?
                                if (!visited.Contains(edge_edge)) {
                                    queue.Enqueue(new LevelItem<TItem>(edge_edge, edge, level + 1));
                                    visited.Add(edge_edge);
                                }
                            }
                        }

                        if (adjacent != null) {
                            // follow adjacent of node:
                            if (!visited.Contains(adjacent)) {
                                LevelItem<TItem> result = new LevelItem<TItem>(adjacent, edge, level);
                                queue.Enqueue(result);
                                visited.Add(adjacent);
                            }
                        } else {
                            if (!visited.Contains(edge.Root)) {
                                LevelItem<TItem> result = new LevelItem<TItem>(edge.Root, edge, level);
                                queue.Enqueue(result);
                                visited.Add(edge.Root);
                            }
                            if (!visited.Contains(edge.Leaf)) {
                                LevelItem<TItem> result = new LevelItem<TItem>(edge.Leaf, edge, level);
                                queue.Enqueue(result);
                                visited.Add(edge.Leaf);
                            }
                        }
                    } else if (item.Node.Equals(start)) {
                        foreach (TEdge edge in graph.Edges(item.Node)) {
                            // follow link:
                            if (!visited.Contains(edge)) {
                                queue.Enqueue(new LevelItem<TItem>(edge, item.Node, level + 1));
                                visited.Add(edge);
                            }
                        }
                    }
                }
            }


        }

        
        public virtual IEnumerable<LevelItem<TItem>> CollapseWalk(TItem start, int level) {
            Queue<TItem> queue = new Queue<TItem>();
            foreach (TEdge edge in graph.Edges(start)) {
                foreach (TEdge subedge in graph.Edges(edge)) {
                    if (!visited.Contains(subedge)) {
                        if (subedge.Leaf.Equals(edge)) {                         
                            continue;
                        }
                        queue.Enqueue(subedge);
                        visited.Add(subedge);
                        if (!visited.Contains(subedge.Leaf)) {
                            queue.Enqueue(subedge.Leaf);
                            visited.Add(subedge.Leaf);
                        }
                    }
                }

                if (!visited.Contains(edge)) {
                    if (edge.Leaf.Equals(start)) {
                        continue;
                    }
                    queue.Enqueue(edge);
                    visited.Add(edge);
                    if (!visited.Contains(edge.Leaf)) {
                        queue.Enqueue(edge.Leaf);
                        visited.Add(edge.Leaf);
                    }                    
                }
            }

            while (queue.Count > 0) {
                TItem item = queue.Dequeue ();
                yield return new LevelItem<TItem>(item, default(TItem), 0);
                foreach (TEdge edge in graph.Twig(item)) {
                    if (!visited.Contains(edge)) {
                        visited.Add (edge);
                        yield return new LevelItem<TItem>(edge, default(TItem), 0);
                    }
                }
            }
        }

        /// <summary>
        /// under construction; should be a more clear algo than CollapseWalk
        /// </summary>
        /// <param name="start"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public virtual IEnumerable<LevelItem<TItem>> CollapseWalk1(TItem start, int level) {
            Queue<TItem> queue = new Queue<TItem>();
            queue.Enqueue(start);

            while (queue.Count > 0) {
                TItem item = queue.Dequeue();
                yield return new LevelItem<TItem>(item, default(TItem), 0);
                foreach (TEdge edge in graph.Twig(item)) {
                    if (!visited.Contains(edge)) {

                        queue.Enqueue(edge);
                        visited.Add(edge);
                        
                        if (edge.Leaf.Equals(item)) {
                            continue;
                        }

                        if (graph.Adjacent(edge,item) !=null) {
                            //if (!done.Contains(edge.Root)) {
                            //    queue.Enqueue(edge.Root);
                            //    done.Add(edge.Root);
                            //} 
                            if (!visited.Contains(edge.Leaf)) {
                                queue.Enqueue(edge.Leaf);
                                visited.Add(edge.Leaf);
                            }
                        }
                    }
                }
            }
        }

        public virtual IEnumerable<TItem> Items(IEnumerable<LevelItem<TItem>> items) {
            foreach (LevelItem<TItem> item in items)
                yield return item.Node;
            
        }
        public virtual IEnumerable<TEdge> Edges(IEnumerable<LevelItem<TItem>> items) {
            foreach (LevelItem<TItem> item in items)
                if (item.Node is TEdge)
                    yield return (TEdge)item.Node;

        }
        }
}