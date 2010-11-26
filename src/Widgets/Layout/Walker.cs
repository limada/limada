using System.Collections.Generic;
using Limaki.Common.Collections;
using Limaki.Graphs;

namespace Limaki.Widgets.Layout {
    public class Walker<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {
        IGraph<TItem, TEdge> graph = null;

        public Order Order = Order.Pre;
        public Algo Algo = Algo.BreathFirst;

        public Walker(IGraph<TItem, TEdge> graph) {
            this.graph = graph;
        }

        public static TItem Adjacent(TEdge edge, TItem item) {
            if (edge != null) {
                if (edge.Root.Equals(item))
                    return edge.Leaf;
                else if (edge.Leaf.Equals(item))
                    return edge.Root;
                else
                    return default( TItem );
            } else {
                return default(TItem);

            }
        }

        public class LevelItem {
            public LevelItem() { }
            public LevelItem(TItem one, TItem two, int level) {
                this.Node = one;
                this.Path = two;
                this.Level = level;
            }
            public TItem Node;
            public TItem Path;
            public int Level;
        }

        public virtual IEnumerable<LevelItem> Execute(TItem root) {
            int level = 0;
            if (!visited.Contains(root)) {
                if (Algo == Algo.BreathFirst) {
                    return BreathFirst (root, default( TEdge ), level);
                    
                } else if (Algo == Algo.DepthFirst) {
                    return DepthFirst (root, default( TEdge ), level) ;
                }
            }
            return null;
        }

        public Set<TItem> visited = new Set<TItem>();
        protected virtual IEnumerable<LevelItem> DepthFirst(TItem root, TEdge path, int level) {
            if (!visited.Contains(root)) {
                visited.Add(root);
                if (Order == Order.Pre) {
                    yield return new LevelItem (root, path, level);
                }
                foreach (TEdge edge in graph.Edges(root)) {
                    TItem adjacent = Adjacent(edge, root);
                    
                    // follow links on links:
                    foreach (LevelItem recurse in DepthFirst(edge, default(TEdge), level)) {
                        yield return recurse;
                    }
                    
                    // follow leaf and edge of adjacent:
                    if (adjacent is TEdge) {
                        TEdge adjEdge = (TEdge)adjacent;
                        foreach (LevelItem recurse in DepthFirst(adjEdge.Leaf, adjEdge, level)) {
                            yield return recurse;
                        }
                        foreach (LevelItem recurse in DepthFirst(adjEdge.Root, adjEdge, level)) {
                            yield return recurse;
                        }
                    }

                    // follow adjacent:
                    foreach (LevelItem recurse in DepthFirst(adjacent, edge, level + 1)) {
                        yield return recurse;
                    }
                }


                if (Order == Order.Post) {
                    yield return new LevelItem (root, path, level);
                }
            }
        }
        protected virtual IEnumerable<LevelItem> BreathFirst(TItem root, TEdge path, int level) {
            if (!visited.Contains(root)) {
                visited.Add(root);
                Queue<LevelItem> queue = new Queue<LevelItem>();
                queue.Enqueue(new LevelItem(root, path, level));
                //if (root is TEdge) {
                //    level--; }
                
                while (queue.Count > 0) {
                    LevelItem item = queue.Dequeue();
                    yield return item;
                    level = item.Level+1;
                    if ( item.Node is TEdge ) {
                        TEdge edge = (TEdge) item.Node;
                        foreach ( TEdge edge_edge in graph.Edges(edge) ) {
                            // follow link:
                            if (!visited.Contains(edge_edge)) {
                                queue.Enqueue(new LevelItem(edge_edge, edge, level));
                                visited.Add(edge_edge);
                            }
                        }
                        TItem adjacent = Adjacent(edge, item.Path);
                        if ( adjacent != null ) {
                            // follow adjacent of node:
                            if ( !visited.Contains(adjacent) ) {
                                queue.Enqueue(new LevelItem(adjacent, edge, level));
                                visited.Add(adjacent);
                            }
                        } else {
                            if ( !visited.Contains(edge.Root) ) {
                                queue.Enqueue(new LevelItem(edge.Root, edge, level));
                                visited.Add(edge.Root);
                            }
                            if ( !visited.Contains(edge.Leaf) ) {
                                queue.Enqueue(new LevelItem(edge.Leaf, edge, level));
                                visited.Add(edge.Leaf);
                            }
                        }
                    } else {
                        foreach (TEdge edge in graph.Edges(item.Node)) {
                            // follow link:
                            if (!visited.Contains(edge)) {
                                queue.Enqueue(new LevelItem(edge, item.Node, level));
                                visited.Add(edge);
                            }

                            //TItem adjacent = Adjacent(link, item.Node);
                            //// follow adjacent of node:
                            //if (!visited.Contains(adjacent)) {
                            //    queue.Enqueue(new LevelItem(adjacent, link, level));
                            //    visited.Add(adjacent);
                            //}
                        }
                    }
                }
            }
        }
    }
}