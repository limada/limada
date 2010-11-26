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
using Limaki.Drawing;
using Limaki.Common;
using Limaki.Graphs;

namespace Limaki.Presenter.UI {
    /// <summary>
    /// Overrides Zooming; if a widget is hit, no zooming is performed
    /// </summary>
    public class GraphItemZoomAction<TItem, TEdge> : ZoomAction
        where TEdge : IEdge<TItem>, TItem{
        public GraphItemZoomAction():base() {}

        public Get<IGraphScene<TItem, TEdge>> SceneHandler { get; set; }
        
        public IGraphScene<TItem, TEdge> Scene {
            get { return SceneHandler(); }
        }

        private int _hitSize = 5;
        /// <summary>
        /// has to be the same as in WidgetResizer
        /// </summary>
        public int HitSize {
            get { return _hitSize; }
            set { _hitSize = value; }
        }

        public override void OnMouseUp(MouseActionEventArgs e) {
            var zoomTarget = this.Viewport ();
            PointI p = zoomTarget.Camera.ToSource(e.Location);
            TItem widget = Scene.Hit(p, HitSize);
            if (widget==null) {
                base.OnMouseUp (e);
            }
        }

        public override bool Check() {
            if (this.SceneHandler == null) {
                throw new CheckFailedException(this.GetType(), typeof(IGraphScene<TItem, TEdge>));
            }
            return base.Check();
        }
    }
}