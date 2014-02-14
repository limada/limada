using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Graphs;
using Limaki.Visuals;

namespace Limaki.Playground.Graph {

    public static class NotifiyExtentsions {
        public static IEnumerable<TItem> DependsOn<TItem, TEdge> (this IGraph<TItem, TEdge> graph, TItem source, GraphEventType eventType) where TEdge : IEdge<TItem> {
            var dep = graph as IGraphDependencies<TItem, TEdge>;
            if (dep != null)
                return dep.DependsOn (GraphCursor.Create (graph, source), eventType);
            return new TItem[0];
        }
    }

    public interface IGraphDependencies<TItem, TEdge> where TEdge : IEdge<TItem> {
        IEnumerable<TItem> DependsOn (GraphCursor<TItem, TEdge> source, GraphEventType eventType);
    }

    public interface INotifyGraphChanged<TItem, TEdge> where TEdge : IEdge<TItem> {
        event GraphChangedEvent<TItem, TEdge> NGraphChanged;
        event ItemPropertyChangedEvent<TItem, TEdge> ItemPropertyChanged;
    }

    public class GraphChangedEventArgs<TItem, TEdge> : EventArgs where TEdge : IEdge<TItem> {

        public GraphChangedEventArgs (object source, GraphCursor<TItem, TEdge> subject, Graphs.GraphEventType graphEventType) {
            this.Source = Source;
            this.Subject = subject;
            this.GraphEventType = graphEventType;
        }

        /// <summary>
        /// the graph that raised the event
        /// </summary>
        public object Source { get; protected set; }
        /// <summary>
        /// the graph and the item 
        /// </summary>
        public GraphCursor<TItem, TEdge> Subject { get; protected set; }
        public GraphEventType GraphEventType { get; protected set; }
    }

    public class ItemPropertyChangedEventArgs<TItem, TEdge> : EventArgs where TEdge : IEdge<TItem> {
        public GraphCursor<TItem, TEdge> Subject { get; protected set; }
    }

    public delegate void GraphChangedEvent<TItem, TEdge> (object sender, GraphChangedEventArgs<TItem, TEdge> e) where TEdge : IEdge<TItem>;

    public delegate void ItemPropertyChangedEvent<TItem, TEdge> (object sender, ItemPropertyChangedEventArgs<TItem, TEdge> e) where TEdge : IEdge<TItem>;

    public class ObservableGraph<TItem, TEdge> : Graph<TItem, TEdge>, INotifyGraphChanged<TItem, TEdge> where TEdge : IEdge<TItem> {
       
        protected GraphChangedEvent<TItem, TEdge> _nGraphChanged;
        protected ItemPropertyChangedEvent<TItem, TEdge> _itemPropertyChanged;

        public event GraphChangedEvent<TItem, TEdge> NGraphChanged {
            add { _nGraphChanged += value; }
            remove { _nGraphChanged -= value; }
        }

        protected virtual void OnNGraphChanged (TItem item, GraphEventType graphEventType) {
            if (_nGraphChanged != null)
                _nGraphChanged(this, new GraphChangedEventArgs<TItem, TEdge>(this, GraphCursor.Create(this, item), graphEventType));
        }

        public event ItemPropertyChangedEvent<TItem, TEdge> ItemPropertyChanged {
            add { _itemPropertyChanged += value; }
            remove { _itemPropertyChanged -= value; }
        }

        protected virtual void OnItemPropertyChanged (GraphCursor<TItem, TEdge> subject) {
            if (_itemPropertyChanged != null)
                _itemPropertyChanged(this, new ItemPropertyChangedEventArgs<TItem, TEdge>());
        }
    }

    public class GraphEvents {
       public void Proto() {
           var ob = new ObservableGraph<IVisual, IVisualEdge>();
          
       }


    }


}
