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
using Limaki.Drawing;
using System.ComponentModel;

namespace Limaki.Actions {
    public abstract class Layer<T> : ActionBase, ILayer<T>, ITransformTarget {
        
        IZoomTarget _zoomable = null;
        IScrollTarget _scrollable = null;
        ///<directed>True</directed>
        public Layer(IZoomTarget zoomTarget, IScrollTarget scrollTarget) {
            this._zoomable = zoomTarget;
            this._scrollable = scrollTarget;
        }

        protected Size _size = Size.Empty;
        public virtual Size Size {
            get { return _size; }
            set { _size = value; }
        }

        protected bool isDataOwner = false;
        protected T _data = default(T);
        protected T GetData() { return Data; }
        
        public abstract T Data { get; set;}
        public abstract void DataChanged();

        public abstract void OnPaint(PaintActionEventArgs e);

        #region IDisposable Member

        public virtual void Clear() {
            if (_transformer != null) {
                _transformer.Dispose();
                _transformer = null;
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

        # region ITransformer member
        ITransformer _transformer;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ITransformer Transformer {
            get {
                if (_transformer == null) {
                    _transformer = new DelegatingTransformer(GetMatrix);
                }
                return _transformer;
            }
        }


        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Matrice GetMatrix() {
            Matrice matrice = new Matrice();
            matrice.Scale(_zoomable.ZoomFactor,_zoomable.ZoomFactor);

            // getting ScrollPosition is very expensive; no idea how to avoid it
            matrice.Translate(
                -_scrollable.ScrollPosition.X / _zoomable.ZoomFactor,
                -_scrollable.ScrollPosition.Y / _zoomable.ZoomFactor);
            
            transformSandBox(matrice);
            
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
    
    }
}