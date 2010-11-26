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
 * 
 */

//#define TraceRenderWidget
//#define TraceRender
//#define useGNURegion
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

        public virtual void Render(Graphics g, Region region) {

            Region clip = g.Clip;
            RectangleF clipBounds = clip.GetBounds(g);
            if (region != null) {
                clip = region;
            }

#if TraceRender
            System.Console.WriteLine("***** g.ClipBounds:\t" + g.ClipBounds +
                                      "\tclipBounds:\t" + clipBounds +
                                      "\tclip.GetBounds(g)\t" + clip.GetBounds(g));
#endif
            
            // Scene gives first items, then links:
            foreach (IWidget widget in Scene.ElementsIn(clipBounds)) {
                //bool rendered = Render(g, widget, clip, clipBounds);
                // lets try this:
                bool rendered = true;
                GetRenderer(widget).Render(g, widget);
#if TraceRenderWidget
                Rectangle widgetBounds = widget.Shape.BoundsRect;
                if (rendered) {
                    System.Console.WriteLine(widget + " bounds: " + widgetBounds);
                } else
                    if (g.ClipBounds.IntersectsWith(clipBounds)) {
                        System.Console.WriteLine("not visible:\t" + widget + "bounds:" + widgetBounds);
                    }

#endif
#if countWidgets
                if (rendered)
                    iWidgets++;
#endif
            }
        }

        
        bool Render(Graphics g, IWidget widget, Region clip, RectangleF clipBounds) {
            Rectangle bounds = widget.Shape.BoundsRect;
            bounds.Inflate(1,1);
            if (clip.IsVisible(bounds, g)) {
                GetRenderer(widget).Render(g, widget);
                return true;
            }
            return false;
        }

      
    }
}