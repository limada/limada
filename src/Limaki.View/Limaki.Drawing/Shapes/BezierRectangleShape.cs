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

#if ! SILVERLIGHT    
    [Serializable]
#endif
    public class BezierRectangleShape : RectangleShapeBase, IBezierRectangleShape {

        public BezierRectangleShape():base() {}
        public BezierRectangleShape(Rectangle data):base(data) {}
        public BezierRectangleShape(Point location, Size size) : base(location,size) { }

        public override Rectangle BoundsRect {
            get {
                return DrawingExtensions.Inflate(Data,(int)_offset+1,(int)_offset+1);
            }
        }

        public override Point this[Anchor i] {
            get {
                Rectangle _data = this.BoundsRect;
                switch (i) {
                    case Anchor.LeftTop:
                    case Anchor.MostLeft:
                    case Anchor.MostTop:
                        return new Point(_data.X, _data.Y);

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
            return new BezierRectangleShape(_data);
        }
    }
}