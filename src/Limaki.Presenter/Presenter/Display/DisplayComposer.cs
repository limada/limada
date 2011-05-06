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
 */


using System;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Presenter.UI;

namespace Limaki.Presenter.Display {

    public class DisplayComposer<TData>:IComposer<Display<TData>>, IDisposable {
        
        public virtual Get<IClipper> Clipper { get; set; }
        public virtual Get<IViewport> Viewport { get; set; }
        public virtual Get<IDeviceRenderer> Renderer { get; set; }
        public virtual Get<ICamera> Camera { get; set; }
        public virtual Get<SizeI> DataSize { get; set; }
        public virtual Get<PointI> DataOrigin { get; set; }
        public virtual Get<IDeviceCursor> DeviceCursor { get; set; }


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

            this.Renderer = () => display.DeviceRenderer;
            this.DeviceCursor = () => display.DeviceCursor;

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

            var scroll = new ScrollAction ();
            scroll.Viewport = this.Viewport;
            scroll.Enabled = false;
            display.ScrollAction = scroll;
            display.EventControler.Add(scroll);

        }

        public virtual void Compose(Display<TData> display, ISelectionRenderer selectionRenderer) {
            
            selectionRenderer.Enabled = true;
            selectionRenderer.Style = display.StyleSheet[StyleNames.ResizerToolStyle];
            selectionRenderer.Clipper = this.Clipper;
            selectionRenderer.GripSize = display.GripSize;
            selectionRenderer.Camera = this.Camera;
            
            display.EventControler.Add(selectionRenderer);
        }

        public virtual void Compose(Display<TData> display, MoveResizeAction action, bool isSelection) {
            
            action.HitSize = display.HitSize;
            action.CameraHandler = this.Camera;
            action.CursorGetter = this.DeviceCursor;

            if (isSelection)
                action.SelectionRenderer = display.SelectionRenderer;
            else
                action.SelectionRenderer = display.MoveResizeRenderer;
            
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