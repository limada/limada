/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2012-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
namespace Limaki.View.Vidgets {

    [Obsolete]
    public interface IToolStripCommandToggle0 {
        ToolStripCommand0 Command { get; set; }
        IToolStripCommandToggle0 ToggleOnClick { get; set; }
    }
}