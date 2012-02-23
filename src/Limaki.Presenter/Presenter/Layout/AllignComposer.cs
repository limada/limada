using System;
using Limaki.Drawing;
using Limaki.Graphs;
using Xwt;

namespace Limaki.Presenter.Layout {

    public class AllignComposer<TItem, TEdge> : Placer<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {

        public AllignComposer(PlacerBase<TItem, TEdge> aligner) : base(aligner) { }


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
                var size = Proxy.GetSize(item);
                var location = new Point(Xer(size), Yer(size));
                Proxy.SetLocation(item, location);
            };
        }
        
        public virtual void Justify(ref Action<TItem> visitor) {
            visitor += item => Proxy.Justify(item);
        }

        #region Refactor this to Locate (above)

        public virtual void AllignX(ref Action<TItem> visitor, Alignment alignment, RectangleD space) {
            AllignX(ref visitor, alignment, space, item => Proxy.GetLocation(item).Y);
        }

        public virtual void AllignX(ref Action<TItem> visitor, Alignment alignment, RectangleD space, Func<TItem,double> yer) {
            visitor += item => {
                var y = yer(item);

                if (alignment == Alignment.Start)
                    Proxy.SetLocation(item, new Point(space.Left, y));
                else if (alignment == Alignment.End)
                    Proxy.SetLocation(item, new Point(space.Right - Proxy.GetSize(item).Width, y));
                else if (alignment == Alignment.Center) {
                    var x = space.Left + (space.Width - Proxy.GetSize(item).Width) / 2;
                    Proxy.SetLocation(item, new Point(x, y));
                }
            };
        }

        public virtual void AllignY(ref Action<TItem> visitor, Alignment alignment, RectangleD space) {
            AllignY(ref visitor, alignment, space, item => Proxy.GetLocation(item).X);
        }

        public virtual void AllignY(ref Action<TItem> visitor, Alignment alignment, RectangleD space, Func<TItem, double> xer) {
            visitor += item => {

                var x = xer(item);

                if (alignment == Alignment.Start)
                    Proxy.SetLocation(item, new Point(x, space.Top));
                else if (alignment == Alignment.End)
                    Proxy.SetLocation(item, new Point(x, space.Bottom - Proxy.GetSize(item).Height));
                else if (alignment == Alignment.Center) {
                    var y = space.Top + (space.Height - Proxy.GetSize(item).Height) / 2;
                    Proxy.SetLocation(item, new Point(x, y));
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
                    i += Proxy.GetSize(item).Height + distance;
                else 
                    i += Proxy.GetSize(item).Width + distance;
                
                return result;
            };
        }

        
        public virtual void LocateItem(ref Action<TItem> visitor, double start, double space, Dimension dimension) {

            var i = start;
            visitor += item => {
                var location = Proxy.GetLocation(item);
                var size = Proxy.GetSize(item);
                if (dimension == Dimension.Y) {
                    Proxy.SetLocation(item, new Point(location.X, i));
                    i += size.Height + space;
                } else {
                    Proxy.SetLocation(item, new Point(i, location.Y));
                    i += size.Width + space;
                }
            };
        }

        #endregion


        #region deprectated

        public Func<TItem, double> Yer(Point at) {
            double? rowStart = null;
            return item => {
                var result = at.Y;
                if (rowStart.HasValue) {
                    result = rowStart.Value;
                } else {
                    rowStart = at.Y;
                }
                rowStart += Proxy.GetSize(item).Height + Layout.Distance.Height;
                return result;
            };
        }
        public Func<TItem, double> Xer(Point at) {
            double? rowStart = null;
            return item => {
                var result = at.Y;
                if (rowStart.HasValue) {
                    result = rowStart.Value;
                } else {
                    rowStart = at.Y;
                }
                rowStart += Proxy.GetSize(item).Height + Layout.Distance.Height;
                return result;
            };
        }
        #endregion   
    }


    
}