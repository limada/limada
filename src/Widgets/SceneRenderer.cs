/*
 * Limaki 
 * Version 0.063
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

#define countWidgets

using System.Drawing;
using System.Drawing.Drawing2D;
using Limaki.Drawing;

namespace Limaki.Widgets {

    public class SceneRenderer : Renderer {
        public SceneRenderer(Renderer parent) : base(parent) { }

        private WidgetRenderer _widgetRenderer = null;
        WidgetRenderer GetRenderer(IWidget widget) {
            if (_widgetRenderer == null) {
                _widgetRenderer = new WidgetRenderer(this);
            }
            return _widgetRenderer;

        }

#if countWidgets
        public int iWidgets = 0;
#endif
        public virtual void Render(Graphics g) {
            Region clip = g.Clip;

            RectangleF clipBounds = clip.GetBounds(g);

            // Scene gives first items, then links:
            foreach (IWidget widget in Scene.Widgets) {
                Render(g, widget, clip, clipBounds);
            }

		}

        
        void Render(Graphics g, IWidget widget, Region clip, RectangleF clipBounds) {
            Rectangle bounds = widget.Shape.BoundsRect;
            // this is necessary, otherwise sometimes occurencies of non visible borders:
            bounds.Inflate(1,1);
            if (clipBounds.IntersectsWith(bounds) && clip.IsVisible(bounds,g)) {
                GetRenderer(widget).Render(g, widget);

#if countWidgets
                iWidgets++;
#endif
            }
        }

        
    }
}
