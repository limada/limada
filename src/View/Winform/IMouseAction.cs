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
 * 
 */

using System.Windows.Forms;
using Limaki.Actions;

namespace Limaki.Winform {
    public interface IMouseAction:IAction {
        /// <summary>
        /// starts mouse action, sets resolved to false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnMouseDown ( MouseEventArgs e );

        void OnMouseHover ( MouseEventArgs e );

        /// <summary>
        /// handles mousemove 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnMouseMove ( MouseEventArgs e );

        /// <summary>
        /// ends mouse action, set resolved=false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnMouseUp ( MouseEventArgs e );
    }
}