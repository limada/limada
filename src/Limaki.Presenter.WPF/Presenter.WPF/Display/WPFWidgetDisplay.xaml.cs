using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Limaki.Common;
using Limaki.Drawing.WPF;
using Limaki.Presenter.Display;
using System;
using System.Windows.Input;
using System.IO;
using System.Diagnostics;
using Limaki.Graphs;


namespace Limaki.Presenter.WPF.Display {



    public abstract class WPFDisplay<T> : UserControl, IWPFControl, IDisplayDevice<T> {
        public WPFDisplay() {
            Initialize();
        }

        public abstract DisplayFactory<T> CreateDisplayFactory(WPFDisplay<T> device);

        protected void Initialize() {
            if (Registry.ConcreteContext == null) {
                var resourceLoader = new WPFContextRecourceLoader();
                Registry.ConcreteContext = resourceLoader.CreateContext();
                resourceLoader.ApplyResources(Registry.ConcreteContext);
            }
            InitComponents();

            var factory = CreateDisplayFactory(this);
            var display = factory.Create();
            _display = display;
            factory.Compose(display);

        }

        IDisplay<T> _display = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IDisplay<T> Display {
            get { return _display; }
            set { _display = value; }
        }

        IDisplay IDisplayDevice.Display {
            get { return this.Display; }
            set { this.Display = value as IDisplay<T>; }
        }

        protected WPFRenderer<T> _deviceRenderer = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IDeviceRenderer DeviceRenderer {
            get { return _deviceRenderer; }
            set { _deviceRenderer = value as WPFRenderer<T>; }
        }

        protected WPFViewPort<T> _deviceViewPort = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IViewport DeviceViewPort {
            get { return _deviceViewPort; }
            set { _deviceViewPort = value as WPFViewPort<T>; }
        }

        public virtual Color BackColor {
            get {
                return DrawingConverter.Convert(Display.BackColor);
            }
            set {
                var color = DrawingConverter.Convert(value);
                if (Display.BackColor != color) {
                    Display.BackColor = color;
                }

            }
        }

        public Limaki.Drawing.PointI ScrollPosition {
            get {
                return new Limaki.Drawing.PointI(
                    (int)this.ScrollViewer.HorizontalOffset,
                    (int)this.ScrollViewer.VerticalOffset);
            }
            set {
                this.ScrollViewer.ScrollToHorizontalOffset(value.X);
                this.ScrollViewer.ScrollToVerticalOffset(value.Y);
            }
        }

        public Limaki.Drawing.SizeI ScrollMinSize {
            get {
                return new Limaki.Drawing.SizeI(
                    (int)this.LayoutRoot.Width,
                    (int)this.LayoutRoot.Height);
            }
            set {
                this.LayoutRoot.Height = value.Height;
                this.LayoutRoot.Width = value.Width;
            }
        }
        #region IControl Member

        public virtual Limaki.Drawing.RectangleI ClientRectangle {
            get {
                return new Limaki.Drawing.RectangleI(
                 ScrollPosition,
                 Limaki.Drawing.SizeI.Subtract(ScrollMinSize, new Limaki.Drawing.SizeI(ScrollPosition))
                 );
            }
        }

        public virtual Limaki.Drawing.SizeI Size {
            get { return new Limaki.Drawing.SizeI((int)this.Width, (int)this.Height); }
        }

        void IControl.Invalidate(Limaki.Drawing.RectangleI rect) {
            this.DeviceRenderer.Render(new BoundsClipper(rect));
        }

        Limaki.Drawing.PointI IControl.PointToClient(Limaki.Drawing.PointI source) {
            return DrawingConverter.Convert(
                this.PointFromScreen(DrawingConverter.Convert(source))
             );
        }

        public void Update() { }

        public void Invalidate() { this.DeviceRenderer.Render(); }

        #endregion

        #region Subcontrols
        internal System.Windows.Controls.ScrollViewer ScrollViewer;
        internal System.Windows.Controls.Canvas LayoutRoot;

        void InitComponents() {
            ScrollViewer = new ScrollViewer();
            ScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            ScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;

            LayoutRoot = new Canvas();
            LayoutRoot.HorizontalAlignment = HorizontalAlignment.Left;
            LayoutRoot.VerticalAlignment = VerticalAlignment.Top;

            ScrollViewer.Content = LayoutRoot;
            this.Background = new SolidColorBrush(Colors.White);
            this.IsHitTestVisible = true;

            LayoutRoot.Background = this.Background;
            LayoutRoot.IsHitTestVisible = true;

            this.Content = ScrollViewer;

        }


        #endregion
        #region IWPFControl Member


        WPFSurface _surface = null;
        public WPFSurface Surface {
            get {
                if (_surface == null) {
                    _surface = new WPFSurface(this.LayoutRoot);
                }
                return _surface;
            }
        }


        #endregion

        #region Event-Routing

        public Point MousePosition { get; set; }
#if ! SILVERLIGHT

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e) {
            MousePosition = e.GetPosition(this);
            base.OnPreviewMouseDown(e);
            Display.EventControler
                .OnMouseDown(Converter.Convert(e, this));

        }

        protected override void OnPreviewMouseMove(MouseEventArgs e) {
            base.OnPreviewMouseMove(e);
            MousePosition = e.GetPosition(this);
            Display.EventControler.OnMouseMove(Converter.Convert(e, this));
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e) {
            base.OnPreviewMouseUp(e);
            MousePosition = e.GetPosition(this);
            Display.EventControler.OnMouseUp(Converter.Convert(e, this));
        }


        protected override void OnPreviewKeyDown(KeyEventArgs e) {
            
            Display.EventControler.OnKeyDown(Converter.Convert(e, this,MousePosition));
            base.OnPreviewKeyDown(e);

        }

#else

        void message(string m) {
            Debug.WriteLine(m);
        }

        void Message(string m, object originalSource) {
            if (originalSource != null)
                message(m + originalSource.ToString());
            else
                message(m);
        }


        // never called, cause LayoutRoot.RoutedMouseLeftButtonDown catches it
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {

            Message("OnMouseLeftButtonDown\t",e.OriginalSource);
            base.OnMouseLeftButtonDown(e);
            //this doesnt work: this.CaptureMouse();
            Display.EventControler.OnMouseDown(Converter.Convert(e, this.LayoutRoot));

        }

        protected override void OnMouseMove(MouseEventArgs e) {
            try {
                Message ("OnMouseMove\t", e.OriginalSource);
                base.OnMouseMove (e);
                Display.EventControler.OnMouseMove(Converter.Convert(e, this.LayoutRoot));
            } catch (Exception ex) {
                Debug.WriteLine (ex.Message);
            }
        }


        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e) {
            Message("OnMouseLeftButtonUp\t",e.OriginalSource);
            base.OnMouseLeftButtonUp(e);
            Display.EventControler.OnMouseUp(Converter.Convert(e, this.LayoutRoot));
            //this doesnt work: this.ReleaseMouseCapture ();

        }

        protected override void OnKeyDown(KeyEventArgs e) {
            Display.EventControler.OnKeyDown(Converter.Convert(e, this.LayoutRoot));
            base.OnKeyDown(e);
        }
#endif

        #endregion

    }
}