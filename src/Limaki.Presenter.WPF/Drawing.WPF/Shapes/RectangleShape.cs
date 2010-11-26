using System.Windows.Media;
using System.Windows.Shapes;

namespace Limaki.Drawing.WPF.Shapes {
    public abstract class RectangleShapeBase:
        Limaki.Drawing.Shapes.RectangleShapeBase, 
        IWPFShape,
        IBezierShape {

        public RectangleShapeBase(): base() {}
        public RectangleShapeBase(RectangleI data) : base(data) { }

        protected Path _path = null;
        protected RectangleGeometry _geom = null;
        public virtual System.Windows.Shapes.Shape Shape {
            get {
                if (_path == null) {
                    _path = new Path();
                    _geom = new RectangleGeometry();
                    
                    _path.Data = _geom;
                }
                _geom.Rect = new System.Windows.Rect(
                    this.Data.Left,
                    this.Data.Top,
                    this.Data.Width,
                    this.Data.Height);


                return _path;
            }
        }
        
        PointS[] IBezierShape.BezierPoints {
            get { throw new System.NotImplementedException(); }
        }

    }

    public class RectangleShape : RectangleShapeBase, IRectangleShape {
        public RectangleShape() : base() { }
        public RectangleShape(RectangleI data) : base(data) { }

        public override object Clone() {
            return new RectangleShape(this.Data);
        }
    }

    public class RoundedRectangleShape:RectangleShape, IRoundedRectangleShape {
        public RoundedRectangleShape() : base() { }
        public RoundedRectangleShape(RectangleI data) : base(data) { }

        public override Shape Shape {
            get {
                Shape result = base.Shape;
                _geom.RadiusX = 10;
                _geom.RadiusY = 10;
                return base.Shape;
            }
        }

        public override object Clone() {
            return new RoundedRectangleShape(this.Data);
        }
    }
}