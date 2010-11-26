/*
 * Limaki 
 * Version 0.071
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
    /// base class for mouse handling
    /// </summary>
    public abstract class MouseActionBase : ActionBase, IMouseAction {
        /// <summary>
        /// starts mouse action, sets resolved to false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnMouseDown(MouseEventArgs e) {
            _resolved = false;    
        }
        public virtual void OnMouseHover(MouseEventArgs e) {}

        /// <summary>
        /// handles mousemove 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public abstract void OnMouseMove ( MouseEventArgs e );


        protected virtual void EndAction() {
            if (_resolved) {
                _resolved = false;
            }            
        }

        /// <summary>
        /// ends mouse action, set resolved=false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnMouseUp(MouseEventArgs e) {
            EndAction ();
        }


        #region IDisposable Member

        #endregion
    }
}