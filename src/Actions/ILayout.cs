/*
 * Limaki 
 * Version 0.064
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

using Limaki.Widgets;
using Limaki.Common;
using Limaki.Drawing;

namespace Limaki.Actions {
    public interface ILayout {
        /// <summary>
        /// performs a full layout
        /// </summary>
        void Invoke();
    }

    public interface ILayout<TData,TItem>:ILayout {
        Handler<TData> DataHandler { get;set;}

        TData Data { get; }

        /// <summary>
        /// Prepares TItem with Layout Data, eg. shapes
        /// </summary>
        /// <param name="item"></param>
        void Invoke ( TItem item );

        /// <summary>
        /// Sets position and others things on target
        /// </summary>
        void Justify(TItem target);

        /// <summary>
        /// Invalidates Item.Bounds to redraw the item
        /// </summary>
        void Perform(TItem item);

        IStyle GetStyle ( TItem item );
    }

    /// <summary>
    /// Used to call specific layout methods in CommandsAction
    /// </summary>
    public enum LayoutActionType {
        Invoke, Justify, Perform
    }

    public class LayoutCommand<TItem> : Command<TItem, LayoutActionType> {
        public LayoutCommand ( TItem item, LayoutActionType action ) : base (item, action) {}
    }
}