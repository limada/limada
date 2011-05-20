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

namespace Limaki.Presenter {
    /// <summary>
    /// executes a request-queue
    /// on the receiver's subject
    /// </summary>
    public interface IReceiver : IAction {
        void Execute();
        void Invoke();
        void Done();
    }
}