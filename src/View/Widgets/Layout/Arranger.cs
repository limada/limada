/*
 * Limaki 
 * Version 0.071
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
using System.Drawing;
using Limaki.Actions;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Graphs;

namespace Limaki.Widgets.Layout {
    public class Arranger<TData, TItem, TEdge>
        where TData : Scene
        where TItem : class, IWidget
        where TEdge : IEdge<TItem>, TItem {

        private Scene _data;
        public Scene Data {
            get { return _data; }
            set { _data = value; }
        }

        public Size AutoSize;

        private bool _centered;
        public bool Centered {
            get { return _centered; }
            set { _centered = value; }
        }

        private Size _distance;
        public Size Distance {
            get { return _distance; }
            set { _distance = value; }
        }

        private Orientation _orientation;
        public Orientation Orientation {
            get { return _orientation; }
            set { _orientation = value; }
        }

        public ILayout<TData, TItem> Layout {
            get {

                return _layout;
            }
            set {
                if (_layout != value) {
                    _layout = value;
                    LayoutProperties prop = new LayoutProperties();
                    prop.Layout = this.Layout;
                    this.Orientation = prop.Orientation;
                    this.Centered = prop.Centered;
                    this.Distance = prop.Distance;
                    this.AutoSize = Layout.StyleSheet.DefaultStyle.AutoSize;
                }
                
            }
        }

        private ILayout<TData, TItem> _layout = null;

        public ICollection<TEdge> AffectedEdges = new List<TEdge>();



        public Arranger(TData data, ILayout<TData,TItem> layout) {
            this.Data = data;
            this.Layout = layout;
        }

        protected Set<TItem> visited = new Set<TItem>();
        protected IDictionary<TItem,int> RowIndices = new Dictionary<TItem,int>();
        protected IDictionary<int, Row<TItem>> rows = new SortedDictionary<int, Row<TItem>>();

        protected Size overallSize = Size.Empty;

        public virtual void AdjustRowBounds(Point startAt) {
            // calculate the extend of the rows
            int iRow = 0;
            foreach (KeyValuePair<int, Row<TItem>> kvp in rows) {
                Row<TItem> row = kvp.Value;
                row.Location = startAt;
                iRow++;
                if (Orientation == Orientation.TopBottom) {
                    row.Location.Y = row.Location.Y +
                        overallSize.Height +//Math.Max(overallSize.Height, iRow - 1 * AutoSize.Height) +
                        (iRow * Distance.Height);
                } else if (Orientation == Orientation.LeftRight) {
                    row.Location.X = row.Location.X + 
                        overallSize.Width+
                        (iRow * Distance.Width);
                }

                foreach (TItem widget in row.Items) {
                    if (!(widget is IEdgeWidget)) {
                        row.Size += new Size(widget.Size.Width + Distance.Width,
                                             widget.Size.Height + Distance.Height);

                        row.biggestWidgetSize = new Size(Math.Max(row.biggestWidgetSize.Width, widget.Size.Width),
                                               Math.Max(row.biggestWidgetSize.Height, widget.Size.Height));


                    }
                }

                if (Orientation == Orientation.TopBottom) {
                    row.Size.Height = row.biggestWidgetSize.Height;
                    overallSize.Height = overallSize.Height + row.biggestWidgetSize.Height;
                    overallSize.Width = Math.Max (overallSize.Width, row.Size.Width);
                } else {
                    row.Size.Width = row.biggestWidgetSize.Width;
                    overallSize.Height = Math.Max (overallSize.Height, row.Size.Height);
                    overallSize.Width = overallSize.Width + row.biggestWidgetSize.Width;
                }

            }            
        }

        public virtual void ArrangeRows(Point startAt, ICollection<TItem> siblings) {
            AdjustRowBounds (startAt);

            Rectangle stripe = Data.Shape.BoundsRect;

            foreach (KeyValuePair<int, Row<TItem>> kvp in rows) {
                Row<TItem> row = kvp.Value;

                if (Centered) {
                    if (Orientation == Orientation.TopBottom) {
                        row.Location.X = ((overallSize.Width - row.Size.Width) / 2) + Distance.Width;
                    } else if (Orientation == Orientation.LeftRight) {
                        row.Location.Y = ((overallSize.Height - row.Size.Height) / 2) + Distance.Height;
                    }
                }

                if (Orientation == Orientation.TopBottom) {
                    stripe.Y = row.Location.Y;
                    stripe.Height = row.biggestWidgetSize.Height;
                } else if (Orientation == Orientation.LeftRight) {
                    stripe.X = row.Location.X;
                    stripe.Width = row.biggestWidgetSize.Width;
                }

                // calculate the most left and right point in stripe
                Point locateRight = new Point();
                Point locateLeft = new Point (int.MaxValue,int.MaxValue);
                foreach (TItem widget in Data.ElementsIn(stripe)) {
                    if (!(widget is TEdge) && !siblings.Contains((TItem)widget)) {
                        Point widgetLocation = GetLocation(widget);
                        locateRight.X = Math.Max(locateRight.X, widgetLocation.X + widget.Size.Width);
                        locateRight.Y = Math.Max(locateRight.Y, widgetLocation.Y + widget.Size.Height);
                        locateLeft.X = Math.Min(locateLeft.X, widgetLocation.X);
                        locateLeft.Y = Math.Min(locateLeft.Y, widgetLocation.Y);
                    }
                }

                if (locateLeft.X == int.MaxValue)
                    locateLeft.X = 0;
                if (locateLeft.Y == int.MaxValue)
                    locateLeft.Y = 0;
                if (Orientation == Orientation.TopBottom) {
                    if ((row.Location.X + row.Size.Width - Distance.Width) < locateLeft.X) {
                        row.Location.X = Math.Min(row.Location.X, locateLeft.X + Distance.Width);
                    } else {
                        row.Location.X = Math.Max(row.Location.X, locateRight.X + Distance.Width);
                    }
                } else if (Orientation == Orientation.LeftRight) {
                    if ((row.Location.Y + row.Size.Height + Distance.Height) < locateLeft.Y) {
                        row.Location.Y = Math.Min(row.Location.X, locateLeft.Y + Distance.Height);
                    } else {
                        row.Location.Y = Math.Max (row.Location.Y, locateRight.Y + Distance.Height);
                    }
                }

                startAt = row.Location;

                foreach (TItem widget in row.Items)
                    if (!(widget is TEdge)) {
                        MoveTo(widget, startAt);
                        foreach (TEdge edge in Data.Twig(widget)) {
                            AffectedEdges.Add(edge);
                        }
                        if (Orientation == Orientation.TopBottom) {
                            startAt.X += widget.Size.Width + Distance.Width;
                        } else if (Orientation == Orientation.LeftRight) {
                            startAt.Y += widget.Size.Height + Distance.Height;
                        }
                    }
            }

            foreach (KeyValuePair<TItem, Point> kvp in locations) {
                Data.Commands.Add(new MoveCommand(kvp.Key, kvp.Value));
            }

        }


        /// <summary>
        /// same as arrangerows, but without stripe-handling
        /// uses layout to invoke and justify affected widgets
        /// </summary>
        /// <param name="startAt"></param>
        /// <param name="layout"></param>
        public virtual void ArrangeRowsOfDeepWalk(ref Point startAt) {

            AdjustRowBounds(startAt);

            foreach (KeyValuePair<int, Row<TItem>> kvp in rows) {
                Row<TItem> row = kvp.Value;

                if (Centered) {
                    if (Orientation == Orientation.TopBottom) {
                        row.Location.X = ((overallSize.Width - row.Size.Width) / 2) + Distance.Width;
                    } else if (Orientation == Orientation.LeftRight) {
                        row.Location.Y = ((overallSize.Height - row.Size.Height) / 2) + Distance.Height;
                    }
                }

                startAt = row.Location;

                foreach (TItem widget in row.Items) {
                    if (!( widget is IEdge )) {
                        MoveTo (widget, startAt);
                        foreach (TEdge edge in Data.Twig (widget)) {
                            AffectedEdges.Add (edge);
                        }
                        if (Orientation == Orientation.TopBottom) {
                            startAt.X += widget.Size.Width + Distance.Width;
                        } else if (Orientation == Orientation.LeftRight) {
                            startAt.Y += widget.Size.Height + Distance.Height;
                        }
                    }
                }

                if (_orientation == Orientation.TopBottom) {
                    startAt.Y = startAt.Y + row.biggestWidgetSize.Height + Distance.Height;
                } else if (_orientation == Orientation.LeftRight) {
                    startAt.X = startAt.X + row.biggestWidgetSize.Width + Distance.Width;
                }
            }

            foreach (KeyValuePair<TItem, Point> kvp in locations) {
                Data.Commands.Add(new MoveCommand(kvp.Key, kvp.Value));
            }

        }


        public virtual void AddWalk(TItem start, ICollection<TItem> siblings) {
            SceneWalker walker = new SceneWalker(Data.Graph);
            walker.visited = this.visited as Set<IWidget>;
            foreach (LevelItem<IWidget> item in walker.Walk(start, 0)) {
                AddToRow(item.Level, (TItem)item.Node, start, siblings);
                if (item.Node is TEdge) {
                    TEdge edge = (TEdge) item.Node;
                    if (!AffectedEdges.Contains (edge)) {
                        AffectedEdges.Add (edge);
                    }
                }
            }
        }


        public virtual void AddDeepWalk(TItem start) {
            SceneWalker walker = new SceneWalker(Data.Graph);
            walker.visited = this.visited as Set<IWidget>;
            foreach (LevelItem<IWidget> item in walker.DeepWalk (start, 0)) {
                this.Layout.Invoke ((TItem) item.Node);
                this.Layout.Justify((TItem)item.Node);

                AddToRow(item.Level, (TItem) item.Node);

                if (item.Node is TEdge) {
                    TEdge edge = (TEdge) item.Node;

                    if (!AffectedEdges.Contains (edge)) {
                        AffectedEdges.Add (edge);
                    }
                }
            }
        }


        public static TItem Adjacent(TEdge edge, TItem item) {
            if (item != null)
                if (item.Equals(edge.Root)) {
                    return edge.Leaf;
                } else if (item.Equals(edge.Leaf)) {
                    return edge.Root;
                }
            return null;
        }

        public virtual Point Arrange(TItem start, ICollection<TItem> siblings) {
            Point startAt = start.Location;
            TItem root = start;
            while (root is TEdge) {
                root = ((TEdge)root).Root;
            }
            startAt = root.Location;


            if (siblings.Count == 1 && siblings.Contains(start)) {
                startAt = (Point)Distance;
                MoveTo(start, startAt);
            }

            if (Orientation == Orientation.TopBottom) {
                startAt.Y = startAt.Y + root.Size.Height;
                startAt.X = Distance.Width;
            } else if (Orientation == Orientation.LeftRight) {
                startAt.X = startAt.X + root.Size.Width;
                startAt.Y = Distance.Height;
            }

            overallSize = Size.Empty;

            AddWalk(start, siblings);

            ArrangeRows(startAt, siblings);

            return startAt;
        }

        protected virtual void AddToRow(int level, TItem widget, TItem start, ICollection<TItem> siblings) {
            if ((widget != null) && (widget != start) && siblings.Contains((TItem)widget)) {
                AddToRow(level, widget);
            }
        }

        protected virtual void AddToRow(int level, TItem widget) {
            if ((widget != null) && !(widget is TEdge)) {
                if (!RowIndices.ContainsKey(widget)) {
                    Row<TItem> row = null;
                    if (!rows.TryGetValue(level, out row)) {
                        row = new Row<TItem>();
                        rows.Add(level, row);
                    }
                    row.Items.Add((TItem)widget);
                    RowIndices.Add(widget, level);
                }
            }
        }

        private IDictionary<TItem, Point> locations = new Dictionary<TItem, Point>();
        public virtual void MoveTo(TItem widget, Point location) {
            locations[widget] = location;
        }

        Point GetLocation(TItem widget) {
            Point result;
            if (!locations.TryGetValue(widget, out result)) {
                result = widget.Location;
            }
            return result;
        }

        #region deprecated
        public virtual void AddToArrange2(TItem start, ICollection<TItem> siblings) {
            int level = 0;
            if (!visited.Contains(start)) {
                visited.Add(start);
                Queue<LevelItem<TItem>> queue = new Queue<LevelItem<TItem>>();
                queue.Enqueue(new LevelItem<TItem>(start, null, level));
                while (queue.Count > 0) {
                    LevelItem<TItem> item = queue.Dequeue();
                    level = item.Level;
                    if (item.Node is TEdge) {

                        TEdge edge = (TEdge)item.Node;
                        if (!AffectedEdges.Contains(edge)) {
                            AffectedEdges.Add(edge);
                        }

                        // follow link of links
                        foreach (TEdge edge_edge in Data.Graph.Edges(edge)) { // Fork!?
                            if (!visited.Contains(edge_edge)) {
                                queue.Enqueue(new LevelItem<TItem>(edge_edge, edge, level + 1));
                                visited.Add(edge_edge);
                            }
                        }
                        TItem adjacent = Adjacent(edge, item.Path);
                        if (adjacent != null) {
                            // follow adjacent of node:
                            if (!visited.Contains(adjacent)) {
                                if (adjacent is TEdge) {
                                    queue.Enqueue(new LevelItem<TItem>(adjacent, edge, level));
                                } else {
                                    AddToRow(level, adjacent, start, siblings);
                                }
                                visited.Add(adjacent);
                            }
                        } else {
                            if (!visited.Contains(edge.Root)) {
                                if (edge.Root is TEdge) {
                                    queue.Enqueue(new LevelItem<TItem>(edge.Root, edge, level));
                                } else {
                                    AddToRow(level, edge.Root, start, siblings);
                                }
                                visited.Add(edge.Root);
                            }
                            if (!visited.Contains(edge.Leaf)) {
                                if (edge.Leaf is TEdge) {
                                    queue.Enqueue(new LevelItem<TItem>(edge.Leaf, edge, level));
                                } else {
                                    AddToRow(level, edge.Leaf, start, siblings);
                                }
                                visited.Add(edge.Leaf);
                            }
                        }
                    } else {
                        foreach (TEdge edge in Data.Graph.Edges(item.Node)) {
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
        #endregion
    }
}