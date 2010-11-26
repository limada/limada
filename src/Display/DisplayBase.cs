/*
 * Limaki 
 * Version 0.064
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
//#define UseRegion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Winform;
using Limaki.Drawing.Shapes;

namespace Limaki.Displays {
    public abstract partial class DisplayBase<T> : UserControl, IDisplayBase
    where T : class {

        protected bool Opaque = true;
        public DisplayBase() {
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

            displayKit.dataHandler = this.DataHandler;

            DataLayer.Enabled = true;
            SelectAction.Enabled = true;
            ScrollAction.Enabled = false;
            ZoomAction.Enabled = true;

            DataChanged();
        }

        public abstract DisplayKit<T> displayKit { get; }

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
        protected Handler<T> DataHandler {
            get { return this.GetData; }
        }

        protected T GetData() {
            return Data;
        }

        protected ILayer<T> _dataLayer = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ILayer<T> DataLayer {
            get {
                if (_dataLayer == null) {
                    ActionDispatcher.Add(_dataLayer = displayKit.Layer(this, this));
                }
                return _dataLayer;
            }
            set { ActionDispatcher.Add(value, ref _dataLayer); }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        ILayer IDisplayBase.DataLayer {
            get { return this.DataLayer; }
            set { this.DataLayer = (ILayer<T>)value; }
        }

        public virtual Size DataSize { get { return DataLayer.Size; } }

        protected virtual void DataChanged() {
            SelectAction.Clear();
            CommandsInvoke();
            UpdateZoom();
        }

        #endregion

        # region Paint

        private SolidBrush backBrush = new SolidBrush(SystemColors.ButtonFace);
        public override Color BackColor {
            get { return base.BackColor; }
            set {
                base.BackColor = value;
                this.backBrush.Color = value;
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
#if UseRegion
                        region = this.clipRegion;
#endif
                        if (Commons.Mono) {
                            Rectangle monoClip = Rectangle.Ceiling(clipRegion.GetBounds (g));
                            monoClip.Intersect (this.ClientRectangle);
                            if (monoClip.IsEmpty) {
#if TracePaint
                                Console.WriteLine ("OnPaint (region) monoClip.IsEmpty");
#endif
                                return;
                            }
                            g.SetClip(monoClip);
                        }
#if UseRegion
#else

                        g.Clip.Intersect(clipRegion);
#endif

#if TracePaint
                        Console.WriteLine ("OnPaint (region)" + g.Clip.GetBounds (g) + "\t" + g.ClipBounds);
#endif
                    } else {
                        if (Commons.Mono) {
                            // mono gives strange g.ClipBounds: 
                            // X=-4194304,Y=-4194304,Width=8388608,Height=8388608
                            // so we have to correct that
                            Rectangle monoClip = e.ClipRectangle;
                            monoClip.Intersect(this.ClientRectangle);
                            if (monoClip.IsEmpty) {
#if TracePaint
                                Console.WriteLine("OnPaint () monoClip.IsEmpty");
#endif
                                return;
                            }

                            g.SetClip(monoClip);
                        }
#if TracePaint
                        Console.WriteLine ("OnPaint ()" + g.Clip.GetBounds (g) + "\t" + g.ClipBounds+"\t" +e.ClipRectangle);
#endif
                    }


                    // draw background
                    if (Opaque) {
#if UseRegion

                        if (clipRegionHasData)
                            g.FillRegion (backBrush, region);
                        else
                            g.FillRegion (backBrush, g.Clip);
                        //g.FillRectangle(backBrush, clipRect);
#else
                        g.FillRectangle(backBrush, clipRect);
#endif
                    }

#if UseRegion
                    ActionDispatcher.OnPaint (Converter.Convert (e, region));
#else
                    ActionDispatcher.OnPaint(Converter.Convert(e));
#endif

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
                    if (Commons.Mono) {
                        Region saveRegion = g.Clip;

                        g.SetClip(clipRegion.GetBounds(g));
                        g.Clip.Intersect(clipRegion);

                        base.OnPaintBackground(e);

                        g.Clip = saveRegion;
                    } else {
                        base.OnPaintBackground(e);
                    }


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
            UpdateZoom();
            base.OnSizeChanged(e);
        }


        # endregion

        #region IControl-Member and Region-Handling

        Region clipRegion = new Region();

        bool clipRegionHasData = false;


        void IControl.Invalidate() {
#if TraceInvalidate
            Console.WriteLine("IControl.Invalidate()");
#endif
            lock (clipRegion) {
#if UseRegion

                if (clipRegionHasData)
                    clipRegion.Union(this.ClientRectangle);
#else
                clipRegionHasData = false;
                clipRegion.MakeInfinite();
#endif
                this.Invalidate();
            }
        }

        void IControl.Invalidate(Rectangle rect) {
#if TraceInvalidate
            Console.WriteLine("IControl.Invalidate(rect)" + rect);
#endif
            lock (clipRegion) {
#if UseRegion

                if (clipRegionHasData)
                    clipRegion.Union(rect);
#else
                clipRegionHasData = false;
                clipRegion.MakeInfinite();
#endif

                this.Invalidate (rect);

            }
        }


        void IControl.Invalidate(Region region) {
#if TraceInvalidate
            Console.WriteLine("IControl.Invalidate(region)");
#endif
            lock (clipRegion) {
                if (!clipRegionHasData) {
                    clipRegion.Intersect(region);
                } else {
                    clipRegion.Union(region);
                }

                clipRegionHasData = true;

#if UseRegion
                // regionHandling and Invalidate(Region) 
                // seems not work too good with more complicated regions
                // so, curiosly, this is much slower:
                base.Invalidate (region);

#else
                // complicated region leeds to more unnecessary OnPaint-Calls, 
                // so invalidate a rectangle instead of a region

                base.Invalidate(Rectangle.Ceiling(region.GetBounds(ShapeUtils.DeviceContext)));

#endif
            }

            // curiosly, calling Update is faster on windows, 
            // but not on linux:
            if (!Commons.Unix) {
#if UseRegion
#else
                Update();
#endif
            }
        }

        void IControl.Invalidate(GraphicsPath path) {
            throw new NotImplementedException("Invalidate over path not implemented");
        }
        #endregion

        #region IScrollable Member

        protected override void OnScroll(ScrollEventArgs se) {
            scrollChanged = true;
            base.OnScroll(se);
        }

        /// <summary>
        /// to avoid calls of AutoScrollPosition as it is expensive
        /// </summary>
        bool scrollChanged = true;
        private Point _scrollPosition = Point.Empty;

        public virtual Point ScrollPosition {
            get {
                if (true) {//(scrollChanged does not work) {
                    Point point = this.AutoScrollPosition;
                    _scrollPosition = new Point(-point.X, -point.Y);
                    scrollChanged = false;
                }
                return _scrollPosition;
            }
            set {
                if (_scrollPosition != value) {
                    _scrollPosition = value;
                    this.AutoScrollPosition = _scrollPosition;
                    scrollChanged = true;
                }
                // we need this as for some zoom-problems, 
                if (Commons.Mono && scrollChanged) {
                    this.Update();
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Size ScrollMinSize {
            get { return this.AutoScrollMinSize; }
            set {
            	// remark: this workaround is for mono 1.9; it is fixed in the development branch
            	// if you use mono <=1.9, uncomment the following line and comment if (false)
            	// if (Commons.Mono){
                if (false){
                # region monoWorkaroud
                    Size _scrollMinSize = this.AutoScrollMinSize;
                    if (value != _scrollMinSize) {
                        this.AutoScrollMinSize = value;
                        // nice, but doesnt give a invalidate 
                        // if scrollbarwasvisible == true && scrollbarIsVisble == false
                        bool vWasVisible = VerticalScroll.Visible;
                        bool hWasVisible = HorizontalScroll.Visible;
                        PerformLayout(this, "AutoScrollMinSize");
                        bool doUpdate = false;
                        if (vWasVisible && !VerticalScroll.Visible) {
                            Rectangle rect = this.Bounds;
                            rect.Location = new Point(rect.Right - SystemInformation.VerticalScrollBarWidth, 0);
                            rect.Size = new Size(SystemInformation.VerticalScrollBarWidth, rect.Height);
                            this.Invalidate(rect);
                            doUpdate = true;
                        }
                        if (hWasVisible && !HorizontalScroll.Visible) {
                            Rectangle rect = this.Bounds;
                            rect.Location = new Point(0, rect.Height - SystemInformation.HorizontalScrollBarHeight);
                            rect.Size = new Size(rect.Width, SystemInformation.HorizontalScrollBarHeight);
                            this.Invalidate(rect);
                            doUpdate = true;
                        }
                        if(doUpdate) {
                            this.Update();
                        }
                        return;
                        // mono does not update the ScrollBars if you set AutoScrollMinSize
                        // this is why I try to workaround this behavior
                        bool vScrollWasVisible = VerticalScroll.Visible;
                        bool hScrollWasVisible = HorizontalScroll.Visible;
                        int VSize = vScrollWasVisible ? SystemInformation.VerticalScrollBarWidth : 0;
                        int HSize = hScrollWasVisible ? SystemInformation.HorizontalScrollBarHeight : 0;
                        // this works, with bugs:
                        // it seems that in mono, we got wrong zoomfactors with FitToWidth
                        bool hScrollNeedsVisible = ( this.Size.Width ) < value.Width;
                        bool vScrollNeedsVisible = this.Size.Height < value.Height;

                        if (hScrollNeedsVisible) {
                            HorizontalScroll.Maximum = value.Width;
                        }

                        if (vScrollNeedsVisible) {
                            VerticalScroll.Maximum = value.Height;
                        }

                        this.HorizontalScroll.Visible = hScrollNeedsVisible;
                        this.VerticalScroll.Visible = vScrollNeedsVisible;
                        bool needsUpdate = false;
                        // Invalidate if visibility has changed:
                        if (!hScrollNeedsVisible && hScrollWasVisible) {
                            Rectangle rect = this.Bounds;
                            rect.Location = new Point (0, rect.Height - HSize);
                            rect.Size = new Size (rect.Width, HSize);
                            ( (IControl) this ).Invalidate (rect);
                            needsUpdate = true;
                        }
                        if (!vScrollNeedsVisible && vScrollWasVisible) {
                            Rectangle rect = this.Bounds;
                            rect.Location = new Point (rect.Width - VSize, 0);
                            rect.Size = new Size (VSize, rect.Height);
                            ( (IControl) this ).Invalidate (rect);
                            needsUpdate = true;
                        }
                        if (needsUpdate) {
                            this.Update ();
                        }
                    }
                }
                #endregion
                else {
                    this.AutoScrollMinSize = value;    
                }
            }
        }
        #endregion

        # region IZoomable-Member

        ///<directed>True</directed>
        protected ZoomState _zoom = ZoomState.FitToScreen;
        public virtual ZoomState ZoomState {
            set {
                if (value != _zoom) {
                    _zoom = value;
                    UpdateZoom();
                }
            }
            get { return _zoom; }
        }

        protected float _zoomFactor = 1.0f;
        public virtual float ZoomFactor {
            get { return _zoomFactor; }
            set { _zoomFactor = value; }
        }

        // Fit to selected zoom
        protected void FitToZoom(ZoomState zoomState) {
            Size rc = ClientRectangle.Size;

            switch (zoomState) {
                case ZoomState.FitToScreen:
                    rc = this.Size;
                    _zoomFactor = Math.Min(
                        (float)rc.Width / DataSize.Width,
                        (float)rc.Height / DataSize.Height);
                    break;
                case ZoomState.FitToWidth:
                    _zoomFactor = (float)rc.Width / DataSize.Width;
                    //ScrollPosition = new Point();
                    break;
                case ZoomState.FitToHeigth:
                    _zoomFactor = (float)rc.Height / DataSize.Height;
                    break;
                case ZoomState.Original:
                    _zoomFactor = 1.0f;
                    break;
            }
        }

        public event EventHandler ZoomChanged;

        // Update zoom factor
        public virtual void UpdateZoom() {
            FitToZoom(_zoom);

            ScrollMinSize = new Size(
                    (int)(DataSize.Width * ZoomFactor),
                    (int)(DataSize.Height * ZoomFactor));

            ((IControl)this).Invalidate();

            // we need this as for some zoom-problems
            if (Commons.Mono) {
                this.Update();
            }
            if (ZoomChanged != null)
                ZoomChanged(this, null);

        }

        #endregion

        # region Action-Handling

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ActionDispatcher ActionDispatcher = new ActionDispatcher();

        public virtual TAction GetAction<TAction>() {
            return ActionDispatcher.GetAction<TAction>();
        }

        protected SelectionBase _selectAction = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual SelectionBase SelectAction {
            get {
                if (_selectAction == null) {
                    ActionDispatcher.Add(_selectAction =
                        displayKit.SelectAction(this, DataLayer.Transformer));
                }
                return _selectAction;
            }
            set { ActionDispatcher.Add(value, ref _selectAction); }
        }

        protected ScrollAction _scrollAction = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ScrollAction ScrollAction {
            get {
                if (_scrollAction == null) {
                    ActionDispatcher.Add(_scrollAction = new ScrollAction(this));
                }
                return _scrollAction;
            }
            set { ActionDispatcher.Add(value, ref _scrollAction); }
        }

        protected ZoomAction _zoomAction = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ZoomAction ZoomAction {
            get {
                if (_zoomAction == null) {
                    ActionDispatcher.Add(_zoomAction = new ZoomAction(this, this, DataLayer.Transformer));
                }
                return _zoomAction;
            }
            set { ActionDispatcher.Add(value, ref _zoomAction); }
        }

        public virtual void Clear() {
            Data = null;
            DataLayer = null;


            if (_selectAction != null) { SelectAction = null; }
            if (_scrollAction != null) { ScrollAction = null; }
            if (_zoomAction != null) { ZoomAction = null; }
        }

        # endregion

        # region Action-Dispatching
        protected override void OnMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e);
            ActionDispatcher.OnMouseDown(e);
        }
        protected override void OnMouseHover(EventArgs e) {
            base.OnMouseHover(e);
            Point pos = this.PointToClient(MousePosition);
            MouseEventArgs mouseEventArgs = new MouseEventArgs(MouseButtons, 0, pos.X, pos.Y, 0);
            ActionDispatcher.OnMouseHover(mouseEventArgs);

        }
        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);
            ActionDispatcher.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            base.OnMouseUp(e);
            ActionDispatcher.OnMouseUp(e);
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            base.OnKeyDown(e);
            ActionDispatcher.OnKeyDown(e);
        }
        protected override void OnKeyPress(KeyPressEventArgs e) {
            base.OnKeyPress(e);
            ActionDispatcher.OnKeyPress(e);
        }
        protected override void OnKeyUp(KeyEventArgs e) {
            base.OnKeyUp(e);
            ActionDispatcher.OnKeyUp(e);
        }
        #endregion

        #region IControl-Layout

        public virtual void CommandsExecute() {
            ActionDispatcher.CommandsExecute();
        }

        public virtual void CommandsInvoke() {
            ActionDispatcher.CommandsInvoke();
        }

        #endregion

        #region dragdrop
        protected override void OnGiveFeedback(GiveFeedbackEventArgs gfbevent) {
            ActionDispatcher.OnGiveFeedback(gfbevent);
            base.OnGiveFeedback(gfbevent);

        }
        protected override void OnQueryContinueDrag(QueryContinueDragEventArgs qcdevent) {
            ActionDispatcher.OnQueryContinueDrag(qcdevent);
            base.OnQueryContinueDrag(qcdevent);

        }

        protected override void OnDragOver(DragEventArgs drgevent) {
            ActionDispatcher.OnDragOver(drgevent);
            base.OnDragOver(drgevent);

        }

        protected override void OnDragDrop(DragEventArgs drgevent) {
            ActionDispatcher.OnDragDrop(drgevent);
            base.OnDragDrop(drgevent);

        }
        protected override void OnDragLeave(EventArgs e) {
            ActionDispatcher.OnDragLeave(e);
            base.OnDragLeave(e);
        }
        #endregion
    }
}

