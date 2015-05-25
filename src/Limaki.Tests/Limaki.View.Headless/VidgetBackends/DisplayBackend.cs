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

using System;
using Limaki.View;
using Limaki.View.Vidgets;
using Limaki.View.Viz;
using Limaki.View.Viz.Rendering;
using Limaki.View.Viz.UI;
using Limaki.View.Viz.Visualizers;
using Xwt;
using Limaki.Common;
using Limaki.Common.IOC;
using System.ComponentModel;
using Xwt.Drawing;
using System.Diagnostics;

namespace Limaki.View.Headless.VidgetBackends {

    public abstract class DisplayBackend : DummyBackend {

    }

    public abstract class DisplayBackend<T> : DisplayBackend, IVidgetBackend, IDisplayBackend<T> {

        public DisplayBackend () {
            Initialize();
        }

        public abstract DisplayFactory<T> CreateDisplayFactory (DisplayBackend<T> backend);

        protected virtual void Initialize () {
            if (Registry.ConcreteContext == null) {
                var resourceLoader = new HeadlessContextResourceLoader();
                Registry.ConcreteContext = new ApplicationContext();
                resourceLoader.ApplyResources(Registry.ConcreteContext);
            }

        }
        
        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
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

        protected HeadlessBackendRenderer<T> _backendRenderer = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IBackendRenderer BackendRenderer {
            get { return _backendRenderer; }
            set { _backendRenderer = value as HeadlessBackendRenderer<T>; }
        }

        protected HeadlessViewport _backendViewPort = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IViewport BackendViewPort {
            get { return _backendViewPort; }
            set { _backendViewPort = value as HeadlessViewport; }
        }

        IVidget IVidgetBackend.Frontend { get { return this.Display; } }

        void IVidgetBackend.Update () {
            base.Update();
        }

        void IVidgetBackend.Invalidate () {
            base.Invalidate();
        }

        void IVidgetBackend.Invalidate (Rectangle rect) {
            base.Invalidate (rect);
        }

        protected virtual void OnDraw (Context ctx, Rectangle dirtyRect) {
            _backendRenderer.OnDraw(ctx, dirtyRect);
        }


        protected virtual void OnBoundsChanged () {

        }

        protected virtual void OnReallocate () {
            
            _backendViewPort.UpdateZoom();
        }

        #region Mouse

        protected virtual void OnMouseEntered (EventArgs args) {

        }

        private global::Limaki.View.Vidgets.MouseActionButtons lastButton = global::Limaki.View.Vidgets.MouseActionButtons.None;
        protected virtual void OnButtonPressed (MouseActionEventArgs args) {
            Display.ActionDispatcher.OnMouseDown (args);
        }

        protected virtual void OnMouseMoved (MouseActionEventArgs args) {
            Display.ActionDispatcher.OnMouseMove (args);
        }

        protected virtual void OnMouseScrolled (MouseScrolledEventArgs args) {
            
        }

        protected virtual void OnButtonReleased (MouseActionEventArgs args) {

            Display.ActionDispatcher.OnMouseUp (args);
        }


        protected virtual void OnKeyPressed (KeyActionEventArgs args) {
            Display.ActionDispatcher.OnKeyPressed(args);
        }

        #endregion

        #region Keyboard

        protected virtual void OnKeyReleased (KeyActionEventArgs args) {
            Display.ActionDispatcher.OnKeyReleased (args);
        }

        #endregion

        [TODO]
        protected virtual void OnDragStarted (DragStartedEventArgs args) {

        }

        #region Drop
        protected virtual void OnHandleDragDropCheck (DragCheckEventArgs args) {
            var dropHandler = Display.ActionDispatcher as IDropAction;

        }

        protected virtual void OnHandleDragOver (DragDrop.DragOverEventArgs args) {
            var dropHandler = Display.ActionDispatcher as IDropAction;
            if (dropHandler != null && Display.Data != null) {
                dropHandler.DragOver (args);
            }
        }

        protected virtual void OnHandleDragDrop (DragDrop.DragEventArgs args) {
            var dropHandler = Display.ActionDispatcher as global::Limaki.View.DragDrop.IDropHandler;
            if (dropHandler != null && Display.Data != null) {
                dropHandler.OnDrop(args);
            }
        }

        protected virtual void OnHandleDragLeave (EventArgs args) {
            var dropHandler = Display.ActionDispatcher as global::Limaki.View.DragDrop.IDropHandler;
            if (dropHandler != null && Display.Data != null) {
                dropHandler.DragLeave(args);
            }
        }

        #endregion
    }
}
