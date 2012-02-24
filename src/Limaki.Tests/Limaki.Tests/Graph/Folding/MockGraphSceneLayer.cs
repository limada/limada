using Limaki.Graphs;
using Limaki.Presenter.Rendering;
using Limaki.Presenter.UI;
using Limaki.Presenter.Display;
using Limaki.Presenter;
using Limaki.Drawing;
using Limaki.Common;
using Limaki.Presenter.UI.GraphScene;
using Xwt;

namespace Limaki.Tests.Graph.Wrappers {
    public class MockGraphSceneLayer<TItem, TEdge> : GraphSceneLayer<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {

        public override void DataChanged() { }

        public override void OnPaint(IRenderEventArgs e) {
            this.Renderer.Render(this.Data, e);
        }
    }

    public class MockRenderer<T> : IDeviceRenderer
where T : class {

        public virtual IDisplayDevice<T> Device { get; set; }

        public virtual IDisplay<T> Display { get; set; }

        public MockRenderer() {}

        public void Render() {
            Device.Invalidate();
        }

        public void Render(IClipper clipper) {
            if (clipper.RenderAll) {
                Device.Invalidate();
            } else {
                Device.Invalidate(clipper.Bounds);
            }
        }

        public bool Opaque { get; set; }

        public Get<Xwt.Drawing.Color> BackColor { get; set; }

        protected object lockRender = new object();

        public virtual void OnPaint(IRenderEventArgs e) {
            var display = this.Display;
            var data = display.Data;

            if (data != null) {

                lock (display.Clipper) {
                    display.EventControler.OnPaint(e);


                    display.Clipper.Clear();
                }

            } else {
                if (Opaque) { // draw background

                }
            }
        }


    }

    public class MockCursorHandler : IDeviceCursor {
        public void SetCursor(Anchor anchor, bool hasHit) { }
        public void SetEdgeCursor(Anchor anchor) { }
        public void SaveCursor() { }
        public void RestoreCursor() { }
    }

    public class MockGripPainter : GripPainterBase {
        public override void Render(ISurface surface) { }
    }

    public class MockMoveResizeRenderer : MoveResizeRendererBase {
        public override GripPainterBase GripPainter {
            get {
                if (base.GripPainter == null) {
                    base.GripPainter = new MockGripPainter();
                }
                return base.GripPainter;
            }
            set {
                base.GripPainter = value;
            }
        }

        public override void InvalidateShapeOutline(IShape oldShape, IShape newShape) {
            var clipper = this.Clipper();
            var camera = this.Camera;
            clipper.Add(oldShape.Hull(camera.Matrice, 0, false));
            clipper.Add(newShape.Hull(camera.Matrice, 0, false));
        }

        public override void OnPaint(IRenderEventArgs e) { }
    }

    public class MockSelectionRenderer : MockMoveResizeRenderer, IShapedSelectionRenderer {
        private IPainter _painter = null;
        public IPainter Painter {
            get {
                if ((_painter == null) && (Shape != null)) {
                    var factory = Registry.Pool.TryGetCreate<IPainterFactory>();
                    _painter = factory.CreatePainter(Shape);
                }
                return _painter;
            }
            set { _painter = value; }
        }

        public RenderType RenderType { get; set; }

    }

    public class MockDevice<T> : IDisplayDevice<T> where T : class {
        public IDisplay<T> Display { get; set; }


        IDisplay IDisplayDevice.Display {
            get { return this.Display; }
            set { this.Display = value as IDisplay<T>; }
        }

        public RectangleD ClientRectangle {
            get { return new RectangleD(Display.Viewport.DataOrigin, Display.Viewport.DataSize); }
        }

        public Size Size {
            get { return Display.Viewport.DataSize; }
        }

        public void Update() { }

        public void Invalidate() {}

        public void Invalidate(RectangleD rect) {}

        public Point PointToClient(Point source) {
            return source;
        }
        public void Dispose(){}
    }

    public class MockDeviceComposer<TData> : DeviceComposer<TData, IDisplayDevice<TData>>
    where TData : class {
        public EventControler EventControler { get; set; }
        public override void Factor(Display<TData> display) {
            this.Device = new MockDevice<TData>();
            Device.Display = display;
            display.Device = Device;

			var deviceRenderer = new MockRenderer<TData>();
			deviceRenderer.Device = Device;
			deviceRenderer.Display = display;
            this.DeviceRenderer = deviceRenderer;
            this.EventControler = new EventControler();
            this.ViewPort = new ViewPort();
            this.DeviceCursor = new MockCursorHandler();

            this.SelectionRenderer = new MockSelectionRenderer();
            this.MoveResizeRenderer = new MockMoveResizeRenderer();
        }

        public override void Compose(Display<TData> display) {
            this.DeviceRenderer.BackColor = () => display.BackColor;

            display.DeviceRenderer = this.DeviceRenderer;
            display.DataLayer = this.DataLayer;
            display.EventControler = this.EventControler;
            display.Viewport = this.ViewPort;
            display.DeviceCursor = this.DeviceCursor;

            this.MoveResizeRenderer.Device = this.Device;
            display.MoveResizeRenderer = this.MoveResizeRenderer;

            this.SelectionRenderer.Device = this.Device;
            display.SelectionRenderer = this.SelectionRenderer;
        }
    }
}