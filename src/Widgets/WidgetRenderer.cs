/*
 * Limaki 
 * Version 0.064
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

using System.Drawing;
using Limaki.Drawing;
using Limaki.Drawing.Painters;

namespace Limaki.Widgets {

    public class WidgetRenderer:Renderer {
        public WidgetRenderer(Renderer parent):base(parent) {}
        
        private StringPainter stringPainter = new StringPainter ();

        public virtual void Render(Graphics g, IWidget widget) {
            IStyle style = Layout.GetStyle (widget);
            IShape shape = widget.Shape;

            IPainter painter = GetPainter(widget.Shape.GetType());
            if (painter != null) {
                painter.Shape = shape;
                painter.Style = style;
                painter.Render(g);
            }

            bool drawText = !( widget is ILinkWidget ); // we don't draw Text on Links in this release
            if (drawText) { 
                stringPainter.Text = widget.Data.ToString();
                stringPainter.Style = style;
                stringPainter.Shape = shape;
                stringPainter.Render(g);
            }
        }
        
    }
}