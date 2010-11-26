/*
 * Limaki 
 * Version 0.07
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

using System.Windows.Forms;
using Limaki.Actions;

namespace Limaki.Winform {
    public interface IKeyAction:IAction {

        void OnKeyDown (KeyEventArgs e) ;

        void OnKeyPress(KeyPressEventArgs e);

        void OnKeyUp ( KeyEventArgs e );

    }
}