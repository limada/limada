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
 * 
 */


using Limaki.Actions;
using System.Collections.Generic;
using System;
using Limaki.View.Viz.Rendering;

namespace Limaki.View.Viz.UI {

    public interface IActionDispatcher : IMouseAction, IRenderAction, IReceiver, IKeyAction, ICopyPasteAction {
        void Add ( IAction action );
        void Remove ( IAction action );
        void Add<T> ( T value, ref T action ) where T : class, IAction;
        T GetAction<T>();
        IEnumerable<IAction> Actions { get;}
        bool UserEventsDisabled { get; set; }
    }

       
    }
}