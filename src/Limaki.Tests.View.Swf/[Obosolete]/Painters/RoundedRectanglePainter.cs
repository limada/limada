using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Limaki.Drawing.Gdi;
using Limaki.Drawing.Gdi.Painters;
using Limaki.Drawing.Shapes;
using Xwt;
using Xwt.Gdi;
using Xwt.Gdi.Backend;

namespace Limaki.Drawing.Gdi.Painters {
    [Obsolete]
    public class RoundedRectanglePainter:RectanglePainter,IPainter<IRoundedRectangleShape,Xwt.Rectangle> {
        
        public override void Render (ISurface surface) {
            var g = ( (GdiSurface) surface ).Graphics;
            var rect = Shape.Data.ToGdi ();
            var style = this.Style;
            var renderType = this.RenderType;
            var path = new GraphicsPath ();
            SetRoundedRect (path, rect, 10f);
            if ((RenderType.Fill & renderType) != 0) {
                g.FillPath(GetSolidBrush(
                               GdiConverter.ToGdi(style.FillColor)
                               ), path);
                
            }
            if ((RenderType.Draw & renderType) != 0) {
                //if (Style.Pen.Alignment == System.Drawing.Drawing2D.PenAlignment.Center) {
                //    int penSize = -(int)Style.Pen.Width/2;
                //    Rectangle rect = Rectangle.Inflate(Shape.Data, penSize, penSize);
                //}
                var pen = ((GdiPen)Style.Pen).Backend;
                g.DrawPath(pen, path);
            }
        }

        private void SetRoundedRect(GraphicsPath mPath, RectangleF baseRect, float radius) {
            // if corner radius is less than or equal to zero, 
            // return the original Rectangle 
            if (radius <= 0.0F) {
                mPath.AddRectangle(baseRect);
                return;
            }

            // if the corner radius is greater than or equal to 
            // half the width, or height (whichever is shorter) 
            // then return a capsule instead of a lozenge 
            if (radius >= (Math.Min(baseRect.Width, baseRect.Height)) / 2.0) {
                GetCapsule(mPath, baseRect);
                return;
            }

            // create the arc for the Rectangle sides and declare 
            // a graphics path object for the drawing 
            var diameter = (radius * 2.0F);
            var size = new SizeF(diameter, diameter);
            var arc = new RectangleF(baseRect.Location, size);

            // top left arc 
            mPath.AddArc(arc, 180f, 90f);

            // top right arc 
            arc.X = baseRect.Right - diameter;
            mPath.AddArc(arc, 270f, 90f);

            // bottom right arc 
            arc.Y = baseRect.Bottom - diameter;
            mPath.AddArc(arc, 0f, 90f);

            // bottom left arc
            arc.X = baseRect.Left;
            mPath.AddArc(arc, 90f, 90f);
            mPath.CloseFigure ();

        }

        private void GetCapsule(GraphicsPath mPath, RectangleF baseRect) {
            float diameter;
            RectangleF arc;
            try {
                if (baseRect.Width > baseRect.Height) {
                    // return horizontal capsule 
                    diameter = baseRect.Height;
                    if (diameter == 0)
                        return;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    mPath.AddArc(arc, 90, 180);
                    arc.X = baseRect.Right - diameter;
                    mPath.AddArc(arc, 270, 180);
                } else if (baseRect.Width < baseRect.Height) {
                    // return vertical capsule 
                    diameter = baseRect.Width;
                    if (diameter == 0)
                        return;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    mPath.AddArc(arc, 180, 180);
                    arc.Y = baseRect.Bottom - diameter;
                    mPath.AddArc(arc, 0, 180);
                } else {
                    // return circle 
                    mPath.AddEllipse(baseRect);
                }
            } catch  {
                mPath.AddEllipse(baseRect);
            } finally {
                mPath.CloseFigure ();
            }
        }
        
        
    }
}