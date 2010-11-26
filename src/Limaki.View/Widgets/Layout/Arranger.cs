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

using System;
using System.Collections.Generic;
using Limaki.Common;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Drawing.UI;
using System.Linq;

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
            this.RowCollisionResolver = FirstFreeRowCollissionResolver;
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


        /// <summary>
        /// adds widget to row[level]
        /// </summary>
        /// <param name="level"></param>
        /// <param name="widget"></param>
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
        protected virtual void AdjustRowSize(Row<TItem> row){
            if (!row.SizeAdjusted) {
                foreach (TItem widget in row.Items) {
                    if (!( widget is TEdge )) {
                        SizeI widgetSize = proxy.GetSize (widget);
                        row.Size += new SizeI (widgetSize.Width + Distance.Width,
                                               widgetSize.Height + Distance.Height);

                        row.biggestItemSize = new SizeI (Math.Max (row.biggestItemSize.Width, widgetSize.Width),
                                                         Math.Max (row.biggestItemSize.Height, widgetSize.Height));
                    }
                }
                row.SizeAdjusted = true;
            }
        }
        protected virtual void AdjustRowLocation(Row<TItem> row, PointI location) {
            row.Location = location;
            if (Orientation == Orientation.TopBottom) {
                row.Size.Height = row.biggestItemSize.Height;
            } else {
                row.Size.Width = row.biggestItemSize.Width;
            }
            if (Centered) {
                if (Orientation == Orientation.TopBottom) {
                    row.Location.X = location.X - ( row.Size.Width/2 ) + (Layout.Distance.Width/2);
                } else if (Orientation == Orientation.LeftRight) {
                    row.Location.Y = location.Y - (row.Size.Height / 2) + (Layout.Distance.Height / 2);
                }
            }
        }



        protected void FirstFreeRowCollissionResolver(Row<TItem> row, ICollection<TItem> ignoring) {
            bool collission = true;
            RectangleI stripe = new RectangleI (row.Location, row.Size);
            var startStripe = stripe;

            Func<RectangleI, int> orderByR = r => r.Right;
            Func<RectangleI, int> orderByB = r => r.Bottom;
            Func<RectangleI, int> orderBy = orderByR;
            Action<RectangleI> moveDown = (r) => { stripe.Y = r.Bottom + Distance.Height; };
            Action<RectangleI> moveRight = (r) => { stripe.X = r.Right + Distance.Width; };

            if (Orientation == Orientation.LeftRight) {
                stripe.Height = stripe.Height - Layout.Distance.Height;
                orderBy = orderByR;
            } else {
                stripe.Width = stripe.Width - Layout.Distance.Width;
                orderBy = orderByB;
            }

            bool collissionDetected = false;
            while (collission) {
                collission = false;
                //stripe.X = stripe.X - Layout.Distance.Width+1;
                var l = from TItem item in Data.ElementsIn(stripe)
                        where !(item is TEdge) && !ignoring.Contains(item)
                        select new RectangleI (proxy.GetLocation (item), proxy.GetSize (item));
                l = l.OrderByDescending(orderBy);

                foreach(var bounds in l) {
                    if (Orientation == Orientation.LeftRight) {
                        if (bounds.X < stripe.X || bounds.Right > stripe.Right) {
                            if (stripe.X < startStripe.Bottom) {
                                moveDown (bounds);
                                orderBy = orderByB;
                            } else {
                                moveRight (bounds);
                                orderBy = orderByB;
                            }
                        } else {
                            moveDown (bounds);
                            orderBy = orderByR;
                        }
                    } else {
                        if (bounds.Y < stripe.Y || bounds.Bottom > stripe.Bottom) {
                            //if (stripe.Y < startStripe.Right) {
                            //    moveRight(bounds);
                            //    orderBy = orderByR;
                            //} 
                            //else {
                                moveDown (bounds);
                                orderBy = orderByR;
                            //}
                        } else {
                            moveRight (bounds);
                            orderBy = orderByB;
                        }
                    }
                    collission = true;
                    collissionDetected |= collission;
                    break;
                }
            }
            if (collissionDetected == false) {
                stripe = startStripe;
            }
            row.Location = stripe.Location;
            row.Size = stripe.Size;
        }
        
        protected Action<Row<TItem>, ICollection<TItem>> RowCollisionResolver = null;
        
        /// <summary>
        /// Arranges this.rows
        /// caculates the area needed by each row
        /// if siblings != null, takes care of free space
        /// sets every item in row and justifies each item if siblings !=null
        /// </summary>
        /// <param name="startAt"></param>
        /// <param name="siblings"></param>
        protected virtual void ArrangeRows(ref PointI startAt, ICollection<TItem> siblings) {

            var ignore = new Set<TItem>(siblings);

            var rowStart = startAt;

            foreach (KeyValuePair<int, Row<TItem>> kvp in this.rows) {
                Row<TItem> row = kvp.Value;

                AdjustRowSize(row);
                AdjustRowLocation(row, rowStart);

                if (siblings != null && RowCollisionResolver != null) {
                    RowCollisionResolver(row, ignore);
                }


                PointI location = row.Location;
                foreach (TItem widget in row.Items)
                    if (!(widget is TEdge)) {

                        proxy.SetLocation(widget, location);
                        if (siblings != null)
                            ignore.Remove(widget);

                        if (siblings == null)
                            proxy.Justify(widget);


                        foreach (TEdge edge in this.Graph.Twig(widget)) {
                            proxy.AffectedEdges.Add(edge);
                        }

                        SizeI size = new SizeI(
                            proxy.GetSize(widget).Width + Distance.Width,
                            proxy.GetSize(widget).Height + Distance.Height);

                        if (Orientation == Orientation.TopBottom) {
                            location.X += size.Width;
                            startAt.X = Math.Max(location.X, startAt.X);
                            startAt.Y = Math.Max(location.Y + size.Height, startAt.Y);
                            rowStart.Y = startAt.Y;
                        } else if (Orientation == Orientation.LeftRight) {
                            location.Y += size.Height;
                            startAt.X = Math.Max(location.X + size.Width, startAt.X);
                            startAt.Y = Math.Max(location.Y, startAt.Y);
                            rowStart.X = startAt.X;
                        }
                    }
            }
        }


        /// <summary>
        /// make rows out of a walk through item according to deep
        /// adds every walk-item to a row according its level
        /// </summary>
        /// <param name="start"></param>
        /// <param name="siblings"></param>
        /// <param name="deep"></param>
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
                    if (!item.Node.Equals(start) && siblings.Contains(item.Node)) {
                        AddToRow(item.Level, item.Node);
                    }
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



        /// <summary>
        /// sets start to location
        /// justify item
        /// arranges all siblings 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="siblings"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public virtual PointI Arrange(TItem start, ICollection<TItem> siblings, PointI location) {

            proxy.Justify(start);

            if (!(start is TEdge))
                proxy.SetLocation(start, location);

            if (siblings.Contains(start))
                AddToRow(0, start);

            ArrangeRows(ref location, siblings);

            foreach (TEdge edge in this.Graph.Twig(start)) {
                proxy.AffectedEdges.Add(edge);
            }

            return location;
        }


        public virtual PointI Arrange(IEnumerable<TItem> items, ICollection<TItem> siblings, bool deep) {
            PointI result = (PointI)Distance;
            foreach (TItem item in items) {
                result = ArrangeExpand(item, siblings, deep);
            }
            return result;
        }

        /// <summary>
        /// Expands a single item 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="siblings"></param>
        /// <param name="deep"></param>
        /// <returns></returns>
        protected virtual PointI ArrangeExpand(TItem start, ICollection<TItem> siblings, bool deep) {
            PointI startAt = (PointI)Distance;
            TItem root = start;
            while (root is TEdge) {
                root = ((TEdge)root).Root;
            }

            startAt = proxy.GetShape(root).Location;

            if (siblings != null && siblings.Count == 1 && siblings.Contains(start)) {
                startAt = (PointI)Distance;
                proxy.SetLocation(start, startAt);
            }

            var startSize = proxy.GetSize (root);


            if (Orientation == Orientation.TopBottom) {
                if (Centered) startAt.X += startSize.Width / 2;
                startAt.Y = startAt.Y + startSize.Height;
                
            } else if (Orientation == Orientation.LeftRight) {
                if (Centered) startAt.Y += startSize.Height / 2;
                startAt.X = startAt.X + startSize.Width + Distance.Width;
            }

            overallSize = SizeI.Empty;

            AddWalk(start, siblings, deep);

            ArrangeRows(ref startAt, siblings);

            return startAt;
        }



        /// <summary>
        /// sets start to location
        /// add rows of a deepwalk
        /// arrangerows
        /// </summary>
        /// <param name="start"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        protected virtual PointI ArrangeDeepWalk(TItem start, PointI location) {
            PointI startAt = location;

            proxy.GetShape(start);
            
            AddWalk(start, null, true);

            if (Centered) {
                var size = new SizeI ();
                foreach(var kvp in rows) {
                    var row = kvp.Value;
                    AdjustRowSize (row);
                    size.Height = Math.Max (size.Height, row.Size.Height);
                    size.Width = Math.Max (size.Width, row.Size.Width);
                }
                if (Orientation == Orientation.LeftRight) {
                    size.Height -= Distance.Height;
                    startAt.Y += size.Height/2;
                }
            }

            proxy.SetLocation(start, startAt);
            ArrangeRows(ref startAt, null);

            return startAt;
        }

        /// <summary>
        /// add deepwalk-rows for each item
        /// </summary>
        /// <param name="items"></param>
        /// <param name="justify"></param>
        /// <param name="location"></param>
        public virtual void ArrangeDeepWalk(IEnumerable<TItem> items, bool justify, PointI location) {
            foreach (TItem item in items) {
                if (!(item is TEdge)) {
                    if (!justify)
                        proxy.EnsureInvoke(item);
                    else
                        proxy.Justify(item);


                    if (!visited.Contains(item)) {
                        location = new PointI(Distance.Width, location.Y);
                        location = ArrangeDeepWalk(item, location);
                        ClearRows();
                    }

                }
            }
        }

        /// <summary>
        /// invokes (if neccessary ) or justifies all items
        /// adds the twig of each item do affectedEdges
        /// </summary>
        /// <param name="items"></param>
        /// <param name="justify"></param>
        public virtual void ArrangeEdges(ICollection<TItem> items, bool justify) {
            foreach (TItem item in items) {
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

        #region Refactoring
        public class ArrangeArgs<TItem>  {
            public IEnumerable<TItem> items { get; set; }
            public ICollection<TItem> siblings { get; set; }
            public PointI location { get; set; }
            public bool justify { get; set; }
            public bool deepwalk { get; set; }

        }
        
        
        // TODO: try to consolidate this with args
        // maybe args without items and siblings??
        // like: Arrange (IEnumerable<TItem> items,ICollection<TItem> siblings, Args args)
 
        public virtual void ArrangeWithArgs(ArrangeArgs<TItem> args) {

            //**** ArrangeDeepWalk (IEnumerable<TItem> items, bool justify, PointI location)
            foreach (TItem item in args.items) {
                if (!(item is TEdge)) {
                    if (!args.justify)
                        proxy.EnsureInvoke(item);
                    else
                        proxy.Justify(item);


                    if (!visited.Contains(item)) {
                        args.location = new PointI(Distance.Width, args.location.Y);
                        
                        //**** PointI ArrangeDeepWalk(TItem start, PointI location) {
                        PointI location = args.location;

                        proxy.GetShape(item);
                        proxy.SetLocation(item, location);

                        AddWalk(item, null, true);

                        ArrangeRows(ref location, null);

                        args.location = location;

                        //**** end ArrangeDeepWalk
                        
                        ClearRows();
                    }

                }
            }

            //**** PointI Arrange(IEnumerable<TItem> items, ICollection<TItem> siblings, bool deep)
            
            args.location = (PointI)Distance;
            foreach (TItem item in args.items) {

                //**** PointI ArrangeExpand(TItem start, ICollection<TItem> siblings, bool deep) {
                PointI location = (PointI)Distance;
                TItem root = item;
                while (root is TEdge) {
                    root = ((TEdge)root).Root;
                }
                
                IShape shape = proxy.GetShape(root);
                location = shape.Location;

                if (args.siblings != null && args.siblings.Count == 1 && args.siblings.Contains(item)) {
                    location = (PointI)Distance;
                    proxy.SetLocation(item, location);
                }

                if (Orientation == Orientation.TopBottom) {
                    location.Y = location.Y + proxy.GetSize(root).Height;
                } else if (Orientation == Orientation.LeftRight) {
                    location.X = location.X + proxy.GetSize(root).Width + Distance.Width;
                }

                overallSize = SizeI.Empty;

                AddWalk(item, args.siblings, args.deepwalk);

                ArrangeRows(ref location, args.siblings);

                args.location = location;

                //**** end ArrangeExpand
            }




            //**** PointI Arrange(TItem start, ICollection<TItem> siblings, PointI location) {
            foreach (TItem item in args.items) {
                proxy.Justify (item);

                if (!( item is TEdge ))
                    proxy.SetLocation (item, args.location);

                if ( args.siblings.Contains (item) )
                    AddToRow (0, item);

                PointI location = args.location;
                ArrangeRows (ref location, args.siblings);
                args.location = location;

                foreach (TEdge edge in this.Graph.Twig (item)) {
                    proxy.AffectedEdges.Add (edge);
                }
            }
        }

        #endregion

        protected ShapeProxy<TData, TItem, TEdge> proxy = null;
        
        public virtual void Commit() {
            proxy.Commit(this.Data);
        }

        #region Deprecated
        protected virtual void AdjustRowBounds(PointI startAt) {
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

                AdjustRowSize(row);

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

        protected virtual Pair<PointI, PointI> StripeLocactions(RectangleI stripe, Row<TItem> row, ICollection<TItem> ignoring) {
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
                if (!(widget is TEdge) && !ignoring.Contains(widget)) {
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

            return new Pair<PointI, PointI>(locateLeft, locateRight);
        }

        protected void LatestFreeRowCollissionResolver(Row<TItem> row, ICollection<TItem> ignoring) {
            RectangleI stripe = Data.Shape.BoundsRect;

            var stripeLocation = StripeLocactions(stripe, row, ignoring);

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

        #endregion
    }
}