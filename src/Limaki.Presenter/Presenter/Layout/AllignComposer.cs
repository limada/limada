using System;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Xwt;

namespace Limaki.Presenter.Layout {

    public class AllignComposer<TItem, TEdge> : Placer<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {

        public AllignComposer(PlacerBase<TItem, TEdge> aligner) : base(aligner) { }


        public Func<SizeI, int> Allign(int start, int space, Alignment alignment, Dimension dimension) {
            return size => {
                var distance = 0;
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

        public Func<SizeI, int> Location(int start, int distance, Dimension dimension) {
            int? i = null;
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

        public virtual void Locate(ref Action<TItem> visitor, Func<SizeI, int> Xer, Func<SizeI, int> Yer) {
            visitor += item => {
                var size = Proxy.GetSize(item);
                var location = new PointI(Xer(size), Yer(size));
                Proxy.SetLocation(item, location);
            };
        }
        
        public virtual void Justify(ref Action<TItem> visitor) {
            visitor += item => Proxy.Justify(item);
        }

        #region Refactor this to Locate (above)

        public virtual void AllignX(ref Action<TItem> visitor, Alignment alignment, RectangleI space) {
            AllignX(ref visitor, alignment, space, item => Proxy.GetLocation(item).Y);
        }

        public virtual void AllignX(ref Action<TItem> visitor, Alignment alignment, RectangleI space, Func<TItem,int> yer) {
            visitor += item => {
                var y = yer(item);

                if (alignment == Alignment.Start)
                    Proxy.SetLocation(item, new PointI(space.Left, y));
                else if (alignment == Alignment.End)
                    Proxy.SetLocation(item, new PointI(space.Right - Proxy.GetSize(item).Width, y));
                else if (alignment == Alignment.Center) {
                    var x = space.Left + (space.Width - Proxy.GetSize(item).Width) / 2;
                    Proxy.SetLocation(item, new PointI(x, y));
                }
            };
        }

        public virtual void AllignY(ref Action<TItem> visitor, Alignment alignment, RectangleI space) {
            AllignY(ref visitor, alignment, space, item => Proxy.GetLocation(item).X);
        }

        public virtual void AllignY(ref Action<TItem> visitor, Alignment alignment, RectangleI space, Func<TItem, int> xer) {
            visitor += item => {

                var x = xer(item);

                if (alignment == Alignment.Start)
                    Proxy.SetLocation(item, new PointI(x, space.Top));
                else if (alignment == Alignment.End)
                    Proxy.SetLocation(item, new PointI(x, space.Bottom - Proxy.GetSize(item).Height));
                else if (alignment == Alignment.Center) {
                    var y = space.Top + (space.Height - Proxy.GetSize(item).Height) / 2;
                    Proxy.SetLocation(item, new PointI(x, y));
                }
            };
        }

        public Func<TItem, int> ItemLocation(int start, int distance, Dimension dimension) {
            int? i = null;
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

        
        public virtual void LocateItem(ref Action<TItem> visitor, int start, int space, Dimension dimension) {

            var i = start;
            visitor += item => {
                var location = Proxy.GetLocation(item);
                var size = Proxy.GetSize(item);
                if (dimension == Dimension.Y) {
                    Proxy.SetLocation(item, new PointI(location.X, i));
                    i += size.Height + space;
                } else {
                    Proxy.SetLocation(item, new PointI(i, location.Y));
                    i += size.Width + space;
                }
            };
        }

        #endregion


        #region deprectated

        public Func<TItem, int> Yer(PointI at) {
            int? rowStart = null;
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
        public Func<TItem, int> Xer(PointI at) {
            int? rowStart = null;
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