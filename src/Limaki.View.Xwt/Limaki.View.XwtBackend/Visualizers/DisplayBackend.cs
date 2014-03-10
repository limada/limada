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
using System.Diagnostics;
using WidgetEvent = Xwt.Backends.WidgetEvent;
using Limaki.View.UI;



namespace Limaki.View.XwtBackend {

    public abstract class DisplayBackend : Canvas {

    }

    public abstract class DisplayBackend<T> : DisplayBackend, IVidgetBackend, IDisplayBackend<T> {

        public DisplayBackend () {
            Initialize();
        }

        public abstract DisplayFactory<T> CreateDisplayFactory (DisplayBackend<T> backend);

        protected virtual void Initialize () {
            if (Registry.ConcreteContext == null) {
                var resourceLoader = new XwtContextResourceLoader();
                Registry.ConcreteContext = new ApplicationContext();
                resourceLoader.ApplyResources(Registry.ConcreteContext);
            }

            // this enables Key-Events
            CanGetFocus = true;

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
            
            // we need to register at least one target
            // otherwise XwtGtk.DragDrop doesnt work
            //SetDragDropTarget(DragDropAction.All, TransferDataType.Text);

            // we need to register drag-handlers
            // XwtGtk.DragDrop doesnt work without Handlers
            this.DragDropCheck += (s, e) => this.HandleDragDropCheck(e);
            this.DragOver += (s, e) => this.HandleDragOver(e);
            this.DragDrop += (s, e) => this.HandleDragDrop(e);
            this.DragLeave += (s, e) => this.HandleDragLeave(e);
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
            _backendViewPort.SetScrollAdjustments(horizontal, vertical);
        }

        protected override void OnBoundsChanged () {
            base.OnBoundsChanged();
            _backendViewPort.OnBoundsChanged();
        }

        protected override void OnReallocate () {
            base.OnReallocate();
            _backendViewPort.UpdateZoom();
        }

        #region Mouse

        protected override void OnMouseEntered (EventArgs args) {
            base.OnMouseEntered(args);
            if (!HasFocus)
                SetFocus();
        }

        private UI.MouseActionButtons lastButton = UI.MouseActionButtons.None;
        protected override void OnButtonPressed (ButtonEventArgs args) {
            base.OnButtonPressed(args);
            if (!HasFocus)
                SetFocus();

            Trace.WriteLine(string.Format("ButtonPressed {0} == {1} | {2}", this.MouseLocation(), args.Position, this.GetType().Name));

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

        protected override void OnKeyPressed (KeyEventArgs args) {
            base.OnKeyPressed(args);
            var ml = this.MouseLocation();
            Trace.WriteLine(string.Format("KeyPressed {0} | {1}", ml, this.GetType().Name));

            Display.EventControler.OnKeyPressed(new UI.KeyActionEventArgs(args.Key, args.Modifiers, ml));
        }

        #endregion

        #region Keyboard

        protected override void OnKeyReleased (KeyEventArgs args) {
            base.OnKeyReleased(args);
            var ml = this.MouseLocation();
            Trace.WriteLine(string.Format("KeyReleased {0} | {1}", ml, this.GetType().Name));

            Display.EventControler.OnKeyReleased(new UI.KeyActionEventArgs(args.Key, args.Modifiers, ml));
        }

        #endregion

        [TODO]
        protected override void OnDragStarted (DragStartedEventArgs args) {

            base.OnDragStarted(args);
        }

        #region Drop
        protected virtual void HandleDragDropCheck (DragCheckEventArgs args) {
            var dropHandler = Display.EventControler as IDropAction;
            SetDragDropTarget(args.DataTypes);

        }

        protected virtual void HandleDragOver (DragOverEventArgs args) {
            var dropHandler = Display.EventControler as IDropAction;
            if (dropHandler != null && Display.Data != null) {

                var ev = new DragDrop.DragOverEventArgs(
                    args.Position,
                    args.Data,
                    args.Action);
                ev.AllowedAction = args.AllowedAction;
                dropHandler.DragOver(ev);
                args.AllowedAction = ev.AllowedAction;
            }
        }

        protected virtual void HandleDragDrop (DragEventArgs args) {
            var dropHandler = Display.EventControler as DragDrop.IDropHandler;
            if (dropHandler != null && Display.Data != null) {
               var e = new DragDrop.DragEventArgs(
                    args.Position,
                    args.Data,
                    args.Action
                    );
                dropHandler.OnDrop(e);
                args.Success = e.Success;
            }
        }

        protected virtual void HandleDragLeave (EventArgs args) {
            var dropHandler = Display.EventControler as DragDrop.IDropHandler;
            if (dropHandler != null && Display.Data != null) {
                dropHandler.DragLeave(args);
            }
        }

        #endregion
    }
}
