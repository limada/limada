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
using System.Collections.Generic;
using System;
using Limaki.Presenter.Rendering;

namespace Limaki.Presenter.UI {
    public interface IEventControler : IMouseAction, IRenderAction, IReceiver, IKeyAction {
        void Add ( IAction action );
        void Remove ( IAction action );
        void Add<T> ( T value, ref T action ) where T : class, IAction;
        T GetAction<T>();
        IDictionary<Type, IAction> Actions { get;}
        bool UserEventsDisabled { get; set; }
    }
}