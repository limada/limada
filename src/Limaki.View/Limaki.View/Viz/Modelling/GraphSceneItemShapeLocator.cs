/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2012 Lytico
 *
 * http://www.limada.org
 * 
 */


using Limaki.Graphs;
using Xwt;

namespace Limaki.View.Viz.Modelling {
    /// <summary>
    /// a locator that querys
    /// with IGraphScene.ItemShape
    /// assumes that every item has a shape
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    public class GraphSceneItemShapeLocator<TItem, TEdge> : ILocator<TItem>
        where TEdge : IEdge<TItem>, TItem {

        public IGraphScene<TItem, TEdge> GraphScene { get; set; }

        public Point GetLocation (TItem item) {
            var shape = GraphScene.ItemShape(item);
            return shape.Location;
        }

        public void SetLocation (TItem item, Point value) {
            var shape = GraphScene.ItemShape(item);
            var bounds = shape.BoundsRect;
            shape.Location = value;
            GraphScene.UpdateBounds(item, bounds);
        }

        public Size GetSize (TItem item) {
            var shape = GraphScene.ItemShape(item);
            return shape.Size;
        }

        public void SetSize (TItem item, Size value) {
            var shape = GraphScene.ItemShape(item);
            var bounds = shape.BoundsRect;
            shape.Size = value;
            GraphScene.UpdateBounds(item, bounds);
        }

        public bool HasLocation (TItem item) {
            return GraphScene.ItemShape(item) != null;
        }

        public bool HasSize (TItem item) {
            return GraphScene.ItemShape(item) != null;
        }
    }
}