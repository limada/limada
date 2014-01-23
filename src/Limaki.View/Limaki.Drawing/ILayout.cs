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
 */

using System;
using System.Collections.Generic;
using Xwt;
using Xwt.Drawing;

namespace Limaki.Drawing {

    public interface ILayout {

        IStyleSheet StyleSheet { get;set;}

        /// <summary>
        /// performs a full layout
        /// </summary>
        void Reset();

        IShapeFactory ShapeFactory { get; set; }

        IPainterFactory PainterFactory { get; set; }
        IPainter GetPainter(Type type);

        Size Distance { get; set; }
        Size Border { get; set; }

    }


    public interface ILayout<TItem> : ILayout, IShaper<TItem> {

        /// <summary>
        /// Prepares TItem with Layout Data, eg. shapes
        /// gives back true if something is done
        /// gives back false if item was already invoked
        /// </summary>
        /// <param name="item"></param>
        bool Perform(TItem item);

        bool Perform(TItem item, IShape shape);

        void BoundsChanged(TItem target);

        /// <summary>
        /// Invalidates Item.Bounds to redraw the item
        /// </summary>
        void Refresh(TItem item);


        IStyle GetStyle(TItem item);
        IStyle GetStyle(TItem item, UiState uiState);

        Point[] GetDataHull(TItem item, Matrix matrix, int delta, bool extend);
        Point[] GetDataHull(TItem item, UiState uiState, Matrix matrix, int delta, bool extend);

        Point[] GetDataHull(TItem item, int delta, bool extend);
        Point[] GetDataHull(TItem item, UiState uiState, int delta, bool extend);

        IComparer<TItem> Comparer { get; set; } 
    }

}