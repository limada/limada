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
 */

using Limaki.Actions;

namespace Limaki.View.UI {
    /// <summary>
    /// Base class for key handling
    /// </summary>
    public abstract class KeyActionBase : ActionBase, IKeyAction {

        public virtual void OnKeyPressed( KeyActionEventArgs e ) {
            _resolved = false;    
        }


        public virtual void OnKeyReleased( KeyActionEventArgs e ) {
            if (_resolved) {
                _resolved = false;
            }
        }

    }
}