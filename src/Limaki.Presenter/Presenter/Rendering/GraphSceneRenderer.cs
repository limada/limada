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
 * http://limada.sourceforge.net
 * 
 */


//#define TraceRenderVisual
//#define TraceRender
#define countVisuals

using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;

namespace Limaki.Presenter.Rendering {
    public class GraphSceneRenderer<TItem, TEdge> : ContentRenderer<IGraphScene<TItem, TEdge>>,
                                                    IGraphSceneRenderer<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {

        public virtual IGraphItemRenderer<TItem,TEdge> ItemRenderer { get; set; }

        public Get<IGraphLayout<TItem, TEdge>> Layout { get; set; }


#if countVisuals
        public int iItems = 0;
#endif

        public override void Render(IGraphScene<TItem, TEdge> data, IRenderEventArgs e) {
            var camera = this.Camera ();
            var clipBounds = camera.ToSource(e.Clipper.Bounds);

#if TraceRender
            System.Console.WriteLine ("***** clipBounds:\t" + clipBounds);
            //+"\toffset(g)"+new SizeS(g.Transform.OffsetX, g.Transform.OffsetY)
#endif

            TItem focused = data.Focused;
            TItem hovered = data.Hovered;
            
            var layout = this.Layout();
            ItemRenderer.Layout = this.Layout;

            foreach (TItem item in data.ElementsIn(clipBounds, ZOrder.EdgesFirst)) {
                bool rendered = true;
                if (!item.Equals(focused) && !item.Equals(hovered)) {
                    ItemRenderer.Render(item, e);

#if countVisuals
                    if (rendered)
                        iItems++;
#endif
                }
            }

            if (hovered != null && DrawingExtensions.Intersects(clipBounds, layout.GetShape(hovered).BoundsRect)) {
                ItemRenderer.Render(hovered, e);
            }

            if (focused != null && DrawingExtensions.Intersects(clipBounds, layout.GetShape(focused).BoundsRect)) {
                ItemRenderer.Render(focused, e);
            }
        }



        


    }
}