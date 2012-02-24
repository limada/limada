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
using Xwt;

namespace Limaki.Drawing.Shapes {

#if ! SILVERLIGHT    
    [Serializable]
#endif
    public class BezierShape : RectangleShapeBase, IBezierShape {
        public BezierShape():base() {}
        public BezierShape(RectangleD data):base(data) {}
        public BezierShape(Point location, Size size) : base(location,size) { }

        public override RectangleD BoundsRect {
            get {
                return DrawingExtensions.Inflate(Data,(int)_offset+1,(int)_offset+1);
            }
        }

        public override Point this[Anchor i] {
            get {
                RectangleD _data = this.BoundsRect;
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

        public override Point[] Hull(Matrice matrix, int delta, bool extend) {
            var _data = BoundsRect;
            var dataX = _data.X; var dataY = _data.Y;
            Point[] p = { new Point(dataX, dataY), new Point(dataX + _data.Width, dataY + _data.Height) };

            matrix.TransformPoints(p);
            return Hull( RectangleD.FromLTRB(p[0].X, p[0].Y, p[1].X, p[1].Y), delta, extend);
        }

        private double _offset = 5d;
        public double Offset {
            get { return _offset; }
            set { _offset = value; }
        }

        public Point[] BezierPoints {
            get { return GetRoundedRectBezier (this.Data, _offset); }
        }

        private Point[] GetRoundedRectBezier(RectangleD rect, double aoffset) {
            double grow = 0d;
            RectangleD r = new RectangleD(rect.X + grow, rect.Y + grow, rect.Width + grow, rect.Height + grow);
            // Create points for curve.
            double offset = 0;
            Point start = new Point(r.Left, r.Top + (r.Height / 2));
            Point control1 = new Point(r.Left - offset, r.Top - offset);
            Point control2 = new Point(r.Left - offset, r.Top - offset);
            offset = 0;
            Point end1 = new Point(r.Left + (r.Width / 2), r.Top);
            Point control3 = new Point(r.Right + offset, r.Top - offset);
            Point control4 = new Point(r.Right + offset, r.Top - offset);
            Point end2 = new Point(r.Right, r.Top + (r.Height / 2));
            Point control5 = new Point(r.Right + offset, r.Bottom + offset);
            offset = aoffset;
            Point control6 = new Point(r.Right + offset, r.Bottom + offset);
            Point end3 = new Point(r.Left + (r.Width / 2), r.Bottom);

            Point control7 = new Point(r.Left - offset, r.Bottom + offset);
            offset = 0;
            Point control8 = new Point(r.Left - offset, r.Bottom + offset);
            return new Point[] {
										start, control1, control2, end1,
										control3, control4, end2,
										control5, control6, end3,
										control7, control8, start
									};

            
        }

        public override object Clone() {
            return new BezierShape(_data);
        }
    }
}