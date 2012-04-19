using Limaki.Common;
using Limaki.Graphs;

namespace Limaki.Drawing {

    public interface IGraphSceneLayout<TItem, TEdge> : ILayout<TItem>
        where TEdge : TItem, IEdge<TItem> {

        Get<IGraphScene<TItem, TEdge>> DataHandler { get; set; }

        IGraphScene<TItem, TEdge> Data { get; }

        Orientation Orientation { get; set; }

        bool Centered { get; set; }

    }
}