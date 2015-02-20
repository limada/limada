using System.Collections.Generic;
using Limaki.Common;
using System;
using Limaki.Drawing;
using Limaki.Drawing.Painters;
using Xwt;
using Xwt.Drawing;

namespace Limaki.View.Viz.Rendering {

    public class GripPainter : Painter<Rectangle> {
        
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
                    var factory = Registry.Pooled<IShapeFactory>();
                    base.Shape = factory.Shape<Rectangle>(new Point(), new Size(GripSize, GripSize),false);
                }
                return base.Shape;
            }
            set { base.Shape = value; }
        }

        private IPainter _innerPainter = null;
        protected IPainter InnerPainter {
            get {
                if (_innerPainter == null) {
                    var factory = Registry.Pooled<IPainterFactory>();
                    _innerPainter = factory.CreatePainter<Rectangle>();
                    _innerPainter.Shape = this.Shape;
                }
                return _innerPainter;
            }
            set { _innerPainter = value; }
        }

		public override void Render (ISurface surface) {
			Shape.Size = new Size (GripSize, GripSize);
			InnerPainter.Style = this.Style;
			int halfWidth = GripSize / 2;
			int halfHeight = GripSize / 2;

            var camera = new Camera (Camera.Matrix);
		    var destCam = new Camera (surface.Matrix);
			foreach (var anchor in Grips) {
				var anchorPoint = TargetShape[anchor];
				anchorPoint = camera.FromSource(anchorPoint);
			    //anchorPoint = destCam.ToSource (anchorPoint);
				Shape.Location = new Point (anchorPoint.X - halfWidth, anchorPoint.Y - halfHeight);
				InnerPainter.Render (surface);
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