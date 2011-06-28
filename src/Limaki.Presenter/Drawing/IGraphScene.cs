using Limaki.Graphs;
using System.Collections.Generic;
using Limaki.Actions;
using Limaki.Common;
using System;
using Limaki.Graphs.Extensions;

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
        int Count { get; }

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

        void Add(TItem visual);
        bool Remove(TItem visual);
        bool ChangeEdge(TEdge edge, TItem target, bool asRoot);
        bool Contains(TItem visual);

        void RemoveBounds(TItem visual);
        void AddBounds(TItem visual);
        void UpdateBounds(TItem visual, RectangleI invalid);

        TItem Hit ( PointI p, int hitSize );
        TItem HitBorder ( PointI p, int hitSize );
        PointI NoHit { get; }
        IShape ItemShape ( TItem item );

        void Clear();
        void ClearView();

        IMarkerFacade<TItem, TEdge> Markers { get; set; }
        IEnumerable<TEdge> Twig(TItem visual);

        
    }

    public interface ISpatialIndex<TItem> {
        bool BoundsDirty { get; set; }
        RectangleI Bounds { get; set; }

        void Add(TItem item);
        void Remove(TItem item);
        void AddRange(IEnumerable<TItem> items);
        void Update(RectangleI invalid, TItem visual);

        IEnumerable<TItem> Query();
        IEnumerable<TItem> Query(RectangleS clipBounds);
        IEnumerable<TItem> Query(RectangleS clipBounds, ZOrder zOrder);
        void Clear();
    }

    public enum ZOrder {
        NodesFirst,
        EdgesFirst
    }

}