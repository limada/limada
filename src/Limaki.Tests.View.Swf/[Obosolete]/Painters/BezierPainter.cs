using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Limaki.Drawing.Shapes;
using Xwt;
using Xwt.Gdi;
using Xwt.Gdi.Backend;

namespace Limaki.Drawing.Gdi.Painters {
    [Obsolete]
    public class BezierPainter:RectanglePainter,IPainter<IBezierRectangleShape,Xwt.Rectangle> {

        public override void RenderGdi( ISurface surface ) {
            var g = ((GdiSurface)surface).Graphics;
            var bezierPoints = 
                GDIConverter.Convert((Shape as IBezierShape).BezierPoints);
            var style = this.Style;
            var renderType = this.RenderType;
            var path = new GraphicsPath ();
            path.AddBeziers (bezierPoints);
            if ((RenderType.Fill & renderType) != 0) {
                g.FillPath(GetSolidBrush(
                               GdiConverter.ToGdi(style.FillColor)
                               ), path);
                
            }
            if ((RenderType.Draw & renderType) != 0) {
                System.Drawing.Pen pen = ((GdiPen)Style.Pen).Backend;
                g.DrawPath(
                    pen, 
                    path);
            }
        }

    }
}