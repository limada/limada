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
using Limaki.Common;
using Limaki.Drawing;
using Limaki.View;
using Xwt.Drawing;

namespace Limaki.Viewers {
    
    public abstract class ContentViewer:IDisposable  {

        public Int64 ContentId { get; set; }

        /// <summary>
        /// attention! this is strange
        /// ContentViewer is not a Vidget in reality, so it has no backend
        /// its a composition of ImageDisplay and ContentStreamViewer
        /// </summary>
        public abstract IVidgetBackend Backend { get; }

        public abstract IVidget Frontend { get; }

        private Color? _backColor;
        public virtual Color BackColor {
            get { return (_backColor ?? (_backColor = SystemColors.Background)).Value; }
            set { _backColor = value; }
        }
        public object Parent { get; set; }

        public Action<IVidgetBackend> AttachBackend { get; set; }
        public Action<IVidgetBackend> DetachBackend { get; set; }

        protected virtual void OnAttachBackend (IVidgetBackend backend) {
            if (AttachBackend != null) {
                AttachBackend(backend);
            }
        }

        public virtual void OnShow () { }

        public virtual void Clear() {
            ContentId = 0;
        }

        protected IExceptionHandler ExceptionHandler {
            get { return Registry.Pool.TryGetCreate<IExceptionHandler>(); }
        }

        public abstract void Dispose();
    }
}