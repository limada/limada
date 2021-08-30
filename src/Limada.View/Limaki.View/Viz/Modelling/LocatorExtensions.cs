/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2012 - 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using Limaki.Common.Linqish;
using Limaki.Drawing;
using Xwt;

namespace Limaki.View.Viz.Modelling {

    public static class LocatorExtensions {

        public static Rectangle Bounds<TItem> (this ILocator<TItem> loc, IEnumerable<TItem> items) {
            Action<TItem> visit = null;
            var measure = new MeasureVisitBuilder<TItem>(loc);
            var fBounds = measure.Bounds(ref visit);
            items.ForEach(e => visit(e));
            return fBounds();
        }

        public static Func<Rectangle> Bounds<TItem> (this ILocator<TItem> loc, Action<TItem> visit) {
            var measure = new MeasureVisitBuilder<TItem> (loc);
            return measure.Bounds (ref visit);
        }

        public static Size SizeToFit<TItem> (this ILocator<TItem> loc, IEnumerable<TItem> items, Size distance, Dimension dimension) {
            Action<TItem> visit = null;
            var measure = new MeasureVisitBuilder<TItem> (loc);
            var fSize = measure.SizeToFit (ref visit, distance, dimension);
            items.ForEach (e => visit (e));
            return fSize ();
        }

        public static double Delta (this Alignment alignement, double boundsSize, double size) {
            var delta = 0d;
            if (Alignment.Center == alignement)
                delta = (boundsSize - size) / 2;
            else if (Alignment.End == alignement)
                delta = (boundsSize - size);
            return delta;
        }

        public static Size SizeToFit (this Size self, Size add, Dimension dimension) {
            
            var w = self.Width;
            var h = self.Height;

            if (dimension == Dimension.Y) {
                h += add.Height;
                if (w < add.Width)
                    w = add.Width;

            } else {
                w += add.Width;
                if (h < add.Height)
                    h = add.Height;
            }
            return new Size (w, h);
        }

        public static Size SumSize (this Size self, Size add, Dimension dimension) =>
            dimension == Dimension.Y ?
                                  new Size (self.Width, self.Height + add.Height) :
                                  new Size (self.Width + add.Width, self.Height);

        public static Rectangle MaxExtend (this Rectangle self, Rectangle add, Dimension dimension) =>
            dimension == Dimension.Y ?
                                  Rectangle.FromLTRB (self.Left, self.Top, self.Right, Math.Max (self.Bottom, add.Bottom)) :
                                  Rectangle.FromLTRB (self.Left, self.Top, Math.Max (self.Right, add.Right), self.Bottom);
        
        
        public static Size InitSum (this Size self, Dimension dimension) => dimension == Dimension.Y ? new Size (0, self.Height) : new Size (self.Width, 0);
        
        public static Dimension Adjacent (this Dimension dimension) => dimension == Dimension.X ? Dimension.Y : Dimension.X;


    }
}