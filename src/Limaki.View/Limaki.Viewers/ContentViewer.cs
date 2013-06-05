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

    public abstract class ContentViewer : IVidget {

        public Int64 ContentId { get; set; }
        
        public virtual void OnShow() { }

        public abstract IVidgetBackend Backend { get; }

        private Color? _backColor;
        public virtual Color BackColor {
            get { return (_backColor??(_backColor= SystemColors.Background)).Value; }
            set { _backColor = value; }
        }
        public object Parent { get; set; }

        public event Action<IVidgetBackend> Attach = null;
        public event Action<IVidgetBackend> DeAttach = null;

        protected virtual void OnAttach (IVidgetBackend backend) {
            if (Attach != null) {
                Attach(backend);
            }
        }

        public abstract void Dispose();
        public virtual void Clear() {
            ContentId = 0;
        }

        protected IExceptionHandler ExceptionHandler {
            get { return Registry.Pool.TryGetCreate<IExceptionHandler>(); }
        }
    }
}