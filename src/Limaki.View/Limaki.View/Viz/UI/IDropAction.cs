/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Actions;
using Limaki.View.DragDrop;

namespace Limaki.View.Viz.UI {

    public interface IDropAction : IAction, IDropHandler {
        bool Dragging { get; set; }
    }

}