using Limaki.Drawing;
using Limaki.Graphs;
using System.Collections.Generic;

namespace Limaki.Presenter.Layout {
    public interface IShapeProxy<TItem,TEdge> where TEdge : IEdge<TItem>, TItem {

        ICollection<TEdge> AffectedEdges { get; set; }

        void Commit ( IGraphScene<TItem, TEdge> Data );
        void SetLocation ( TItem item, PointI location );
        PointI GetLocation ( TItem item );
        SizeI GetSize ( TItem item );

        /// <summary>
        /// gives back a valid shape
        /// if item has no shape:
        /// Layout.CreateShape is called 
        /// item, shape is added to invokeList
        /// shape is Justified
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        IShape GetShape ( TItem item );

        /// <summary>
        /// adds item to invokeList
        /// if item.Shape == null, Invoke(item) is called
        /// else item,null is added to invokeList
        /// </summary>
        /// <param name="item"></param>
        IShape EnsureInvoke ( TItem item );

        void Justify ( TItem item );
    }
}