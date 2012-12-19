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
using Xwt;
using Xwt.Drawing;

namespace Limaki.Drawing {
    public interface ILayout {

        IStyleSheet StyleSheet { get;set;}
        /// <summary>
        /// performs a full layout
        /// </summary>
        void Invoke();

        IShapeFactory ShapeFactory { get; set; }

        IPainterFactory PainterFactory { get; set; }
        IPainter GetPainter(Type type);

        Size Distance { get; set; }
        Size Border { get; set; }

    }


    public interface ILayout<TItem> : ILayout,IShaper<TItem> {

        /// <summary>
        /// Prepares TItem with Layout Data, eg. shapes
        /// gives back true if something is done
        /// gives back false if item was already invoked
        /// </summary>
        /// <param name="item"></param>
        bool Invoke(TItem item);

        bool Invoke(TItem item, IShape shape);

        void AddBounds(TItem target);

        /// <summary>
        /// Invalidates Item.Bounds to redraw the item
        /// </summary>
        void Perform(TItem item);


        IStyle GetStyle(TItem item);
        IStyle GetStyle(TItem item, UiState uiState);

        Point[] GetDataHull(TItem item, Matrix matrix, int delta, bool extend);
        Point[] GetDataHull(TItem item, UiState uiState, Matrix matrix, int delta, bool extend);

        Point[] GetDataHull(TItem item, int delta, bool extend);
        Point[] GetDataHull(TItem item, UiState uiState, int delta, bool extend);

        Func<TItem, string> OrderBy { get; set; }
    }

    public enum Order {
        Pre,
        Post
    }

    public enum Algo {
        DepthFirst,
        BreathFirst
    }
}