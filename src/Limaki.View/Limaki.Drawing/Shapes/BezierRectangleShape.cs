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
            get { return _location; }
            set {
                if (Location != value) {
                    _location = value;
                    _data.Location = new Point (value.X + Offset.Width, value.Y + Offset.Height);
                }
            }
        }
                
        public override Size Size {
            get { return new Size(_data.Size.Width + Offset.Width * 2, _data.Size.Height + Offset.Height * 2); }
            set {
                if (Size != value) {
                    _data.Size = _data.Size - (Size - value);
                    _offset = null;
                    _data.Size = new Size (value.Width - Offset.Width*2, value.Height - Offset.Height*2);
                }
            }
        }

        public override Size DataSize {
            get { return base.DataSize; }
            set {
                if (DataSize != value) {
                    _offset = null;
                    base.DataSize = value;
                }
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
                var boundsRect = this.BoundsRect;
                switch (i) {
                    case Anchor.LeftTop:
                    case Anchor.MostLeft:
                    case Anchor.MostTop:
                        return boundsRect.Location;

                    case Anchor.LeftBottom:
                        return new Point(
                            boundsRect.X,
                            boundsRect.Y + boundsRect.Height);

                    case Anchor.RightTop:
                    case Anchor.MostRight:
                        return new Point(
                            boundsRect.X + boundsRect.Width,
                            boundsRect.Y);

                    case Anchor.RightBottom:
                    case Anchor.MostBottom:
                        return new Point(
                            boundsRect.X + boundsRect.Width,
                            boundsRect.Y + boundsRect.Height
                            );

                    case Anchor.MiddleTop:
                        return new Point(
                            boundsRect.X + boundsRect.Width / 2,
                            boundsRect.Y);

                    case Anchor.LeftMiddle:
                        return new Point(
                            boundsRect.X,
                            boundsRect.Y + boundsRect.Height / 2);

                    case Anchor.RightMiddle:
                        return new Point(
                            boundsRect.X + boundsRect.Width,
                            boundsRect.Y + boundsRect.Height / 2);

                    case Anchor.MiddleBottom:
                        return new Point(
                            boundsRect.X + boundsRect.Width / 2,
                            boundsRect.Y + boundsRect.Height);

                    case Anchor.Center:
                        return new Point(
                            boundsRect.X + boundsRect.Width / 2,
                            boundsRect.Y + boundsRect.Height / 2);

                    default:
                        return new Point(boundsRect.X, boundsRect.Y);
                }

            }

            set { }
        }

        public override Point[] Hull(int delta, bool extend) {
            return Hull( BoundsRect,delta,extend);
        }

        public override Point[] Hull(Matrix matrix, int delta, bool extend) {
            var boundsRect = BoundsRect;
            var dataX = boundsRect.X; var dataY = boundsRect.Y;
            Point[] p = { new Point(dataX, dataY), new Point(dataX + boundsRect.Width, dataY + boundsRect.Height) };
            if (matrix != null && !matrix.IsIdentity)
                matrix.Transform (p);
            return Hull( Rectangle.FromLTRB(p[0].X, p[0].Y, p[1].X, p[1].Y), delta, extend);
        }

        protected double _jitter = 0;
        public double Jitter {
            get { return _jitter; }
            set {
                _jitter = value;
                _offset = null;
            }
        }

        Size CalculateOffset (Rectangle data) {
            var points = BezierExtensions.GetRoundedRectBezier (data, Jitter);
            var bb = BezierExtensions.BezierBoundingBox (points);
            return new Size ((bb.Size.Width - data.Size.Width) / 2, (bb.Size.Height - data.Size.Height) / 2);
        }

        protected Size? _offset = null;
        public Size Offset {
            get {
                if (_offset == null) {
                    if(_data.IsEmpty) {
                        _offset = Size.Zero;
                    } else {
                        _offset = CalculateOffset (this.Data);
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