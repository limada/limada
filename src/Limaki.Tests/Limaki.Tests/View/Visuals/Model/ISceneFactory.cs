using Limaki.Visuals;
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;

namespace Limaki.Tests.Visuals {
    public interface ISceneFactory:IGraphFactory<IVisual, IVisualEdge> {
        Scene Scene { get; }
        void Populate ( Scene scene );
    }

}