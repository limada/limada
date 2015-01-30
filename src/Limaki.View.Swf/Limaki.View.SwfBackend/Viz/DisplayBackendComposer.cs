/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.View.GdiBackend;
using Limaki.View.Viz.Rendering;
using Limaki.View.Viz.UI;
using Limaki.View.Viz.Visualizers;

namespace Limaki.View.SwfBackend.Viz {

    public class DisplayBackendComposer<TData> : BackendComposer<TData, DisplayBackend<TData>> {
        
        public ActionDispatcher ActionDispatcher { get; set; }
        public override void Factor(Display<TData> display) {
            
            display.Backend = Backend;

            var surfaceRenderer = new SwfBackendRenderer<TData> {
                Backend = this.Backend,
                Display = display
            };
			
            this.BackendRenderer = surfaceRenderer;

            this.ActionDispatcher = new ActionDispatcher ();

            this.ViewPort = new SwfViewport (Backend);
            this.CursorHandler = new CursorHandlerBackend (Backend);

            this.SelectionRenderer = new  SelectionRenderer();
            this.MoveResizeRenderer = new MoveResizeRenderer ();
        }

        public override void Compose(Display<TData> display) {
            this.BackendRenderer.BackColor = () => display.BackColor;
            Backend.BackendRenderer = this.BackendRenderer;
            Backend.BackendViewPort = this.ViewPort;

            display.BackendRenderer = this.BackendRenderer;
            display.DataLayer = this.DataLayer;
            display.ActionDispatcher = this.ActionDispatcher;
            display.Viewport = this.ViewPort;
            display.CursorHandler = this.CursorHandler;

            this.MoveResizeRenderer.Backend = this.Backend;
            ComposeSelectionRenderer(this.MoveResizeRenderer);
            display.MoveResizeRenderer = this.MoveResizeRenderer;
            
            this.SelectionRenderer.Backend = this.Backend;
            ComposeSelectionRenderer(this.SelectionRenderer);
            display.SelectionRenderer = this.SelectionRenderer;
            
        }

        public void ComposeSelectionRenderer (ISelectionRenderer renderer) {
            var emptyMatrix = new System.Drawing.Drawing2D.Matrix();
            renderer.SaveMatrix = s => ((GdiSurface) s).Graphics.Transform;
            renderer.SetMatrix = s => ((GdiSurface) s).Graphics.Transform = emptyMatrix;
            renderer.RestoreMatrix = (s, o) => ((GdiSurface) s).Graphics.Transform = (System.Drawing.Drawing2D.Matrix) o;
            //TODO: dispose emptyMatrix
        }
    }

}