using Limaki.Visuals;
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;
using Limaki.Drawing;

namespace Limaki.Tests.Visuals {

    public interface ISceneFactory:ISampleGraphFactory<IVisual, IVisualEdge> {
        IGraphScene<IVisual, IVisualEdge> Scene { get; }

        void PopulateScene (IGraphScene<IVisual, IVisualEdge> scene);
    }

}