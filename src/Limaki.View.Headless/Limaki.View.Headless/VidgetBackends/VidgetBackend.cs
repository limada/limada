﻿/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.View;
using Limaki.View.Vidgets;
using Limaki.View.Viz;
using Xwt;

namespace Limaki.View.Headless.VidgetBackends {

    public class VidgetBackend : IVidgetBackend {

        public virtual void Update () {
            this.QueueDraw();
        }

        public virtual void QueueDraw () {
            if (Frontend is IDisplay) {
                ((IDisplay)Frontend).BackendRenderer.Render ();

            }
        }

        public virtual void SetFocus () { }

        public virtual void QueueDraw (Rectangle rect) {
            if(Frontend is IDisplay) {
                ((IDisplay) Frontend).BackendRenderer.Render();

            }
        }

        public virtual IVidget Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = frontend;
        }

        IVidgetEventSink EventSink { get; set; }
        public void InitializeEvents (IVidgetEventSink eventSink) {
            EventSink = eventSink;
        }

        public virtual Size Size { get; set; }
        
        public string ToolTipText { get; set; }

        public void Dispose () {
           
        }


    }
}
