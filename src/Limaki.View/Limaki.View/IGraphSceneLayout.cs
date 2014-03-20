using Limaki.Drawing;
using Limaki.Graphs;
using System;

namespace Limaki.View {

    public interface IGraphSceneLayout<TItem, TEdge> : ILayout<TItem>
        where TEdge : TItem, IEdge<TItem> {

        Func<IGraphScene<TItem, TEdge>> DataHandler { get; set; }

        IGraphScene<TItem, TEdge> Data { get; }

        Dimension Dimension { get; set; }

        bool Centered { get; set; }

        void AdjustSize (TItem visual);

    }
}