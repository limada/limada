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
using System.Drawing;
using System.Windows.Forms;
using Limada.Model;
using System.IO;
using Limaki.Model.Streams;

namespace Limaki.App.StreamViewers {
    public abstract class StreamViewerController:IDisposable {
        
        public Int64 CurrentThingId = 0;
        
        public abstract bool CanView ( Int64 streamType );
        public abstract bool CanSave();
        public abstract void SetContent ( StreamInfo<Stream> info );
        public abstract void Save( StreamInfo<Stream> info);

        public virtual bool IsStreamOwner { get; set; }

        public virtual void OnShow(){}

        public abstract Control Control { get; }
        
        public System.Drawing.Color BackColor = SystemColors.Control;
        public Control Parent = null;

        public event Action<Control> Attach = null;
        public event Action<Control> DeAttach = null;

        protected virtual void OnAttach(Control control) {
            if (Attach != null) {
                Attach(control);
            }
        }

        public abstract void Dispose();
        public virtual void Clear() {
            CurrentThingId = 0;
        }
    }
}