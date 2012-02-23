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
using Xwt;

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

        public virtual Point SetShapeGetAnchor(T shape, Anchor i) {
            this._data = shape;
            return this[i];
        }

        public abstract Point this[Anchor i] { get; set; }

        public virtual Anchor IsAnchorHit(Point p, int hitSize) {
            RectangleD hitRect = new RectangleD(0, 0, hitSize, hitSize);
            double halfWidth = hitRect.Width / 2;
            double halfHeight = hitRect.Height / 2;

            foreach (Anchor anchor in Grips) {
                Point anchorPoint = this[anchor];
                hitRect.Location = new Point(anchorPoint.X - halfWidth, anchorPoint.Y - halfHeight);
                if (hitRect.Contains(p))
                    return anchor;
            }
            return Anchor.None;
        }

        public virtual bool IsBorderHit(Point p, int hitSize) {
            bool result = false;
            RectangleD hitRect = this.BoundsRect;
            int halfSize = hitSize / 2;
            hitRect.Inflate(halfSize, halfSize);
            if (hitRect.Contains(p)) {
                hitRect.Inflate(-hitSize, -hitSize);
                result = !hitRect.Contains(p);

            }
            return result;
        }

        public virtual bool IsHit(Point p, int hitSize) {
            RectangleD hitRect = this.BoundsRect;
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
        public virtual RectangleD BoundsRect {
            get {
                Point lt = this[Anchor.LeftTop];
                Point rb = this[Anchor.RightBottom];
                return RectangleD.FromLTRB(lt.X, lt.Y, rb.X, rb.Y);
            }
        }

        public abstract object Clone();

        public abstract Point Location { get; set;}

        public abstract Size Size { get; set;}

        public virtual IEnumerable<Anchor> Grips {
            get {
                for (Anchor anchor = Anchor.LeftTop; anchor < Anchor.Center; anchor++) {
                    yield return anchor;
                }

            }
        }

        public abstract Point[] Hull ( int delta, bool extend );

        public abstract Point[] Hull ( Matrice matrix, int delta, bool extend );

        #endregion


    }
}