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
 * http://www.limada.org
 * 
 */


using System.Collections.Generic;
using Limaki.Drawing;
using Xwt;
using Limaki.Drawing.Indexing;

namespace Limaki.View.Visuals {
    public class VisualsQuadTreeIndex: SpatialQuadTreeIndex<IVisual>,ISpatialZIndex<IVisual> {
        public VisualsQuadTreeIndex () {
            BoundsOf = visual => visual.Shape.BoundsRect;
            HasBounds = visual => visual.Shape != null;
        }


        public virtual IEnumerable<IVisual> Query( Rectangle clipBounds, ZOrder zOrder ) {
            IEnumerable<IVisual> search = GeoIndex.Query(clipBounds);

            if (zOrder==ZOrder.EdgesFirst) {
                foreach (var visual in search) {
                    if (visual is IVisualEdge) {
                        if (DrawingExtensions.Intersects(clipBounds, visual.Shape.BoundsRect))
                            yield return visual;
                    }
                }
            }

            foreach (var visual in search) {
                if (!(visual is IVisualEdge)) {
                    if (DrawingExtensions.Intersects(clipBounds, visual.Shape.BoundsRect))
                        yield return visual;
                }
            }

            if (zOrder==ZOrder.NodesFirst) {
                foreach (var visual in search) {
                    if (visual is IVisualEdge) {
                        if (DrawingExtensions.Intersects(clipBounds, visual.Shape.BoundsRect))
                            yield return visual;
                    }
                }
            }
        }

        public override IEnumerable<IVisual> Query(Rectangle clipBounds) {
            return Query (clipBounds, ZOrder.NodesFirst);
        }

        public override IEnumerable<IVisual> Query() {
            var search = GeoIndex.QueryAll ();

            foreach (var visual in search) {
                if (!(visual is IVisualEdge)) {
                        yield return visual;
                }
            }

            foreach (var visual in search) {
                if (visual is IVisualEdge) {
                        yield return visual;
                }
            }
        }

        

     

       
    }
}