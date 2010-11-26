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
using System.IO;
using Limaki.Drawing;
using Limaki.Model.Streams;
using Limaki.Common;

namespace Limaki.UseCases.Viewers {
    public abstract class StreamViewerController:IDisposable {
        
        public Int64 CurrentThingId = 0;
        
        public abstract bool CanView ( Int64 streamType );
        public abstract bool CanSave();
        
        public abstract void SetContent ( StreamInfo<Stream> info );
        public abstract void Save( StreamInfo<Stream> info);

        public virtual bool IsStreamOwner { get; set; }

        public virtual void OnShow(){}

        public abstract object Control { get; }
        
        public Color BackColor = KnownColors.FromKnownColor(KnownColor.Control);
        public object Parent {get;set;}

        public event Action<object> Attach = null;
        public event Action<object> DeAttach = null;

        protected virtual void OnAttach(object control) {
            if (Attach != null) {
                Attach(control);
            }
        }

        public abstract void Dispose();
        public virtual void Clear() {
            CurrentThingId = 0;
        }

        protected IExceptionHandler ExceptionHandler {
            get { return Registry.Pool.TryGetCreate<IExceptionHandler>(); }
        }
    }
}