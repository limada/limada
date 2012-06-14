using System;
using Limaki.Drawing;
using Limaki.Graphs;
using Xwt;

namespace Limaki.View.Layout {
    public interface ILocateVisits<TItem> {
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
        Func<Size, double> Allign(double start, double space, Alignment alignment, Dimension dimension);

        /// <summary>
        /// gives back a func which returns the
        /// prefered location according to size (param of func)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="distance"></param>
        /// <param name="dimension"></param>
        /// <returns></returns>
        Func<Size, double> Location (double start, double distance, Dimension dimension);

        void Locate(ref Action<TItem> visitor, Func<Size, double> Xer, Func<Size, double> Yer);
    }

    public class LocateVisits<TItem> : ILocateVisits<TItem> {

        public LocateVisits (ILocator<TItem> locator) {
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
        public virtual Func<Size, double> Allign(double start, double space, Alignment alignment, Dimension dimension) {
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

        public virtual void Locate(ref Action<TItem> visitor, Func<Size, double> Xer, Func<Size, double> Yer) {
            visitor += item => {
                var size = Locator.GetSize(item);
                var location = new Point(Xer(size), Yer(size));
                Locator.SetLocation(item, location);
            };
        }
    }
}