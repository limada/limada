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
using Xwt;
using DragEventArgs = Limaki.View.DragDrop.DragEventArgs;
using DragOverEventArgs = Limaki.View.DragDrop.DragOverEventArgs;

namespace Limaki.View.UI {
    /// <summary>
    /// base class for mouse handling
    /// </summary>
    public abstract class MouseActionBase : ActionBase, IMouseAction {
        /// <summary>
        /// starts mouse action, sets resolved to false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnMouseDown(MouseActionEventArgs e) {
            _resolved = false;    
        }

        public virtual void OnMouseHover(MouseActionEventArgs e) { }

        /// <summary>
        /// handles mousemove 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public abstract void OnMouseMove(MouseActionEventArgs e);


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
        public virtual void OnMouseUp(MouseActionEventArgs e) {
            EndAction ();
        }

        public static void ForewardDragOver (IMouseAction baseAction, IVidgetBackend backend, DragOverEventArgs e) {
            if (baseAction.Enabled) {
                var pt = e.Position;
                baseAction.OnMouseMove(new MouseActionEventArgs(MouseActionButtons.None, ModifierKeys.None, 0, pt.X, pt.Y, 0));
            }
        }

        #region IDisposable Member

        #endregion
    }
}