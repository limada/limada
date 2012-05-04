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
using Limaki.Drawing.Shapes;
using Xwt;

namespace Limaki.View.Clipping {
    public class BoundsClipper : IClipper {
        public BoundsClipper(Rectangle bounds) {
            this._bounds = bounds;
        }

        public IEnumerable<Point> Hull {
            get { return RectangleShape.Hull(_bounds,0,false); }
        }

        protected Rectangle _bounds = Rectangle.Zero;
        public Rectangle Bounds {
            get { return _bounds; }
        }

        public bool RenderAll {get;set;}


        public void Add(IEnumerable<Point> hull) {
            var l = _bounds.X;
            var t = _bounds.Y;
            var b = _bounds.Bottom;
            var r = _bounds.Right;
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
            this._bounds = Rectangle.FromLTRB(l, t, r, b);
        }

        public bool IsEmpty {
            get { return _bounds.IsEmpty; }
        }

        public void Clear() {
            _bounds = Rectangle.Zero;
            RenderAll = false;
        }

        
    }
}