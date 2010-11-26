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
using Limaki.Drawing;

namespace Limaki.Widgets {
    public abstract class SpatialIndex {
        private bool _boundsDirty = true;
        public bool BoundsDirty {
            get { return _boundsDirty; }
            set { _boundsDirty = value; }
        }

        private RectangleI _bounds = default( RectangleI );
        public RectangleI Bounds {
            get {
                if (BoundsDirty) {
                    _bounds = CalculateBounds ();
                    _boundsDirty = false;
                }
                return _bounds;
            }
            set { _bounds = value; }
        }

        public static RectangleI GetRectangle(IWidget widget) {
            if (widget.Shape != null)
                return widget.Shape.BoundsRect;
            else
                return RectangleI.Empty;
        }

        public virtual void Update ( RectangleI invalid, IWidget widget ) {
            if (!(widget is IToolWidget)) {
                if (widget.Shape != null) {
                    RectangleI bounds = widget.Shape.BoundsRect;
                    if (!invalid.Equals(bounds)) {
                        Remove(invalid, widget);
                        Add(bounds, widget);
                        checkBoundsRemove(ref invalid);
                        checkBoundsAdd(ref bounds);

                    }
                } else {
                    Remove(invalid, widget);
                    checkBoundsRemove(ref invalid);
                }
            }
        }

        protected virtual void checkBoundsAdd ( ref RectangleI bounds ) {
            if ( !_boundsDirty ) {
                if ( bounds.Bottom > this._bounds.Bottom ||
                    bounds.Right > this._bounds.Right )
                    BoundsDirty = true;
            }
        }

        public virtual void Add(IWidget item) {
            if (item.Shape != null) {
                RectangleI bounds = item.Shape.BoundsRect;
                Add (bounds, item);
                checkBoundsAdd(ref bounds);
            }
        }

        protected abstract void Add ( RectangleI bounds, IWidget item );

        protected virtual void checkBoundsRemove ( ref RectangleI bounds ) {
            if ( !_boundsDirty ) {
                if ( bounds.Bottom >= this._bounds.Bottom ||
                    bounds.Right >= this._bounds.Right )
                    BoundsDirty = true;
            }
        }
        public virtual void Remove(IWidget item) {
            if (item.Shape != null) {
                RectangleI bounds = item.Shape.BoundsRect;
                checkBoundsRemove(ref bounds);
                Remove (bounds, item);
            }
        }

        protected abstract void Remove(RectangleI bounds, IWidget item);

        public virtual void Fill(IEnumerable<IWidget> items) {
            int h = 0;
            int w = 0;
            foreach (IWidget widget in items) {
                RectangleI bounds = GetRectangle (widget);
                Add(bounds, widget);
                int r = bounds.Right;
                int b = bounds.Bottom;
                if (r > w) w = r;
                if (b > h) h = b;
            }
            _bounds = new RectangleI (0, 0, w, h);
            _boundsDirty = false;
        }

        protected abstract RectangleI CalculateBounds();

        public abstract IEnumerable<IWidget> Query(RectangleS clipBounds);

        public abstract IEnumerable<IWidget> Query( RectangleS clipBounds, ZOrder zOrder );

        public abstract IEnumerable<IWidget> Query();

        public abstract void Clear();

    }

    public enum ZOrder {
        NodesFirst,
        EdgesFirst
    }
}