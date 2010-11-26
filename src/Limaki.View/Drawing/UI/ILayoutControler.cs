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


using Limaki.Actions;

namespace Limaki.Drawing.UI {
    /// <summary>
    /// Executes a ICommand-Collection
    /// </summary>
    public interface ILayoutControler:IAction {

        /// <summary>
        /// Prepares the data
        /// </summary>
        void Invoke();

        /// <summary>
        /// Executes ICommands in an ICommand-Collection
        /// </summary>
        void Execute();

        /// <summary>
        /// Revoves all executed commands
        /// </summary>
        void Done();
        ILayout Layout { get;set;}

    }

    /// <summary>
    /// Decouples Data (Model) from User-Commands (View)
    /// uses a Layout
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    public interface ILayoutControler<TData,TItem>:ILayoutControler {
        TData Data { get; }
        void Execute(ICommand<TItem> command);
        new ILayout<TData, TItem> Layout {get;set;}
    }
}