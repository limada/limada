using Limaki.Visuals;
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;
using Limaki.Drawing;

namespace Limaki.Tests.Visuals {
    public interface ISceneFactory:ITestGraphFactory<IVisual, IVisualEdge> {
        IGraphScene<IVisual, IVisualEdge> Scene { get; }
        void Populate (IGraphScene<IVisual, IVisualEdge> scene);
    }

}