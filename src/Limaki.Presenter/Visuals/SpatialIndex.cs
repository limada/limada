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
using Limaki.Drawing;

namespace Limaki.Visuals {
    public abstract class SpatialIndex : ISpatialIndex<IVisual> {
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

        public static RectangleI GetRectangle(IVisual visual) {
            if (visual.Shape != null)
                return visual.Shape.BoundsRect;
            else
                return RectangleI.Empty;
        }

        public virtual void Update ( RectangleI invalid, IVisual visual ) {
            if (!(visual is IVisualTool)) {
                if (visual.Shape != null) {
                    RectangleI bounds = visual.Shape.BoundsRect;
                    if (!invalid.Equals(bounds)) {
                        Remove(invalid, visual);
                        Add(bounds, visual);
                        checkBoundsRemove(ref invalid);
                        checkBoundsAdd(ref bounds);

                    }
                } else {
                    Remove(invalid, visual);
                    checkBoundsRemove(ref invalid);
                }
            }
        }

        protected virtual void checkBoundsAdd ( ref RectangleI bounds ) {
            if ( !_boundsDirty ) {
                if ( bounds.Bottom > this._bounds.Bottom ||
                    bounds.Right > this._bounds.Right  ||
                    bounds.X < this._bounds.X ||
                    bounds.Y < this._bounds.Y
                    )
                    BoundsDirty = true;
            }
        }

        public virtual void Add(IVisual item) {
            if (item.Shape != null) {
                RectangleI bounds = item.Shape.BoundsRect;
                Add (bounds, item);
                checkBoundsAdd(ref bounds);
            }
        }

        protected abstract void Add ( RectangleI bounds, IVisual item );

        protected virtual void checkBoundsRemove ( ref RectangleI bounds ) {
            if ( !_boundsDirty ) {
                if ( bounds.Bottom >= this._bounds.Bottom ||
                    bounds.Right >= this._bounds.Right ||
                    bounds.X <= this._bounds.X ||
                    bounds.Y <= this._bounds.Y)
                    BoundsDirty = true;
            }
        }
        public virtual void Remove(IVisual item) {
            if (item.Shape != null) {
                RectangleI bounds = item.Shape.BoundsRect;
                checkBoundsRemove(ref bounds);
                Remove (bounds, item);
            }
        }

        protected abstract void Remove(RectangleI bounds, IVisual item);

        public virtual void AddRange(IEnumerable<IVisual> items) {
            var r = _bounds.Right;
            var b = _bounds.Bottom;
            var l = _bounds.Left;
            var t = _bounds.Top;
            foreach (var visual in items) {
                var bounds = GetRectangle (visual);
                Add(bounds, visual);
                var wr = bounds.Right;
                var wb = bounds.Bottom;
                var wx = bounds.X;
                var wy = bounds.Y;
                if (wr > r) r = wr;
                if (wb > b) b = wb;
                if (wx < l) l = wx;
                if (wy < t) t = wy;
            }
            if (l > 0) l = 0;
            if (t > 0) t = 0;
            if (r < 0) r = 0;
            if (b < 0) b = 0;
                   
            _bounds = RectangleI.FromLTRB (l,t,r,b);
            _boundsDirty = false;
        }

        protected abstract RectangleI CalculateBounds();

        public abstract IEnumerable<IVisual> Query(RectangleS clipBounds);

        public abstract IEnumerable<IVisual> Query( RectangleS clipBounds, ZOrder zOrder );

        public abstract IEnumerable<IVisual> Query();

        public abstract void Clear();

    }


}