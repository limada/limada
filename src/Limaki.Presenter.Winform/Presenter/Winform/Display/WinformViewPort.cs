using System;
using System.Drawing;
using System.Windows.Forms;
using Limaki.Drawing;
using Limaki.Drawing.GDI;

namespace Limaki.Presenter.Winform.Display {
    public class GDIViewPort : ViewPort {
        public override Limaki.Drawing.Matrice CreateMatrix() {
            return new GDIMatrice();
        }
    }

    public class WinformViewPort : GDIViewPort {
        protected ScrollableControl device = null;
        public WinformViewPort(ScrollableControl userControl) {
            this.device = userControl;
        }

        bool scrollChanged = false;
        private PointI _scrollPosition = PointI.Empty;
        public override Limaki.Drawing.PointI ClipOrigin {
            get {
                if (!useOnScroll) {
                    Point point = device.AutoScrollPosition;
                    _scrollPosition = new PointI(-point.X, -point.Y);
                    scrollChanged = false;
                }
                return _scrollPosition;
            }
            set {
                if (_scrollPosition != value) {
                    _scrollPosition = value;
                    device.AutoScrollPosition =
                        GDIConverter.Convert(_scrollPosition);

                    scrollChanged = true;
                }

            }
        }

        public override SizeI ClipSize {
            get { return GDIConverter.Convert(device.Size); }
        }

        private bool _scrollMinSizeChanging = false;
        public override SizeI DataSize {
            get {
                if (useOnScroll) {
                    return _scrollMinSize;
                } else {
                    return GDIConverter.Convert(device.AutoScrollMinSize);
                }
            }
            set {
                _scrollMinSizeChanging = true;
                device.AutoScrollMinSize = GDIConverter.Convert(value);
                _scrollMinSize = value;
                _scrollMinSizeChanging = false;
            }
        }

        private bool useOnScroll = true; // false: works on mono.windows, but not on linux
        public virtual void OnScroll(ScrollEventArgs se) {
            if (useOnScroll) {
                if (se.OldValue != se.NewValue) {
                    scrollChanged = true;
                  
                    if (Commons.Mono) {
                        // mono does not deliver ScrollOrientation
                        var deltaX = 0;
                        var deltaY = 0;

                        var point = device.AutoScrollPosition;
                        deltaX = _scrollPosition.X + point.X;
                        deltaY = _scrollPosition.Y + point.Y;

                        //update and set new scrollPosition
                        _scrollPosition = new PointI(-point.X, -point.Y);
                        
                        if(deltaX != 0 || deltaY != 0){
                            // OnScroll works different on mono
                            // Mono first calls OnPaint, then OnScroll
                            // everything is drawn with wrong offset
                            // so we have to invalidate the device again
                            // this is a workaroud; we have two OnPaint then!
                            // think about a whole ViewPort for Mono.Linux
#if TraceInvalidate                           
                            System.Console.Out.WriteLine ("OnScroll  x={0} y={1}", deltaX, deltaY);
#endif
                            UpdateCamera ();

                            device.Invalidate ();
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
                var point = device.AutoScrollPosition;
                _scrollPosition = new PointI(-point.X, -point.Y);
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

            device.Invalidate();

            if (ZoomChanged != null)
                ZoomChanged(this, null);

        }

    }
}