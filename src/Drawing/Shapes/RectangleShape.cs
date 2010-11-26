/*
 * Limaki 
 * Version 0.063
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

    public class RectangleShape : Shape<Rectangle> {
        public RectangleShape():base() {}
        public RectangleShape(Rectangle data):base() {
            this.Data = Rectangle.FromLTRB(
                Math.Min(data.X,data.Right),
                Math.Min(data.Y, data.Bottom),
                Math.Max(data.X, data.Right),
                Math.Max(data.Y, data.Bottom));
        }
         
        public override Point this[Anchor i] {
            get {
                switch (i) {
                    case Anchor.LeftTop:
                    case Anchor.MostLeft:
                    case Anchor.MostTop:
                        return _data.Location;
                        
                    case Anchor.LeftBottom:
                        return new Point(
                            _data.Left,
                            _data.Bottom);
                        
                    case Anchor.RightTop:
                    case Anchor.MostRight:
                        return new Point(
                            _data.Right,
                            _data.Top);
								
                    case Anchor.RightBottom:
                    case Anchor.MostBottom:
                        return new Point(
                            _data.Right,
                            _data.Bottom
                            );

                    case Anchor.MiddleTop:
                        return new Point(
                            _data.Left + _data.Width / 2,
                            _data.Top);

                    case Anchor.LeftMiddle:
                        return new Point(
                            _data.Left,
                            _data.Top + _data.Height / 2);

                    case Anchor.RightMiddle:
                        return new Point(
                            _data.Right,
                            _data.Top + _data.Height / 2);

                    case Anchor.MiddleBottom:
                        return new Point(
                            _data.Left + _data.Width / 2,
                            _data.Bottom);

                    case Anchor.Center:
                        return new Point(
                            _data.Left + _data.Width / 2,
                            _data.Top + _data.Height / 2);

                    default:
                        return _data.Location;
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
            get { return this._data.Location; }
            set { this._data.Location = value; }
        }

        public override void Transform(Matrice matrice) {
            Point[] p = { _data.Location, new Point(_data.Right, _data.Bottom) };
            matrice.TransformPoints(p);
            _data = Rectangle.FromLTRB(p[0].X, p[0].Y, p[1].X, p[1].Y);
        }
        public override object Clone() {
            return new RectangleShape (_data);
        }
    }
}