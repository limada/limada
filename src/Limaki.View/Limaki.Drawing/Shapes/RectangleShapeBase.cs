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

namespace Limaki.Drawing.Shapes {
#if ! SILVERLIGHT    
    [Serializable]
#endif
    public abstract class RectangleShapeBase : Shape<Rectangle> {
        public RectangleShapeBase():base() {}
        public RectangleShapeBase(Rectangle data):base() {
            this.Data = Rectangle.FromLTRB(
                Math.Min(data.X,data.Right),
                Math.Min(data.Y, data.Bottom),
                Math.Max(data.X, data.Right),
                Math.Max(data.Y, data.Bottom));
        }

        public RectangleShapeBase(Point location, Size size) : base() {
            this.Data = new Rectangle (location, size);
        }

        public override Point this[Anchor i] {
            get {
                switch (i) {
                    case Anchor.LeftTop:
                    case Anchor.MostLeft:
                    case Anchor.MostTop:
                        return new Point(_data.X , _data.Y);
                        
                    case Anchor.LeftBottom:
                        return new Point(
                            _data.X,
                            _data.Y + _data.Height);
                        
                    case Anchor.RightTop:
                    case Anchor.MostRight:
                        return new Point(
                            _data.X + _data.Width,
                            _data.Y);
								
                    case Anchor.RightBottom:
                    case Anchor.MostBottom:
                        return new Point(
                            _data.X + _data.Width,
                            _data.Y + _data.Height
                            );

                    case Anchor.MiddleTop:
                        return new Point(
                            _data.X + _data.Width / 2,
                            _data.Y);

                    case Anchor.LeftMiddle:
                        return new Point(
                            _data.X,
                            _data.Y + _data.Height / 2);

                    case Anchor.RightMiddle:
                        return new Point(
                            _data.X + _data.Width,
                            _data.Y + _data.Height / 2);

                    case Anchor.MiddleBottom:
                        return new Point(
                            _data.X + _data.Width / 2,
                            _data.Y + _data.Height);

                    case Anchor.Center:
                        return new Point(
                            _data.X + _data.Width / 2,
                            _data.Y + _data.Height / 2);

                    default:
                        return new Point(_data.X , _data.Y);
                }

            }

            set {}
        }
        public override Rectangle BoundsRect {
            get { return this._data; }
        }
        public override Size Size {
            get { return _data.Size;}
            set { _data.Size = value; }
        }
        public override Point Location {
            get { return new Point(_data.X , _data.Y); }
            set { this._data.Location = value; }
        }

        public override void Transform(Matrice matrice) {
            var dataX = _data.X;
            var dataY = _data.Y;
            Point[] p = { new Point(dataX, dataY), new Point(dataX + _data.Width, dataY + _data.Height) };
            matrice.TransformPoints(p);
            _data = Rectangle.FromLTRB(p[0].X, p[0].Y, p[1].X, p[1].Y);
        }

        public static Point[] Hull(Rectangle rect, int delta, bool extend) {
            var startX = rect.X;
            var startY = rect.Y;
            var endX = startX+rect.Width;
            var endY = startY+rect.Height;
            return new Point[] {
                                   new Point ((startX - delta), (startY - delta)),
                                   new Point ((endX + delta), (startY - delta)),
                                   new Point((endX + delta), (endY + delta)),
                                   new Point ((startX - delta), (endY + delta))
                               };
        }

        public override Point[] Hull(int delta, bool extend) {
            return Hull(_data,delta, extend);
        }

        public override Point[] Hull(Matrice matrix, int delta, bool extend) {
            var dataX = _data.X; var dataY = _data.Y;
            Point[] p = { new Point(dataX, dataY), new Point(dataX + _data.Width, dataY + _data.Height) };

            matrix.TransformPoints(p);
            return Hull(Rectangle.FromLTRB(p[0].X, p[0].Y, p[1].X, p[1].Y), delta, extend);
        }


    }
}