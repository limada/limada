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
 */

using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using Limaki.Actions;

namespace Limaki.Winform {
    /// <summary>
    /// Base class for key handling
    /// </summary>
    public abstract class KeyActionBase : ActionBase, IKeyAction {
         #region IKeyAction Member

        public virtual void OnKeyDown(KeyEventArgs e) {
            _resolved = false;    
        }

        public virtual void OnKeyPress(KeyPressEventArgs e) { }

        public virtual void OnKeyUp(KeyEventArgs e) {
            if (_resolved) {
                _resolved = false;
            }
        }

        #endregion 



      
    }
}