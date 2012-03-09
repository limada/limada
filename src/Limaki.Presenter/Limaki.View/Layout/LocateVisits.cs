using System;
using Limaki.Drawing;
using Limaki.Graphs;
using Xwt;

namespace Limaki.View.Layout {

    public class LocateVisits<TItem>  {

        public LocateVisits (ILocator<TItem> locator) {
            this.Locator = locator;
        }

        public virtual ILocator<TItem> Locator { get; protected set; }

        public Func<Size, double> Allign(double start, double space, Alignment alignment, Dimension dimension) {
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

        public Func<Size, double> Location(double start, double distance, Dimension dimension) {
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
        
       

        #region Refactor this to Locate (above)

        public virtual void AllignX(ref Action<TItem> visitor, Alignment alignment, Rectangle space) {
            AllignX(ref visitor, alignment, space, item => Locator.GetLocation(item).Y);
        }

        public virtual void AllignX(ref Action<TItem> visitor, Alignment alignment, Rectangle space, Func<TItem,double> yer) {
            visitor += item => {
                var y = yer(item);

                if (alignment == Alignment.Start)
                    Locator.SetLocation(item, new Point(space.Left, y));
                else if (alignment == Alignment.End)
                    Locator.SetLocation(item, new Point(space.Right - Locator.GetSize(item).Width, y));
                else if (alignment == Alignment.Center) {
                    var x = space.Left + (space.Width - Locator.GetSize(item).Width) / 2;
                    Locator.SetLocation(item, new Point(x, y));
                }
            };
        }

        public virtual void AllignY(ref Action<TItem> visitor, Alignment alignment, Rectangle space) {
            AllignY(ref visitor, alignment, space, item => Locator.GetLocation(item).X);
        }

        public virtual void AllignY(ref Action<TItem> visitor, Alignment alignment, Rectangle space, Func<TItem, double> xer) {
            visitor += item => {

                var x = xer(item);

                if (alignment == Alignment.Start)
                    Locator.SetLocation(item, new Point(x, space.Top));
                else if (alignment == Alignment.End)
                    Locator.SetLocation(item, new Point(x, space.Bottom - Locator.GetSize(item).Height));
                else if (alignment == Alignment.Center) {
                    var y = space.Top + (space.Height - Locator.GetSize(item).Height) / 2;
                    Locator.SetLocation(item, new Point(x, y));
                }
            };
        }

        public Func<TItem, double> ItemLocation(double start, double distance, Dimension dimension) {
            double? i = null;
            return item => {
                var result = start;
                if (i.HasValue) 
                    result = i.Value;
               else 
                    i = start;
                
                if (dimension == Dimension.Y) 
                    i += Locator.GetSize(item).Height + distance;
                else 
                    i += Locator.GetSize(item).Width + distance;
                
                return result;
            };
        }

        
        public virtual void LocateItem(ref Action<TItem> visitor, double start, double space, Dimension dimension) {

            var i = start;
            visitor += item => {
                var location = Locator.GetLocation(item);
                var size = Locator.GetSize(item);
                if (dimension == Dimension.Y) {
                    Locator.SetLocation(item, new Point(location.X, i));
                    i += size.Height + space;
                } else {
                    Locator.SetLocation(item, new Point(i, location.Y));
                    i += size.Width + space;
                }
            };
        }

        #endregion


      
    }


    
}