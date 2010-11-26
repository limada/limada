/*
 * Limaki 
 * Version 0.064
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
    public class GraphLayout<TData, TItem> : WidgetLayout<TData, TItem>
        where TData : Scene
        where TItem : class,IWidget {
        
        protected Orientation _orientation = Orientation.TopBottom;
        public Orientation Orientation {
            get { return _orientation; }
            set { _orientation = value; }
        }
        protected bool _centered = true;
        public bool Centered {
            get { return _centered; }
            set { _centered = value; }
        }

        protected Algo _algo = Algo.BreathFirst;
        public Algo Algo {
            get { return _algo; }
            set { _algo = value; }
        }

        public bool showDebug = false;

        public Size distance = new Size(30, 15);

        public GraphLayout(Handler<TData> handler, IStyleSheet stylesheet):base(handler, stylesheet) { }

        IEnumerable<TItem> findRoots() {
            if (Data != null) {
                List<Pair<TItem, int>> graphroots = new List<Pair<TItem, int>>();
                SceneWalker walker = new SceneWalker(Data.Graph);
                walker.Algo = this._algo;
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
                            if (levelItem.Node == Data.Focused) {
                                selected = true;
                            }
                        }
                        if (selected) {
                            root = new Pair<TItem, int>((TItem)Data.Focused, 0);
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
            walker.Algo = this._algo;

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
                        if (_orientation == Orientation.TopBottom) {
                            if (_centered)
                                ident = (maxSize.Width - row.Size.Width) / 2;
                            startAt.X = ident + distance.Width;
                        } else if (_orientation == Orientation.LeftRight) {
                            if (_centered)
                                ident = (maxSize.Height - row.Size.Height) / 2;
                            startAt.Y = ident + distance.Height;
                        }
                        foreach (IWidget widget in row.Widgets) {

                            Data.RemoveBounds (widget);
                            widget.Location = startAt;
                            Data.AddBounds(widget);

                            if (_orientation == Orientation.TopBottom) {
                                startAt.X += widget.Size.Width + distance.Width;
                            } else if (_orientation == Orientation.LeftRight) {
                                startAt.Y += widget.Size.Height + distance.Height;
                            }
                        }
                        if (_orientation == Orientation.TopBottom) {
                            startAt.Y = startAt.Y + row.MaxSize.Height + distance.Height;
                        } else if (_orientation == Orientation.LeftRight) {
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

    public class SceneWalker : Walker<IWidget, ILinkWidget> {
        public SceneWalker(IGraph<IWidget, ILinkWidget> graph) : base(graph) { }
    }




}

