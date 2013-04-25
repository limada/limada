using Limaki.Drawing.Shapes;
using Xwt.Drawing;
using Xwt;
using System.Collections.Generic;

namespace Limaki.Drawing.Painters {

    public class ContextPainterExtensions {

        public static void DrawBezier (Context ctx, IList<Point> bezierPoints) {
            ctx.MoveTo(bezierPoints[0]);
            for (int i = 1; i < bezierPoints.Count; i += 3)
                ctx.CurveTo(bezierPoints[i], bezierPoints[i + 1], bezierPoints[i + 2]);
            ctx.ClosePath();

        }

        public static void DrawBezier (Context ctx, BezierSegment seg) {
            ctx.MoveTo(seg.Start);
            ctx.CurveTo(seg.Cp1, seg.Cp2, seg.End);
        }

        public static void DrawPoligon (Context ctx, IList<Point> poligon) {
            ctx.MoveTo(poligon[0]);
            for (int i = 1; i < poligon.Count; i ++)
                ctx.LineTo(poligon[i]);
            ctx.ClosePath();

        }

        public static void DrawRoundedRect (Context ctx, Rectangle rectangle, double radius) {
            // create the arc for the Rectangle sides 
            var l = rectangle.Left;
            var t = rectangle.Top;
            var w = rectangle.Width;
            var h = rectangle.Height;

            // top left  
            ctx.Arc(l + radius, t + radius, radius, 180, 270);
            // top right 
            ctx.Arc(l + w - radius, t + radius, radius, 270, 0);
            // bottom right  
            ctx.Arc(l + w - radius, t + h - radius, radius, 0, 90);
            // bottom left 
            ctx.Arc(l + radius, t + h - radius, radius, 90, 180);
        }

        public static void DrawText (Context ctx, Rectangle rect, string text, Font font, Color textColor) {
            var w = rect.Width;
            var h = rect.Height;
            if (w > 1 && h > 1) {
                var textLayout = new TextLayout(ctx) {
                    Trimming = TextTrimming.WordElipsis,
                    Text = text, Font = font, Width = w, Height = h,
                };
                var size = textLayout.GetSize();
                w = size.Width < w ? (w - size.Width - 5) / 2d : 0;
                var lh = (font.Size + (font.Size / 2d)) / 2d;
                h = size.Height < h ? (h - size.Height - 5 + lh) / 2d : 0;
                ctx.SetColor(textColor);
                ctx.SetLineWidth(1);
                ctx.DrawTextLayout(textLayout, rect.X + w, rect.Y + h);

            }
        }
    }
}