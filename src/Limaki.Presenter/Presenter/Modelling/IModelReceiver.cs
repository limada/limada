/*
 * Limaki 
 * Version 0.081
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


using System;
using System.Collections.Generic;
using Limaki.Actions;

namespace Limaki.Presenter {
    /// <summary>
    /// Executes the commands and changes the GraphicsModel (Visuals)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IModelReceiver<T> {
        void Execute ( ICommand<T>  request);
        void Execute ( ICollection<ICommand<T>> requests );
        Action<ICommand<T>> BeforeExecute { get; set; }
        Action<ICommand<T>> AfterExecute { get; set; }
    }
}