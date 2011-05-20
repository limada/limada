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


using Limaki.Drawing;

namespace Limaki.Presenter.UI {
    public interface IUISystemInformation {
        SizeI DragSize { get; }
        int DoubleClickTime { get; }
    }
}