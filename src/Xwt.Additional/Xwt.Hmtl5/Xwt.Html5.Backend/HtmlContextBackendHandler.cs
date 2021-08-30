// 
// HtmlContextBackendHandler.cs
//  
// Author:
//       Lytico 
// 
// Copyright (c) 2012 Lytico (http://limada.org)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;


using Xwt.Backends;
using System.Globalization;
using Xwt.Drawing;
using System.Linq;

namespace Xwt.Html5.Backend {
    
    public class HtmlContextBackendHandler : ContextBackendHandler {

        public virtual object CreateContext (Widget w) {
            var b = (IHtml5CanvasBackend)w.GetBackend();

            var ctx = new Html5Context();
            if (b.Context != null) {
                ctx.Context = b.Context;
            }
            return ctx;
        }

        public override void Save (object backend) {
            var c = (Html5Context) backend;
            c.Context.CommandLine("save()");
            c.Save();
        }

        public override void Restore (object backend) {
            var c = (Html5Context) backend;
            c.Context.CommandLine ("restore()");
            c.Restore();
        }

        public override void Clip (object backend) {
            var c = (Html5Context) backend;
        }

        public override void ClipPreserve (object backend) {
            var c = (Html5Context) backend;
        }

        public void ResetClip (object backend) {
            var c = (Html5Context) backend;
        }

        public override void NewPath (object backend) {
            var c = (Html5Context) backend;
            c.Context.CommandLine ("beginPath()");
        }

        public override void ClosePath (object backend) {
            var c = (Html5Context) backend;
            c.Context.CommandLine ("closePath()");
        }

        const double degrees = System.Math.PI / 180d;

        public override void Arc (object backend, double xc, double yc, double radius, double angle1, double angle2) {
            var c = (Html5Context) backend;
            if (angle1 > 0 && angle2 == 0)
                angle2 = 360;
            c.Context.CommandLine ("arc ({0},{1},{2},{3},{4},false)", 
                xc.ToHtml(),yc.ToHtml(),
                radius.ToHtml(),
                (angle1 * degrees).ToHtml (),
                (angle2 * degrees).ToHtml ()
                );

        }

        public override void CurveTo (object backend, double x1, double y1, double x2, double y2, double x3, double y3) {
            var c = (Html5Context) backend;
            c.Context.CommandLine ("bezierCurveTo ({0},{1},{2},{3},{4},{5})",
                                   x1.ToHtml(), y1.ToHtml(),
                                   x2.ToHtml(), y2.ToHtml(),
                                   x3.ToHtml(), y3.ToHtml());
            c.Current = new Point (x3, y3);
        }

        public override void LineTo (object backend, double x, double y) {
            var c = (Html5Context) backend;
            c.Context.CommandLine ("lineTo ({0},{1})", x.ToHtml (), y.ToHtml ());
            c.Current = new Point (x, y);
        }

        public override void MoveTo (object backend, double x, double y) {
            var c = (Html5Context) backend;
            c.Context.CommandLine ("moveTo ({0},{1})", x.ToHtml (), y.ToHtml ());
            c.Current = new Point (x, y);
        }

        public override void Rectangle (object backend, double x, double y, double width, double height) {
            var c = (Html5Context) backend;
            c.Context.CommandLine ("rect ({0},{1},{2},{3})", x.ToHtml (), y.ToHtml (), width.ToHtml (), height.ToHtml ());
        }

        public override void RelCurveTo (object backend, double dx1, double dy1, double dx2, double dy2, double dx3, double dy3) {
            var c = (Html5Context) backend;
            c.Context.CommandLine("bezierCurveTo ({0},{1},{2},{3},{4},{5})",
                (c.Current.X + dx1).ToHtml(), (c.Current.Y + dy1).ToHtml(),
                (c.Current.X + dx2).ToHtml(), (c.Current.Y + dy2).ToHtml(),
                (c.Current.X + dx3).ToHtml(), (c.Current.Y + dy3).ToHtml());
            c.Current += new Size (dx3, dy3);

        }

        public override void RelLineTo (object backend, double dx, double dy) {
            var c = (Html5Context) backend;
            c.Current += new Size (dx, dy);
            c.Context.CommandLine("lineTo({0},{1})", c.Current.X.ToHtml(), c.Current.Y.ToHtml());
        }

        public override void RelMoveTo (object backend, double dx, double dy) {
            var c = (Html5Context) backend;
            c.Current += new Size (dx, dy);
            c.Context.CommandLine ("moveTo({0},{1})", c.Current.X.ToHtml (), c.Current.Y.ToHtml ());
        }

        public override void Fill (object backend) {
            FillPreserve(backend);
            NewPath(backend);
        }

        public override void FillPreserve (object backend) {
            var c = (Html5Context) backend;
            c.Context.CommandLine("fillStyle={0}", c.Color.ToStyle());
            c.Context.CommandLine("fill()");
           
        }

        public override void Stroke (object backend) {
            StrokePreserve(backend);
            NewPath(backend);
        }

        public override void StrokePreserve (object backend) {
            var c = (Html5Context)backend;
            c.Context.CommandLine("strokeStyle={0}", c.Color.ToStyle());
            c.Context.CommandLine("stroke()");
        }

        public override void SetColor (object backend, Drawing.Color color) {
            var c = (Html5Context) backend;
            c.Color = color;
        }

        public override void SetLineWidth (object backend, double width) {
            var c = (Html5Context) backend;
            c.LineWidth = width;
            c.Context.CommandLine ("lineWidth = {0}", width.ToHtml ());
        }

        public override void SetLineDash (object backend, double offset, params double [] pattern) {
            var c = (Html5Context)backend;
            if (pattern.Length ==0)
                c.Context.CommandLine ($"setLineDash([])");
            else
                // TODO: set offset, set patters > 2
                c.Context.CommandLine ($"setLineDash([{string.Join (",", pattern.Select (d => d.ToHtml ()))}])");
        }

        public override void SetPattern (object backend, object p) {
            var c = (Html5Context) backend;
        }

        public void SetFont (object backend, Drawing.Font font) {
            var c = (Html5Context) backend;
            c.Font = font;
        }

        public override void DrawTextLayout (object backend, Drawing.TextLayout layout, double x, double y) {
            var c = (Html5Context) backend;
            c.Context.CommandLine ("fillStyle={0}", c.Color.ToStyle ());
            var tl = (HtmlTextLayoutBackend)layout.GetBackend();
            var font = tl.Font;
            c.Context.CommandLine("font=\"{1}pt {0} {2}\"", font.Family, font.Size.ToHtml(), font.Style.ToHtml());

            var text = layout.Text;
            text = text.Replace ("\r", "").Replace ("\n", "");
            // NO: text = System.Net.WebUtility.HtmlEncode(text);
            c.Context.CommandLine ("fillText(\"{0}\",{1},{2})",text,x.ToHtml(),y.ToHtml());
        }

        public void DrawImage (object backend, object img, double x, double y, double alpha) {
            var c = (Html5Context) backend;
        }

        public void DrawImage (object backend, object img, double x, double y, double width, double height, double alpha) {
            var c = (Html5Context) backend;
        }

        public override void ModifyCTM (object backend, Drawing.Matrix transform) {
            var c = (Html5Context)backend;
            c.CTM.Prepend(transform);
        }

        public override Drawing.Matrix GetCTM (object backend) {
            var c = (Html5Context)backend;
            return c.CTM;
        }

        public void ResetTransform (object backend) {
            var c = (Html5Context) backend;
            c.CTM.SetIdentity();
            c.Context.CommandLine ("setTransform(1, 0, 0, 1, 0, 0)");
            //c.Context.CommandLine ("resetTransform()");
        }

        public override void Rotate (object backend, double angle) {
            var c = (Html5Context) backend;
            c.Angle = angle;
            c.Context.CommandLine ("rotate({0})", (angle * degrees).ToHtml ());
        }

        public override void Scale (object backend, double scaleX, double scaleY) {
            var c = (Html5Context) backend;
            c.ScaleX = scaleX;
            c.ScaleY = scaleY;
            c.Context.CommandLine ("scale({0},{1})", scaleX.ToHtml (), scaleY.ToHtml ());
        }

        public override void Translate (object backend, double tx, double ty) {
            var c = (Html5Context) backend;
            c.TranslateX = tx;
            c.TranslateX = ty;
            c.Context.CommandLine ("translate({0}, {1})", tx.ToHtml (), ty.ToHtml ());
            
        }

        public override void SetGlobalAlpha (object backend, double globalAlpha) {
            
        }

        public override void Dispose (object backend) {
          
        }
        public override void DrawImage (object backend, ImageDescription img, double x, double y) {
            throw new NotImplementedException();
        }

        public override void DrawImage (object backend, ImageDescription img, Rectangle srcRect, Rectangle destRect) {
            throw new NotImplementedException();
        }

       

        public override bool IsPointInStroke (object backend, double x, double y) {
            throw new NotImplementedException();
        }

        public override void ArcNegative (object backend, double xc, double yc, double radius, double angle1, double angle2) {
            throw new NotImplementedException();
        }

        public override object CreatePath () {
            throw new NotImplementedException();
        }

        public override object CopyPath (object backend) {
            throw new NotImplementedException();
        }

        public override void AppendPath (object backend, object otherBackend) {
            throw new NotImplementedException();
        }

        public override bool IsPointInFill (object backend, double x, double y) {
            throw new NotImplementedException();
        }

        public override double GetScaleFactor (object backend) {
            throw new NotImplementedException();
        }

        public override void SetStyles(object backend, StyleSet styles)
        {
            throw new NotImplementedException();
        }
    }
}