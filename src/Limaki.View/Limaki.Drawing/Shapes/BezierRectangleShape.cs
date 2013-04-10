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

        public BezierRectangleShape() {
            Jitter = 5d;
        }
        
        public BezierRectangleShape(Rectangle data) {
            this.Data = data.NormalizedRectangle();
        }

        public BezierRectangleShape(Point location, Size size) {
            this.Location = location;
            this.Size = size;
        }

        public override Rectangle BoundsRect {
            get {
                return Data.Inflate(Offset);
            }
        }

        public override Point Location {
            get { return new Point(_data.X - Offset.Width, _data.Y - Offset.Height); }
            set {
                _offset = null;
                this._data.Location = new Point(value.X + Offset.Width, value.Y + Offset.Height);
            }
        }

        public override Size Size {
            get { return new Size(_data.Size.Width + Offset.Width * 2, _data.Size.Height + Offset.Height * 2); }
            set {
                _offset = null;
                _data.Size = new Size(value.Width - Offset.Width * 2, value.Height - Offset.Height * 2);
            }
        }

        public override Size DataSize {
            get { return base.DataSize; }
            set {
                _offset = null;
                base.DataSize = value;
            }
        }

        public override Rectangle Data {
            get { return base.Data; }
            set {
                _offset = null;
                base.Data = value;
            }
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

        protected double _jitter = 0;
        public double Jitter {
            get { return _jitter; }
            set { _jitter = value;
            _offset = null;
            }
        }

        protected Size? _offset = null;
        public Size Offset {
            get {
                if (_offset == null) {
                    if(_data.IsEmpty) {
                        _offset = Size.Zero;
                    } else {
                        var bb = BezierExtensions.BezierBoundingBox(this.BezierPoints);
                        _offset = new Size((bb.Size.Width - Data.Size.Width)/2, (bb.Size.Height - Data.Size.Height)/2);
                    }
                }
                return _offset.Value;
            }
        }

        public Point[] BezierPoints {
            get { return BezierExtensions.GetRoundedRectBezier(this.Data, Jitter); }
        }

        public override object Clone() {
            return new BezierRectangleShape(_data) { Jitter = this.Jitter };
        }
    }
}