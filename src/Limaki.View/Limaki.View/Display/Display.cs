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
 * http://limada.sourceforge.net
 */

using System;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.View.Clipping;
using Limaki.View.Rendering;
using Limaki.View.UI;
using Xwt.Drawing;

namespace Limaki.View.Display {

    public class Display<TData> : IDisplay<TData>, IDisposable, ICheckable {

        public Display() { }

        public virtual IComposer<Display<TData>> Composer { get; set; }

        protected TData _data = default(TData);
        public virtual TData Data {
            get { return _data; }
            set {
                bool refresh = !object.ReferenceEquals(value,_data);
                if (refresh) {
                    _data = value;
                    DataChanged();
                }
            }
        }

        public virtual Int64 DataId { get; set; }
        public virtual string Text { get; set; }

        protected State _state = default(State);
        public virtual State State { get { return _state ?? (_state = new State{Hollow=true}); } }

        public virtual void DataChanged() {
            if (SelectAction != null)
                SelectAction.Clear();
            Viewport.ClipOrigin = Viewport.DataOrigin;
            Invoke();
            UpdateZoom();
        }

        //public virtual ILayout<TData> Layout { get; set; }
        public virtual IStyleSheet StyleSheet { get; set; }
        public virtual IShapeFactory ShapeFactory { get; set; }
        public virtual IPainterFactory PainterFactory { get; set; }

        public virtual IEventControler EventControler { get; set; }

        public virtual IMoveResizeAction SelectAction { get; set; }
        public virtual MouseScrollAction MouseScrollAction { get; set; }

        public virtual IClipper Clipper { get; set; }
        public virtual IClipReceiver ClipReceiver { get; set; }

        public virtual IDisplayBackend<TData> Backend { get; set; }
        public virtual IBackendRenderer BackendRenderer { get; set; }
        public virtual ICursorHandler CursorHandler { get; set; }

        public virtual object ActiveControl { get; set; }

        public virtual ILayer<TData> DataLayer { get; set; }
        public virtual IContentRenderer<TData> DataRenderer { get; set; }

        public virtual IViewport Viewport { get; set; }

        public virtual ISelectionRenderer SelectionRenderer { get; set; }
        public virtual ISelectionRenderer MoveResizeRenderer { get; set; }


        private Color _backColor = Colors.Black;
        public virtual Color BackColor {
            get { return _backColor; }
            set {
                if (!_backColor.Equals(value)) {
                    _backColor = value;
                }
            }
        }

        public virtual int HitSize { get; set; }
        public virtual int GripSize { get; set; }

        public virtual ZoomState ZoomState {
            get { return Viewport.ZoomState; }
            set { Viewport.ZoomState = value; }
        }

        public virtual void UpdateZoom() {
            Viewport.UpdateZoom();
        }

        public virtual void Invoke() {
            EventControler.Invoke();
        }

        public virtual void Execute() {
            EventControler.Execute();
        }

        public virtual void Done() {
            EventControler.Done();
        }

        #region IDisposable Member

        public void Dispose() {
            var instrumenter = this.Composer as IDisposable;
            this.Composer = null;
            instrumenter.Dispose();

            // the rest should be done in instrumenter!!!
            var eventController = this.EventControler;
            this.EventControler = null;
            eventController.Dispose();

            var device = this.Backend;
//            device.Display = null;
            this.Backend = null;

        }

        #endregion



        public virtual bool Check() {
            if (this.Viewport == null) {
                throw new CheckFailedException(this.GetType(), typeof(Viewport));
            } 
            if (this.Viewport is ICheckable) {
                ( (ICheckable) this.Viewport ).Check ();
            }
            if (this.EventControler == null) {
                throw new CheckFailedException(this.GetType(), typeof(IEventControler));
            }
            if (this.DataRenderer == null) {
                throw new CheckFailedException(this.GetType(), typeof(IContentRenderer<TData>));
            }
            if (this.BackendRenderer == null) {
                throw new CheckFailedException(this.GetType(), typeof(IBackendRenderer));
            }
            if (SelectionRenderer is ICheckable) {
                ((ICheckable)SelectionRenderer).Check();
            }

            if (MoveResizeRenderer is ICheckable) {
                ((ICheckable)MoveResizeRenderer).Check();
            }

            foreach (var action in EventControler.Actions) {
                var checkable = action.Value as ICheckable;
                if (checkable != null) {
                    checkable.Check ();
                }
            }
            return true;
        }

    }
}