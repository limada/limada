/*
 * Limaki 
 * Version 0.08
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

namespace Limaki.Drawing {
    public interface ILayout {

        IStyleSheet StyleSheet { get;set;}
        /// <summary>
        /// performs a full layout
        /// </summary>
        void Invoke();
    }

    public interface ILayout<TData,TItem>:ILayout {
        Func<TData> DataHandler { get;set;}

        TData Data { get; }

        IShapeFactory ShapeFactory { get;set;}

        /// <summary>
        /// Gives back a propriate shape for this item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        IShape CreateShape ( TItem item );

        /// <summary>
        /// Prepares TItem with Layout Data, eg. shapes
        /// gives back true if something is done
        /// gives back false if item was already invoked
        /// </summary>
        /// <param name="item"></param>
        bool Invoke ( TItem item );


        bool Invoke(TItem item, IShape shape);

        /// <summary>
        /// Sets position and others things on target
        /// </summary>
        void Justify(TItem target);

        void AddBounds(TItem target);

        /// <summary>
        /// Sets position and others things on shape
        /// </summary>
        void Justify(TItem target, IShape shape);

        /// <summary>
        /// Invalidates Item.Bounds to redraw the item
        /// </summary>
        void Perform(TItem item);
        

        IStyle GetStyle ( TItem item );
        IStyle GetStyle(TItem item, UiState uiState);

        IPainterFactory PainterFactory { get;set;}
        IPainter GetPainter ( Type type );

        PointI[] GetDataHull ( TItem item, Matrice matrix, int delta, bool extend );
        PointI[] GetDataHull(TItem item, UiState uiState, Matrice matrix, int delta, bool extend);

        SizeI Distance {get; set;}
    }
}