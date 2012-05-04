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

namespace Limaki.View.Layout {
    public class LayoutCommand<TItem> : Command<TItem, LayoutActionType> {
        public LayoutCommand ( TItem item, LayoutActionType action ) : base (item, action) {}
    }

    /// <summary>
    /// Used to call specific layout methods in CommandsAction
    /// </summary>
    public enum LayoutActionType {
        Invoke, Justify, Perform, AddBounds
    }

    public class LayoutCommand<TItem, IShape> : Command<TItem, LayoutActionType, IShape> {
        public LayoutCommand(TItem item, IShape shape, LayoutActionType action) : base(item, action, shape) { }
    }
}