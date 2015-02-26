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
 * http://www.limada.org
 * 
 */

using System;
using Xwt;
using Xwt.Drawing;

namespace Limaki.Drawing.Shapes {
    public interface IEdgeShape {
        Point Start { get;set;}
        Point End  { get;set;}
    }

    
#if ! SILVERLIGHT
    [Serializable]
#endif
    public class VectorShape : Shape<Vector>, IVectorShape,IEdgeShape {


        public VectorShape():base() {}
        public VectorShape(Vector data) : base(data) { }


        public override Point this[Anchor i] {
            get {
                switch (i) {
                    case Anchor.LeftTop:
                    case Anchor.MostLeft:
                    case Anchor.MostTop:
                    case Anchor.RightTop:
                    case Anchor.MiddleTop:
                    case Anchor.LeftBottom:
                    case Anchor.LeftMiddle:
                        return _data.Start;
                    case Anchor.MostRight:
                    case Anchor.RightBottom:
                    case Anchor.MostBottom:
                    case Anchor.MiddleBottom:
                    case Anchor.RightMiddle:
                        return _data.End;
                    case Anchor.Center:
                        var _dataStartX = _data.Start.X;
                        var _dataStartY = _data.Start.Y;
                        return new Point(
                            (_dataStartX + (_data.End.X - _dataStartX) / 2),
                            (_dataStartY + (_data.End.Y - _dataStartY) / 2));
                    default:
                        return _data.Start;
                }
                
            }

            set { }
        }

        
        public override Rectangle BoundsRect {
            get {
                var _dataStartX = _data.Start.X;
                var _dataStartY = _data.Start.Y;
                var _dataEndX = _data.End.X;
                var _dataEndY = _data.End.Y;
                if (_dataStartX > _dataEndX) {
                    var x = _dataStartX;
                    _dataStartX = _dataEndX;
                    _dataEndX = x;
                }
                if (_dataStartY > _dataEndY) {
                    var start = _dataStartY;
                    _dataStartY = _dataEndY;
                    _dataEndY = start;
                }
                var _width = _dataEndX - _dataStartX;
                var _heigth = _dataEndY - _dataStartY;
                if (_width ==0) _width = 1;
                if (_heigth==0) _heigth = 1;

                return new Rectangle(_dataStartX, _dataStartY, _width, _heigth);
            }
        }

        public override Point Location {
            get { return this._data.Start; }
            set {
                Size oldSize = this.Size;
                this._data.Start = value;
                Size = oldSize;
            }
        }

        public override Size Size {
            get { return DataSize; }
            set { DataSize = value; }
        }
        public override Size DataSize {
            get { return new Size (_data.End.X - _data.Start.X, _data.End.Y - _data.Start.Y); }
            set { _data.End = _data.Start + value; }
        }



        public override void Transform(Matrix matrix) {
            Point[] p = { _data.Start, _data.End };
            matrix.Transform(p);
            _data.Start = p[0];
            _data.End = p[1];
        }




        public override object Clone() {
            return new VectorShape(_data);
        }
        public override System.Collections.Generic.IEnumerable<Anchor> Grips {
            get {
                yield return Anchor.LeftTop;
                yield return Anchor.Center;
                yield return Anchor.RightBottom;

            }
        }
        public override Anchor IsAnchorHit(Point p, int hitSize) {
            // TODO: first ask, if p is in boundsRect
            Rectangle hitRect = new Rectangle(0, 0, hitSize, hitSize);
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

        public override bool IsBorderHit(Point p, int hitSize) {
            return IsHit(p,hitSize);
        }

        public override bool IsHit(Point p, int hitSize) {
            return Polygon.Intersect (p,_data.Hull(hitSize / 2, false));

        }

        public override Point[] Hull(int delta, bool extend) {
            return _data.Hull(delta,extend);
        }

        public override Point[] Hull(Matrix matrix, int delta, bool extend) {
            var vector = _data;
            if (matrix != null && !matrix.IsIdentity)
                vector.Transform (matrix);
            return vector.Hull(delta, extend);
        }

        #region ILinkShape Member

        public Point Start {
            get {return _data.Start;}
            set { _data.Start = value; }
        }

        public Point End {
            get { return _data.End; }
            set { _data.End = value; }
        }

        #endregion
    }
}