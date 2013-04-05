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

        
    }
}