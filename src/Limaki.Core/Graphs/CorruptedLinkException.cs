using System;

namespace Limaki.Graphs {
    public class CorruptedLinkException<TItem, TEdge> : ArgumentException
        where TEdge : IEdge<TItem> {
        public CorruptedLinkException(IEdge<TItem> edge, string m)
            : base(m) {
            this.Edge = edge;
        }

        public IEdge<TItem> Edge { get; protected set; }
    }
}