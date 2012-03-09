using System.Collections.Generic;
using System.ComponentModel;
using Xwt;
using Xwt.Backends;

namespace Limaki.Painting {

    public class PaintCanvas : Widget {

        Size minSize;
        Size naturalSize;
        Dictionary<Widget, Rectangle> positions;

        protected new class EventSink : Widget.EventSink, ICanvasEventSink {
            public void OnDraw(object context) {
                PaintContext ctx = null;
                try {
                    if (context == null)
                        ctx = new PaintContext(Parent);
                    else
                        ctx = new PaintContext(context);
                    ((PaintCanvas)Parent).OnDraw(ctx);
                } finally {
                    ctx.Dispose();
                }
            }

            public void OnBoundsChanged() {
                ((PaintCanvas)Parent).OnBoundsChanged();
            }
        }

        public PaintCanvas() {
        }

        public void AddChild(Widget w) {
            AddChild(w, 0, 0);
        }

        public void AddChild(Widget w, double x, double y) {
            var ws = w as IWidgetSurface;
            var pw = ws.GetPreferredWidth().NaturalSize;
            AddChild(w, new Rectangle(x, y, pw, ws.GetPreferredHeightForWidth(pw).NaturalSize));
        }

        public void AddChild(Widget w, Rectangle rect) {
            if (positions != null)
                positions = new Dictionary<Widget, Rectangle>();
            var bk = (IWidgetBackend)Widget.GetBackend(w);
            Backend.AddChild(bk);
            Backend.SetChildBounds(bk, rect);
            RegisterChild(w);
            OnPreferredSizeChanged();
        }

        public void RemoveChild(Widget w) {
            if (positions != null)
                positions.Remove(w);
            Backend.RemoveChild((IWidgetBackend)Widget.GetBackend(w));
            UnregisterChild(w);
            OnPreferredSizeChanged();
        }

        public void SetChildBounds(Widget w, Rectangle rect) {
            Backend.SetChildBounds((IWidgetBackend)Widget.GetBackend(w), rect);
            OnPreferredSizeChanged();
        }

        public IEnumerable<Widget> Children {
            get { return ((IWidgetSurface)this).Children; }
        }

        public Rectangle GetChildBounds(Widget w) {
            Rectangle rect;
            if (positions.TryGetValue(w, out rect))
                return rect;
            return Rectangle.Zero;
        }

        protected override Widget.EventSink CreateEventSink() {
            return new EventSink();
        }

        new IPaintCanvasBackend Backend {
            get { return (IPaintCanvasBackend)base.Backend; }
        }

        public void QueueDraw() {
            Backend.QueueDraw();
        }

        public void QueueDraw(Rectangle rect) {
            Backend.QueueDraw(rect);
        }

        protected virtual void OnDraw(PaintContext ctx) {
        }

        protected virtual void OnBoundsChanged() {
        }

        public Rectangle Bounds {
            get { return Backend.Bounds; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Size MinSize {
            get { return minSize; }
            set {
                minSize = value;
                if (naturalSize.Width < minSize.Width)
                    naturalSize.Width = minSize.Width;
                if (naturalSize.Height < minSize.Height)
                    naturalSize.Height = minSize.Height;
                OnPreferredSizeChanged();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Size NaturalSize {
            get { return naturalSize; }
            set {
                naturalSize = value;
                if (minSize.Width > naturalSize.Width)
                    minSize.Width = naturalSize.Width;
                if (minSize.Height > naturalSize.Height)
                    minSize.Height = naturalSize.Height;
                OnPreferredSizeChanged();
            }
        }

        protected override WidgetSize OnGetPreferredWidth() {
            return new WidgetSize(minSize.Width);
        }

        protected override WidgetSize OnGetPreferredHeight() {
            return new WidgetSize(minSize.Height);
        }

        protected override WidgetSize OnGetPreferredHeightForWidth(double width) {
            return OnGetPreferredHeight();
        }

        protected override WidgetSize OnGetPreferredWidthForHeight(double height) {
            return OnGetPreferredWidth();
        }

        protected override void OnPreferredSizeChanged() {
            base.OnPreferredSizeChanged();
            Backend.OnPreferredSizeChanged();
        }
    }
}