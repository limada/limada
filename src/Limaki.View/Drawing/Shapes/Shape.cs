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

namespace Limaki.Drawing.Shapes {
#if !SILVERLIGHT
    [Serializable]
#endif
    public abstract class Shape<T> : IShape<T> {
        protected T _data;
        public virtual T Data {
            get { return _data; }
            set { _data = value; }
        }
        public virtual Type ShapeDataType {
            get { return typeof(T); }
        }

        public Shape() {
            _data = default(T);
        }

        public Shape(T data): this() {
            this._data = data;
        }

        public virtual PointI SetShapeGetAnchor(T shape, Anchor i) {
            this._data = shape;
            return this[i];
        }

        public abstract PointI this[Anchor i] { get; set; }

        public virtual Anchor IsAnchorHit(PointI p, int hitSize) {
            RectangleS hitRect = new RectangleS(0, 0, hitSize, hitSize);
            float halfWidth = hitRect.Width / 2;
            float halfHeight = hitRect.Height / 2;

            foreach (Anchor anchor in Grips) {
                PointS anchorPoint = this[anchor];
                hitRect.Location = new PointS(anchorPoint.X - halfWidth, anchorPoint.Y - halfHeight);
                if (hitRect.Contains(p))
                    return anchor;
            }
            return Anchor.None;
        }

        public virtual bool IsBorderHit(PointI p, int hitSize) {
            bool result = false;
            RectangleI hitRect = this.BoundsRect;
            int halfSize = hitSize / 2;
            hitRect.Inflate(halfSize, halfSize);
            if (hitRect.Contains(p)) {
                hitRect.Inflate(-hitSize, -hitSize);
                result = !hitRect.Contains(p);

            }
            return result;
        }

        public virtual bool IsHit(PointI p, int hitSize) {
            RectangleI hitRect = this.BoundsRect;
            int halfSize = hitSize / 2;
            hitRect.Inflate(halfSize, halfSize);
            return hitRect.Contains(p);
        }

        #region IDisposable Member
        public virtual void Dispose(bool disposing) { }
        public virtual void Dispose() {
            Dispose(true);
        }

        #endregion

        #region IShape Member
        public abstract void Transform(Matrice matrice);
        public virtual RectangleI BoundsRect {
            get {
                PointI lt = this[Anchor.LeftTop];
                PointI rb = this[Anchor.RightBottom];
                return RectangleI.FromLTRB(lt.X, lt.Y, rb.X, rb.Y);
            }
        }

        public abstract object Clone();

        public abstract PointI Location { get; set;}

        public abstract SizeI Size { get; set;}

        public virtual IEnumerable<Anchor> Grips {
            get {
                for (Anchor anchor = Anchor.LeftTop; anchor < Anchor.Center; anchor++) {
                    yield return anchor;
                }

            }
        }

        public abstract PointI[] Hull ( int delta, bool extend );

        public abstract PointI[] Hull ( Matrice matrix, int delta, bool extend );
        #endregion


    }
}