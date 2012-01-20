using System.Drawing;
using System.Drawing.Drawing2D;
using GDIPen=Limaki.Drawing.GDI.GDIPen;

namespace Limaki.Drawing.GDI.Painters {
    public class BezierPainter:RectanglePainter,IPainter<IBezierShape,RectangleI> {
        public override void Render( ISurface surface ) {
            Graphics g = ((GDISurface)surface).Graphics;
            PointF[] bezierPoints = 
                GDIConverter.Convert((Shape as IBezierShape).BezierPoints);
            IStyle style = this.Style;
            RenderType renderType = this.RenderType;
            GraphicsPath path = new GraphicsPath ();
            path.AddBeziers (bezierPoints);
            if ((RenderType.Fill & renderType) != 0) {
                g.FillPath(GetSolidBrush(
                               GDIConverter.Convert(style.FillColor)
                               ), path);
                
            }
            if ((RenderType.Draw & renderType) != 0) {
                System.Drawing.Pen pen = ((GDIPen)Style.Pen).Native;
                g.DrawPath(
                    pen, 
                    path);
            }
        }

    }
}