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
 * 
 */

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Gdi;
using Limaki.View.Visualizers;
using Limaki.View.Gdi.UI;
using Limaki.View.Rendering;
using Limaki.View.Swf.UI;
using Limaki.View.UI;
using Xwt;
using ApplicationContext = Limaki.Common.IOC.ApplicationContext;
using DragEventArgs = System.Windows.Forms.DragEventArgs;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using Point = Xwt.Point;
using Size = Xwt.Size;
using Xwt.Gdi;
using Xwt.Gdi.Backend;

namespace Limaki.View.Swf.Visualizers {

    public abstract class SwfWidgetBackend: UserControl {
        public bool ScrollBarsVisible {
            set {
                this.HScroll = value;
                this.VScroll = value;
            }
        }
    }

    public abstract class SwfWidgetBackend<T> : SwfWidgetBackend, IGdiBackend, IDisplayBackend<T>, IDragDopControl {

        public SwfWidgetBackend() {
            Initialize();
        }

        public abstract DisplayFactory<T> CreateDisplayFactory(SwfWidgetBackend<T> device);

        protected void Initialize() {
            if (Registry.ConcreteContext == null) {
                var resourceLoader = new SwfContextRecourceLoader();
                Registry.ConcreteContext = new ApplicationContext();
                resourceLoader.ApplyResources(Registry.ConcreteContext);
            }


            this.AllowDrop = true;
            this.AutoScroll = true;

            var factory = CreateDisplayFactory(this);
            var display = factory.Create ();
			_display = display;
            factory.Compose (display);
            
            if (!this.DesignMode) {
                var Opaque = true;//!Commons.Mono; // opaque works on mono too, but is slower

                ControlStyles controlStyle =
                    ControlStyles.UserPaint
                    | ControlStyles.AllPaintingInWmPaint
                    | ControlStyles.OptimizedDoubleBuffer;

                if (Opaque) {
                    controlStyle = controlStyle | ControlStyles.Opaque;
                }

                this.SetStyle(controlStyle, true);
                this._backendRenderer.Opaque = Opaque;
            }
        }

		IDisplay<T> _display = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IDisplay<T> Display { 
			get {return _display;}
			set {_display = value;} 
		}

        IDisplay IDisplayBackend.Frontend {
            get { return this.Display; }
            set { this.Display = value as IDisplay<T>; }
        }

        protected SwfBackendRenderer<T> _backendRenderer = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IBackendRenderer BackendRenderer {
            get { return _backendRenderer; }
            set { _backendRenderer = value as SwfBackendRenderer<T>; }
        }

        protected SwfViewport _backendViewPort = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IViewport BackendViewPort {
            get { return _backendViewPort; }
            set { _backendViewPort = value as SwfViewport; }
        }

        public override Color BackColor {
            get { return base.BackColor; }
            set {
                base.BackColor = value;
                var color = GdiConverter.ToXwt (value);
                if (!Display.BackColor.Equals(color)) {
                    Display.BackColor = color;
                }
                
            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            _backendRenderer.OnPaint(e);
        }

        protected override void OnSizeChanged(System.EventArgs e) {
            if (_backendViewPort != null) {
                _backendViewPort.OnSizeChanged(e,base.OnSizeChanged);
            } else {
                base.OnSizeChanged(e);
            }
            
        }

        protected override void OnScroll(ScrollEventArgs se) {
            if (_backendViewPort != null) {
                _backendViewPort.OnScroll(se);
            } else {
                base.OnScroll (se);
            }
        }

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;

        /// <summary>
        /// workaround as cursor keys don't rise OnKeyDown-Events
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            bool result = false;
            if (this.Focused &&
                (keyData == Keys.Down || keyData == Keys.Up || keyData==Keys.Left || keyData== Keys.Right)) {
                var e = new KeyEventArgs(keyData);
                if (msg.Msg == WM_KEYDOWN)
                    OnKeyDown(e);
                if (msg.Msg == WM_KEYUP)
                    OnKeyUp(e);
                result = true;
            } else {
                result = base.ProcessCmdKey(ref msg, keyData);    
            }
            return result;
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            var ev = Converter.Convert(e,this.PointToClient(MousePosition));
            if (Display.Data != null)
                Display.EventControler.OnKeyPressed(ev);
            if (!ev.Handled)
                base.OnKeyDown(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e) {
            base.OnKeyPress(e);
            //if (Display.Data != null)
            //    Display.EventControler.OnKeyPress(new KeyActionPressEventArgs(e.KeyChar));
        }

        protected override void OnKeyUp(KeyEventArgs e) {
            base.OnKeyUp(e);
            if (Display.Data != null)
                Display.EventControler.OnKeyReleased(Converter.Convert(e,this.PointToClient(MousePosition)));
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e);
            if (Display.Data != null)
                Display.EventControler.OnMouseDown(Converter.Convert(e));
        }

        protected override void OnMouseHover(EventArgs e) {
            base.OnMouseHover(e);

            var pos = this.PointToClient(MousePosition);
            MouseActionEventArgs mouseEventArgs =
                new MouseActionEventArgs(
                    Converter.Convert(MouseButtons),
                    Converter.ConvertModifiers(Form.ModifierKeys),
                    0, pos.X, pos.Y, 0);
            if (Display.Data != null)
                Display.EventControler.OnMouseHover(mouseEventArgs);

        }

        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);
            if (Display.Data != null)
                Display.EventControler.OnMouseMove(Converter.Convert(e));
        }

        protected override void OnMouseWheel (MouseEventArgs e) {
            if (Display.Data != null) {
                var ev = Converter.Convert(e);
                if (ev.Modifiers == Xwt.ModifierKeys.None) {
                    // remark: another possibility would be to call OnScroll or _backenViewPort.OnScroll
                    base.OnMouseWheel(e);
                    Display.Viewport.Update();
                } else if (ev.Modifiers == Xwt.ModifierKeys.Control) {
                    var zoomAction = Display.EventControler.GetAction<ZoomAction>();
                    if (zoomAction != null) {
                        zoomAction.Zoom(ev.Location, ev.Delta > 0);
                    }
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            base.OnMouseUp(e);
            if (Display.Data != null)
                Display.EventControler.OnMouseUp(Converter.Convert(e));
        }

        #region dragdrop
        protected override void OnGiveFeedback(GiveFeedbackEventArgs gfbevent) {
            var EventControler = Display.EventControler as SwfEventControler;
            if (Display.Data != null)
                EventControler.OnGiveFeedback(gfbevent);
            base.OnGiveFeedback(gfbevent);

        }
        protected override void OnQueryContinueDrag(QueryContinueDragEventArgs qcdevent) {
            var EventControler = Display.EventControler as SwfEventControler;
            if (Display.Data != null)
                EventControler.OnQueryContinueDrag(qcdevent);
            base.OnQueryContinueDrag(qcdevent);

        }

        protected override void OnDragOver(DragEventArgs drgevent) {
            var EventControler = Display.EventControler as SwfEventControler;
            if (Display.Data != null)
                EventControler.OnDragOver(drgevent);
            base.OnDragOver(drgevent);

        }

        protected override void OnDragDrop(DragEventArgs drgevent) {
            var EventControler = Display.EventControler as SwfEventControler;
            if (Display.Data != null)
                EventControler.OnDragDrop(drgevent);
            base.OnDragDrop(drgevent);

        }
        protected override void OnDragLeave(EventArgs e) {
            var EventControler = Display.EventControler as SwfEventControler;
            if (Display.Data != null)
                EventControler.OnDragLeave(e);
            base.OnDragLeave(e);
        }
        #endregion

        #region IGDIControl Member

        void IGdiBackend.Invalidate(System.Drawing.Drawing2D.GraphicsPath path) {
                Registry.Pool.TryGetCreate<IExceptionHandler>()
                    .Catch(new Exception(this.GetType().Name + ".Invalidate(GraphicsPath path) not implemented"), MessageType.OK);
        }
        

        #endregion

        #region IControl Member

        Xwt.Rectangle IVidgetBackend.ClientRectangle {
            get { return GdiConverter.ToXwt (this.ClientRectangle); }
        }

        Size IVidgetBackend.Size {
            get { return this.Size.ToXwt (); }
        }


        void IVidgetBackend.Invalidate(Xwt.Rectangle rect) {
            this.Invalidate (rect.ToGdi ());
        }

        Point IVidgetBackend.PointToClient(Point source) {
            return this.PointToClient (source.ToGdi ()).ToXwt ();
        }

        #endregion

    
    }

}
