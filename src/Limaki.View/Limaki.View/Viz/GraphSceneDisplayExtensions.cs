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
using Limaki.Graphs;
using Limaki.View.Common;
using Xwt;
using Xwt.Drawing;

namespace Limaki.View.Viz {

    public static class GraphSceneDisplayExtensions {

        public static void Clear<TItem, TEdge>(this IGraphSceneDisplay<TItem, TEdge> d) where TEdge : TItem, IEdge<TItem> {
            d.Info = new SceneInfo { };
            d.DataId = 0;
            d.Text = string.Empty;
        }

        public static void Wink<TItem, TEdge> (this IGraphSceneDisplay<TItem, TEdge> display) where TEdge : TItem, IEdge<TItem> {
            if (display == null)
                return;
            
            var color = display.BackColor;
            var zoomState = display.ZoomState;
            var zoomFactor = display.Viewport.ZoomFactor;

            Action<Color, ZoomState, double> setDispay = (c, s, f) => {
                display.BackColor = c;
                display.ZoomState = s;
                display.Viewport.ZoomFactor = f;
                display.Viewport.UpdateZoom ();
                Application.MainLoop.DispatchPendingEvents ();
            };

            Application.MainLoop.QueueExitAction (() => {
                setDispay (Colors.AliceBlue, ZoomState.Custom, zoomFactor + 0.1);
                Application.TimeoutInvoke (10, () => { setDispay (color, zoomState, zoomFactor); display.QueueDraw ();return false;});

            });

            Application.MainLoop.DispatchPendingEvents ();
        }
    }
}