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

//#define TracePaint
//#define TraceInvalidate

// UseOnScroll does not work on mono
// Reason: Mono first calls OnPaint, then OnScroll
// Windows first calls OnScroll, then OnPaint
#define UseOnScroll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.GDI;
using Limaki.Drawing.GDI.UI;
using Limaki.Drawing.UI;


namespace Limaki.Winform.Displays {
    /// <summary>
    /// this partial class deals with srcoll, zoom
    /// and is the source of the Camera which is used in several subcomponents
    /// TODO: refactor this to a Camera/ViewPort class without gdi/winform dependencies
    /// with a CameraController/WinformCameraControler
    /// look at GDISceneControler.Execute where are some parts of the functionality
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class DisplayBase<T> 
    where T : class {

        // TODO: refactor this without DataLayer
        // Remark: DataLayer.Size is overriden in WidgetLayerBase
        // where Size and Offset are set to 0,0,0,0 if negative
        // Remark: DataSize is in world coordinates
        public virtual SizeI DataSize { get { return DataLayer.Size; } }

        # region ICameraTarget member
        ICamera _camera;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
            return new GDIMatrice ();
        }

        [Browsable(false)]
        public virtual Matrice GetMatrix() {

            var zoomFactor = this.ZoomFactor;
            var scrollPosition = this.ScrollPosition;
            var offset = this.Offset;
            var matrice = CreateMatrix();
            matrice.Scale(zoomFactor, zoomFactor);

            matrice.Translate(
                (-offset.X - scrollPosition.X) / zoomFactor,//
                (-offset.Y - scrollPosition.Y) / zoomFactor);//

            //transformSandBox(matrice);

            return matrice;
        }

        /// <summary>
        /// used to try out other tranformation possibitities
        /// </summary>
        /// <param name="m"></param>
        void transformSandBox(Matrice m) {
            //m.Rotate(20f);
            //m.Shear(0.1f, 0.1f);

        }
        # endregion 

        #region IScrollable Member

        protected override void OnScroll(ScrollEventArgs se) {

#if UseOnScroll
            if (se.OldValue != se.NewValue) {
                scrollChanged = true;
                if (se.ScrollOrientation == ScrollOrientation.HorizontalScroll) {
                    _scrollPosition.X = se.NewValue;
                } else {
                    _scrollPosition.Y = se.NewValue;
                }
#if TraceInvalidate
            System.Console.Out.WriteLine("OnScroll scrollChanged");
#endif
            }
#endif
            base.OnScroll(se);
        }

        bool scrollChanged = true;
        private PointI _scrollPosition = PointI.Empty;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual PointI ScrollPosition {
            get {
#if !UseOnScroll
                    Point point = this.AutoScrollPosition;
                    _scrollPosition = new PointI(-point.X, -point.Y);
                    scrollChanged = false;

#endif
                return _scrollPosition;
            }
            set {
                if (_scrollPosition != value) {
                    _scrollPosition = value;
                    this.AutoScrollPosition =
                        GDIConverter.Convert(_scrollPosition);

                    scrollChanged = true;
                }

            }
        }



        private SizeI _scrollMinSize = new SizeI();
        private bool _scrollMinSizeChanging = false;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual SizeI ScrollMinSize {
            get {
#if UseOnScroll
                return _scrollMinSize;
#else
                return GDIExtensions.Toolkit(this.AutoScrollMinSize);
#endif
            }
            set {
                _scrollMinSizeChanging = true;
                this.AutoScrollMinSize = GDIConverter.Convert(value);
                _scrollMinSize = value;
                _scrollMinSizeChanging = false;
            }
        }

        private PointI _offset = new PointI();
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual PointI Offset {
            get { return _offset; }
            set { _offset = value; }
        }

        public virtual void UpdateCamera() {
#if TraceInvalidate
            System.Console.Out.WriteLine("Data.Bounds\t" + DataSize);
#endif
            var scrollMinSize = this.Camera.FromSource(DataSize);
            var offset = (PointI)this.Camera.FromSource((SizeI)DataLayer.Offset);

            this._scrollMinSize = scrollMinSize;


            // this should only be done if DataLayer.Offset changed!
            // eg. NOT if zoom changed (ZoomIn, ZoomOut)
            //if (offset.X < 0 || offset.Y < 0) {
            //    var scrollPosition = this.ScrollPosition;
            //    if (offset.X != _offset.X) {
            //        scrollPosition.X = 0;
            //    }

            //    if (offset.Y != _offset.Y) {
            //        scrollPosition.Y = 0;
            //    }
            //    this.ScrollPosition = scrollPosition;
            //}

            this._offset = offset;
        }

        public virtual void UpdateScrollSize() {
            UpdateCamera ();
            this.ScrollMinSize = _scrollMinSize;
            this.Offset = this._offset;
#if UseOnScroll
            var point = this.AutoScrollPosition;
            _scrollPosition = new PointI(-point.X, -point.Y);
#endif
#if TraceInvalidate
            System.Console.Out.WriteLine("scrollMinSize\t" + scrollMinSize);
            System.Console.Out.WriteLine("offset\t" + offset);
#endif
        }

        public virtual void ResetScroll() {
            ScrollPosition = new PointI();
            this.CommandsExecute();
            UpdateScrollSize();
        }

        #endregion

        # region IZoomable-Member

        ///<directed>True</directed>
        protected ZoomState _zoom = ZoomState.Original;
        public virtual ZoomState ZoomState {
            set {
                if (value != _zoom) {
                    _zoom = value;
                    UpdateZoom();
                }
            }
            get { return _zoom; }
        }

        protected float _zoomFactor = 1.0f;
        public virtual float ZoomFactor {
            get { return _zoomFactor; }
            set { _zoomFactor = value; }
        }

        // Fit to selected zoom
        protected void FitToZoom(ZoomState zoomState) {
            Size rc = ClientRectangle.Size;

            switch (zoomState) {
                case ZoomState.FitToScreen:
                    rc = this.Size;
                    _zoomFactor = Math.Min(
                        (float)rc.Width / DataSize.Width,
                        (float)rc.Height / DataSize.Height);
                    break;
                case ZoomState.FitToWidth:
                    _zoomFactor = (float)rc.Width / DataSize.Width;
                    //ScrollPosition = new Point();
                    break;
                case ZoomState.FitToHeigth:
                    _zoomFactor = (float)rc.Height / DataSize.Height;
                    break;
                case ZoomState.Original:
                    _zoomFactor = 1.0f;
                    break;
            }
        }

        public event EventHandler ZoomChanged;

        // Update zoom factor
        public virtual void UpdateZoom() {
            FitToZoom(_zoom);

            UpdateScrollSize();

            ((IControl)this).Invalidate();

            if (ZoomChanged != null)
                ZoomChanged(this, null);

        }

        #endregion

    }
}
