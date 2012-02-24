using Limaki.Painting;
using SD=System.Drawing;
using Xwt.Backends;
using Xwt.Engine;
using Xwt.Drawing;
using Xwt;

using Xwt.GDIBackend;

namespace Limaki.GDI.Painting {

    class GdiContext {
        public SD.Graphics Graphics;
        public SD.Drawing2D.GraphicsState State { get; set; }

        public SD.Pen Pen { get; set; }

        public SD.PointF Current { get; set; }
        public SD.Drawing2D.GraphicsPath Path { get; set; }
    }

    public class PaintContextBackendHandler:IPaintContextBackendHandler {

        public PaintContextBackendHandler () {}

        public object CreateContext (Widget w) {
            var ctx = new GdiContext ();
            var b = (IGdiGraphicsBackend) WidgetRegistry.GetBackend (w);
            if (b.Graphics!=null) {
                ctx.Graphics = b.Graphics;
            } else {
                ctx.Graphics = Limaki.Drawing.GDI.GDIUtils.CreateGraphics ();
            }
            return ctx;
        }

        public void Save (object backend) {
            var gc = (GdiContext) backend;
            gc.State = gc.Graphics.Save ();
        }

        public void Restore (object backend) {
            var gc = (GdiContext) backend;
            gc.Graphics.Restore (gc.State);
        }

        // http://cairographics.org/documentation/cairomm/reference/classCairo_1_1Context.html

        // mono-libgdiplus\src\graphics-cairo.c

        /// <summary>
        /// Adds a circular arc of the given radius to the current path.
        /// The arc is centered at (xc, yc), 
        /// begins at angle1 and proceeds in the direction of increasing angles to end at angle2. 
        /// If angle2 is less than angle1 
        /// it will be progressively increased by 2*M_PI until it is greater than angle1.
        /// If there is a current point, an initial line segment will be added to the path 
        /// to connect the current point to the beginning of the arc. 
        /// If this initial line is undesired, 
        /// it can be avoided by calling begin_new_sub_path() before calling arc().
        /// </summary>
        /// <param name="backend"></param>
        /// <param name="xc"></param>
        /// <param name="yc"></param>
        /// <param name="radius"></param>
        /// <param name="angle1"></param>
        /// <param name="angle2"></param>
        public void Arc (object backend, double xc, double yc, double radius, double angle1, double angle2) {
            var gc = (GdiContext) backend;
           //?? look in mono-libgdiplus 
            gc.Path.AddArc(
                (float) xc, (float) yc, 
                (float) radius, (float) radius, 
                (float) angle1, (float) angle2);
        }

        public void Clip (object backend) {
            var gc = (GdiContext) backend;
            gc.Graphics.DrawPath(gc.Pen, gc.Path);
            gc.Path.Dispose ();
            gc.Path = new SD.Drawing2D.GraphicsPath();
        }

        public void ClipPreserve (object backend) {
            throw new System.NotImplementedException ();
        }

        public void ResetClip (object backend) {
            throw new System.NotImplementedException ();
        }

        public void ClosePath (object backend) {
            var gc = (GdiContext) backend;
            gc.Path.CloseFigure();
        }

        public void CurveTo (object backend, double x1, double y1, double x2, double y2, double x3, double y3) {
            var gc = (GdiContext) backend;

            gc.Path.AddBezier(gc.Path.GetLastPoint(),
                new SD.PointF ((float) x1, (float) y1),
                new SD.PointF ((float) x2, (float) y2),
                new SD.PointF((float) x3, (float) y3));

        }

        public void Fill (object backend) {
            throw new System.NotImplementedException ();
        }

        public void FillPreserve (object backend) {
            throw new System.NotImplementedException ();
        }

        public void LineTo (object backend, double x, double y) {
            var gc = (GdiContext) backend;

            gc.Path.AddLine (gc.Path.GetLastPoint (), new SD.PointF ((float) x, (float) y));
        }

        /// <summary>
        /// If the current subpath is not empty, begin a new subpath.
        /// After this call the current point will be (x, y).
        /// </summary>
        /// <param name="backend"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveTo (object backend, double x, double y) {
            var gc = (GdiContext) backend;
            var pt = new SD.PointF ((float) x, (float) y);
            //???
            gc.Path.AddLine(pt, pt);
        }

        public void NewPath (object backend) {
            throw new System.NotImplementedException ();
        }

        public void Rectangle (object backend, double x, double y, double width, double height) {
            throw new System.NotImplementedException ();
        }

        public void RelCurveTo (object backend, double dx1, double dy1, double dx2, double dy2, double dx3, double dy3) {
            throw new System.NotImplementedException ();
        }

        public void RelLineTo (object backend, double dx, double dy) {
            throw new System.NotImplementedException ();
        }

        public void RelMoveTo (object backend, double dx, double dy) {
            throw new System.NotImplementedException ();
        }

        public void Stroke (object backend) {
            throw new System.NotImplementedException ();
        }

        public void StrokePreserve (object backend) {
            throw new System.NotImplementedException ();
        }

        public void SetColor (object backend, Xwt.Drawing.Color color) {
            throw new System.NotImplementedException ();
        }

        public void SetLineWidth (object backend, double width) {
            throw new System.NotImplementedException ();
        }

        public void SetLineDash (object backend, double offset, params double[] pattern) {
            throw new System.NotImplementedException ();
        }

        public void SetPattern (object backend, object p) {
            throw new System.NotImplementedException ();
        }

        public void SetFont (object backend, Xwt.Drawing.Font font) {
            throw new System.NotImplementedException ();
        }

        public void DrawTextLayout (object backend, Xwt.Drawing.TextLayout layout, double x, double y) {
            throw new System.NotImplementedException ();
        }

        public void DrawImage (object backend, object img, double x, double y, double alpha) {
            throw new System.NotImplementedException ();
        }

        public void DrawImage (object backend, object img, double x, double y, double width, double height, double alpha) {
            throw new System.NotImplementedException ();
        }

        public void Rotate (object backend, double angle) {
            throw new System.NotImplementedException ();
        }

        public void Translate (object backend, double tx, double ty) {
            throw new System.NotImplementedException ();
        }

        public void Dispose (object backend) {
            throw new System.NotImplementedException ();
        }
    }
}