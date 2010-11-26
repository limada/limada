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

//#define TraceRenderWidget
//#define TraceRender
//#define useGNURegion
#define countWidgets

using Limaki.Drawing.GDI;
using Limaki.Drawing.Shapes;
using Limaki.Drawing;


namespace Limaki.Widgets.Paint {
    public class GDISceneRenderer : Renderer {
        public GDISceneRenderer(Renderer parent) : base(parent) { }

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

        public virtual void Render(ISurface surface) {
            System.Drawing.Graphics g = ((GDISurface)surface).Graphics;
            System.Drawing.Region clip = g.Clip;
            RectangleS clipBounds = GDIConverter.Convert(clip.GetBounds(g));
            //if (region != null) {
            //    clip = region;
            //}

#if TraceRender
            System.Console.WriteLine("***** g.ClipBounds:\t" + g.ClipBounds +
                                      "\tclipBounds:\t" + clipBounds +
                                      "\tclip.GetBounds(g)\t" + clip.GetBounds(g)+
                                      "\toffset(g)"+new SizeS(g.Transform.OffsetX, g.Transform.OffsetY)
            );
#endif

            IWidget focused = Scene.Focused;
            IWidget hovered = Scene.Hovered;
            foreach (IWidget widget in Scene.ElementsIn(clipBounds,ZOrder.EdgesFirst)) {
                //bool rendered = Render(g, widget, clip, clipBounds);
                // lets try this:
                bool rendered = true;
                if (widget != focused && widget != hovered) {
                    GetRenderer (widget).Render (surface, widget);
#if TraceRenderWidget
                RectangleI widgetBounds = widget.Shape.BoundsRect;
                if (rendered) {
                    System.Console.WriteLine(widget + " bounds: " + widgetBounds);
                } else
                    if (clipBounds.IntersectsWith(widgetBounds)) {
                        System.Console.WriteLine("not visible:\t" + widget + "bounds:" + widgetBounds);
                    }

#endif
#if countWidgets
                    if (rendered)
                        iWidgets++;
#endif
                }
            }
            
            if (hovered != null && ShapeUtils.Intersects(clipBounds, hovered.Shape.BoundsRect)) {
                GetRenderer(hovered).Render(surface, hovered);
            }
            
            if (focused != null && ShapeUtils.Intersects(clipBounds, focused.Shape.BoundsRect)) {
                GetRenderer(focused).Render(surface, focused);
            }
            

        }

        
        bool Render(ISurface surface, IWidget widget, System.Drawing.Region clip, RectangleS clipBounds) {
            System.Drawing.Graphics g = ((GDISurface)surface).Graphics;
            System.Drawing.Rectangle bounds = 
                GDIConverter.Convert(widget.Shape.BoundsRect);

            bounds.Inflate(1,1);
            if (clip.IsVisible(bounds, g)) {
                GetRenderer(widget).Render(surface, widget);
                return true;
            }
            return false;
        }

      
    }
}