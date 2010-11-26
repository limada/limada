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

        public static TItem Adjacent(TEdge link, TItem item) {
            if (link != null) {
                if (link.Root.Equals(item))
                    return link.Leaf;
                else if (link.Leaf.Equals(item))
                    return link.Root;
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
                foreach (TEdge link in graph.Edges(root)) {
                    TItem adjacent = Adjacent(link, root);
                    
                    // follow links on links:
                    foreach (LevelItem recurse in DepthFirst(link, default(TEdge), level)) {
                        yield return recurse;
                    }
                    
                    // follow leaf and edge of adjacent:
                    if (adjacent is TEdge) {
                        TEdge edge = (TEdge)adjacent;
                        foreach (LevelItem recurse in DepthFirst(edge.Leaf, edge, level)) {
                            yield return recurse;
                        }
                        foreach (LevelItem recurse in DepthFirst(edge.Root, edge, level)) {
                            yield return recurse;
                        }
                    }

                    // follow adjacent:
                    foreach (LevelItem recurse in DepthFirst(adjacent, link, level + 1)) {
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
                        TEdge link = (TEdge) item.Node;
                        foreach ( TEdge linklink in graph.Edges(link) ) {
                            // follow link:
                            if (!visited.Contains(linklink)) {
                                queue.Enqueue(new LevelItem(linklink, link, level));
                                visited.Add(linklink);
                            }
                        }
                        TItem adjacent = Adjacent(link, item.Path);
                        if ( adjacent != null ) {
                            // follow adjacent of node:
                            if ( !visited.Contains(adjacent) ) {
                                queue.Enqueue(new LevelItem(adjacent, link, level));
                                visited.Add(adjacent);
                            }
                        } else {
                            if ( !visited.Contains(link.Root) ) {
                                queue.Enqueue(new LevelItem(link.Root, link, level));
                                visited.Add(link.Root);
                            }
                            if ( !visited.Contains(link.Leaf) ) {
                                queue.Enqueue(new LevelItem(link.Leaf, link, level));
                                visited.Add(link.Leaf);
                            }
                        }
                    } else {
                        foreach (TEdge link in graph.Edges(item.Node)) {
                            // follow link:
                            if (!visited.Contains(link)) {
                                queue.Enqueue(new LevelItem(link, item.Node, level));
                                visited.Add(link);
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