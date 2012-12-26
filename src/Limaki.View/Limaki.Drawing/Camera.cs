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

namespace Limaki.Drawing {
    public abstract class CameraBase:ICamera {
        #region ICamera Member
        public abstract Matrix Matrix { get; set;}

        # region IShape
        public virtual void ToSource(IShape s) {
            using (var m = new Matrix(Matrix)) {
                m.Invert();
                s.Transform(m);
            }
        }

        public virtual void FromSource(IShape s) {
            s.Transform(Matrix);
        }

        #endregion

        # region int
        public virtual Point ToSource(Point s) {
            Point[] result = { s };
            using (var m = new Matrix(Matrix)) {
                m.Invert();
                m.Transform(result);
            }
            return result[0];
        }
        
        public virtual Size ToSource(Size s) {
            Point[] result = { new Point(s.Width, s.Height) };
            using (var m = new Matrix(Matrix)) {
                m.Invert();
                m.TransformVector(result);
            }
            return new Size(result[0].X, result[0].Y);
        }

        public virtual Rectangle ToSource(Rectangle r) {
            Point[] p = { r.Location, new Point(r.Right, r.Bottom) };
            using (var m = new Matrix(Matrix)) {
                m.Invert();
                m.Transform(p);
            }
            return Rectangle.FromLTRB(p[0].X, p[0].Y, p[1].X, p[1].Y);
        }
        
       

        public virtual Point FromSource(Point s) {
            var m = Matrix;
            Point[] result = { s };
            m.Transform(result);
            return result[0];
        }

        public virtual Size FromSource(Size s) {
            var m = Matrix;
            Point[] result =  { new Point(s.Width, s.Height) };
            m.TransformVector(result);
            return new Size(result[0].X,result[0].Y);
        }
       
        public virtual Rectangle FromSource(Rectangle r) {
            Point[] p = { r.Location, new Point(r.Right, r.Bottom) };
            Matrix m = Matrix;
            m.Transform(p);

            return Rectangle.FromLTRB(p[0].X, p[0].Y, p[1].X, p[1].Y);
        }

       

        # endregion

       

        #endregion

        #region IDisposable Member

        public abstract void Dispose ( bool disposing );

        public void Dispose() {
            Dispose (true);
        }

        #endregion
    }

    public class Camera:CameraBase {
        Matrix _matrix = null;
        public Camera(Matrix matrix) {
            this._matrix = matrix;
        }


        #region ITransformable Member

        public override Matrix Matrix {
            get {
                if (_matrix == null)
                    _matrix = new Matrix ();
                return _matrix;
            }
            set {
                if(_matrix != value) {
                    _matrix.Dispose ();
                    _matrix = null;
                }
                _matrix = value;
            }
        }
        #endregion

        #region IDisposable Member
        public override void Dispose(bool disposing) {
            if (disposing) {
                _matrix.Dispose();
                _matrix = null;
            }
        }
        #endregion
    }

    public class DelegatingCamera : CameraBase {
        public delegate Matrix MatrixHandler();

        MatrixHandler _transform = null;
        public DelegatingCamera(MatrixHandler transform) {
            this._transform = transform;
        }

        #region ITransformable Member

        public override Matrix Matrix {
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
