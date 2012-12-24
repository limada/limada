using Limaki.Common;
using Limaki.Graphs;
using System;

namespace Limaki.Drawing {

    public interface IGraphSceneLayout<TItem, TEdge> : ILayout<TItem>
        where TEdge : TItem, IEdge<TItem> {

        Func<IGraphScene<TItem, TEdge>> DataHandler { get; set; }

        IGraphScene<TItem, TEdge> Data { get; }

        Dimension Dimension { get; set; }

        bool Centered { get; set; }

    }
}