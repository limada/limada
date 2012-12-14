using System.Collections.Generic;
using Limaki.Drawing;
using Xwt;
using System;

namespace Limaki.Drawing.Indexing {

    public abstract class SpatialIndex<TItem> : ISpatialIndex<TItem> {

        public Func<TItem, Rectangle> BoundsOf  { get;set; }
        public Func<TItem, bool> HasBounds  { get; set; }

        private bool _boundsDirty = true;
        public bool BoundsDirty {
            get { return _boundsDirty; }
            set { _boundsDirty = value; }
        }

        private Rectangle _bounds = default(Rectangle);
        public Rectangle Bounds {
            get {
                if (BoundsDirty) {
                    _bounds = CalculateBounds();
                    _boundsDirty = false;
                }
                return _bounds;
            }
            set { _bounds = value; }
        }

        public virtual void Update (Rectangle invalid, TItem item) {
            if (HasBounds(item)) {
                var bounds = BoundsOf(item);
                if (!invalid.Equals(bounds)) {
                    Remove(invalid, item);
                    Add(bounds, item);
                    checkBoundsRemove(ref invalid);
                    checkBoundsAdd(ref bounds);

                }
            } else {
                Remove(invalid, item);
                checkBoundsRemove(ref invalid);
            }

        }

        protected virtual void checkBoundsAdd (ref Rectangle bounds) {
            if (!_boundsDirty) {
                if (bounds.Bottom > this._bounds.Bottom ||
                    bounds.Right > this._bounds.Right ||
                    bounds.X < this._bounds.X ||
                    bounds.Y < this._bounds.Y
                    )
                    BoundsDirty = true;
            }
        }

        public virtual void Add (TItem item) {
            if (HasBounds(item)) {
                var bounds = BoundsOf(item);
                Add(bounds, item);
                checkBoundsAdd(ref bounds);
            }
        }

        protected abstract void Add (Rectangle bounds, TItem item);

        protected virtual void checkBoundsRemove (ref Rectangle bounds) {
            if (!_boundsDirty) {
                if (bounds.Bottom >= this._bounds.Bottom ||
                    bounds.Right >= this._bounds.Right ||
                    bounds.X <= this._bounds.X ||
                    bounds.Y <= this._bounds.Y)
                    BoundsDirty = true;
            }
        }

        public virtual void Remove (TItem item) {
            if (HasBounds(item)) {
                var bounds = BoundsOf(item);
                checkBoundsRemove(ref bounds);
                Remove(bounds, item);
            }
        }

        protected abstract void Remove (Rectangle bounds, TItem item);

        public virtual void AddRange (IEnumerable<TItem> items) {
            var r = _bounds.Right;
            var b = _bounds.Bottom;
            var l = _bounds.Left;
            var t = _bounds.Top;
            foreach (var visual in items) {
                var bounds = HasBounds(visual) ? BoundsOf(visual) : Rectangle.Zero;
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

            _bounds = Rectangle.FromLTRB(l, t, r, b);
            _boundsDirty = false;
        }

        protected abstract Rectangle CalculateBounds ();

        public abstract IEnumerable<TItem> Query (Rectangle clipBounds);


        public abstract IEnumerable<TItem> Query ();

        public abstract void Clear ();

    }
}