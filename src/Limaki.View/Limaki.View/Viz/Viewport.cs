/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using System;
using Xwt;
using Xwt.Drawing;

namespace Limaki.View.Viz {

    public class Viewport:IViewport, ICheckable {

        #region Camera
        ICamera _camera;
        public virtual ICamera Camera {
            get { return _camera ?? (_camera = new DelegatingCamera(() => Matrix)); } 
            set { _camera = value; }
        }

        Matrix _matrix = null;
        protected virtual Matrix CachedMatrix {
            get { return _matrix ?? (_matrix = new Matrix()); }
        }

        public virtual Matrix Matrix {
            get {
                if (ZoomNeedsUpdate)
                    FitToZoom(ZoomState);

                var zoomFactor = this.ZoomFactor;
                var scrollPosition = this.ClipOrigin;
                var offset = this.DataOrigin;

                CachedMatrix.SetIdentity();
                CachedMatrix.ScaleAppend(zoomFactor, zoomFactor);

                CachedMatrix.TranslateAppend(
                    (-offset.X - scrollPosition.X),
                    (-offset.Y - scrollPosition.Y));

                return CachedMatrix;
            }
        }

        #endregion

        #region Data
        protected Point _offset = new Point();
        public virtual Point DataOrigin {
            get { return _offset; }
            protected set { _offset = value; }
        }

        protected Size _dataSize = new Size();
        public virtual Size DataSize {
            get { return _dataSize; }
            set { _dataSize = value; }
        }

        /// <summary>
        /// DataLayer.Origin
        /// </summary>
        public Func<Point> GetDataOrigin {get;set;}
        public Func<Size> GetDataSize{get;set;}

        #endregion

        #region Clipping
        
        public virtual Point ClipOrigin { get; set; }
        
        public virtual Size ClipSize {
            get { return this.DataSize; }
        }

        public virtual void Update(IClipper clipper) {
            var matrix = this.Camera.Matrix;
            var camera = new Camera(matrix);
            var oldOffset = this.DataOrigin;
            var oldSize = this.DataSize;
            var oldPosition = this.ClipOrigin;
            var matrixOffset = new Point(-matrix.OffsetX, -matrix.OffsetY);

            this.Update();

            var newOffset = this.DataOrigin;
            var newSize = this.DataSize;

            if (newSize.Width < oldSize.Width &&
                (oldPosition.X + oldOffset.X) == matrixOffset.X) {
                var h = Math.Max(oldSize.Height, newSize.Height);
                var rect = new Rectangle(
                    newSize.Width,
                    matrix.OffsetY,
                    oldSize.Width - newSize.Width,
                    h);
                clipper.Add(RectangleShape.Hull(camera.ToSource(rect), 0, false));
            }

            if (newSize.Height < oldSize.Height &&
                (oldPosition.Y + oldOffset.Y) == matrixOffset.Y) {
                var w = Math.Max(oldSize.Width, newSize.Width);
                var rect = new Rectangle(
                    matrix.OffsetX,
                    newSize.Height,
                    w,
                    oldSize.Height - newSize.Height);
                clipper.Add(RectangleShape.Hull(camera.ToSource(rect), 0, false));
            }

            if (newOffset.X != oldOffset.X || newOffset.Y != oldOffset.Y) {
                clipper.RenderAll = true;
            }
        }



        public virtual void UpdateCamera() {
#if TraceInvalidate
            System.Console.Out.WriteLine("Data.Bounds\t" + DataSize);
#endif
            var dataSize = this.Camera.FromSource(GetDataSize());
            var offset = (Point)this.Camera.FromSource((Size)GetDataOrigin());
            this._dataSize = dataSize;
            this._offset = offset;
        }

        public virtual void Update() {
            UpdateCamera();
            this.DataSize = _dataSize;
            this.DataOrigin = this._offset;
            
#if TraceInvalidate
            System.Console.Out.WriteLine("scrollMinSize\t" + scrollMinSize);
            System.Console.Out.WriteLine("offset\t" + offset);
#endif
        }

        public Action CommandsExecute { get; set; }

        public virtual void Reset() {
            ClipOrigin = new Point();
            if (this.CommandsExecute != null) {
                this.CommandsExecute ();
            }
            Update();
        }

        #endregion

        #region Zoom
        protected double _zoomFactor = 1.0f;
        public virtual double ZoomFactor {
            get { return _zoomFactor; }
            set { _zoomFactor = value; }
        }

        public virtual ZoomState ZoomState { get; set; }

        public virtual void UpdateZoom() {
            FitToZoom (ZoomState);
            Update ();
        }

        protected bool ZoomNeedsUpdate { get; set; }
        // Fit to selected zoom
        protected virtual void FitToZoom(ZoomState zoomState) {
            var rc = this.ClipSize;
            var dataSize = this.GetDataSize ();
            if (rc.IsZero || dataSize.IsZero) {
                ZoomNeedsUpdate = true;
                return;
            }
            ZoomNeedsUpdate = false;
            dataSize = new Size(dataSize.Width + 2, dataSize.Height + 2);
            switch (zoomState) {
                case ZoomState.FitToScreen:
                    _zoomFactor = Math.Min(
                        rc.Width / dataSize.Width,
                        rc.Height / dataSize.Height);
                    break;
                case ZoomState.FitToWidth:
                    _zoomFactor = rc.Width / dataSize.Width;
                    break;
                case ZoomState.FitToHeigth:
                    _zoomFactor = rc.Height / dataSize.Height;
                    break;
                case ZoomState.Original:
                    _zoomFactor = 1.0f;
                    break;
            }
        }
        #endregion

        #region ICheckable Member

        public bool Check() {
            if (this.GetDataOrigin == null) {
                throw new CheckFailedException(this.GetType()+"needs a DataOrigin-Action");
            }
            if (this.GetDataSize == null) {
                throw new CheckFailedException(this.GetType() + "needs a DataSize-Action");
            }
            return true;
        }

        #endregion
    }
}