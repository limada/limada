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
 * 
 */

//#define TracePaint
//#define TraceInvalidate

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.GDI;
using Limaki.Drawing.GDI.UI;
using Limaki.Drawing.UI;


namespace Limaki.Winform.Displays {
    public abstract partial class DisplayBase<T> : UserControl, IDisplayBase<T>
    where T : class {

        protected bool Opaque = true;
        public DisplayBase() {

            if (Registry.ConcreteContext == null) {
                var resourceLoader = new Limaki.Context.WinformContextRecourceLoader();
                Registry.ConcreteContext = resourceLoader.CreateContext();
                resourceLoader.ApplyResources(Registry.ConcreteContext);
            }

            InitializeComponent();
            Opaque = true;//!Commons.Mono; // opaque works on mono too, but is slower

            ControlStyles controlStyle =
                ControlStyles.UserPaint
                | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer;

            if (Opaque) {
                controlStyle = controlStyle | ControlStyles.Opaque;
            }

            this.SetStyle(controlStyle, true);
            this.AllowDrop = true;
            this.AutoScroll = true;

            DisplayKit.DataHandler = this.DataHandler;

            if (!this.DesignMode) {
                Registry.ApplyProperties<DisplayContextProcessor<T>, DisplayBase<T>>(this);

                DataLayer.Enabled = true;
                SelectAction.Enabled = true;
                ScrollAction.Enabled = false;
                ZoomAction.Enabled = true;

                DataChanged();
            }
        }

        public abstract DisplayKit<T> DisplayKit { get; }

        #region Data-Handling

        protected T _data = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual T Data {
            get { return _data; }
            set {
                bool refresh = value != _data;
                if (refresh) {
                    _data = value;
                    if (_data != null) {
                        DataLayer.Data = _data;
                    } else {
                        DataLayer.Data = null;
                    }
                    DataChanged();
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected Func<T> DataHandler {
            get { return () => { return Data; }; }
        }

        protected ILayer<T> _dataLayer = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ILayer<T> DataLayer {
            get {
                if (_dataLayer == null) {
                    EventControler.Add(_dataLayer = DisplayKit.Layer(this.Camera));
                }
                return _dataLayer;
            }
            set { EventControler.Add(value, ref _dataLayer); }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        ILayer IDisplayBase.DataLayer {
            get { return this.DataLayer; }
            set { this.DataLayer = (ILayer<T>)value; }
        }



        protected virtual void DataChanged() {
            SelectAction.Clear();
            //if (this.Parent != null) {
            CommandsInvoke();
            UpdateZoom();
            //}
        }

        #endregion

        # region Paint

        protected SolidBrush _backBrush = new SolidBrush(SystemColors.ButtonFace);
        protected SolidBrush backBrush {
            get { 
                _backBrush.Color = this.BackColor;
                return _backBrush;
            }
        }
        public override System.Drawing.Color BackColor {
            get { return base.BackColor; }
            set {
                if (base.BackColor != value) {
                    base.BackColor = value;
                    this.backBrush.Color = value;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            if (DataLayer.Data != null) {
                Graphics g = e.Graphics;
                Region saveRegion = g.Clip;
                Rectangle clipRect = e.ClipRectangle;

                lock (clipRegion) {
                    if (clipRegionHasData) {

                        //g.Clip.Intersect(clipRegion);


#if TracePaint
                        Console.WriteLine ("OnPaint (region)" + 
                            g.Clip.GetBounds (g) + "\t" + g.ClipBounds);
#endif
                    } else {
#if TracePaint
                        Console.WriteLine ("OnPaint ()" + 
                            g.Clip.GetBounds (g) + "\t" + g.ClipBounds+"\t" +e.ClipRectangle);
#endif
                    }


                    // draw background
                    if (Opaque) {
                        g.FillRectangle(backBrush, clipRect);
                    }


                    EventControler.OnPaint(Converter.Convert(e));


                    if (clipRegionHasData) {
                        clipRegion.MakeInfinite();
                        clipRegionHasData = false;
                    }
                }
                g.Clip = saveRegion;
                g.Transform.Reset();
            } else {
                if (Opaque) { // draw background
                    e.Graphics.FillRectangle(backBrush, e.ClipRectangle);
                }
            }
        }

        /// <summary>
        /// if opaque = true, onpaintbackground is never called
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e) {
            if (!Opaque) {
                Graphics g = e.Graphics;
                g.InterpolationMode = InterpolationMode.Low;
                g.SmoothingMode = SmoothingMode.None;
                g.CompositingMode = CompositingMode.SourceCopy;
                if (clipRegionHasData) {

                    base.OnPaintBackground(e);

#if TracePaint
                    Console.WriteLine("OnPaintBackground (region)" + g.Clip.GetBounds(g) + "\t" + g.ClipBounds);
#endif
                } else {
                    base.OnPaintBackground(e);
#if TracePaint
                    Console.WriteLine("OnPaintBackground ()" + g.ClipBounds + "\t" + g.Clip.GetBounds(g));
#endif
                }
            }
        }

        protected override void OnSizeChanged(EventArgs e) {
            if (!_scrollMinSizeChanging) {
                if (ZoomState == ZoomState.FitToHeigth
                    || ZoomState == ZoomState.FitToWidth
                    || ZoomState == ZoomState.FitToScreen) {
                    UpdateZoom();
                }
            }
            base.OnSizeChanged(e);
            UpdateScrollSize();
        }


        # endregion

        #region IControl-Member and Region-Handling

        RectangleI IControl.ClientRectangle {
            get { return GDIConverter.Convert(this.ClientRectangle); }
        }
        SizeI IControl.Size {
            get { return new SizeI(this.Width, this.Height); }
        }
        Region clipRegion = new Region();

        bool clipRegionHasData = false;


        void IControl.Invalidate() {
#if TraceInvalidate
            Console.WriteLine("IControl.Invalidate()");
#endif
            lock (clipRegion) {
                //if (this.Data != null) {

                if (Data != null) {
                    clipRegionHasData = false;
                    clipRegion.MakeInfinite();
                }

                //}
                this.Invalidate();
            }
        }

        void IControl.Invalidate(RectangleI rect) {
#if TraceInvalidate
            Console.WriteLine("IControl.Invalidate(rect)" + rect);
#endif
            lock (clipRegion) {

                clipRegionHasData = false;
                clipRegion.MakeInfinite();

                this.Invalidate(GDIConverter.Convert(rect));

            }
        }


        void IGDIControl.Invalidate(Region region) {

            lock (clipRegion) {
                if (!clipRegionHasData) {
                    clipRegion.Intersect(region);
                } else {
                    clipRegion.Union(region);
                }

                clipRegionHasData = true;


                // complicated region leeds to more unnecessary OnPaint-Calls, 
                // so invalidate a rectangle instead of a region

                // if region.IsEmpty, GetBounds under mono 2.2 gives back a weird Rectangle
                // TODO: Report this error
                RectangleF rect = region.GetBounds(GDIUtils.DeviceContext);
#if TraceInvalidate
                Console.WriteLine("IControl.Invalidate(region)\t"+rect);
#endif
                if (!Commons.Mono || (Commons.Mono && (rect.Width < int.MaxValue)))
                    base.Invalidate(Rectangle.Ceiling(rect));


            }

            // curiosly, calling Update is faster on windows, 
            // but not on linux:
            if (!Commons.Unix) {

                Update();
            }
        }

        void IGDIControl.Invalidate(GraphicsPath path) {
            throw new NotImplementedException("Invalidate over path not implemented");
        }
        #endregion



        # region Action-Handling

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EventControler EventControler = new EventControler();

        public virtual TAction GetAction<TAction>() {
            return EventControler.GetAction<TAction>();
        }

        protected SelectionBase _selectAction = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual SelectionBase SelectAction {
            get {
                if (_selectAction == null) {
                    EventControler.Add(_selectAction =
                        DisplayKit.SelectAction(this, this.Camera));
                }
                return _selectAction;
            }
            set { EventControler.Add(value, ref _selectAction); }
        }

        protected ScrollAction _scrollAction = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ScrollAction ScrollAction {
            get {
                if (_scrollAction == null) {
                    EventControler.Add(_scrollAction = new ScrollAction(this));
                }
                return _scrollAction;
            }
            set { EventControler.Add(value, ref _scrollAction); }
        }

        protected ZoomAction _zoomAction = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ZoomAction ZoomAction {
            get {
                if (_zoomAction == null) {
                    EventControler.Add(_zoomAction = new ZoomAction(this, this, this.Camera));
                }
                return _zoomAction;
            }
            set { EventControler.Add(value, ref _zoomAction); }
        }

        public virtual void Clear() {
            Data = null;


            if (_selectAction != null) { SelectAction = null; }
            if (_scrollAction != null) { ScrollAction = null; }
            if (_zoomAction != null) { ZoomAction = null; }

            DataLayer = null;
            Camera = null;
        }

        # endregion

        # region Action-Dispatching
        protected override void OnMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e);
            if (Data != null)
                EventControler.OnMouseDown(Converter.Convert(e));
        }
        protected override void OnMouseHover(EventArgs e) {
            base.OnMouseHover(e);

            Point pos = this.PointToClient(MousePosition);
            MouseActionEventArgs mouseEventArgs =
                new MouseActionEventArgs(
                    Converter.Convert(MouseButtons),
                    Converter.ConvertModifiers(Form.ModifierKeys),
                    0, pos.X, pos.Y, 0);
            if (Data != null)
                EventControler.OnMouseHover(mouseEventArgs);

        }
        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);
            if (Data != null)
                EventControler.OnMouseMove(Converter.Convert(e));
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            base.OnMouseUp(e);
            if (Data != null)
                EventControler.OnMouseUp(Converter.Convert(e));
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            var ev = Converter.Convert(e);
            if (Data != null)
                EventControler.OnKeyDown(ev);
            if (!ev.Handled)
                base.OnKeyDown(e);
        }
        protected override void OnKeyPress(KeyPressEventArgs e) {
            base.OnKeyPress(e);
            if (Data != null)
                EventControler.OnKeyPress(new KeyActionPressEventArgs(e.KeyChar));
        }
        protected override void OnKeyUp(KeyEventArgs e) {
            base.OnKeyUp(e);
            if (Data != null)
                EventControler.OnKeyUp(Converter.Convert(e));
        }
        #endregion

        #region IControl-Layout

        public virtual void CommandsExecute() {
            if (Data != null)
                EventControler.CommandsExecute();
        }

        public virtual void CommandsInvoke() {
            if (Data != null)
                EventControler.CommandsInvoke();
        }

        #endregion

        #region dragdrop
        protected override void OnGiveFeedback(GiveFeedbackEventArgs gfbevent) {
            if (Data != null)
                EventControler.OnGiveFeedback(gfbevent);
            base.OnGiveFeedback(gfbevent);

        }
        protected override void OnQueryContinueDrag(QueryContinueDragEventArgs qcdevent) {
            if (Data != null)
                EventControler.OnQueryContinueDrag(qcdevent);
            base.OnQueryContinueDrag(qcdevent);

        }

        protected override void OnDragOver(DragEventArgs drgevent) {
            if (Data != null)
                EventControler.OnDragOver(drgevent);
            base.OnDragOver(drgevent);

        }

        protected override void OnDragDrop(DragEventArgs drgevent) {
            if (Data != null)
                EventControler.OnDragDrop(drgevent);
            base.OnDragDrop(drgevent);

        }
        protected override void OnDragLeave(EventArgs e) {
            if (Data != null)
                EventControler.OnDragLeave(e);
            base.OnDragLeave(e);
        }
        #endregion
    }
}

