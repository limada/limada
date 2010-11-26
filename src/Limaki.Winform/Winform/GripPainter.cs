/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System.Collections.Generic;
using Limaki.Drawing;
using Limaki.Drawing.GDI;
using Limaki.Drawing.GDI.Painters;
using Limaki.Drawing.Painters;
using Limaki.Drawing.Shapes;
using Limaki.Common;

namespace Limaki.Winform {
    /// <summary>
    /// Paints the grips of a rectangle
    /// </summary>
    public class GripPainter : Painter<RectangleI> {
            
        public int gripSize = 4;
        public IShape TargetShape;
        public ICamera camera;

        private List<Anchor> _customGrips = null;
        public List<Anchor> CustomGrips {
            get { return _customGrips; }
            set { _customGrips = value; }
        }

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
                    base.Shape = new RectangleShape (new RectangleI(0,0,gripSize,gripSize));
                }
                return base.Shape;
            }
            set {base.Shape = value;}
        }

        private IPainter _innerPainter = null;
        protected IPainter innerPainter {
            get {
                if (_innerPainter == null) {
                    var factory = Registry.Pool.TryGetCreate<IPainterFactory> ();
                    _innerPainter = factory.CreatePainter<RectangleI> ();
                    _innerPainter.Shape = this.Shape;
                }
                return _innerPainter;
            }
            set { _innerPainter = value; }
        }

        public override void Render( ISurface surface ) {
            System.Drawing.Graphics g = ((GDISurface)surface).Graphics;
            Shape.Size = new SizeI(gripSize, gripSize);
            innerPainter.Style = this.Style;
            int halfWidth = gripSize / 2;
            int halfHeight = gripSize / 2;
                
            RectangleI clipBounds = 
                RectangleI.Ceiling(GDIConverter.Convert(g.ClipBounds));
            // get near:
            Camera camera = new Camera(this.camera.Matrice);

            foreach (Anchor anchor in Grips) {
                PointI anchorPoint = TargetShape[anchor];
                anchorPoint = camera.FromSource(anchorPoint);
                Shape.Location = new PointI(anchorPoint.X - halfWidth, anchorPoint.Y - halfHeight);
                if (clipBounds.IntersectsWith(Shape.BoundsRect)) {
                    innerPainter.Render(surface);
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