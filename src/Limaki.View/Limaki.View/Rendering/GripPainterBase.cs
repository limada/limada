using System.Collections.Generic;
using Limaki.Common;
using System;
using Limaki.Drawing;
using Limaki.Drawing.Painters;
using Xwt;

namespace Limaki.View.Rendering {

    public abstract class GripPainterBase : Painter<Rectangle> {
        
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

        public override IShape<Rectangle> Shape {
            get {
                if (base.Shape == null) {
                    var factory = Registry.Pool.TryGetCreate<IShapeFactory>();
                    base.Shape = factory.Shape<Rectangle>(new Point(), new Size(GripSize, GripSize));
                }
                return base.Shape;
            }
            set { base.Shape = value; }
        }

        private IPainter _innerPainter = null;
        protected IPainter InnerPainter {
            get {
                if (_innerPainter == null) {
                    var factory = Registry.Pool.TryGetCreate<IPainterFactory>();
                    _innerPainter = factory.CreatePainter<Rectangle>();
                    _innerPainter.Shape = this.Shape;
                }
                return _innerPainter;
            }
            set { _innerPainter = value; }
        }

        public Action<IShape> UpdateGrip { get; set; }
        public virtual void UpdateGrips() {
            if (UpdateGrip != null) {
                Shape.Size = new Size(GripSize, GripSize);
                InnerPainter.Style = this.Style;
                int halfWidth = GripSize / 2;
                int halfHeight = GripSize / 2;
                Camera camera = new Camera(this.Camera.Matrice);

                foreach (Anchor anchor in Grips) {
                    Point anchorPoint = TargetShape[anchor];
                    anchorPoint = camera.FromSource(anchorPoint);
                    Shape.Location = new Point(anchorPoint.X - halfWidth, anchorPoint.Y - halfHeight);
                    UpdateGrip(Shape);
                }
            }
        }
        public override void Dispose(bool disposing) {
            if (disposing) {
                InnerPainter.Dispose();
                InnerPainter = null;
            }
            base.Dispose(disposing);
        }
    }
}