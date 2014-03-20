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
using Xwt.Drawing;
using Xwt;

namespace Limaki.View.ContentViewers {
    
    public abstract class ContentViewer:IDisposable  {

        public Int64 ContentId { get; set; }

        public abstract IVidget Frontend { get; }

        /// <summary>
        /// attention! this is strange
        /// ContentViewer is not a Vidget in reality, so it has no backend
        /// this is in the most cases the Frontend.Backend
        /// </summary>
        public abstract IVidgetBackend Backend { get; }

        private Color? _backColor;
        public virtual Color BackColor {
            get { return (_backColor ?? (_backColor = SystemColors.Background)).Value; }
            set { _backColor = value; }
        }

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
            get { return Registry.Pooled<IExceptionHandler>(); }
        }

        public abstract void Dispose();
    }
}