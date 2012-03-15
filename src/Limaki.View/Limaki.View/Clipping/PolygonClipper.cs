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


using System.Collections.Generic;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Xwt;

namespace Limaki.View.Clipping {
    public class PolygonClipper:IClipper {

        Set<Point> clipPolygon = new Set<Point>();

        public IEnumerable<Point> Hull {
            get { return new GrahamConvexHull ().FindHull (clipPolygon);}
        }

        public Rectangle Bounds {
            get {
                var resultI = clipPolygon;
                if (resultI.Count > 0) {
                    var l = double.MaxValue;
                    var t = double.MaxValue;
                    var b = double.MinValue;
                    var r = double.MinValue;
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
                    return Rectangle.FromLTRB (l, t, r, b);
                } else {
                    return Rectangle.Zero;
                }
            }
        }
        
        public virtual bool RenderAll { get; set; }

        public void Add(IEnumerable<Point> hull) {
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