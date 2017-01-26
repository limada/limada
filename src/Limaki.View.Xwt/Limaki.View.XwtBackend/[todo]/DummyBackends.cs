/*
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

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.View;
using Limaki.View.Vidgets;
using Xwt;

namespace Limaki.View.XwtBackend {

    public class DummyBackend : Frame, IVidgetBackend {

        #region IVidgetBackend

        public IVidget Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = frontend;
        }

        IVidgetEventSink EventSink { get; set; }
        public void InitializeEvents (IVidgetEventSink eventSink) {
            EventSink = eventSink;
        }

        void IVidgetBackend.Update () { XwtBackendHelper.VidgetBackendUpdate(this); }

        void IVidgetBackend.QueueDraw () { XwtBackendHelper.VidgetBackendInvalidate(this); }

        void IVidgetBackend.QueueDraw (Rectangle rect) { XwtBackendHelper.VidgetBackendInvalidate(this, rect); }

        public string ToolTipText { get; set; }


        #endregion

    }
}
