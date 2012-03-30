// 
// ContextBackendHandler.cs
//  
// Author:
//       Lytico 
// 
// Copyright (c) 2012 Lytico (http://limada.sourceforge.net)
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
using Xwt.Engine;


namespace Xwt.Html5.Backend {
    
    public class ContextBackendHandler : IContextBackendHandler {

        public virtual object CreateContext (Widget w) {
            var b = (IHtml5CanvasBackend) WidgetRegistry.GetBackend (w);

            var ctx = new Html5Context ();
            if (b.Context != null) {
                ctx.Context = b.Context;
            } 
            return ctx;
        }

        public void Save (object backend) {
            var c = (Html5Context) backend;
            c.Context.CommandLine("save()");
            c.Save();
        }

        public void Restore (object backend) {
            var c = (Html5Context) backend;
            c.Context.CommandLine ("restore()");
            c.Restore();
        }

        public void Arc (object backend, double xc, double yc, double radius, double angle1, double angle2) {
            var c = (Html5Context) backend;
        }

        public void Clip (object backend) {
            var c = (Html5Context) backend;
        }

        public void ClipPreserve (object backend) {
            var c = (Html5Context) backend;
        }

        public void ResetClip (object backend) {
            var c = (Html5Context) backend;
        }

        public void ClosePath (object backend) {
            var c = (Html5Context) backend;
        }

        public void CurveTo (object backend, double x1, double y1, double x2, double y2, double x3, double y3) {
            var c = (Html5Context) backend;
        }

        public void Fill (object backend) {
            var c = (Html5Context) backend;
        }

        public void FillPreserve (object backend) {
            var c = (Html5Context) backend;
        }

        public void LineTo (object backend, double x, double y) {
            var c = (Html5Context) backend;
        }

        public void MoveTo (object backend, double x, double y) {
            var c = (Html5Context) backend;
        }

        public void NewPath (object backend) {
            var c = (Html5Context) backend;
        }

        public void Rectangle (object backend, double x, double y, double width, double height) {
            var c = (Html5Context) backend;
        }

        public void RelCurveTo (object backend, double dx1, double dy1, double dx2, double dy2, double dx3, double dy3) {
            var c = (Html5Context) backend;
        }

        public void RelLineTo (object backend, double dx, double dy) {
            var c = (Html5Context) backend;
        }

        public void RelMoveTo (object backend, double dx, double dy) {
            var c = (Html5Context) backend;
        }

        public void Stroke (object backend) {
            var c = (Html5Context) backend;
        }

        public void StrokePreserve (object backend) {
            var c = (Html5Context) backend;
        }

        public void SetColor (object backend, Drawing.Color color) {
            var c = (Html5Context) backend;
        }

        public void SetLineWidth (object backend, double width) {
            var c = (Html5Context) backend;
        }

        public void SetLineDash (object backend, double offset, params double[] pattern) {
            var c = (Html5Context) backend;
        }

        public void SetPattern (object backend, object p) {
            var c = (Html5Context) backend;
        }

        public void SetFont (object backend, Drawing.Font font) {
            var c = (Html5Context) backend;
        }

        public void DrawTextLayout (object backend, Drawing.TextLayout layout, double x, double y) {
            var c = (Html5Context) backend;
        }

        public void DrawImage (object backend, object img, double x, double y, double alpha) {
            var c = (Html5Context) backend;
        }

        public void DrawImage (object backend, object img, double x, double y, double width, double height, double alpha) {
            var c = (Html5Context) backend;
        }

        public void ResetTransform (object backend) {
            var c = (Html5Context) backend;
            c.Angle = 0;
            c.ScaleX = 1;
            c.ScaleY = 1;
            c.TranslateX = 1;
            c.TranslateX = 1;
        }

        public void Rotate (object backend, double angle) {
            var c = (Html5Context) backend;
            c.Angle = angle;
        }

        public void Scale (object backend, double scaleX, double scaleY) {
            var c = (Html5Context) backend;
            c.ScaleX = scaleX;
            c.ScaleY = scaleY;
        }

        public void Translate (object backend, double tx, double ty) {
            var c = (Html5Context) backend;
            c.TranslateX = tx;
            c.TranslateX = ty;
        }

        public void SetGlobalAlpha (object backend, double globalAlpha) {
            
        }

        public void Dispose (object backend) {
          
        }

        
    }
}