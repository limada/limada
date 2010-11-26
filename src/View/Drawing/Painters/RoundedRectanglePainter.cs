using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Limaki.Drawing.Painters {
    public class RoundedRectanglePainter:RectanglePainter {
        public override void Render(Graphics g) {
            Rectangle rect = Shape.Data;
            IStyle style = this.Style;
            RenderType renderType = this.RenderType;
            GraphicsPath path = new GraphicsPath ();
            SetRoundedRect (path, rect, 10f);
            if ((RenderType.Fill & renderType) != 0) {
                g.FillPath(GetSolidBrush(style.FillColor), path);
                
            }
            if ((RenderType.Draw & renderType) != 0) {
                //if (Style.Pen.Alignment == System.Drawing.Drawing2D.PenAlignment.Center) {
                //    int penSize = -(int)Style.Pen.Width/2;
                //    Rectangle rect = Rectangle.Inflate(Shape.Data, penSize, penSize);
                //}
                g.DrawPath(style.Pen, path);
            }
        }

        private void SetRoundedRect(GraphicsPath mPath, Rectangle baseRect, float radius) {
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
            int diameter = (int)(radius * 2.0F);
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(baseRect.Location, size);

            // top left arc 
            mPath.AddArc(arc, 180, 90);

            // top right arc 
            arc.X = baseRect.Right - diameter;
            mPath.AddArc(arc, 270, 90);

            // bottom right arc 
            arc.Y = baseRect.Bottom - diameter;
            mPath.AddArc(arc, 0, 90);

            // bottom left arc
            arc.X = baseRect.Left;
            mPath.AddArc(arc, 90, 90);
            mPath.CloseFigure ();

        }

        private void GetCapsule(GraphicsPath mPath, RectangleF baseRect) {
            float diameter;
            RectangleF arc;
            try {
                if (baseRect.Width > baseRect.Height) {
                    // return horizontal capsule 
                    diameter = baseRect.Height;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    mPath.AddArc(arc, 90, 180);
                    arc.X = baseRect.Right - diameter;
                    mPath.AddArc(arc, 270, 180);
                } else if (baseRect.Width < baseRect.Height) {
                    // return vertical capsule 
                    diameter = baseRect.Width;
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