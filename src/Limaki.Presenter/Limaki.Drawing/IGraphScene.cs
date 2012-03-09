using System;
using System.Collections.Generic;
using Limaki.Actions;
using Limaki.Common;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Xwt;

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
        IEnumerable<TItem> ElementsIn(Rectangle clipBounds);
        IEnumerable<TItem> ElementsIn(Rectangle clipBounds, ZOrder order);

        State State { get; }

        void Add(TItem visual);
        bool Remove(TItem visual);
        bool ChangeEdge(TEdge edge, TItem target, bool asRoot);
        bool Contains(TItem visual);

        void RemoveBounds(TItem visual);
        void AddBounds(TItem visual);
        void UpdateBounds(TItem visual, Rectangle invalid);

        TItem Hit ( Point p, int hitSize );
        TItem HitBorder ( Point p, int hitSize );
        Point NoHit { get; }
        IShape ItemShape ( TItem item );

        void Clear();
        void ClearView();

        IMarkerFacade<TItem, TEdge> Markers { get; set; }
        IEnumerable<TEdge> Twig(TItem visual);

        
    }

    public enum ZOrder {
        NodesFirst,
        EdgesFirst
    }

    public class SceneInfo {
        public Int64 Id;
        public string Name;
        private State _state ;
        public State State { get { return _state ?? (_state = new State { Hollow = true }); } }
    }
}