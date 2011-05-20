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

namespace Limaki.Drawing {
    public abstract class CameraBase:ICamera {
        #region ICamera Member
        public abstract Matrice Matrice { get; set;}

        # region int
        public virtual PointI ToSource(PointI s) {
            PointI[] result = { s };
            using (Matrice m = (Matrice)Matrice.Clone()) {
                m.Invert();
                m.TransformPoints(result);
            }
            return result[0];
        }
        
        public virtual SizeI ToSource(SizeI s) {
            PointI[] result = { new PointI(s.Width, s.Height) };
            using (Matrice m = (Matrice)Matrice.Clone()) {
                m.Invert();
                m.VectorTransformPoints(result);
            }
            return new SizeI(result[0].X, result[0].Y);
        }

        public virtual RectangleI ToSource(RectangleI r) {
            PointI[] p = { r.Location, new PointI(r.Right, r.Bottom) };
            using (Matrice m = (Matrice)Matrice.Clone()) {
                m.Invert();
                m.TransformPoints(p);
            }
            return RectangleI.FromLTRB(p[0].X, p[0].Y, p[1].X, p[1].Y);
        }
        
        public virtual void ToSource(IShape s) {
            using (Matrice m = (Matrice)Matrice.Clone()) {
                m.Invert();
                s.Transform (m);
            }
        }

        public virtual PointI FromSource(PointI s) {
            Matrice m = Matrice;
            PointI[] result = { s };
            m.TransformPoints(result);
            return result[0];
        }

        public virtual SizeI FromSource(SizeI s) {
            Matrice m = Matrice;
            PointI[] result =  { new PointI(s.Width, s.Height) };
            m.VectorTransformPoints(result);
            return new SizeI(result[0].X,result[0].Y);
        }
       
        public virtual RectangleI FromSource(RectangleI r) {
            PointI[] p = { r.Location, new PointI(r.Right, r.Bottom) };
            Matrice m = Matrice;
            m.TransformPoints(p);

            return RectangleI.FromLTRB(p[0].X, p[0].Y, p[1].X, p[1].Y);
        }

        public virtual void FromSource(IShape s) {
            s.Transform (Matrice);
        }

        # endregion

        #region float
        public virtual PointS ToSource(PointS s) {
            PointS[] result = { s };
            using (Matrice m = (Matrice)Matrice.Clone()) {
                m.Invert();
                m.TransformPoints(result);
            }
            return result[0];
        }
        public virtual SizeS ToSource(SizeS s) {
            PointS[] result = { (PointS)s };
            using (Matrice m = (Matrice)Matrice.Clone()) {
                m.Invert();
                m.TransformVectors(result);
            }
            
            return  new SizeS (result[0]);
        }

        public virtual RectangleS ToSource(RectangleS r) {
            PointS[] p = { r.Location, new PointS(r.Right, r.Bottom) };
            using (Matrice m = (Matrice)Matrice.Clone()) {
                m.Invert();
                m.TransformPoints(p);
            }
            return RectangleS.FromLTRB(p[0].X, p[0].Y, p[1].X, p[1].Y);
        }

        public virtual PointS FromSource(PointS s) {
            Matrice m = Matrice;
            PointS[] result = { s };
            m.TransformPoints(result);
            return result[0];
        }
        public virtual SizeS FromSource(SizeS s) {
            Matrice m = Matrice;
            PointS[] result =  { (PointS)s };
            m.TransformVectors(result);
            return new SizeS(result[0]);
        }

        public virtual RectangleS FromSource(RectangleS r) {
            PointS[] p = { r.Location, new PointS(r.Right, r.Bottom) };
            Matrice m = Matrice;
            m.TransformPoints(p);

            return RectangleS.FromLTRB(p[0].X, p[0].Y, p[1].X, p[1].Y);
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
