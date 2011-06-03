/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Limaki.Common;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;

namespace Limaki.Presenter.Layout {
    /// <summary>
    /// Arranger is a Unit of Work to arrange items
    /// be carefull: create a new arranger for every unit of work
    /// the Items have valid data AFTER commiting scene.Commands
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    public class Arranger<TItem, TEdge>
        where TEdge : IEdge<TItem>, TItem {

        public Arranger(IGraphScene<TItem, TEdge> data, IGraphLayout<TItem, TEdge> layout) {
            this.Data = data;
            this.Layout = layout;
            this.RowCollisionResolver = FirstFreeRowCollissionResolver;
            this.OrderBy = (item) => item.ToString();
        }

        public bool Adjust = false;

        public IGraphScene<TItem, TEdge> Data { get; protected set; }
        public IGraph<TItem, TEdge> Graph {
            get { return Data.Graph; }
        }

        public SizeI AutoSize { get; set; }
        public bool Centered { get; set; }
        public SizeI Distance { get; set; }
        public SizeI Border { get; set; }
        public Orientation Orientation { get; set; }
        public virtual Func<TItem, string> OrderBy { get; set; }

        private IGraphLayout<TItem, TEdge> _layout = null;
        public IGraphLayout<TItem, TEdge> Layout {
            get { return _layout; }
            protected set {
                if (_layout != value) {
                    _layout = value;
                    this.Distance = _layout.Distance;
                    this.Border = _layout.Border;
                    this.Orientation = _layout.Orientation;
                    this.Centered = _layout.Centered;
                    this.AutoSize = _layout.StyleSheet.BaseStyle.AutoSize;
                }
            }
        }

        protected IShapeProxy<TItem, TEdge> _proxy = null;
        public IShapeProxy<TItem, TEdge> Proxy {
            get { return _proxy ?? (_proxy = CreateProxy(this.Layout)); }
            set { _proxy = value; }
        }

        protected virtual IShapeProxy<TItem, TEdge> CreateProxy(IGraphLayout<TItem, TEdge> layout) {
            return new GraphItemShapeProxy<TItem, TEdge>(layout);
        }


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
        /// adds item to row[level]
        /// </summary>
        /// <param name="level"></param>
        /// <param name="item"></param>
        protected virtual void AddToRow(int level, TItem item) {
            if ((item != null) && !(item is TEdge)) {
                if (!RowIndices.ContainsKey(item)) {
                    Row<TItem> row = null;
                    if (!rows.TryGetValue(level, out row)) {
                        row = new Row<TItem>();
                        rows.Add(level, row);
                    }
                    row.Items.Add((TItem)item);
                    RowIndices.Add(item, level);
                }
            }
        }
        protected virtual void AdjustRowSize(Row<TItem> row) {
            if (!row.SizeAdjusted) {
                foreach (TItem item in row.Items) {
                    if (!(item is TEdge)) {
                        SizeI visualSize = Proxy.GetSize(item);
                        row.Size += new SizeI(visualSize.Width + Distance.Width,
                                               visualSize.Height + Distance.Height);

                        row.biggestItemSize = new SizeI(Math.Max(row.biggestItemSize.Width, visualSize.Width),
                                                         Math.Max(row.biggestItemSize.Height, visualSize.Height));
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
                    row.Location.X = location.X - (row.Size.Width / 2) + (Distance.Width / 2);
                } else if (Orientation == Orientation.LeftRight) {
                    row.Location.Y = location.Y - (row.Size.Height / 2) + (Distance.Height / 2);
                }
            }
        }



        protected void FirstFreeRowCollissionResolver(Row<TItem> row, ICollection<TItem> ignoring) {
            bool collission = true;
            RectangleI stripe = new RectangleI(row.Location, row.Size);
            var startStripe = stripe;

            Func<RectangleI, int> orderByR = r => r.Right;
            Func<RectangleI, int> orderByB = r => r.Bottom;
            Func<RectangleI, int> orderBy = orderByR;
            Action<RectangleI> moveDown = (r) => { stripe.Y = r.Bottom + Distance.Height; };
            Action<RectangleI> moveRight = (r) => { stripe.X = r.Right + Distance.Width; };

            if (Orientation == Orientation.LeftRight) {
                stripe.Height = stripe.Height - Distance.Height;
                orderBy = orderByR;
            } else {
                stripe.Width = stripe.Width - Distance.Width;
                orderBy = orderByB;
            }

            bool collissionDetected = false;
            int max = 100;
            while (collission && max>0) {
                max--;
                collission = false;
                //stripe.X = stripe.X - Layout.Distance.Width+1;
                var l = from item in Data.ElementsIn(stripe)
                        where !(item is TEdge) && !ignoring.Contains(item)
                        select new RectangleI(Proxy.GetLocation(item), Proxy.GetSize(item));
                l = l.OrderByDescending(orderBy);

                foreach (var bounds in l) {
                    if (Orientation == Orientation.LeftRight) {
                        if (bounds.X < stripe.X || bounds.Right > stripe.Right) {
                            if (stripe.X < startStripe.Bottom) {
                                moveDown(bounds);
                                orderBy = orderByB;
                            } else {
                                moveRight(bounds);
                                orderBy = orderByB;
                            }
                        } else {
                            moveDown(bounds);
                            orderBy = orderByR;
                        }
                    } else {
                        if (bounds.Y < stripe.Y || bounds.Bottom > stripe.Bottom) {
                            //if (stripe.Y < startStripe.Right) {
                            //    moveRight(bounds);
                            //    orderBy = orderByR;
                            //} 
                            //else {
                            moveDown(bounds);
                            orderBy = orderByR;
                            //}
                        } else {
                            moveRight(bounds);
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
                var row = kvp.Value;
                row.Items = row.Items.OrderBy<TItem, string>(this.OrderBy).ToList();
                AdjustRowSize(row);
                AdjustRowLocation(row, rowStart);

                if (siblings != null && RowCollisionResolver != null) {
                    RowCollisionResolver(row, ignore);
                }


                var location = row.Location;
                foreach (TItem item in row.Items)
                    if (!(item is TEdge)) {

                        Proxy.SetLocation(item, location);
                        if (siblings != null)
                            ignore.Remove(item);

                        if (siblings == null)
                            Proxy.Justify(item);


                        foreach (TEdge edge in this.Graph.Twig(item)) {
                            Proxy.AffectedEdges.Add(edge);
                        }

                        SizeI size = new SizeI(
                            Proxy.GetSize(item).Width + Distance.Width,
                            Proxy.GetSize(item).Height + Distance.Height);

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

            IEnumerable<LevelItem<TItem>> walk = null;
            if (deep) {
                walker.visited = this.visited;
                walk = walker.DeepWalk(start, 0);
            } else {
                walk = walker.Walk(start, 0);
                this.visited.AddRange(walker.visited);
            }

            foreach (LevelItem<TItem> item in walk) {
                //Justify(item.Node);
                if (siblings != null) {
                    if (!item.Node.Equals(start) && siblings.Contains(item.Node)) {
                        AddToRow(item.Level, item.Node);
                    }
                } else {
                    AddToRow(item.Level, item.Node);
                }
                if (false)
                    if (item.Node is TEdge) {
                        TEdge edge = (TEdge)item.Node;
                        Proxy.AffectedEdges.Add(edge);
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

            Proxy.Justify(start);

            if (!(start is TEdge))
                Proxy.SetLocation(start, location);

            if (siblings.Contains(start))
                AddToRow(0, start);

            ArrangeRows(ref location, siblings);

            foreach (TEdge edge in this.Graph.Twig(start)) {
                Proxy.AffectedEdges.Add(edge);
            }

            return location;
        }


        public virtual PointI Arrange(IEnumerable<TItem> items, ICollection<TItem> siblings, bool deep) {
            PointI result = (PointI)Border;

            foreach (TItem item in items) {
                result = ArrangeExpand(item, siblings, deep);
            }
            ArrangeEdges (items, true);
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
            PointI startAt = (PointI)Border;
            TItem root = start;
            while (root is TEdge) {
                root = ((TEdge)root).Root;
            }

            startAt = Proxy.GetShape(root).Location;

            if (siblings != null && siblings.Count == 1 && siblings.Contains(start)) {
                startAt = (PointI)Border;
                Proxy.SetLocation(start, startAt);
            }

            var startSize = Proxy.GetSize(root);


            if (Orientation == Orientation.TopBottom) {
                if (Centered)
                    startAt.X += startSize.Width / 2;
                startAt.Y = startAt.Y + startSize.Height;

            } else if (Orientation == Orientation.LeftRight) {
                if (Centered)
                    startAt.Y += startSize.Height / 2;
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

            Proxy.GetShape(start);

            AddWalk(start, null, true);

            if (Centered) {
                var size = new SizeI();
                foreach (var kvp in rows) {
                    var row = kvp.Value;
                    AdjustRowSize(row);
                    size.Height = Math.Max(size.Height, row.Size.Height);
                    size.Width = Math.Max(size.Width, row.Size.Width);
                }
                if (Orientation == Orientation.LeftRight) {
                    size.Height -= Distance.Height;
                    startAt.Y += size.Height / 2;
                }
            }

            Proxy.SetLocation(start, startAt);
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
                        Proxy.EnsureInvoke(item);
                    else
                        Proxy.Justify(item);


                    if (!visited.Contains(item)) {
                        location = new PointI(Distance.Width, location.Y);
                        location = ArrangeDeepWalk(item, location);
                        ClearRows();
                    }

                }
            }
        }
        public virtual void ArrangeItems(IEnumerable<TItem> items, bool justify, PointI location) {
            var startAt = location;
            var firstRow = true;
            foreach (TItem item in items) {
                if (!(item is TEdge)) {
                    if (!justify)
                        Proxy.EnsureInvoke(item);
                    else
                        Proxy.Justify(item);

                    if (!visited.Contains(item)) {
                        location = new PointI(Distance.Width, location.Y);
                        location = ArrangeDeepWalk(item, location);
                        
                        Proxy.GetShape(item);

                        AddWalk(item, null, true);

                        if (Centered) {
                            var size = new SizeI();
                            foreach (var kvp in rows) {
                                var row = kvp.Value;
                                AdjustRowSize(row);
                                size.Height = Math.Max(size.Height, row.Size.Height);
                                size.Width = Math.Max(size.Width, row.Size.Width);
                            }
                            if (Orientation == Orientation.LeftRight) {
                                size.Height -= Distance.Height;
                                location.Y += size.Height / 2;
                                if (firstRow) {
                                    startAt.Y += size.Height / 2;
                                    firstRow = false;
                                }
                            }
                        }

                        Proxy.SetLocation(item, location);
                        
                    }

                }
            }
            ArrangeRows(ref startAt, null);
        }

        /// <summary>
        /// invokes (if neccessary ) or justifies all items
        /// adds the twig of each item to affectedEdges
        /// </summary>
        /// <param name="items"></param>
        /// <param name="justify"></param>
        public virtual void ArrangeEdges(IEnumerable<TItem> items, bool justify) {
            foreach (TItem item in items) {
                if (!(item is TEdge)) {
                    if (justify)
                        Proxy.Justify(item);
                    else
                        Proxy.EnsureInvoke(item);

                    foreach (TEdge edge in this.Graph.Twig(item)) {
                        Proxy.AffectedEdges.Add(edge);
                    }
                }
            }
        }

        #region Refactoring
        public class ArrangeArgs<TItem> {
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
                        Proxy.EnsureInvoke(item);
                    else
                        Proxy.Justify(item);


                    if (!visited.Contains(item)) {
                        args.location = new PointI(Distance.Width, args.location.Y);

                        //**** PointI ArrangeDeepWalk(TItem start, PointI location) {
                        PointI location = args.location;

                        Proxy.GetShape(item);
                        Proxy.SetLocation(item, location);

                        AddWalk(item, null, true);

                        ArrangeRows(ref location, null);

                        args.location = location;

                        //**** end ArrangeDeepWalk

                        ClearRows();
                    }

                }
            }

            //**** PointI Arrange(IEnumerable<TItem> items, ICollection<TItem> siblings, bool deep)

            args.location = (PointI)Border;
            foreach (TItem item in args.items) {

                //**** PointI ArrangeExpand(TItem start, ICollection<TItem> siblings, bool deep) {
                PointI location = (PointI)Border;
                TItem root = item;
                while (root is TEdge) {
                    root = ((TEdge)root).Root;
                }

                IShape shape = Proxy.GetShape(root);
                location = shape.Location;

                if (args.siblings != null && args.siblings.Count == 1 && args.siblings.Contains(item)) {
                    location = (PointI)Border;
                    Proxy.SetLocation(item, location);
                }

                if (Orientation == Orientation.TopBottom) {
                    location.Y = location.Y + Proxy.GetSize(root).Height;
                } else if (Orientation == Orientation.LeftRight) {
                    location.X = location.X + Proxy.GetSize(root).Width + Distance.Width;
                }

                overallSize = SizeI.Empty;

                AddWalk(item, args.siblings, args.deepwalk);

                ArrangeRows(ref location, args.siblings);

                args.location = location;

                //**** end ArrangeExpand
            }




            //**** PointI Arrange(TItem start, ICollection<TItem> siblings, PointI location) {
            foreach (TItem item in args.items) {
                Proxy.Justify(item);

                if (!(item is TEdge))
                    Proxy.SetLocation(item, args.location);

                if (args.siblings.Contains(item))
                    AddToRow(0, item);

                PointI location = args.location;
                ArrangeRows(ref location, args.siblings);
                args.location = location;

                foreach (TEdge edge in this.Graph.Twig(item)) {
                    Proxy.AffectedEdges.Add(edge);
                }
            }
        }

        #endregion



        public virtual void Commit() {
            Proxy.Commit(this.Data);
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
            var locateRight = new PointI();
            var locateLeft = new PointI(int.MaxValue, int.MaxValue);
            foreach (TItem item in Data.ElementsIn(stripe)) {
                if (!(item is TEdge) && !ignoring.Contains(item)) {
                    var visualLocation = Proxy.GetLocation(item);
                    var visualSize = Proxy.GetSize(item);

                    locateRight.X = Math.Max(locateRight.X, visualLocation.X + visualSize.Width);
                    locateRight.Y = Math.Max(locateRight.Y, visualLocation.Y + visualSize.Height);
                    locateLeft.X = Math.Min(locateLeft.X, visualLocation.X);
                    locateLeft.Y = Math.Min(locateLeft.Y, visualLocation.Y);
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