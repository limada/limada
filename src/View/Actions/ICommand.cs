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
namespace Limaki.Actions {
    public interface ICommand : IDisposable {
        void Execute();
    }

    public interface ICommand<T> : ICommand {
        T Target { get; set; }
    }
    public interface ICommand<T,P>:ICommand<T> {
        P Parameter { get; set; }
    }
}