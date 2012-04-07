using System;
using Limaki.Drawing.Shapes;
using Xwt.Drawing;
using Xwt;

namespace Limaki.Drawing.Painters {
    public class VectorPainter : Painter<Vector>, IPainter<IVectorShape, Vector> {

        public override void Render (ISurface surface) {
            var ctx = ((ContextSurface) surface).Context;
            var vector = Shape.Data;

            var width = this.Style.Pen.Thickness;
            var arrowHeigth = width * 1.5d;
            GetCustomLineCap (ctx, vector, width * 5.5d, arrowHeigth);
            ctx.SetColor (Style.PenColor);
            ctx.Fill ();

            var a = (vector.End.X - vector.Start.X);
            var b = (vector.End.Y - vector.Start.Y);
            // todo: calculate end - arrowHeigth
            var end = new Point (vector.End.X, vector.End.Y);
            Render (ctx, (c, d) => {
                ctx.MoveTo (vector.Start);
                ctx.LineTo (end);
                ctx.ClosePath ();

            });
        }

        protected Matrice Matrix = new Matrice ();

        public void GetCustomLineCap (Context ctx, Vector v, double arrowWidth, double arrowHeigth) {
            if (arrowHeigth == 0 || arrowWidth == 0)
                throw new ArgumentException ("ArrowWidth must not be 0");

            var w = arrowWidth;
            var h = arrowHeigth;
            var arrow = new Point[] {
                            new Point(0,0),
                            new Point(-h, -w),
                            new Point(h, -w)
                        };
            Matrix.Reset ();
            Matrix.RotateAt (Vector.Angle (v) - 90d, v.End);
            Matrix.Translate (v.End.X, v.End.Y);
            Matrix.TransformPoints (arrow);
            ctx.MoveTo (arrow[0]);
            ctx.LineTo (arrow[1]);
            ctx.LineTo (arrow[2]);
            ctx.ClosePath ();

        }
    }
}