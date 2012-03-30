using System.Drawing;
using System.Drawing.Drawing2D;
using Limaki.Drawing.Shapes;
using Xwt;
using GDIPen=Limaki.Drawing.GDI.GDIPen;
using Xwt.Gdi;

namespace Limaki.Drawing.GDI.Painters {
    public class BezierPainter:RectanglePainter,IPainter<IBezierShape,Xwt.Rectangle> {
        public override void Render( ISurface surface ) {
            var g = ((GDISurface)surface).Graphics;
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
                System.Drawing.Pen pen = ((GDIPen)Style.Pen).Backend;
                g.DrawPath(
                    pen, 
                    path);
            }
        }

    }
}