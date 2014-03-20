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
using Xwt;

namespace Limaki.View.Vidgets {

    public class KeyActionEventArgs : Xwt.KeyEventArgs {

        public KeyActionEventArgs (Key key, ModifierKeys modifiers, Point location)
            : base (key, modifiers, false, Environment.TickCount) {
            this.Location = location;
        }

        public Point Location { get; protected set; }

        public int PlatformKeyCode {
            get {
                Registry.Pooled<IExceptionHandler> ()
                    .Catch (new Exception (this.GetType ().Name + ".PlatformKeyCode not implemented"), MessageType.OK);
                return 0;
            }
        }
    }
    
}