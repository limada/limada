using Limaki.Graphs;
using Limaki.View.Rendering;
using Limaki.View.UI;
using Limaki.View.Visualizers;
using Limaki.View;
using Limaki.Drawing;
using Limaki.Common;
using Limaki.View.UI.GraphScene;
using Xwt;

namespace Limaki.Tests.Graph.Wrappers {

    public class MockGraphSceneLayer<TItem, TEdge> : GraphSceneLayer<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {

        public override void DataChanged() { }

        public override void OnPaint(IRenderEventArgs e) {
            this.Renderer.Render(this.Data, e);
        }
    }

    public class MockRenderer<T> : IBackendRenderer
        where T : class {

        public virtual IDisplayBackend<T> Backend { get; set; }

        public virtual IDisplay<T> Display { get; set; }

        public MockRenderer() {}

        public void Render() {
            Backend.Invalidate();
        }

        public void Render(IClipper clipper) {
            if (clipper.RenderAll) {
                Backend.Invalidate();
            } else {
                Backend.Invalidate(clipper.Bounds);
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

    public class MockCursorHandler : ICursorHandler {
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
            clipper.Add(oldShape.Hull(camera.Matrix, 0, false));
            clipper.Add(newShape.Hull(camera.Matrix, 0, false));
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

    public class MockDisplayBackend<T> : IDisplayBackend<T> where T : class {
        public IDisplay<T> Display { get; set; }


        IDisplay IDisplayBackend.Display {
            get { return this.Display; }
            set { this.Display = value as IDisplay<T>; }
        }

        public Rectangle ClientRectangle {
            get { return new Rectangle(Display.Viewport.DataOrigin, Display.Viewport.DataSize); }
        }

        public Size Size {
            get { return Display.Viewport.DataSize; }
        }

        public void Update() { }

        public void Invalidate() {}

        public void Invalidate(Rectangle rect) {}

        public Point PointToClient(Point source) {
            return source;
        }
        public void Dispose(){}
    }

    public class MockBackendComposer<TData> : BackendComposer<TData, IDisplayBackend<TData>>
        where TData : class {
        public EventControler EventControler { get; set; }
        public override void Factor(Display<TData> display) {
            this.Backend = new MockDisplayBackend<TData>();
            Backend.Display = display;
            display.Backend = Backend;

			var deviceRenderer = new MockRenderer<TData>();
			deviceRenderer.Backend = Backend;
			deviceRenderer.Display = display;
            this.BackendRenderer = deviceRenderer;
            this.EventControler = new EventControler();
            this.ViewPort = new Viewport();
            this.CursorHandler = new MockCursorHandler();

            this.SelectionRenderer = new MockSelectionRenderer();
            this.MoveResizeRenderer = new MockMoveResizeRenderer();
        }

        public override void Compose(Display<TData> display) {
            this.BackendRenderer.BackColor = () => display.BackColor;

            display.BackendRenderer = this.BackendRenderer;
            display.DataLayer = this.DataLayer;
            display.EventControler = this.EventControler;
            display.Viewport = this.ViewPort;
            display.CursorHandler = this.CursorHandler;

            this.MoveResizeRenderer.Backend = this.Backend;
            display.MoveResizeRenderer = this.MoveResizeRenderer;

            this.SelectionRenderer.Backend = this.Backend;
            display.SelectionRenderer = this.SelectionRenderer;
        }
    }
}