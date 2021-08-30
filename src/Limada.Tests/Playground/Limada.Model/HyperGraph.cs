using System.Collections.Generic;
using Limada.Model;
using Limaki.Graphs;

namespace Limaki.Playground.Limada.Model {

    public interface IHyperEdge<T> {
        ICollection<T> Leafs { get; set; }
    }

    public interface IHyperLink<T> : IHyperEdge<T> {
        T Marker { get; set; }
    }

    public interface IHyperLink : IThing, IThing<IThing>, IHyperLink<IThing> {

    }
}