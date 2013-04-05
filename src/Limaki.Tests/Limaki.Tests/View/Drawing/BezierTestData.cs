using Limaki.Drawing.Shapes;
using Xwt;
using Xwt.Drawing;

namespace Limaki.Tests.Drawing {
    public class BezierTestData {
        Matrix _matrix = null;
        public Matrix Matrix {
            get { return _matrix ?? (_matrix = new Matrix()); }
            set { _matrix = value; }
        }

        public Point[] RoundedRectBezier {
            get {
                var r = BezierExtensions.GetRoundedRectBezier(new Rectangle(0, 0, 400, 100), 5d);
                Matrix.Transform(r);
                return r;
            }
        }

        public Point[] Curve1 {
            get {
                var curve = new Point[] {
                                            new Point(0, 300),
                                            new Point(200, 0), new Point(500, 0), new Point(600, 250),
                                            new Point(200, 300), new Point(500, 300), new Point(600, 50),
                                        };
                Matrix.Transform(curve);
                return curve;
            }
        }
    }
}