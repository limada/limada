using System.Drawing;
using System.Drawing.Drawing2D;
using System;

namespace Limaki.GDI.Painting {

    public class GdiContext:IDisposable {

        public Graphics Graphics;
        public GraphicsState State { get; set; }

        private PointF _current;
        public PointF Current {
            get {
                if (Path.PointCount==0)
                    return _current;
                return Path.GetLastPoint();
            }
            set { _current=value;}
        }

        GraphicsPath _path = null;
        public GraphicsPath Path {
            get { return _path ?? (_path = new GraphicsPath()); }
            set {
                if (_path != value && _path != null)
                    _path.Dispose();
                _path = value;
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
        }

        public Xwt.Drawing.Font Font { get; set; }
    }
}