using Limaki.Graphs;
using System.Collections.Generic;
using Limaki.Actions;
using Limaki.Common;
using System;

namespace Limaki.Drawing {
    /// <summary>
    /// holds and manages graph-oriented models
    /// 
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    public interface IGraphScene<TItem,TEdge> 
    where TEdge:TItem, IEdge<TItem> {
        
        IGraph<TItem, TEdge> Graph { get; set; }

        /// <summary>
        /// Requests
        /// the commands to be performed by a Receiver
        /// </summary>
        ICollection<ICommand<TItem>> Requests {get;set;}

        TItem Focused { get; set; }
        TItem Hovered { get; set; }
        IComposite<TItem> Selected { get; set; }
        Action<IGraphScene<TItem, TEdge>, TItem> FocusChanged { get; set; }
        
        IShape Shape { get; }
        
        ISpatialIndex<TItem> SpatialIndex { get; }
        void ClearSpatialIndex();

        IEnumerable<TItem> Elements {get;}
        IEnumerable<TItem> ElementsIn(RectangleS clipBounds);
        IEnumerable<TItem> ElementsIn(RectangleS clipBounds, ZOrder order);

        State State { get; }

        void Add(TItem widget);
        bool Remove(TItem widget);
        bool ChangeEdge(TEdge edge, TItem target, bool asRoot);
        bool Contains ( TItem widget );

        void RemoveBounds(TItem widget);
        void AddBounds(TItem widget);
        void UpdateBounds(TItem widget, RectangleI invalid);

        TItem Hit ( PointI p, int hitSize );
        TItem HitBorder ( PointI p, int hitSize );
        IShape ItemShape ( TItem item );

        void Clear();
        void ClearView();
    }

    public interface ISpatialIndex<IWidget> {
        bool BoundsDirty { get; set; }
        RectangleI Bounds { get; set; }

        void Add(IWidget item);
        void Remove(IWidget item);
        void AddRange(IEnumerable<IWidget> items);
        void Update(RectangleI invalid, IWidget widget);

        IEnumerable<IWidget> Query();
        IEnumerable<IWidget> Query(RectangleS clipBounds);
        IEnumerable<IWidget> Query(RectangleS clipBounds, ZOrder zOrder);
        void Clear();
    }

    public enum ZOrder {
        NodesFirst,
        EdgesFirst
    }

}