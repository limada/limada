/*
 * Limaki 
 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Common;
using Limaki.Drawing;
using Limaki.View.Rendering;

namespace Limaki.View.Rendering {

    public class SelectionRenderer : MoveResizeRenderer, IShapedSelectionRenderer {
        protected IPainter _painter = null;
        public IPainter Painter {
            get {
                if ((_painter == null) && (Shape != null)) {
                    var factory = Registry.Pooled<IPainterFactory>();
                    _painter = factory.CreatePainter(Shape);
                }
                return _painter;
            }
            set { _painter = value; }
        }

        public RenderType RenderType { get; set; }

        public override void OnPaint (IRenderEventArgs e) {
            if (Shape != null) {
 
                // we paint the Shape untransformed, otherwise it looses its line-size
                // that means, that the linesize is zoomed which makes an ugly effect

                if (RenderType != RenderType.None) {
                   
                    var save = SaveMatrix(e.Surface);
                    SetMatrix(e.Surface);

                    var paintShape = (IShape) this.Shape.Clone();
                    Camera.FromSource(paintShape);

                    Painter.RenderType = RenderType;
                    Painter.Shape = paintShape;
                    Painter.Style = this.Style;
                    Painter.Render(e.Surface);

                    //restore
                    RestoreMatrix(e.Surface,save);
                }

                // paint the grips
                base.OnPaint(e);
            }
        }

    }
}