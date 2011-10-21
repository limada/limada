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
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;

namespace Limaki.Presenter {
    public class ViewPort:IViewport, ICheckable {

        #region Camera
        ICamera _camera;
        public virtual ICamera Camera {
            get {
                if (_camera == null) {
                    _camera = new DelegatingCamera(GetMatrix);
                }
                return _camera;
            }
            set { _camera = value; }
        }

        public virtual Matrice CreateMatrix() {
            return new Matrice();
        }

        public virtual Matrice GetMatrix() {
            var zoomFactor = this.ZoomFactor;
            var scrollPosition = this.ClipOrigin;
            var offset = this.DataOrigin;
            var matrice = CreateMatrix();
            matrice.Scale(zoomFactor, zoomFactor);

            matrice.Translate(
                (-offset.X - scrollPosition.X) / zoomFactor,//
                (-offset.Y - scrollPosition.Y) / zoomFactor);//

            //transformSandBox(matrice);

            return matrice;
        }

        #endregion

        #region Data
        protected PointI _offset = new PointI();
        public virtual PointI DataOrigin {
            get { return _offset; }
            protected set { _offset = value; }
        }

        protected SizeI _scrollMinSize = new SizeI();
        public virtual SizeI DataSize {
            get { return _scrollMinSize; }
            set { _scrollMinSize = value; }
        }

        /// <summary>
        /// DataLayer.Origin
        /// </summary>
        public Get<PointI> GetDataOrigin {get;set;}
        public Get<SizeI> GetDataSize{get;set;}

        #endregion

        #region Clipping
        
        public virtual PointI ClipOrigin { get; set; }
        
        public virtual SizeI ClipSize {
            get { return this.DataSize; }
        }

        public virtual void Update(IClipper clipper) {
            var matrix = this.Camera.Matrice;
            var camera = new Camera(matrix);
            var oldOffset = this.DataOrigin;
            var oldSize = this.DataSize;
            var oldPosition = this.ClipOrigin;
            var matrixOffset = new PointI((int)-matrix.OffsetX, (int)-matrix.OffsetY);

            this.Update();

            var newOffset = this.DataOrigin;
            var newSize = this.DataSize;

            if (newSize.Width < oldSize.Width &&
                (oldPosition.X + oldOffset.X) == matrixOffset.X) {
                var h = Math.Max(oldSize.Height, newSize.Height);
                var rect = new Drawing.RectangleI(
                    newSize.Width,
                    (int)matrix.OffsetY,
                    oldSize.Width - newSize.Width,
                    h);
                clipper.Add(RectangleShape.Hull(camera.ToSource(rect), 0, false));
            }

            if (newSize.Height < oldSize.Height &&
                (oldPosition.Y + oldOffset.Y) == matrixOffset.Y) {
                var w = Math.Max(oldSize.Width, newSize.Width);
                var rect = new Drawing.RectangleI(
                    (int)matrix.OffsetX,
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
            var scrollMinSize = this.Camera.FromSource(GetDataSize());
            var offset = (PointI)this.Camera.FromSource((SizeI)GetDataOrigin());
            this._scrollMinSize = scrollMinSize;
            this._offset = offset;
        }

        public virtual void Update() {// == UpdateScrollSize
            UpdateCamera();
            this.DataSize = _scrollMinSize;
            this.DataOrigin = this._offset;
            
#if TraceInvalidate
            System.Console.Out.WriteLine("scrollMinSize\t" + scrollMinSize);
            System.Console.Out.WriteLine("offset\t" + offset);
#endif
        }

        public Action CommandsExecute=null;
        public virtual void Reset() {
            ClipOrigin = new PointI();
            if (this.CommandsExecute != null) {
                this.CommandsExecute ();
            }
            Update();
        }

        #endregion

        #region Zoom
        protected float _zoomFactor = 1.0f;
        public virtual float ZoomFactor {
            get { return _zoomFactor; }
            set { _zoomFactor = value; }
        }

        public virtual ZoomState ZoomState { get; set; }

        public virtual void UpdateZoom() {
            FitToZoom (ZoomState);
            Update ();
        }

        // Fit to selected zoom
        protected virtual void FitToZoom(ZoomState zoomState) {
            SizeI rc = this.ClipSize;
            SizeI dataSize = this.GetDataSize ();
            dataSize = new SizeI(dataSize.Width - 1, dataSize.Height - 1);
            switch (zoomState) {
                case ZoomState.FitToScreen:
                    _zoomFactor = Math.Min(
                        (float)rc.Width / dataSize.Width,
                        (float)rc.Height / dataSize.Height);
                    break;
                case ZoomState.FitToWidth:
                    _zoomFactor = (float)rc.Width / dataSize.Width;
                    break;
                case ZoomState.FitToHeigth:
                    _zoomFactor = (float)rc.Height / dataSize.Height;
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