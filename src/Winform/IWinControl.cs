/*
 * Limaki 
 * Version 0.064
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


using System.Drawing;
using System.Windows.Forms;
using Limaki.Drawing;

namespace Limaki.Winform {
    /// <summary>
    /// extends IControl with Cursor
    /// </summary>
    public interface IWinControl:IControl {
        Cursor Cursor { get;set;}
        Point PointToClient( Point source );
    }
}
