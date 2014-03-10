/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Drawing.XwtBackend;
using Limaki.View.Visualizers;
using Limaki.View.UI;
using Limaki.View.XwtBackend;
using Limaki.View.Rendering;
using Limaki.Drawing;

namespace Limaki.View.XwtBackend {

    public class DisplayBackendComposer<TData> : BackendComposer<TData, DisplayBackend<TData>> {
        
        public EventControler EventControler { get; set; }
        public override void Factor(Display<TData> display) {
            
            display.Backend = Backend;

            var surfaceRenderer = new XwtBackendRenderer<TData> {
                Backend = this.Backend,
                Display = display
            };
			
            this.BackendRenderer = surfaceRenderer;

            this.EventControler = new EventControler ();
            this.ViewPort = new XwtViewport (Backend);
            this.CursorHandler = new XwtCursorHandlerBackend (Backend);

            this.SelectionRenderer = new  SelectionRenderer();
            this.MoveResizeRenderer = new MoveResizeRenderer ();
          
        }

        public override void Compose(Display<TData> display) {
            this.BackendRenderer.BackColor = () => display.BackColor;
            Backend.BackendRenderer = this.BackendRenderer;
            Backend.BackendViewPort = this.ViewPort;

            display.BackendRenderer = this.BackendRenderer;
            display.DataLayer = this.DataLayer;
            display.EventControler = this.EventControler;
            display.Viewport = this.ViewPort;
            display.CursorHandler = this.CursorHandler;

            this.MoveResizeRenderer.Backend = this.Backend;
            ComposeSelectionRenderer(MoveResizeRenderer);
            display.MoveResizeRenderer = this.MoveResizeRenderer;
            

            this.SelectionRenderer.Backend = this.Backend;
            ComposeSelectionRenderer(SelectionRenderer);
            display.SelectionRenderer = this.SelectionRenderer;
           
            
        }

        public void ComposeSelectionRenderer (ISelectionRenderer renderer) {
            renderer.SaveMatrix = s => {
                ((ContextSurface) s).Context.Save();
                return null;
            };
            renderer.RestoreMatrix = (s, o) => ((ContextSurface) s).Context.Restore();
            renderer.SetMatrix = s => {
                ((ContextSurface) s).Context.Scale(1, 1);
            };
        }
    }

}