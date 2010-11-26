/*
 * Limaki 
 * Version 0.07
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

namespace Limaki.Drawing {
    public abstract class CameraBase:ICamera {
        #region ICamera Member
        public abstract Matrice Matrice { get; set;}

        # region int
        public virtual Point ToSource(Point s) {
            Point[] result = { s };
            using (Matrice m = (Matrice)Matrice.Clone()) {
                m.Invert();
                m.TransformPoints(result);
            }
            return result[0];
        }
        
        public virtual Size ToSource(Size s) {
            Point[] result = { new Point(s.Width, s.Height) };
            using (Matrice m = (Matrice)Matrice.Clone()) {
                m.Invert();
                m.VectorTransformPoints(result);
            }
            return new Size(result[0].X, result[0].Y);
        }

        public virtual Rectangle ToSource(Rectangle r) {
            Point[] p = { r.Location, new Point(r.Right, r.Bottom) };
            using (Matrice m = (Matrice)Matrice.Clone()) {
                m.Invert();
                m.TransformPoints(p);
            }
            return Rectangle.FromLTRB(p[0].X, p[0].Y, p[1].X, p[1].Y);
        }
        
        public virtual void ToSource(IShape s) {
            using (Matrice m = (Matrice)Matrice.Clone()) {
                m.Invert();
                s.Transform (m);
            }
        }

        public virtual Point FromSource(Point s) {
            Matrice m = Matrice;
            Point[] result = { s };
            m.TransformPoints(result);
            return result[0];
        }

        public virtual Size FromSource(Size s) {
            Matrice m = Matrice;
            Point[] result =  { new Point(s.Width, s.Height) };
            m.VectorTransformPoints(result);
            return new Size(result[0].X,result[0].Y);
        }
       
        public virtual Rectangle FromSource(Rectangle r) {
            Point[] p = { r.Location, new Point(r.Right, r.Bottom) };
            Matrice m = Matrice;
            m.TransformPoints(p);

            return Rectangle.FromLTRB(p[0].X, p[0].Y, p[1].X, p[1].Y);
        }

        public virtual void FromSource(IShape s) {
            s.Transform (Matrice);
        }

        # endregion

        #region float
        public virtual PointF ToSource(PointF s) {
            PointF[] result = { s };
            using (Matrice m = (Matrice)Matrice.Clone()) {
                m.Invert();
                m.TransformPoints(result);
            }
            return result[0];
        }
        public virtual SizeF ToSource(SizeF s) {
            PointF[] result = { (PointF)s };
            using (Matrice m = (Matrice)Matrice.Clone()) {
                m.Invert();
                m.TransformVectors(result);
            }
            
            return  new SizeF (result[0]);
        }

        public virtual RectangleF ToSource(RectangleF r) {
            PointF[] p = { r.Location, new PointF(r.Right, r.Bottom) };
            using (Matrice m = (Matrice)Matrice.Clone()) {
                m.Invert();
                m.TransformPoints(p);
            }
            return RectangleF.FromLTRB(p[0].X, p[0].Y, p[1].X, p[1].Y);
        }

        public virtual PointF FromSource(PointF s) {
            Matrice m = Matrice;
            PointF[] result = { s };
            m.TransformPoints(result);
            return result[0];
        }
        public virtual SizeF FromSource(SizeF s) {
            Matrice m = Matrice;
            PointF[] result =  { (PointF)s };
            m.TransformVectors(result);
            return new SizeF(result[0]);
        }

        public virtual RectangleF FromSource(RectangleF r) {
            PointF[] p = { r.Location, new PointF(r.Right, r.Bottom) };
            Matrice m = Matrice;
            m.TransformPoints(p);

            return RectangleF.FromLTRB(p[0].X, p[0].Y, p[1].X, p[1].Y);
        }
        #endregion

        #endregion

        #region IDisposable Member

        public abstract void Dispose ( bool disposing );

        public void Dispose() {
            Dispose (true);
        }

        #endregion
    }

    public class Camera:CameraBase {
        Matrice _matrice = null;
        public Camera(Matrice matrice) {
            this._matrice = matrice;
        }
        public Camera(Matrix matrix) {
            this.Matrice.Matrix = matrix;
        }
        #region ITransformable Member

        public override Matrice Matrice {
            get {
                if (_matrice == null)
                    _matrice = new Matrice ();
                return _matrice;
            }
            set {
                if(_matrice != value) {
                    _matrice.Dispose ();
                    _matrice = null;
                }
                _matrice = value;
            }
        }
        #endregion

        #region IDisposable Member
        public override void Dispose(bool disposing) {
            if (disposing) {
                _matrice.Dispose();
                _matrice = null;
            }
        }
        #endregion
    }

    public class DelegatingCamera : CameraBase {
        public delegate Matrice MatrixHandler();

        MatrixHandler _transform = null;
        public DelegatingCamera(MatrixHandler transform) {
            this._transform = transform;
        }

        #region ITransformable Member

        public override Matrice Matrice {
            get {
                if (_transform != null)
                    return _transform();
                else
                    return null;
            }
            set { throw new  NotSupportedException ("setting the Matrix in DelegatingCamera is not supported"); }
        }
        #endregion

        #region IDisposable Member
        public override void Dispose(bool disposing) {
            if (disposing) {
                _transform = null;
            }
        }
        #endregion
    }
}
