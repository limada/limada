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

using Limaki.Actions;
using Limaki.Drawing;
using Xwt;
using System;

namespace Limaki.View.Viz.Rendering {

    public abstract class Layer<T> : ActionBase, ILayer<T> {

        public Layer () {
            this.Priority = ActionPriorities.DataLayerPriority;
            this.Alpha = 1d;
        }

        public virtual T Data {
            get {
                if (_data != null) {
                    return _data();
                }
                return default(T);
            }
        }

        public abstract void DataChanged();

        public virtual Size Size { get; set; }

        public virtual Point Origin { get; set; }

        public virtual double Alpha { get; set; }

        public virtual ICamera Camera {
            get {
                if (_camera != null) {
                    return _camera();
                }
                return null;
            }
        }
        
        public virtual IContentRenderer<T> Renderer {
            get {
                if (_renderer != null) {
                    return _renderer();
                }
                return null;
            }
        }
        
        public abstract void OnPaint(IRenderEventArgs e);
        
        #region IDisposable Member

        public virtual void Clear() {}

        protected override void Dispose(bool disposing) {
            if (disposing) {
                Clear ();
            }
            base.Dispose(disposing);
        }
        #endregion


        #region ILayer<T> Member

        protected Func<T> _data = null;
        Func<T> ILayer<T>.Data {
            get { return _data; }
            set { _data = value; }
        }

        protected Func<ICamera> _camera = null;
        Func<ICamera> ILayer<T>.Camera {
            get { return _camera; }
            set { _camera = value; }
        }
        protected Func<IContentRenderer<T>> _renderer = null;
        Func<IContentRenderer<T>> ILayer<T>.Renderer {
            get { return _renderer; }
            set { _renderer = value; }
        }


        #endregion

    }
}