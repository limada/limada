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

using System;
using Limaki.Drawing;
using Limaki.Graphs;
using Xwt;

namespace Limaki.View.Layout {

    public interface ILocateVisitBuilder<TItem> {
        ILocator<TItem> Locator { get; }

        /// <summary>
        /// gives back a func which returns the
        /// prefered location according to size (param of func)
        /// aligned according to alignment
        /// </summary>
        /// <param name="start"></param>
        /// <param name="space">the space within to align</param>
        /// <param name="alignment"></param>
        /// <param name="dimension"></param>
        /// <returns></returns>
        Func<Size, double> Align(double start, double space, Alignment alignment, Dimension dimension);

        /// <summary>
        /// gives back a func which returns the
        /// prefered location according to size (param of func)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="distance"></param>
        /// <param name="dimension"></param>
        /// <returns></returns>
        Func<Size, double> Location (double start, double distance, Dimension dimension);

        /// <summary>
        /// Locate-Action:
        /// set location to Point{ Xer(size), Yer(size) })
        /// visit += item => Locate-Action
        /// </summary>
        /// <param name="visit"></param>
        /// <param name="Xer"></param>
        /// <param name="Yer"></param>
        void Locate(ref Action<TItem> visit, Func<Size, double> Xer, Func<Size, double> Yer);
    }

    public class LocateVisitBuilder<TItem> : ILocateVisitBuilder<TItem> {

        public LocateVisitBuilder (ILocator<TItem> locator) {
            this.Locator = locator;
        }

        public virtual ILocator<TItem> Locator { get; protected set; }

        /// <summary>
        /// gives back a func which returns the
        /// prefered location according to size (param of func)
        /// aligned according to alignment
        /// </summary>
        /// <param name="start"></param>
        /// <param name="space">the space within to align</param>
        /// <param name="alignment"></param>
        /// <param name="dimension"></param>
        /// <returns></returns>
        public virtual Func<Size, double> Align(double start, double space, Alignment alignment, Dimension dimension) {
            return size => {
                var distance = 0d;
                if (dimension == Dimension.X)
                    distance = size.Width;
                else
                    distance = size.Height;

                if (alignment == Alignment.End)
                    return start + space - distance;

                else if (alignment == Alignment.Center) {
                    return start + (space - distance) / 2;
                }
                return start;
            };
        }

        /// <summary>
        /// gives back a func which returns the
        /// prefered location according to size (param of func)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="distance"></param>
        /// <param name="dimension"></param>
        /// <returns></returns>
        public virtual Func<Size, double> Location (double start, double distance, Dimension dimension) {
            double? i = null;
            return size => {
                var result = start;
                if (i.HasValue)
                    result = i.Value;
                else
                    i = start;

                if (dimension == Dimension.Y)
                    i += size.Height + distance;
                else
                    i += size.Width + distance;

                return result;
            };
        }

        /// <summary>
        /// visit += item => Locate-Action
        /// </summary>
        /// <param name="visit"></param>
        /// <param name="Xer"></param>
        /// <param name="Yer"></param>
        public virtual void Locate(ref Action<TItem> visit, Func<Size, double> Xer, Func<Size, double> Yer) {
            visit += item => {
                var size = Locator.GetSize(item);
                var location = new Point(Xer(size), Yer(size));
                Locator.SetLocation(item, location);
            };
        }
    }
}