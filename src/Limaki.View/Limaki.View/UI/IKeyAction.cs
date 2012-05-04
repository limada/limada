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

using Limaki.Actions;

namespace Limaki.View.UI {
    public interface IKeyAction:IAction {

        void OnKeyDown ( KeyActionEventArgs e ) ;

        void OnKeyPress( KeyActionPressEventArgs e );

        void OnKeyUp ( KeyActionEventArgs e );

    }
}