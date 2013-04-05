/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Xwt;
using Xwt.Drawing;

namespace Limaki.Drawing.Shapes {

#if ! SILVERLIGHT    
    [Serializable]
#endif
    public class BezierRectangleShape : RectangleShapeBase, IBezierRectangleShape {

        public BezierRectangleShape():base() {}
        public BezierRectangleShape(Rectangle data):base(data) {}
        public BezierRectangleShape(Point location, Size size) {
            this.Location = location;
            this.Size = size;
        }

        public override Rectangle BoundsRect {
            get {
                return DrawingExtensions.Inflate(Data, _offset, _offset);
            }
        }

        public override Point Location {
            get { return new Point(_data.X - _offset, _data.Y - _offset); }
            set { this._data.Location = new Point(value.X + _offset, value.Y + _offset); }
        }

        public override Size Size {
            get { return new Size(_data.Size.Width + _offset * 2, _data.Size.Height + _offset * 2); }
            set { _data.Size = new Size(value.Width - _offset * 2, value.Height - _offset * 2); }
        }

        public override Point this[Anchor i] {
            get {
                Rectangle _data = this.BoundsRect;
                switch (i) {
                    case Anchor.LeftTop:
                    case Anchor.MostLeft:
                    case Anchor.MostTop:
                        return _data.Location;

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
                        return new Point(_data.X, _data.Y);
                }

            }

            set { }
        }

        public override Point[] Hull(int delta, bool extend) {
            return Hull( BoundsRect,delta,extend);
        }

        public override Point[] Hull(Matrix matrix, int delta, bool extend) {
            var _data = BoundsRect;
            var dataX = _data.X; var dataY = _data.Y;
            Point[] p = { new Point(dataX, dataY), new Point(dataX + _data.Width, dataY + _data.Height) };

            matrix.Transform(p);
            return Hull( Rectangle.FromLTRB(p[0].X, p[0].Y, p[1].X, p[1].Y), delta, extend);
        }

        private double _offset = 5d;
        public double Offset {
            get { return _offset; }
            set { _offset = value; }
        }

        public Point[] BezierPoints {
            get { return BezierExtensions.GetRoundedRectBezier(this.Data, _offset); }
        }

        public override object Clone() {
            return new BezierRectangleShape(_data) { Offset = _offset };
        }
    }
}