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

using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;

namespace Limaki.Presenter.UI {
    public abstract class Layer<T> : ActionBase, ILayer<T> {

        public Layer () {
            this.Priority = ActionPriorities.DataLayerPriority;
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

        protected SizeI _size = SizeI.Empty;
        public virtual SizeI Size {
            get { return _size; }
            set { _size = value; }
        }

        protected PointI _origin = new PointI();
        public virtual PointI Origin {
            get { return _origin; }
            set { _origin = value; }
        }

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

        protected Get<T> _data = null;
        Get<T> ILayer<T>.Data {
            get { return _data; }
            set { _data = value; }
        }

        protected Get<ICamera> _camera = null;
        Get<ICamera> ILayer<T>.Camera {
            get { return _camera; }
            set { _camera = value; }
        }
        protected Get<IContentRenderer<T>> _renderer = null;
        Get<IContentRenderer<T>> ILayer<T>.Renderer {
            get { return _renderer; }
            set { _renderer = value; }
        }


        #endregion


    }
}