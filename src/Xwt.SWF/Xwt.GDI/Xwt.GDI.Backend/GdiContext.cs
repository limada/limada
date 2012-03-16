using System.Drawing;
using System.Drawing.Drawing2D;
using System;

namespace Xwt.Gdi.Backend {
    public class GdiContext:IDisposable {

        public Graphics Graphics;
        public GraphicsState State { get; set; }

        private PointF _current;
        public PointF Current {
            get {
                if (Path.PointCount == 0)
                    return _current;
                return Path.GetLastPoint ();
            }
            set { _current=value;}
        }

        GraphicsPath _path = null;
        public GraphicsPath Path {
            get {
                if (_path == null) {
                    _path = new GraphicsPath ();
                    if (_transformPath)
                        _transformed = false;
                }
                return _path;
            }
            set {
                if (_path != value && _path != null)
                    _path.Dispose ();
                if (_transformPath)
                    _transformed = false;
                _path = value;
            }
        }

        Matrix _matrix = null;
        public Matrix Matrix {
            get {

                if (_matrix == null) {
                    _matrix = new Matrix ();
                }
                return _matrix;

            }
            set {
                if (_matrix != value && _matrix != null)
                    _matrix.Dispose ();
                _matrix = value;
                if (_transformPath)
                    _path.Transform (Matrix);
            }
        }
        public Color Color { get; set; }
        public double LineWidth { get; set; }
        public double[] LineDash { get; set; }
        public object Pattern { get; set; }

        SolidBrush _brush = null;
        public Brush Brush {
            get {
                if (_brush == null)
                    _brush = new SolidBrush(this.Color);
                else
                    _brush.Color = this.Color;
                return _brush;
            }
            set { _brush = value as SolidBrush; }
        }

        Pen _pen = null;
        public Pen Pen {
            get {
                if (_pen == null)
                    _pen = new Pen(Color);
                else
                    _pen.Color = Color;

                _pen.Width = (float) LineWidth;
                return _pen;
            }
            set { _pen = value; }
        }

        public void Dispose() {
            if (_path != null)
                Path.Dispose();
            if (_pen != null)
                _pen.Dispose();
            if (_brush != null)
                _brush.Dispose();
            if (_matrix != null)
                _matrix.Dispose();
        }

        public Xwt.Drawing.Font Font { get; set; }

        public bool HasTransform {
            get { return _matrix != null; }
        }

        private bool _transformed = false;
        bool _transformPath = true; //false: very slow, but normal text is transformed too
        public void Transform () {
            if (!_transformed && _path != null) {
                if (_transformPath)
                    // this transforms path.points immedeatly:
                    Path.Transform (Matrix);
                else
                    Graphics.Transform = Matrix;
                _transformed = true;
            }
        }
        public void Rotate (float p) {
            Matrix.Rotate (p,MatrixOrder.Append);
        }

        public  void ResetTransform () {
            if (_matrix != null) {
                Matrix.Reset ();

                if (_path != null && _transformPath) {
                    //Path.Transform (Matrix); //has no effect as all points are already transformed
                    //Graphics.ResetTransform();
                    
                } else
                    Graphics.Transform = Matrix;
               
            }
            _transformed = false;
        }

        public void Translate (float x, float y) {
            //Matrix.Translate (x,y);
            Graphics.TranslateTransform (x, y);
           
        }

        public void TranslatePath (float x, float y) {
            Matrix.Translate (x, y);
        }
    }
}