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
            visitor += item => {
                visits++;

                var y = Proxy.GetLocation(item).Y;

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
        public void Allign(ref Action<TItem> visitor, VerticalAlignment alignment, RectangleI space) {
            visitor += item => {
              
                var x = Proxy.GetLocation(item).X;

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

        public virtual void Distribute(ref Action<TItem> visitor, int start, int space, Distribution distribution) {
            
            var i = start;
            visitor += item => {
                var location = Proxy.GetLocation(item);
                var size = Proxy.GetSize(item);
                if (distribution == Distribution.Y) {
                    Proxy.SetLocation(item, new PointI(location.X, i));
                    i += size.Height + space;
                } else {
                    Proxy.SetLocation(item, new PointI(i,location.Y));
                    i += size.Width + space;
                }
            };
        }

      
    }
    public enum Distribution {
        X,Y
    }
}