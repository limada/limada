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


using System;
using System.Collections.Generic;
using Limaki.Actions;

namespace Limaki.View.Viz.Modelling {

    /// <summary>
    /// Performs the commands and changes the GraphicsModel (Visuals)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICommandModeller<T> {

        void Perform ( ICommand<T>  request);
        void Perform ( ICollection<ICommand<T>> requests );

        Action<ICommand<T>> BeforePerform { get; set; }
        Action<ICommand<T>> AfterPerform { get; set; }

    }
}