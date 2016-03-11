using System;
using Xwt.Backends;
using Xwt.Drawing;

namespace Xwt.Headless.Backend {
    
    public class HeadlessContext { }

    public class HeadlessContextBackendHandler : ContextBackendHandler {

        public override void Save (object backend) {
            
        }

        public override void Restore (object backend) {
           
        }

        public override void Clip (object backend) {
          
        }

        public override void ClipPreserve (object backend) {
            
        }

        public override void Fill (object backend) {
           
        }

        public override void FillPreserve (object backend) {
          
        }

        public override void NewPath (object backend) {
            
        }

        public override void Stroke (object backend) {
            
        }

        public override void StrokePreserve (object backend) {
            
        }

        public override void SetColor (object backend, Color color) {
            
        }

        public override void SetLineWidth (object backend, double width) {
            
        }

        public override void SetLineDash (object backend, double offset, params double[] pattern) {
           
        }

        public override void SetPattern (object backend, object p) {
            
        }

        public override void DrawTextLayout (object backend, TextLayout layout, double x, double y) {
           
        }

        public override void DrawImage (object backend, ImageDescription img, double x, double y) {
        
        }

        public override void DrawImage (object backend, ImageDescription img, Rectangle srcRect, Rectangle destRect) {
            
        }

        Matrix Matrix = new Matrix ();

        public override void Rotate (object backend, double angle) {
            Matrix.Rotate (angle);
        }

        public override void Scale (object backend, double scaleX, double scaleY) {
            Matrix.Scale(scaleX,scaleY);
        }

        public override void Translate (object backend, double tx, double ty) {
            Matrix.Translate (tx, ty);
        }

        public override void ModifyCTM (object backend, Matrix transform) {
            Matrix.Prepend (transform);
        }

        public override Matrix GetCTM (object backend) {
            return Matrix;
        }

        public override bool IsPointInStroke (object backend, double x, double y) {
            throw new NotImplementedException ();
        }

        public override void SetGlobalAlpha (object backend, double globalAlpha) {
         
        }

        public override double GetScaleFactor (object backend) {
            return 1;
        }

        public override void Arc (object backend, double xc, double yc, double radius, double angle1, double angle2) {
           
        }

        public override void ArcNegative (object backend, double xc, double yc, double radius, double angle1, double angle2) {
            
        }

        public override void ClosePath (object backend) {
           
        }

        public override void CurveTo (object backend, double x1, double y1, double x2, double y2, double x3, double y3) {
           
        }

        public override void LineTo (object backend, double x, double y) {
           
        }

        public override void MoveTo (object backend, double x, double y) {
          
        }

        public override void Rectangle (object backend, double x, double y, double width, double height) {
           
        }

        public override void RelCurveTo (object backend, double dx1, double dy1, double dx2, double dy2, double dx3, double dy3) {
            
        }

        public override void RelLineTo (object backend, double dx, double dy) {
        
        }

        public override void RelMoveTo (object backend, double dx, double dy) {
           
        }

        public class Path { }

        public override object CreatePath () { return new Path (); }

        public override object CopyPath (object backend) { return new Path (); }

        public override void AppendPath (object backend, object otherBackend) {
          
        }

        public override bool IsPointInFill (object backend, double x, double y) {
            return false;
        }

        public override void SetStyles(object backend, StyleSet styles)
        {
        }
    }
}