/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Windows.Forms;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.View.Viz;
using Xwt;
using Xwt.GdiBackend;

namespace Limaki.SwfBackend.Viz {

    public class SwfViewport : Viewport {

        protected DisplayBackend backend = null;
        public SwfViewport(DisplayBackend userControl) {
            this.backend = userControl;
        }

        bool scrollChanged = false;
        private Xwt.Point _scrollPosition = Xwt.Point.Zero;
        public override Xwt.Point ClipOrigin {
            get {
                if (!useOnScroll) {
                    var point = backend.AutoScrollPosition;
                    _scrollPosition = new Xwt.Point(-point.X, -point.Y);
                    scrollChanged = false;
                }
                return _scrollPosition;
            }
            set {
                if (_scrollPosition != value) {
                    _scrollPosition = value;
                    backend.AutoScrollPosition = _scrollPosition.ToGdi();

                    scrollChanged = true;
                }

            }
        }

        public override Size ClipSize {
            get { return backend.ClientSize.ToXwt(); }
        }

        private bool _scrollMinSizeChanging = false;
        public override Size DataSize {
            get {
                if (useOnScroll) {
                    return _dataSize;
                } else {
                    return backend.AutoScrollMinSize.ToXwt();
                }
            }
            set {
                _scrollMinSizeChanging = true;
                backend.AutoScrollMinSize = value.ToGdi();
                _dataSize = value;
                _scrollMinSizeChanging = false;
                //device.ScrollBarsVisible = false;
            }
        }

        private bool useOnScroll = true; // false: works on mono.windows, but not on linux
        public virtual void OnScroll(ScrollEventArgs se) {
            if (useOnScroll) {
                if (se.OldValue != se.NewValue) {
                    scrollChanged = true;
                  
                    if (OS.Mono) {
                        // mono does not deliver ScrollOrientation
                        var deltaX = 0d;
                        var deltaY = 0d;

                        var point = backend.AutoScrollPosition;
                        deltaX = _scrollPosition.X + point.X;
                        deltaY = _scrollPosition.Y + point.Y;

                        //update and set new scrollPosition
                        _scrollPosition = new Xwt.Point(-point.X, -point.Y);
                        
                        if(deltaX != 0 || deltaY != 0){
                            // OnScroll works different on mono
                            // Mono first calls OnPaint, then OnScroll
                            // everything is drawn with wrong offset
                            // so we have to invalidate the device again
                            // this is a workaroud; we have two OnPaint then!
                            // think about a whole Viewport for Mono.Linux
#if TraceInvalidate                           
                            System.Console.Out.WriteLine ("OnScroll  x={0} y={1}", deltaX, deltaY);
#endif
                            UpdateCamera ();

                            backend.Invalidate ();
                        }
                    } else {
                        if (se.ScrollOrientation == ScrollOrientation.HorizontalScroll) {
                            _scrollPosition.X = se.NewValue;
                        } else {
                            _scrollPosition.Y = se.NewValue;
                        }
                    }
                   
#if TraceInvalidate
            System.Console.Out.WriteLine("OnScroll scrollChanged");
#endif
                }

            }
        }
        public override void Update() {
            base.Update();
            if (useOnScroll) {
                var point = backend.AutoScrollPosition;
                _scrollPosition = new Xwt.Point(-point.X, -point.Y);
            }
           
        }

        public virtual void OnSizeChanged(EventArgs e, Action<EventArgs> onSizeChanged) {
            if (!_scrollMinSizeChanging) {
                if (this.ZoomState == ZoomState.FitToHeigth
                    || this.ZoomState == ZoomState.FitToWidth
                    || this.ZoomState == ZoomState.FitToScreen) {
                    UpdateZoom();
                }
            }
            onSizeChanged(e);
            Update();
        }
        
        public event EventHandler ZoomChanged;

        public override void UpdateZoom() {
            
            FitToZoom(ZoomState);

            Update();

            backend.Invalidate();

            if (ZoomChanged != null)
                ZoomChanged(this, null);

        }

    }
}