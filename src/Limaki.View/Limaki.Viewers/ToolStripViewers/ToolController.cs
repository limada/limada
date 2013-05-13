/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2010 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Diagnostics;
namespace Limaki.Viewers.ToolStripViewers {

    public abstract class ToolController<TDisplay, TTool>
        where TDisplay : class {
        
        public virtual TTool Tool { get; set; }
        public virtual TDisplay CurrentDisplay { get; set; }

        public virtual void Attach(object sender) {
            var display = sender as TDisplay;
            if (display != null)
                CurrentDisplay = display;
            else
                Trace.WriteLine("ToolController: display not set");
        }

        public abstract void Detach(object sender);
    }
}