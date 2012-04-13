using Xwt.Drawing;

namespace Xwt.Widgets {
    public class ScrollableCanvas : Canvas {


        protected override void OnDraw (Context ctx) {
            ctx.Translate (-VisibleRect.X, -VisibleRect.Y);
        }

        protected override bool SupportsCustomScrolling { get { return true; } }

        public Size ImageSize { get; set; }
        Rectangle? _visibleRect = null;
        protected Rectangle VisibleRect {
            get {
                if (_visibleRect == null)
                    _visibleRect = new Rectangle (0, 0, Bounds.Width, Bounds.Height);
                return _visibleRect.Value;
            }
            set { _visibleRect = value; }
        }

        ScrollAdjustment hscroll;
        ScrollAdjustment vscroll;

        protected override void SetScrollAdjustments (ScrollAdjustment horizontal, ScrollAdjustment vertical) {
            hscroll = horizontal;
            vscroll = vertical;

            hscroll.UpperValue = ImageSize.Width;
            hscroll.PageIncrement = Bounds.Width;
            hscroll.PageSize = Bounds.Width;
            hscroll.ValueChanged += delegate {
                VisibleRect = new Rectangle (hscroll.Value, vscroll.Value, hscroll.PageSize, vscroll.PageSize);
                QueueDraw ();
            };

            vscroll.UpperValue = ImageSize.Height;
            vscroll.PageIncrement = Bounds.Height;
            vscroll.PageSize = Bounds.Height;
            vscroll.ValueChanged += delegate {
                VisibleRect = new Rectangle (hscroll.Value, vscroll.Value, hscroll.PageSize, vscroll.PageSize);
                QueueDraw ();
            };
        }

        protected override void OnBoundsChanged () {
            vscroll.PageSize = vscroll.PageIncrement = Bounds.Height;
            hscroll.PageSize = hscroll.PageIncrement = Bounds.Width;
            VisibleRect = new Rectangle (hscroll.Value, vscroll.Value, hscroll.PageSize, vscroll.PageSize);

        }

    }
}