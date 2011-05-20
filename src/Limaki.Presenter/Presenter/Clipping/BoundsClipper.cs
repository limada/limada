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
using Limaki.Drawing;
using Limaki.Drawing.Shapes;

namespace Limaki.Presenter {
    public class BoundsClipper : IClipper {
        public BoundsClipper(RectangleI bounds) {
            this._bounds = bounds;
        }

        public IEnumerable<PointI> Hull {
            get { return RectangleShape.Hull(_bounds,0,false); }
        }

        protected RectangleI _bounds = RectangleI.Empty;
        public RectangleI Bounds {
            get { return _bounds; }
        }

        public bool RenderAll {get;set;}


        public void Add(IEnumerable<PointI> hull) {
            int l = _bounds.X;
            int t = _bounds.Y;
            int b = _bounds.Bottom;
            int r = _bounds.Right;
            foreach (var env in hull) {
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
            this._bounds = Drawing.RectangleI.FromLTRB(l, t, r, b);
        }

        public bool IsEmpty {
            get { return _bounds.IsEmpty; }
        }

        public void Clear() {
            _bounds = RectangleI.Empty;
            RenderAll = false;
        }

        
    }
}