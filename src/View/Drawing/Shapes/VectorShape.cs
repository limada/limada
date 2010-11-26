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
using System.Drawing;
using System.Drawing.Drawing2D;
using Limaki.Drawing;

namespace Limaki.Drawing.Shapes {
    public interface IEdgeShape {
        Point Start { get;set;}
        Point End  { get;set;}
    }
    public class VectorShape : Shape<Vector>, IEdgeShape {


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
                        int _dataStartX = _data.Start.X;
                        int _dataStartY = _data.Start.Y;
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
                int _dataStartX = _data.Start.X;
                int _dataStartY = _data.Start.Y;
                int _dataEndX = _data.End.X;
                int _dataEndY = _data.End.Y;
                if (_dataStartX > _dataEndX) {
                    int x = _dataStartX;
                    _dataStartX = _dataEndX;
                    _dataEndX = x;
                }
                if (_dataStartY > _dataEndY) {
                    int start = _dataStartY;
                    _dataStartY = _dataEndY;
                    _dataEndY = start;
                }
                int _width = _dataEndX - _dataStartX;
                int _heigth = _dataEndY - _dataStartY;
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
            get { return new Size (_data.End.X - _data.Start.X, _data.End.Y - _data.Start.Y); }
            set { _data.End = Point.Add (_data.Start, value); }
        }



        public override void Transform(Matrice matrice) {
            Point[] p = { _data.Start, _data.End };
            matrice.TransformPoints(p);
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
            RectangleF hitRect = new RectangleF(0, 0, hitSize, hitSize);
            float halfWidth = hitRect.Width / 2;
            float halfHeight = hitRect.Height / 2;

            foreach (Anchor anchor in Grips) {
                PointF anchorPoint = this[anchor];
                hitRect.Location = new PointF(anchorPoint.X - halfWidth, anchorPoint.Y - halfHeight);
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

        public override Point[] Hull(Matrice matrix, int delta, bool extend) {
            Vector vector = _data;
            vector.Transform(matrix);
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