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
using System.ComponentModel;
using Limaki.Actions;
using Limaki.Common;

namespace Limaki.Drawing.UI {
    public abstract class Layer<T> : ActionBase, ILayer<T>, ICameraTarget {
        
        //IZoomTarget _zoomable = null;
        //IScrollTarget _scrollable = null;
        /////<directed>True</directed>
        //public Layer(IZoomTarget zoomTarget, IScrollTarget scrollTarget) {
        //    this._zoomable = zoomTarget;
        //    this._scrollable = scrollTarget;
        //}

        public Layer(ICamera camera) {
            this._camera = camera;
        }

        protected SizeI _size = SizeI.Empty;
        public virtual SizeI Size {
            get { return _size; }
            set { _size = value; }
        }

        private PointI _offset = new PointI();
        public virtual PointI Offset {
            get { return _offset; }
            set { _offset = value; }
        }
            
        protected bool isDataOwner = false;
        protected T _data = default(T);
        protected T GetData() { return Data; }
        
        public abstract T Data { get; set;}
        public abstract void DataChanged();

        public abstract void OnPaint(IPaintActionEventArgs e);

        #region IDisposable Member

        public virtual void Clear() {
            if (_camera != null) {
                _camera.Dispose();
                _camera = null;
            }
        }

        protected virtual void DisposeData() {
            if (isDataOwner && (_data != null) && (_data is IDisposable)) {
                ((IDisposable)_data).Dispose();
            }
            _data = default(T);
        }


        protected override void Dispose(bool disposing) {
            if (disposing) {
                Clear ();
                DisposeData();
            }
            base.Dispose(disposing);
        }
        #endregion

        # region ICameraTarget member
        ICamera _camera;
        [Browsable(false)]
        public virtual ICamera Camera {
            get { return _camera; }
            set { _camera = value;}
        }

        # endregion 
    
    }
}