using Limaki.Drawing;
using Limaki.Graphs;
using System.Collections.Generic;
using Limaki.Actions;

namespace Limaki.View.Layout {

    public interface IShapeGraphProxy<TItem,TEdge>:ILocator<TItem> where TEdge : IEdge<TItem>, TItem {

        ICollection<TEdge> AffectedEdges { get; set; }

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

        void Commit (ICollection<ICommand<TItem>> requests);
    }
}