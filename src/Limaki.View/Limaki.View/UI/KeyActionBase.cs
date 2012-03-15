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
 * http://limada.sourceforge.net
 */

using Limaki.Actions;

namespace Limaki.View.UI {
    /// <summary>
    /// Base class for key handling
    /// </summary>
    public abstract class KeyActionBase : ActionBase, IKeyAction {

        public virtual void OnKeyDown( KeyActionEventArgs e ) {
            _resolved = false;    
        }

        public virtual void OnKeyPress( KeyActionPressEventArgs e ) { }

        public virtual void OnKeyUp( KeyActionEventArgs e ) {
            if (_resolved) {
                _resolved = false;
            }
        }

    }
}