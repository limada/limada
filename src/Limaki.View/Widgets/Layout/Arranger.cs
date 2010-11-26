/*
 * Limaki 
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
using Limaki.Common;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Drawing.UI;

namespace Limaki.Widgets.Layout {
    /// <summary>
    /// Arranger is a Unit of Work to arrange items
    /// be carefull: create a new arranger for every unit of work
    /// the Items have valid data AFTER commiting scene.Commands
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    public class Arranger<TData, TItem, TEdge>
        where TData : Scene
        where TItem : IWidget
        where TEdge : IEdge<TItem>, TItem {

        public Arranger(TData data, ILayout<TData, TItem> layout) {
            this.Data = data;
            this.Layout = layout;
            this.proxy = new ShapeProxy<TData, TItem, TEdge>(layout);
        }

        public bool Adjust = false;

        private TData _data;
        public TData Data {
            get { return _data; }
            set { _data = value; }
        }

        public IGraph<TItem, TEdge> Graph {
            get { return (IGraph<TItem, TEdge>)Data.Graph; }
        }

        public SizeI AutoSize;

        private bool _centered;
        public bool Centered {
            get { return _centered; }
            set { _centered = value; }
        }

        private SizeI _distance;
        public SizeI Distance {
            get { return _distance; }
            set { _distance = value; }
        }

        private Orientation _orientation;
        public Orientation Orientation {
            get { return _orientation; }
            set { _orientation = value; }
        }

        public ILayout<TData, TItem> Layout {
            get { return _layout; }
            set {
                if (_layout != value) {
                    _layout = value;
                    this.Distance = _layout.Distance;
                    LayoutProperties prop = new LayoutProperties();
                    prop.Layout = this.Layout;
                    this.Orientation = prop.Orientation;
                    this.Centered = prop.Centered;
                    this.AutoSize = Layout.StyleSheet.DefaultStyle.AutoSize;
                }

            }
        }

        private ILayout<TData, TItem> _layout = null;

        protected Set<TItem> visited = new Set<TItem>();
        protected IDictionary<TItem, int> RowIndices = new Dictionary<TItem, int>();

        protected IDictionary<int, Row<TItem>> rows = new SortedDictionary<int, Row<TItem>>();

        protected SizeI overallSize = SizeI.Empty;

        #region row-handling

        public virtual void ClearRows() {
            rows.Clear();
            overallSize = SizeI.Empty;
        }

        protected virtual void AddToRow(int level, TItem widget, TItem start, ICollection<TItem> siblings) {
            if ((widget != null) && (!widget.Equals(start)) && siblings.Contains((TItem)widget)) {
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

        public virtual void AdjustRowBounds(PointI startAt) {
            // calculate the extend of the rows
            int iRow = 0;

            if (Orientation == Orientation.LeftRight)
                iRow--;

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
                        overallSize.Width +
                        (iRow * Distance.Width);
                }

                foreach (TItem widget in row.Items) {
                    if (!(widget is TEdge)) {
                        SizeI widgetSize = proxy.GetSize(widget);
                        row.Size += new SizeI(widgetSize.Width + Distance.Width,
                                             widgetSize.Height + Distance.Height);

                        row.biggestItemSize = new SizeI(Math.Max(row.biggestItemSize.Width, widgetSize.Width),
                                               Math.Max(row.biggestItemSize.Height, widgetSize.Height));


                    }
                }

                if (Orientation == Orientation.TopBottom) {
                    row.Size.Height = row.biggestItemSize.Height;
                    overallSize.Height = overallSize.Height + row.biggestItemSize.Height;
                    overallSize.Width = Math.Max(overallSize.Width, row.Size.Width);
                } else {
                    row.Size.Width = row.biggestItemSize.Width;
                    overallSize.Height = Math.Max(overallSize.Height, row.Size.Height);
                    overallSize.Width = overallSize.Width + row.biggestItemSize.Width;
                }

            }
        }


        public virtual Pair<PointI, PointI> StripeLocactions(RectangleI stripe, Row<TItem> row, ICollection<TItem> siblings) {
            if (Orientation == Orientation.TopBottom) {
                stripe.Y = row.Location.Y;
                stripe.Height = row.biggestItemSize.Height;
            } else if (Orientation == Orientation.LeftRight) {
                stripe.X = row.Location.X;
                stripe.Width = row.biggestItemSize.Width;
            }

            // calculate the most left and right point in stripe
            PointI locateRight = new PointI();
            PointI locateLeft = new PointI(int.MaxValue, int.MaxValue);
            foreach (TItem widget in Data.ElementsIn(stripe)) {
                if (!(widget is TEdge) && !siblings.Contains((TItem)widget)) {
                    PointI widgetLocation = proxy.GetLocation(widget);
                    SizeI widgetSize = proxy.GetSize(widget);

                    locateRight.X = Math.Max(locateRight.X, widgetLocation.X + widgetSize.Width);
                    locateRight.Y = Math.Max(locateRight.Y, widgetLocation.Y + widgetSize.Height);
                    locateLeft.X = Math.Min(locateLeft.X, widgetLocation.X);
                    locateLeft.Y = Math.Min(locateLeft.Y, widgetLocation.Y);
                }
            }

            if (locateLeft.X == int.MaxValue)
                locateLeft.X = 0;
            if (locateLeft.Y == int.MaxValue)
                locateLeft.Y = 0;

            return new Pair<PointI, PointI> (locateLeft, locateRight);
        }

        public virtual void ArrangeRows(ref PointI startAt, ICollection<TItem> siblings) {
            AdjustRowBounds(startAt);

            RectangleI stripe = Data.Shape.BoundsRect;

            foreach (KeyValuePair<int, Row<TItem>> kvp in rows) {
                Row<TItem> row = kvp.Value;

                if (Centered) {
                    if (Orientation == Orientation.TopBottom) {
                        row.Location.X += ((overallSize.Width - row.Size.Width) / 2);
                    } else if (Orientation == Orientation.LeftRight) {
                        row.Location.Y = row.Location.Y+((overallSize.Height - row.Size.Height) / 2);
                    }
                }


                if (siblings != null) {
                    var stripeLocation = StripeLocactions (stripe, row, siblings);
                    PointI locateLeft = stripeLocation.One;
                    PointI locateRight = stripeLocation.Two;

                    if (Orientation == Orientation.TopBottom) {
                        if ((row.Location.X + row.Size.Width - Distance.Width) < locateLeft.X) {
                            row.Location.X = Math.Min(row.Location.X, locateLeft.X + Distance.Width);
                        } else {
                            row.Location.X = Math.Max(row.Location.X, locateRight.X + Distance.Width);
                        }
                    } else if (Orientation == Orientation.LeftRight) {
                        if ((row.Location.Y + row.Size.Height + Distance.Height) < locateLeft.Y) {
                            row.Location.Y = Math.Min(row.Location.Y, locateLeft.Y + Distance.Height);
                        } else {
                            row.Location.Y = Math.Max(row.Location.Y, locateRight.Y + Distance.Height);
                        }
                    }
                } 


                PointI location = row.Location;

                foreach (TItem widget in row.Items)
                    if (!(widget is TEdge)) {

                        proxy.SetLocation(widget, location);
                        
                        if (siblings == null)
                            proxy.Justify(widget);

                        foreach (TEdge edge in this.Graph.Twig(widget)) {
                            proxy.AffectedEdges.Add(edge);
                        }
                        SizeI size = new SizeI (
                            proxy.GetSize (widget).Width + Distance.Width,
                            proxy.GetSize (widget).Height + Distance.Height);

                        if (Orientation == Orientation.TopBottom) {
                            location.X += size.Width;
                            startAt.X = Math.Max(location.X, startAt.X);
                            startAt.Y = Math.Max(location.Y+size.Height, startAt.Y);
                        } else if (Orientation == Orientation.LeftRight) {
                            location.Y += size.Height;
                            startAt.X = Math.Max(location.X + size.Width, startAt.X);
                            startAt.Y = Math.Max(location.Y, startAt.Y);
                        }

                    }


            }


        }



        public virtual void AddWalk(TItem start, ICollection<TItem> siblings, bool deep) {
            Walker<TItem, TEdge> walker = new Walker<TItem, TEdge>(this.Graph);
            walker.visited = this.visited as Set<TItem>;
            IEnumerable<LevelItem<TItem>> walk = null;
            if (deep)
                walk = walker.DeepWalk(start, 0);
            else
                walk = walker.Walk(start, 0);

            foreach (LevelItem<TItem> item in walk) {
                //Justify(item.Node);
                if (siblings != null) {
                    AddToRow(item.Level, item.Node, start, siblings);
                } else {
                    AddToRow(item.Level, item.Node);
                }
                if (item.Node is TEdge) {
                    TEdge edge = (TEdge)item.Node;
                    if (!proxy.AffectedEdges.Contains(edge)) {
                        proxy.AffectedEdges.Add(edge);
                    }
                }
            }
        }
        #endregion

        public virtual PointI Arrange(TItem start, PointI location) {
            PointI startAt = location;

            proxy.GetShape(start);
            proxy.SetLocation(start, startAt);

            AddWalk(start, null, true);

            ArrangeRows(ref startAt, null);

            return startAt;
        }

        public virtual PointI Arrange(TItem item, ICollection<TItem> siblings, PointI moveTo) {
            proxy.Justify(item);

            if (!(item is TEdge))
                proxy.SetLocation(item, moveTo);

            AddToRow(0, item, default(TItem), siblings);
            ArrangeRows(ref moveTo, siblings);

            foreach (TEdge edge in this.Graph.Twig(item)) {
                proxy.AffectedEdges.Add(edge);
            }

            return moveTo;
        }

        public virtual void ArrangeEdges(ICollection<TItem> siblings, bool justify) {
            foreach (TItem item in siblings) {
                if (!(item is TEdge)) {
                    if (!justify)
                        proxy.EnsureInvoke(item);
                    else
                        proxy.Justify(item);

                    foreach (TEdge edge in this.Graph.Twig(item)) {
                        proxy.AffectedEdges.Add(edge);
                    }
                }
            }
        }

        public virtual void Arrange(ICollection<TItem> siblings, bool justify, PointI startAt) {
            foreach (TItem item in siblings) {
                if (!(item is TEdge)) {
                    if (!justify)
                        proxy.EnsureInvoke(item);
                    else
                        proxy.Justify(item);


                    if (!visited.Contains(item)) {
                        startAt = new PointI(Distance.Width, startAt.Y);
                        startAt = Arrange(item, startAt);
                        ClearRows();
                    }

                }
            }
        }

        public virtual PointI Arrange(IEnumerable<TItem> items, ICollection<TItem> siblings, bool deep) {
            PointI result = (PointI)Distance;
            foreach (TItem item in items) {
                result = Arrange(item, siblings, deep);
            }
            return result;
        }

        public virtual PointI Arrange(TItem start, ICollection<TItem> siblings, bool deep) {
            PointI startAt = (PointI)Distance;
            TItem root = start;
            while (root is TEdge) {
                root = ((TEdge)root).Root;
            }
            IShape shape = proxy.GetShape(root);
            startAt = shape.Location;

            if (siblings!=null && siblings.Count == 1 && siblings.Contains(start)) {
                startAt = (PointI)Distance;
                proxy.SetLocation(start, startAt);
            }

            if (Orientation == Orientation.TopBottom) {
                startAt.Y = startAt.Y + proxy.GetSize(root).Height;
                //startAt.X = Distance.Width;
            } else if (Orientation == Orientation.LeftRight) {
                startAt.X = startAt.X + proxy.GetSize (root).Width + Distance.Width;
                //startAt.Y = Distance.Height;
            }

            overallSize = SizeI.Empty;

            AddWalk(start, siblings, deep);

            ArrangeRows(ref startAt, siblings);

            return startAt;
        }



        ShapeProxy<TData, TItem, TEdge> proxy = null;
        public virtual void Commit() {
            proxy.Commit(this.Data);
        }
    }
}