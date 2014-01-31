﻿/*
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.View;
using Limaki.View.Rendering;
using Limaki.View.Visualizers;
using Xwt;
using Limaki.Common;
using Limaki.Common.IOC;
using System.ComponentModel;
using Xwt.Drawing;

namespace Limaki.View.XwtBackend {

    public abstract class DisplayBackend : Canvas {
        
    }

    public abstract class DisplayBackend1 : ScrollView {
        public DisplayBackend1 () {
            this.Canvas = new DisplayCanvas();
            this.Content = this.Canvas;
        }

        public class DisplayCanvas:Canvas {
            internal virtual void InternalDraw (Context ctx, Rectangle dirtyRect) {
                this.OnDraw(ctx, dirtyRect);
            }
        }

        public DisplayCanvas Canvas { get; set; }

        protected virtual void OnDraw (Context ctx, Rectangle dirtyRect) {
            Canvas.InternalDraw(ctx, dirtyRect);
        }

        public void QueueDraw () { Canvas.QueueDraw(); }

        public void QueueDraw (Rectangle rectangle) { Canvas.QueueDraw(rectangle); }
        public Rectangle Bounds { get { return Canvas.Bounds; } }
    }

    public abstract class DisplayBackend<T> : DisplayBackend, IVidgetBackend, IDisplayBackend<T> {

        public DisplayBackend () {
            Initialize();
        }

        public abstract DisplayFactory<T> CreateDisplayFactory (DisplayBackend<T> backend);

        protected virtual void Initialize () {
            if (Registry.ConcreteContext == null) {
                var resourceLoader = new XwtContextRecourceLoader();
                Registry.ConcreteContext = new ApplicationContext();
                resourceLoader.ApplyResources(Registry.ConcreteContext);
            }
        }

        
        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            Display<T> display = null;
            var factory = CreateDisplayFactory(this);
            if (frontend != null)
                display = (Display<T>)frontend;
            else
                display = factory.Create();
            _display = display;
            factory.Compose(display);
        }

        protected IDisplay<T> _display = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IDisplay<T> Display {
            get {
                if (_display == null) {
                    InitializeBackend(null, null);
                }
                return _display;
            }
            set { _display = value; }
        }

        IDisplay IDisplayBackend.Frontend {
            get { return this.Display; }
            set { this.Display = value as IDisplay<T>; }
        }

        IDisplay<T> IDisplayBackend<T>.Frontend {
            get { return this.Display; }
            set { this.Display = value; }
        }

        protected XwtBackendRenderer<T> _backendRenderer = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IBackendRenderer BackendRenderer {
            get { return _backendRenderer; }
            set { _backendRenderer = value as XwtBackendRenderer<T>; }
        }

        protected XwtViewport _backendViewPort = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IViewport BackendViewPort {
            get { return _backendViewPort; }
            set { _backendViewPort = value as XwtViewport; }
        }

        void IVidgetBackend.Update () {
            base.QueueDraw();
        }

        void IVidgetBackend.Invalidate () {
            base.QueueDraw();
        }

        void IVidgetBackend.Invalidate (Rectangle rect) {
            base.QueueDraw(rect);
        }

        protected override void OnDraw (Context ctx, Rectangle dirtyRect) {
            base.OnDraw(ctx, dirtyRect);
            _backendRenderer.OnDraw(ctx, dirtyRect);
        }

        protected override bool SupportsCustomScrolling { get { return true; } }

        protected override void SetScrollAdjustments (ScrollAdjustment horizontal, ScrollAdjustment vertical) {
            base.SetScrollAdjustments(horizontal, vertical);
            _backendViewPort.SetScrollAdjustments (horizontal, vertical);
        }

        protected override void OnBoundsChanged () {
            base.OnBoundsChanged();
            _backendViewPort.OnBoundsChanged();
        }
        
        protected override void OnMouseEntered (EventArgs args) {
            base.OnMouseEntered(args);
        }

        private UI.MouseActionButtons lastButton = UI.MouseActionButtons.None;
        protected override void OnButtonPressed (ButtonEventArgs args) {
            base.OnButtonPressed(args);
            lastButton = args.Button.ToLmk();
            Display.EventControler.OnMouseDown(new UI.MouseActionEventArgs(
                lastButton,
                Keyboard.CurrentModifiers,
                args.MultiplePress,
                args.X,
                args.Y,
                0
                ));
        }

        protected override void OnMouseMoved (MouseMovedEventArgs args) {
            base.OnMouseMoved(args);
             Display.EventControler.OnMouseMove(new UI.MouseActionEventArgs(
                lastButton,
                Keyboard.CurrentModifiers,
                0,
                args.X,
                args.Y,
                0
                ));
        }

        protected override void OnMouseScrolled (MouseScrolledEventArgs args) {
            base.OnMouseScrolled(args);
        }

        protected override void OnButtonReleased (ButtonEventArgs args) {
            base.OnButtonReleased(args);
            lastButton = args.Button.ToLmk();
            Display.EventControler.OnMouseUp(new UI.MouseActionEventArgs(
                lastButton,
                Keyboard.CurrentModifiers,
                args.MultiplePress,
                args.X,
                args.Y,
                0
                ));
            lastButton = UI.MouseActionButtons.None;
        }

        protected override void OnMouseExited (EventArgs args) {
            base.OnMouseExited(args);
        }
        
    }
}