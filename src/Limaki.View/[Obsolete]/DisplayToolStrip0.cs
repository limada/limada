/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Diagnostics;
using Limaki.View.Vidgets;
using Xwt;
using System;

namespace Limaki.View.Viz.Visualizers.ToolStrips {
    [Obsolete]
    public abstract class DisplayToolStrip0<TDisplay, TBackend> : ToolStrip
        where TDisplay : class
        where TBackend : class,IDisplayToolStripBackend {

        public virtual TDisplay CurrentDisplay { get; set; }
        public virtual new TBackend Backend { get { return BackendHost.Backend as TBackend; } }

        public virtual void Attach (object sender) {
            if (sender == null)
                return;
            var display = sender as TDisplay;
            if (display == null)
                Trace.WriteLine(this.GetType().Name + ": display not set");
            CurrentDisplay = display;
        }

        public abstract void Detach (object sender);


        public override void Dispose () {
            CurrentDisplay = null;
        }

        public Size DefaultSize = new Size(36, 36);
    }

    
}