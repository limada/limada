using System;
using Xwt;
using Xwt.Drawing;

namespace Xwt.Drawing {

    public class PaintingImage : DrawingImage {

        public Action<Context> Paint { get; protected set; }

        public PaintingImage (Size s, Action<Context> paint)
        {
            this.Paint = paint;
            Size = s;
        }

        protected override void OnDraw (Context ctx, Rectangle bounds)
        {
            ctx.Save ();
            ctx.Translate (bounds.Location);
            ctx.Scale (bounds.Width / Size.Width, bounds.Height / Size.Height);
            Paint (ctx);
            ctx.Restore ();
        }

        
    }
}