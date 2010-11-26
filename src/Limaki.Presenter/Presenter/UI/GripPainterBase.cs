using System.Collections.Generic;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Painters;
using Limaki.Drawing.Shapes;
using System;

namespace Limaki.Presenter.UI {
    public abstract class GripPainterBase : Painter<RectangleI> {
        
        public int GripSize {get;set;}
        public IShape TargetShape { get; set; }
        public ICamera Camera { get; set; }
        public List<Anchor> CustomGrips { get; set; }

        public virtual IEnumerable<Anchor> Grips {
            get {
                if (CustomGrips == null) {
                    return TargetShape.Grips;
                } else {
                    return CustomGrips;
                }
            }
        }

        public override IShape<RectangleI> Shape {
            get {
                if (base.Shape == null) {
                    var factory = Registry.Pool.TryGetCreate<IShapeFactory>();
                    base.Shape = factory.Shape<RectangleI>(new PointI(), new SizeI(GripSize, GripSize));
                }
                return base.Shape;
            }
            set { base.Shape = value; }
        }

        private IPainter _innerPainter = null;
        protected IPainter innerPainter {
            get {
                if (_innerPainter == null) {
                    var factory = Registry.Pool.TryGetCreate<IPainterFactory>();
                    _innerPainter = factory.CreatePainter<RectangleI>();
                    _innerPainter.Shape = this.Shape;
                }
                return _innerPainter;
            }
            set { _innerPainter = value; }
        }

        public Action<IShape> UpdateGrip { get; set; }
        public virtual void UpdateGrips() {
            if (UpdateGrip != null) {
                Shape.Size = new SizeI(GripSize, GripSize);
                innerPainter.Style = this.Style;
                int halfWidth = GripSize / 2;
                int halfHeight = GripSize / 2;
                Camera camera = new Camera(this.Camera.Matrice);

                foreach (Anchor anchor in Grips) {
                    PointI anchorPoint = TargetShape[anchor];
                    anchorPoint = camera.FromSource(anchorPoint);
                    Shape.Location = new PointI(anchorPoint.X - halfWidth, anchorPoint.Y - halfHeight);
                    UpdateGrip(Shape);
                }
            }
        }
        public override void Dispose(bool disposing) {
            if (disposing) {
                innerPainter.Dispose();
                innerPainter = null;
            }
            base.Dispose(disposing);
        }
    }
}