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
 * 
 */

using Limaki.Actions;

namespace Limaki.Presenter.UI {
    public interface IMouseAction:IAction {
        /// <summary>
        /// starts mouse action, sets resolved to false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnMouseDown(MouseActionEventArgs e);

        void OnMouseHover(MouseActionEventArgs e);

        /// <summary>
        /// handles mousemove 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnMouseMove(MouseActionEventArgs e);

        /// <summary>
        /// ends mouse action, set resolved=false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnMouseUp(MouseActionEventArgs e);
    }
}