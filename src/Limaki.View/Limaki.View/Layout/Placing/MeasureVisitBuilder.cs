/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2012 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Drawing;
using Limaki.Graphs;
using System;
using Xwt;

namespace Limaki.View.Layout {

    public class MeasureVisitBuilder<TItem> {

        public MeasureVisitBuilder(ILocator<TItem> locator) {
            this.Locator = locator;
        }
        
        public virtual ILocator<TItem> Locator { get; set; }

        public Func<Rectangle> Bounds(ref Action<TItem> visit) {
            var l = double.MaxValue;
            var t = double.MaxValue;
            var b = double.MinValue;
            var r = double.MinValue;

            visit += item => {
                if (Locator.HasLocation(item)) {
                    var loc = Locator.GetLocation(item);
                    var il = loc.X;
                    var it = loc.Y;

                    var size = Locator.GetSize(item);
                    var ir = il + size.Width;
                    var ib = it + size.Height;
                    if (il < l)
                        l = il;
                    if (it < t)
                        t = it;
                    if (ir > r)
                        r = ir;
                    if (ib > b)
                        b = ib;
                }
              };

            return () => Rectangle.FromLTRB(l, t, r, b);
        }

        public Func<Size> SizeSum(ref Action<TItem> visit) {
            var w = 0d;
            var h = 0d;
            visit += item => {
                var size = Locator.GetSize(item);
               
                w += size.Width;
                h += size.Height;
            };

            return () => new Size(w,h);
        }

        public Func<Size> SizeSum(ref Action<TItem> visit, Size distance) {
            var w = 0d;
            var h = 0d;
            visit += item => {
                var size = Locator.GetSize(item);

                w += size.Width + distance.Width;
                h += size.Height + distance.Height;
            };

            w -= distance.Width;
            h -= distance.Height;

            return () => new Size(w, h);
        }

        public Func<Size> SizeToFit(ref Action<TItem> visit, Size distance,  Dimension dimension) {
            var w = 0d;
            var h = 0d;
            
            visit += item => {
                var size = Locator.GetSize(item);
                //WHY???
                if (dimension == Dimension.X) {
                    h += size.Height + distance.Height;
                    if (w < size.Width)
                        w = size.Width;
                    
                } else {
                    w += size.Width + distance.Width;
                    if (h < size.Height)
                        h = size.Height;
                }
            };

            if (dimension == Dimension.X)
                h -= distance.Height;
            else
                w -= distance.Width;

            return () => new Size(w, h);
        }

        public Func<Size> MinSize(ref Action<TItem> visit) {
            var w = 0d;
            var h = 0d;
            visit += item => {
                var size = Locator.GetSize(item);
                if (w < size.Width)
                    w = size.Width;
                if (h < size.Height)
                    h = size.Height;
            };

            return () => new Size(w, h);
        }
      
    }
}