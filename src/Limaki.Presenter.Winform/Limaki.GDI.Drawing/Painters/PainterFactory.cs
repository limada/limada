/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using Limaki.Drawing.Painters;
using Limaki.Drawing.Shapes;
using Xwt;

namespace Limaki.Drawing.GDI.Painters {
    public class PainterFactory: PainterFactoryBase, IPainterFactory {
        

        protected override void InstrumentClazzes() {
           Add<IPainter<IShape<Rectangle>,Rectangle>>(()=> new RectanglePainter());
           Add<IPainter<IShape<Vector>,Vector>>(()=> new VectorPainter());
           Add<IPainter<IRoundedRectangleShape,Rectangle>>(()=> new RoundedRectanglePainter());
           Add<IPainter<IRectangleShape, Rectangle>>(() => new RectanglePainter());
           Add<IPainter<IBezierShape,Rectangle>>(()=> new BezierPainter());
           Add<IPainter<IVectorShape, Vector>>(() => new VectorPainter());

            //Add<IPainter<RectangleShape>, RectanglePainter>();
           //Add<IPainter<RectangleShape>>(() => new RectanglePainter());
           //Add<IPainter<VectorShape>>(() => new VectorPainter());
           //Add<IPainter<RoundedRectangleShape>>(() => new RoundedRectanglePainter());
           //Add<IPainter<BezierShape>>(() => new BezierPainter());

           Add<IPainter<Rectangle>>(()=> new RectanglePainter());

           Add<IPainter<string>>(()=> new StringPainter());
            
        }
    }
}