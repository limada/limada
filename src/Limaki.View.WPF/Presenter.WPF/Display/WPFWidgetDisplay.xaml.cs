using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Limaki.Common;
using Limaki.Drawing.WPF;
using System;
using System.Windows.Input;
using System.IO;
using System.Diagnostics;
using Limaki.Graphs;
using Limaki.View.Clipping;
using Limaki.View.Display;
using Limaki.View.Rendering;
using Xwt;
using Canvas = System.Windows.Controls.Canvas;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Point = System.Windows.Point;
using Size = Xwt.Size;


namespace Limaki.View.WPF.Display {



    public abstract class WPFDisplay<T> : UserControl, IWPFControl, IDisplayBackend<T> {
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

        IDisplay IDisplayBackend.Display {
            get { return this.Display; }
            set { this.Display = value as IDisplay<T>; }
        }

        protected WPFRenderer<T> _deviceRenderer = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IBackendRenderer DeviceRenderer {
            get { return _deviceRenderer; }
            set { _deviceRenderer = value as WPFRenderer<T>; }
        }

        protected WpfViewport<T> DeviceViewport = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IViewport DeviceViewPort {
            get { return DeviceViewport; }
            set { DeviceViewport = value as WpfViewport<T>; }
        }

        public virtual Color BackColor {
            get {
                return DrawingConverter.Convert(Display.BackColor);
            }
            set {
                var color = DrawingConverter.ToXwt(value);
                if (!Display.BackColor.Equals(color)) {
                    Display.BackColor = color;
                }

            }
        }

        public Xwt.Point ScrollPosition {
            get {
                return new Xwt.Point(
                    (int)this.ScrollViewer.HorizontalOffset,
                    (int)this.ScrollViewer.VerticalOffset);
            }
            set {
                this.ScrollViewer.ScrollToHorizontalOffset(value.X);
                this.ScrollViewer.ScrollToVerticalOffset(value.Y);
            }
        }

        public Size ScrollMinSize {
            get {
                return new Size(
                    (int)this.LayoutRoot.Width,
                    (int)this.LayoutRoot.Height);
            }
            set {
                this.LayoutRoot.Height = value.Height;
                this.LayoutRoot.Width = value.Width;
            }
        }
        #region IControl Member

        public virtual Rectangle ClientRectangle {
            get {
                return new Rectangle(
                 ScrollPosition,
                 ScrollMinSize - (Size)ScrollPosition
                 );
            }
        }

        public virtual Size Size {
            get { return new Size((int)this.Width, (int)this.Height); }
        }

        void IWidgetBackend.Invalidate(Rectangle rect) {
            this.DeviceRenderer.Render(new BoundsClipper(rect));
        }

        Xwt.Point IWidgetBackend.PointToClient(Xwt.Point source) {
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
            
            Display.EventControler.OnKeyPressed(Converter.Convert(e, this,MousePosition));
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

        public void Dispose(){}
    }
}