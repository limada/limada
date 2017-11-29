using Xwt.Drawing;
using System;

namespace Xwt.Widgets {
    
    public class DelegateCanvas : Canvas {

        public Action<Context, Rectangle> Draw { get; set; }

        protected override void OnDraw (Context ctx, Rectangle dirtyRect) {
            base.OnDraw (ctx, dirtyRect);
            Draw.Invoke (ctx, dirtyRect);
        }
    }
}