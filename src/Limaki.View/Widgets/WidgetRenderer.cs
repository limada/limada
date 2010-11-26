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
 * 
 */

using Limaki.Drawing;

namespace Limaki.Widgets {
    public class WidgetRenderer:Renderer {
        public WidgetRenderer(Renderer parent):base(parent) {}
        
        public virtual void Render(ISurface surface, IWidget widget) {
            IStyle style = Layout.GetStyle (widget);
            IShape shape = widget.Shape;

            IPainter shapePainter = Layout.GetPainter(widget.Shape.GetType());
            if (shapePainter != null) {
                shapePainter.Shape = shape;
                shapePainter.Style = style;
                shapePainter.Render(surface);
            }

            bool paintData = style.PaintData;
            if (paintData) {
                IDataPainter dataPainter = Layout.GetPainter (widget.Data.GetType ()) as IDataPainter;
                if (dataPainter != null) {
                    dataPainter.Data = widget.Data;
                    dataPainter.Style = style;
                    dataPainter.Shape = shape;
                    dataPainter.Render(surface);
                }
            }
        }
        
    }
}