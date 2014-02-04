using Xwt;
using Xwt.Drawing;

namespace Limaki.View.XwtBackend {
    public abstract class ScrollDisplayBackend : ScrollView {
        public ScrollDisplayBackend () {
            this.Canvas = new DisplayCanvas();
            this.Content = this.Canvas;
        }

        public class DisplayCanvas : Canvas {
            internal virtual void InternalDraw (Context ctx, Rectangle dirtyRect) {
                this.OnDraw(ctx, dirtyRect);
            }
        }

        public DisplayCanvas Canvas { get; set; }

        protected virtual void OnDraw (Context ctx, Rectangle dirtyRect) {
            Canvas.InternalDraw(ctx, dirtyRect);
        }

        public void QueueDraw () { Canvas.QueueDraw(); }

        public void QueueDraw (Rectangle rectangle) { Canvas.QueueDraw(rectangle); }
        public Rectangle Bounds { get { return Canvas.Bounds; } }
    }
}