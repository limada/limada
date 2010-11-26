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

using Limaki.Actions;

namespace Limaki.Presenter.UI {
    public interface IKeyAction:IAction {

        void OnKeyDown ( KeyActionEventArgs e ) ;

        void OnKeyPress( KeyActionPressEventArgs e );

        void OnKeyUp ( KeyActionEventArgs e );

    }
}