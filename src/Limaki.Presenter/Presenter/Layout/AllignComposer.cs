using System;
using Limaki.Drawing;
using Limaki.Graphs;

namespace Limaki.Presenter.Layout {
    public class AllignComposer<TItem, TEdge> : Placer<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {
        public AllignComposer(PlacerBase<TItem, TEdge> aligner) : base(aligner) { }

        /// <summary>
        /// testing if too much visits
        /// </summary>
        internal int visits = 0;
    

        public virtual void Allign(ref Action<TItem> visitor, HorizontalAlignment alignment, RectangleI space) {
            Allign(ref visitor, alignment, space, item => Proxy.GetLocation(item).Y);
        }

        public virtual void Allign(ref Action<TItem> visitor, HorizontalAlignment alignment, RectangleI space, Func<TItem,int> yer) {
            visitor += item => {
                visits++;

                var y = yer(item);

                if (alignment == HorizontalAlignment.Left)
                    Proxy.SetLocation(item, new PointI(space.Left, y));
                else if (alignment == HorizontalAlignment.Right)
                    Proxy.SetLocation(item, new PointI(space.Right - Proxy.GetSize(item).Width, y));
                else if (alignment == HorizontalAlignment.Center) {
                    var x = space.Left + (space.Width - Proxy.GetSize(item).Width) / 2;
                    Proxy.SetLocation(item, new PointI(x, y));
                }
            };
        }

        public virtual void Allign(ref Action<TItem> visitor, VerticalAlignment alignment, RectangleI space) {
            Allign(ref visitor, alignment, space, item => Proxy.GetLocation(item).X);
        }

        public void Allign(ref Action<TItem> visitor, VerticalAlignment alignment, RectangleI space, Func<TItem, int> xer) {
            visitor += item => {

                var x = xer(item);
                
                if (alignment == VerticalAlignment.Top)
                    Proxy.SetLocation(item, new PointI(x,space.Top));
                else if (alignment == VerticalAlignment.Bottom)
                    Proxy.SetLocation(item, new PointI(x,space.Bottom - Proxy.GetSize(item).Height));
                else if (alignment == VerticalAlignment.Center) {
                    var y = space.Top + (space.Height- Proxy.GetSize(item).Height) / 2;
                    Proxy.SetLocation(item, new PointI(x, y));
                }
            };
        }

        public virtual void Justify(ref Action<TItem> visitor) {
            visitor += item => {
                Data.Requests.Add(new LayoutCommand<TItem>(item, LayoutActionType.Justify));
            };
        }

        public Func<TItem, int> Location(int start, int distance, Distribution distribution) {
            int? i = null;
            return item => {
                var result = start;
                if (i.HasValue) 
                    result = i.Value;
               else 
                    i = start;
                
                if (distribution == Distribution.Vertical) 
                    i += Proxy.GetSize(item).Height + distance;
                else 
                    i += Proxy.GetSize(item).Width + distance;
                
                return result;
            };
        }

        public virtual void Locate(ref Action<TItem> visitor, int start, int space, Distribution distribution) {
            
            var i = start;
            visitor += item => {
                var location = Proxy.GetLocation(item);
                var size = Proxy.GetSize(item);
                if (distribution == Distribution.Vertical) {
                    Proxy.SetLocation(item, new PointI(location.X, i));
                    i += size.Height + space;
                } else {
                    Proxy.SetLocation(item, new PointI(i,location.Y));
                    i += size.Width + space;
                }
            };
        }

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