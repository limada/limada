using System.Windows.Media;
using System.Windows.Shapes;

namespace Limaki.Drawing.WPF.Shapes {
    public class VectorShape : Limaki.Drawing.Shapes.VectorShape, IWPFShape {

        public VectorShape() : base() { }
        public VectorShape(Vector data) : base(data) { }

        Path _shape = null;
        LineGeometry _geom = null;
        public System.Windows.Shapes.Shape Shape {
            get {
                if (_shape == null) {
                    _shape = new Path();
                    _geom = new LineGeometry();

                    _shape.Data = _geom;
                }
                _geom.StartPoint = new System.Windows.Point (this._data.Start.X, this._data.Start.Y);
                _geom.EndPoint = new System.Windows.Point(this._data.End.X, this._data.End.Y);
                
                return _shape;
            }
        }
    }
}