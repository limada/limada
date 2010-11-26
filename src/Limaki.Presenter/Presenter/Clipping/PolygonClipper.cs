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
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;

namespace Limaki.Presenter {
    public class PolygonClipper:IClipper {

        Set<PointI> clipPolygon = new Set<PointI>();

        public IEnumerable<PointI> Hull {
            get { return new GrahamConvexHull ().FindHull (clipPolygon);}
        }

        public RectangleI Bounds {
            get {
                var resultI = clipPolygon;
                if (resultI.Count > 0) {
                    int l = int.MaxValue;
                    int t = int.MaxValue;
                    int b = int.MinValue;
                    int r = int.MinValue;
                    foreach (var env in resultI) {
                        var envX = env.X;
                        var envY = env.Y;
                        if (envX < l)
                            l = envX;
                        if (envY < t)
                            t = envY;
                        if (envX > r)
                            r = envX;
                        if (envY > b)
                            b = envY;
                    }
                    return Drawing.RectangleI.FromLTRB (l, t, r, b);
                } else {
                    return RectangleI.Empty;
                }
            }
        }
        
        public virtual bool RenderAll { get; set; }

        public void Add(IEnumerable<PointI> hull) {
            clipPolygon.AddRange (hull);
        }

        public bool IsEmpty {
            get { return clipPolygon.Count==0; }
        }
        public void Clear() {
            clipPolygon.Clear ();
            RenderAll = false;
        }

        
    }
}