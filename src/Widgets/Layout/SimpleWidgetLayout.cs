/*
 * Limaki 
 * Version 0.063
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
using System.Drawing;
using System.Collections.Generic;
using Limaki.Graphs;
using Limaki.Common;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Widgets;

namespace Limaki.Widgets.Layout {
    public class SimpleWidgetLayout<TData, TItem> : BasicWidgetLayout<TData, TItem>
        where TData : Scene
        where TItem : class,IWidget {
        
        public Orientation Orientation = Orientation.TopBottom;
        public bool centered = true;
        public Algo Algo = Algo.BreathFirst;

        public bool showDebug = false;

        public Size distance = new Size(30, 15);

        public SimpleWidgetLayout(Handler<TData> handler, IStyleSheet stylesheet):base(handler, stylesheet) { }


        IEnumerable<TItem> findRoots() {
            if (Data != null) {
                List<Pair<TItem, int>> graphroots = new List<Pair<TItem, int>>();
                SceneWalker walker = new SceneWalker(Data.Graph);
                walker.Algo = this.Algo;
                foreach (TItem item in Data.Graph) {
                    if (!walker.visited.Contains(item)) {
                        bool selected = false;
                        int count = 0;
                        Pair<TItem, int> root = new Pair<TItem, int>(item, int.MaxValue);
                        foreach (SceneWalker.LevelItem levelItem in walker.Execute(item)) {
                            //if ((levelItem.Link != null) //&& (levelItem.Link.Root == levelItem.Node)) 
                            if (!(levelItem.Node is IEdge<TItem>))
                            {
                                count = Data.Graph.EdgeCount(levelItem.Node);
                                if (root.Two > count) {
                                    root = new Pair<TItem, int>((TItem)levelItem.Node, count);
                                }
                            }
                            if (levelItem.Node == Data.Selected) {
                                selected = true;
                            }
                        }
                        if (selected) {
                            root = new Pair<TItem, int>((TItem)Data.Selected, 0);
                        }
                        graphroots.Add(root);
                    }
                }
                Comparison<Pair<TItem, int>> comparision = delegate ( Pair<TItem, int> a, Pair<TItem, int> b ) {
                                                               return -a.Two.CompareTo (b.Two);
                                                           };
                graphroots.Sort(comparision);
                foreach (Pair<TItem, int> item in graphroots) {
                    yield return item.One;
                }
            }

        }
        
        protected virtual void ComputeRows(TItem root, IList<Row> rows, ref Size maxSize) {

            SceneWalker walker = new SceneWalker(Data.Graph);
            walker.Algo = this.Algo;

            int visit = 0;
            Set<TItem> visited = new Set<TItem> ();

            Row currentRow = null;
            foreach (SceneWalker.LevelItem levelItem in walker.Execute(root)) {
                IWidget widget = levelItem.Node;
                base.Invoke((TItem)widget);
                #region debugcode
                if (showDebug) {
                    System.Console.WriteLine (levelItem.Node.Data.ToString()); }
                #endregion
                if (!(widget is ILinkWidget) && !visited.Contains((TItem)widget)) {
                    #region debugcode
                    if (showDebug) {
                        visit++;
                        string s = widget.Data.ToString ();
                        string debugmarker = " || ";
                        int i = s.IndexOf(debugmarker);
                        if (i > 0) {
                            s = s.Remove (i);
                        }
                        widget.Data = s + debugmarker +
                                      "(" + visit.ToString () + ") " +
                                      "[" + levelItem.Level + "]";
                    }

                    #endregion

                    visited.Add ((TItem) widget);
                    base.Justify((TItem)widget);

                    while (levelItem.Level >= rows.Count) {
                        currentRow = new Row();
                        rows.Add(currentRow);
                    }

                    currentRow = rows[levelItem.Level];

                    currentRow.Widgets.Add((TItem)widget);
                    int rowCount = currentRow.Widgets.Count;
                    currentRow.Size += new Size ( widget.Size.Width + distance.Width , 
                                                  widget.Size.Height + distance.Height);
                    currentRow.MaxSize = new Size ( Math.Max (currentRow.MaxSize.Width, widget.Size.Width),
                                                    Math.Max (currentRow.MaxSize.Height, widget.Size.Height));
                    maxSize = new Size( Math.Max(maxSize.Width, currentRow.Size.Width), 
                                        Math.Max(maxSize.Height, currentRow.Size.Height));
                }
            }

        }


        public override void Invoke() {
            if (this.Data != null) {
                #region debugCode
                if (showDebug) {
                    System.Console.WriteLine ("******************* Layout Invoke()"); }
                #endregion
                Point startAt = (Point)distance;
                foreach (TItem root in findRoots()) {
                    IList<Row> rows = new List<Row>();
                    Size maxSize = Size.Empty;

                    ComputeRows(root, rows, ref maxSize);

                    foreach (Row row in rows) {
                        int ident = 0;
                        if (Orientation == Orientation.TopBottom) {
                            if (centered)
                                ident = (maxSize.Width - row.Size.Width) / 2;
                            startAt.X = ident + distance.Width;
                        } else if (Orientation == Orientation.LeftRight) {
                            if (centered)
                                ident = (maxSize.Height - row.Size.Height) / 2;
                            startAt.Y = ident + distance.Height;
                        }
                        foreach (IWidget widget in row.Widgets) {
                            widget.Location = startAt;
                            if (Orientation == Orientation.TopBottom) {
                                startAt.X += widget.Size.Width + distance.Width;
                            } else if (Orientation == Orientation.LeftRight) {
                                startAt.Y += widget.Size.Height + distance.Height;
                            }
                        }
                        if (Orientation == Orientation.TopBottom) {
                            startAt.Y = startAt.Y + row.MaxSize.Height + distance.Height;
                        } else if (Orientation == Orientation.LeftRight) {
                            startAt.X = startAt.X + row.MaxSize.Width + distance.Width;
                        }
                    }
                }
                InvokeLinks();
            }
        }

        public class Row {
            public IList<TItem> Widgets = new List<TItem>();
            public Size MaxSize;
            public Size Size;

            public Row() { }
        }
    }

    public enum Orientation {
        LeftRight,
        TopBottom
        //,RightLeft,
        //BottomTop,
        //Center
    }
    public enum Order {
        Pre, Post
    }

    public enum Algo {
        DepthFirst, BreathFirst
    }

    public class Walker<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {
        Graph<TItem, TEdge> graph = null;

        public Order Order = Order.Pre;
        public Algo Algo = Algo.BreathFirst;

        public Walker(Graph<TItem, TEdge> graph) {
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
                    foreach (LevelItem widget in BreathFirst (root, default( TEdge ), level)) {
                        yield return widget;
                    }
                } else if (Algo == Algo.DepthFirst) {
                    foreach (LevelItem widget in DepthFirst (root, default( TEdge ), level)) {
                        yield return widget;
                    }
                }
            }
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

    public class SceneWalker : Walker<IWidget, ILinkWidget> {
        public SceneWalker(Graph<IWidget, ILinkWidget> graph) : base(graph) { }
    }




}

