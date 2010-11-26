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
    public interface ILinkShape {
        Point Start { get;set;}
        Point End  { get;set;}
    }
    public class VectorShape : Shape<Vector>, ILinkShape {

        private static GraphicsPath helperPath = new GraphicsPath ();
        private static Pen helperPen = new Pen (Color.Black);

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
                return new Rectangle(_dataStartX, _dataStartY, _dataEndX - _dataStartX, _dataEndY-_dataStartY);
            }
        }

        public override Size Size {
            get { return new Size (_data.End.X - _data.Start.X, _data.End.Y - _data.Start.Y); }
            set { _data.End = Point.Add (_data.Start, value); }
        }

        public override Point Location {
            get { return this._data.Start; }
            set {
                Size oldSize = this.Size;
                this._data.Start = value;
                Size = oldSize;
            }
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
            // TODO: first ask, if p is in boundsRect
            lock (helperPath) {
                helperPath.Reset ();
                helperPath.AddLine (_data.Start, _data.End);
                helperPen.Width = hitSize;
                //helperPen.Width = hitSize/2;
                //helperPath.Widen (helperPen);
                return helperPath.IsOutlineVisible (p, helperPen);
            }
        }

        public Point[] PolygonHull_AtanSinCos(int width) {
            Point[] result = new Point[4];
            int x1 = _data.Start.X;
            int x2 = _data.End.X;
            int y1 = _data.Start.Y;
            int y2 = _data.End.Y;
            if ((x2 - x1) != 0.0) {
                double theta = Math.Atan((y2 - y1) / (x2 - x1));
                int dx = (int)(Math.Sin(theta) * width);
                int dy = (int)(Math.Cos(theta) * width);
                result[0] = new Point(x1 - dx, y1 + dy);
                result[1] = new Point(x1 + dx, y1 - dy);
                result[2] = new Point(x2 + dx, y2 - dy);
                result[3] = new Point(x2 - dx, y2 + dy);

            } else {
                // special case, vertical line
                result[0] = new Point(x1 - width, y1);
                result[1] = new Point(x1 + width, y1);
                result[2] = new Point(x2 + width, y2);
                result[3] = new Point(x2 - width, y2);

            }
            return result;
        }

        public virtual Point[] PolygonHull_Transform(int delta) {
            Point[] line = null;

                //if (_data.Start.X <= _data.End.X && _data.Start.Y <= _data.End.Y) {
                if (_data.Start.X <= _data.End.X) {
                    line = new Point[] { _data.Start, _data.End };
                } else {
                    line = new Point[] { _data.End, _data.Start };
                }


                Matrice lineMatrice = new Matrice ();
                float angle = (float) Vector.Angle (Data);
                lineMatrice.Rotate (-angle);
                lineMatrice.TransformPoints (line);
                Point[] poly = new Point[] {
                                                 new Point (line[0].X - delta, line[0].Y - delta),
                                                 new Point(line[1].X + delta, line[1].Y - delta),
                                                 new Point (line[1].X + delta, line[1].Y + delta),
                                                 new Point (line[0].X - delta, line[0].Y + delta)
                                             };
                lineMatrice.Reset ();
                lineMatrice.Rotate (angle);
                lineMatrice.TransformPoints (poly);
                return poly;
            
        }

        public virtual Point[] PolygonHull ( int delta ) {
            // get it near:
            double startX = _data.Start.X;
            double startY = _data.Start.Y;
            double endX = _data.End.X;
            double endY = _data.End.Y;

            double deltaSinusAlpha = 0;
            double deltaSinusBeta = 0;

            double a = endX - startX;
            double b = endY - startY;

            if (a == 0d) {
                if (b > 0d) {
                    deltaSinusBeta = delta;
                } else {
                    deltaSinusBeta = -delta;
                }
            } else if (b == 0d) {
                if (a > 0d) {
                    deltaSinusAlpha = delta;
                } else {
                    deltaSinusAlpha = -delta;
                }
            } else {
                // calculation of the hypotenuse:
                double c = Math.Sqrt((a * a + b * b));

                // calculation of Sinus Alpha and Beta, factorized with delta:
                deltaSinusAlpha = (delta * (a / c));
                deltaSinusBeta = (delta * (b / c));
            }

            // extending the original line to make it longer:
            startX = _data.Start.X - deltaSinusAlpha;
            startY = _data.Start.Y - deltaSinusBeta;
            endX = _data.End.X + deltaSinusAlpha;
            endY = _data.End.Y + deltaSinusBeta;


            return new Point[] {
                new Point ((int)(startX - deltaSinusBeta), (int)(startY + deltaSinusAlpha)),
                new Point ((int)(startX + deltaSinusBeta), (int)(startY - deltaSinusAlpha)),
                new Point((int)(endX + deltaSinusBeta), (int)(endY - deltaSinusAlpha)),
                new Point ((int)(endX - deltaSinusBeta), (int)(endY + deltaSinusAlpha))
                                             };
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