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

using Limaki.Widgets;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Painters;
using System;
using System.Drawing;
using System.Collections.Generic;

namespace Limaki.Actions {
    public interface ILayout {

        IStyleSheet StyleSheet { get;set;}
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
        IStyle GetStyle(TItem item, UiState uiState);

        PainterFactory PainterFactory { get;set;}
        IPainter GetPainter ( Type type );

        Point[] GetDataHull ( TItem item, Matrice matrix, int delta, bool extend );
        Point[] GetDataHull(TItem item, UiState uiState, Matrice matrix, int delta, bool extend);

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