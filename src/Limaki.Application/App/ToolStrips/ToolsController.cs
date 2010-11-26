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
using System.Windows.Forms;
using Limaki.Winform.Displays;

namespace Limaki.Winform.Controls.ToolStrips {
    public abstract class ToolsController<TDisplay, TControl> {
        public Func<TDisplay> DisplayHandler = null;
        public Func<TControl> ControlHandler = null;

        protected virtual TDisplay CurrentDisplay {
            get {
                if (DisplayHandler != null) {
                    return DisplayHandler ();
                }
                return default(TDisplay);
            }
        }

        protected virtual TControl CurrentControl {
            get {
                if (ControlHandler != null) {
                    return ControlHandler();
                }
                return default(TControl);
            }
        }

        public abstract void Attach();
    }
}