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

namespace Limaki.Drawing.Shapes {
#if ! SILVERLIGHT    
    [Serializable]
#endif
    public abstract class RectangleShapeBase : Shape<RectangleI> {
        public RectangleShapeBase():base() {}
        public RectangleShapeBase(RectangleI data):base() {
            this.Data = RectangleI.FromLTRB(
                Math.Min(data.X,data.Right),
                Math.Min(data.Y, data.Bottom),
                Math.Max(data.X, data.Right),
                Math.Max(data.Y, data.Bottom));
        }

        public RectangleShapeBase(PointI location, SizeI size) : base() {
            this.Data = new RectangleI (location, size);
        }

        public override PointI this[Anchor i] {
            get {
                switch (i) {
                    case Anchor.LeftTop:
                    case Anchor.MostLeft:
                    case Anchor.MostTop:
                        return new PointI(_data.X , _data.Y);
                        
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
                        return new PointI(_data.X , _data.Y);
                }

            }

            set {}
        }
        public override RectangleI BoundsRect {
            get { return this._data; }
        }
        public override SizeI Size {
            get { return _data.Size;}
            set { _data.Size = value; }
        }
        public override PointI Location {
            get { return new PointI(_data.X , _data.Y); }
            set { this._data.Location = value; }
        }

        public override void Transform(Matrice matrice) {
            int dataX = _data.X;
            int dataY = _data.Y;
            PointI[] p = { new PointI(dataX, dataY), new PointI(dataX + _data.Width, dataY + _data.Height) };
            matrice.TransformPoints(p);
            _data = RectangleI.FromLTRB(p[0].X, p[0].Y, p[1].X, p[1].Y);
        }

        public static PointI[] Hull(RectangleI rect, int delta, bool extend) {
            int startX = rect.X;
            int startY = rect.Y;
            int endX = startX+rect.Width;
            int endY = startY+rect.Height;
            return new PointI[] {
                                   new PointI ((startX - delta), (startY - delta)),
                                   new PointI ((endX + delta), (startY - delta)),
                                   new PointI((endX + delta), (endY + delta)),
                                   new PointI ((startX - delta), (endY + delta))
                               };
        }

        public override PointI[] Hull(int delta, bool extend) {
            return Hull(_data,delta, extend);
        }

        public override PointI[] Hull(Matrice matrix, int delta, bool extend) {
            int dataX = _data.X; int dataY = _data.Y;
            PointI[] p = { new PointI(dataX, dataY), new PointI(dataX + _data.Width, dataY + _data.Height) };

            matrix.TransformPoints(p);
            return Hull(RectangleI.FromLTRB(p[0].X, p[0].Y, p[1].X, p[1].Y), delta, extend);
        }


    }
}