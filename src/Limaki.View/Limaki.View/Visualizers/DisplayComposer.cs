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
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Styles;
using Limaki.View.Clipping;
using Limaki.View.Rendering;
using Limaki.View.UI;
using Xwt;

namespace Limaki.View.Visualizers {

    public class DisplayComposer<TData>:IComposer<Display<TData>>, IDisposable {
        
        public virtual Func<IClipper> Clipper { get; set; }
        public virtual Func<IViewport> Viewport { get; set; }
        public virtual Func<IBackendRenderer> Renderer { get; set; }
        public virtual Func<ICamera> Camera { get; set; }
        public virtual Func<Size> DataSize { get; set; }
        public virtual Func<Point> DataOrigin { get; set; }
        public virtual Func<ICursorHandler> DeviceCursor { get; set; }


        public virtual void Factor(Display<TData> display) {
            display.Composer = this;
            
            var context = Registry.ConcreteContext;

            var styleSheets = context.Pool.TryGetCreate<StyleSheets>();
            display.StyleSheet = styleSheets.DefaultStyleSheet;


            display.ShapeFactory = context.Factory.Create<IShapeFactory>();
            display.PainterFactory = context.Factory.Create<IPainterFactory>();
            display.EventControler = context.Factory.Create<EventControler>();

            display.Clipper = context.Factory.Create<IClipper>();
            display.ClipReceiver = context.Factory.Create<IClipReceiver>();
            display.Viewport = context.Factory.Create<IViewport>();
        }

        public virtual void Compose(Display<TData> display) {
            
            display.HitSize = 6;
            display.GripSize = 4;

            this.Clipper = () => display.Clipper;
            this.Viewport = () => display.Viewport;
            this.Camera = () => display.Viewport.Camera;

            this.Renderer = () => display.BackendRenderer;
            this.DeviceCursor = () => display.CursorHandler;

            display.ClipReceiver.Clipper = display.Clipper;
            display.ClipReceiver.Renderer = this.Renderer;
            display.ClipReceiver.Viewport = this.Viewport;


            display.DataLayer.Data = () => display.Data;
            display.DataLayer.Camera = this.Camera;
            display.DataLayer.Renderer = () => display.DataRenderer;

            display.Viewport.GetDataOrigin = this.DataOrigin;
            display.Viewport.GetDataSize = this.DataSize;


            display.EventControler.Add(display.DataLayer);
            display.EventControler.Add (display.ClipReceiver);

            var zoomAction = new ZoomAction ();
            zoomAction.Viewport = this.Viewport;
            display.EventControler.Add (zoomAction);

            var scroll = new MouseScrollAction ();
            scroll.Viewport = this.Viewport;
            scroll.Enabled = false;
            display.MouseScrollAction = scroll;
            display.EventControler.Add(scroll);

        }

        public virtual ISelectionRenderer Compose (Display<TData> display, ISelectionRenderer selectionRenderer) {
            
            selectionRenderer.Enabled = true;
            selectionRenderer.Style = display.StyleSheet[StyleNames.ResizerToolStyle];
            selectionRenderer.Clipper = this.Clipper;
            selectionRenderer.GripSize = display.GripSize;
            selectionRenderer.Camera = this.Camera;
            
            display.EventControler.Add(selectionRenderer);
            return selectionRenderer;
        }

        public virtual MoveResizeAction Compose (Display<TData> display, MoveResizeAction action, bool isSelection) {
            
            action.HitSize = display.HitSize;
            action.CameraHandler = this.Camera;
            action.CursorGetter = this.DeviceCursor;

            if (isSelection)
                action.SelectionRenderer = display.SelectionRenderer;
            else
                action.SelectionRenderer = display.MoveResizeRenderer;

            action.SelectionRenderer.Enabled = action.Enabled;

            return action;

        }

        public virtual void Dispose() {
            this.Clipper = null;
            this.Viewport = null;
            this.Camera = null;
            this.Renderer = null;
            this.DataSize = null;
            this.DataOrigin = null;
            this.DeviceCursor = null;
        }


        }
}