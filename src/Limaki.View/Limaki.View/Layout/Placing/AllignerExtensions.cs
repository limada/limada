using System.Collections.Generic;
using Limaki.Graphs;
using Xwt;
using System.Linq;

namespace Limaki.View.Layout {

    public static class AllignerExtensions {

        public static void OneColumn<TItem, TEdge> (this Alligner<TItem, TEdge> alligner, IEnumerable<TItem> items, Point at) where TEdge : IEdge<TItem>, TItem {
            alligner.OneColumn(items, at, alligner.Layout.Distance.Height);
        }

        public static void OneColumn<TItem, TEdge>(this Alligner<TItem, TEdge> alligner, IEnumerable<TItem> items) where TEdge : IEdge<TItem>, TItem {
            if (items.Count() < 2)
                return;
            alligner.OneColumn(items, alligner.Locator.GetLocation(items.First()), alligner.Layout.Distance.Height);
        }
    }
}