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

namespace Limaki.Drawing.Shapes {

#if ! SILVERLIGHT    
    [Serializable]
#endif
    public class BezierShape : RectangleShapeBase, IBezierShape {
        public BezierShape():base() {}
        public BezierShape(RectangleI data):base(data) {}
        public BezierShape(PointI location, SizeI size) : base(location,size) { }

        public override RectangleI BoundsRect {
            get {
                return RectangleI.Inflate(Data,(int)_offset+1,(int)_offset+1);
            }
        }

        public override PointI this[Anchor i] {
            get {
                RectangleI _data = this.BoundsRect;
                switch (i) {
                    case Anchor.LeftTop:
                    case Anchor.MostLeft:
                    case Anchor.MostTop:
                        return new PointI(_data.X, _data.Y);

                    case Anchor.LeftBottom:
                        return new PointI(
                            _data.X,
                            _data.Y + _data.Height);

                    case Anchor.RightTop:
                    case Anchor.MostRight:
                        return new PointI(
                            _data.X + _data.Width,
                            _data.Y);

                    case Anchor.RightBottom:
                    case Anchor.MostBottom:
                        return new PointI(
                            _data.X + _data.Width,
                            _data.Y + _data.Height
                            );

                    case Anchor.MiddleTop:
                        return new PointI(
                            _data.X + _data.Width / 2,
                            _data.Y);

                    case Anchor.LeftMiddle:
                        return new PointI(
                            _data.X,
                            _data.Y + _data.Height / 2);

                    case Anchor.RightMiddle:
                        return new PointI(
                            _data.X + _data.Width,
                            _data.Y + _data.Height / 2);

                    case Anchor.MiddleBottom:
                        return new PointI(
                            _data.X + _data.Width / 2,
                            _data.Y + _data.Height);

                    case Anchor.Center:
                        return new PointI(
                            _data.X + _data.Width / 2,
                            _data.Y + _data.Height / 2);

                    default:
                        return new PointI(_data.X, _data.Y);
                }

            }

            set { }
        }

        public override PointI[] Hull(int delta, bool extend) {
            return Hull(BoundsRect,delta,extend);
        }

        public override PointI[] Hull(Matrice matrix, int delta, bool extend) {
            RectangleI _data = BoundsRect;
            int dataX = _data.X; int dataY = _data.Y;
            PointI[] p = { new PointI(dataX, dataY), new PointI(dataX + _data.Width, dataY + _data.Height) };

            matrix.TransformPoints(p);
            return Hull(RectangleI.FromLTRB(p[0].X, p[0].Y, p[1].X, p[1].Y), delta, extend);
        }

        private float _offset = 5f;
        public float Offset {
            get { return _offset; }
            set { _offset = value; }
        }

        public PointS[] BezierPoints {
            get { return GetRoundedRectBezier (this.Data, _offset); }
        }

        private PointS[] GetRoundedRectBezier(RectangleS rect, float aoffset) {
            float grow = 0f;
            RectangleS r = new RectangleS(rect.X + grow, rect.Y + grow, rect.Width + grow, rect.Height + grow);
            // Create points for curve.
            float offset = 0;
            PointS start = new PointS(r.Left, r.Top + (r.Height / 2));
            PointS control1 = new PointS(r.Left - offset, r.Top - offset);
            PointS control2 = new PointS(r.Left - offset, r.Top - offset);
            offset = 0;
            PointS end1 = new PointS(r.Left + (r.Width / 2), r.Top);
            PointS control3 = new PointS(r.Right + offset, r.Top - offset);
            PointS control4 = new PointS(r.Right + offset, r.Top - offset);
            PointS end2 = new PointS(r.Right, r.Top + (r.Height / 2));
            PointS control5 = new PointS(r.Right + offset, r.Bottom + offset);
            offset = aoffset;
            PointS control6 = new PointS(r.Right + offset, r.Bottom + offset);
            PointS end3 = new PointS(r.Left + (r.Width / 2), r.Bottom);

            PointS control7 = new PointS(r.Left - offset, r.Bottom + offset);
            offset = 0;
            PointS control8 = new PointS(r.Left - offset, r.Bottom + offset);
            return new PointS[] {
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