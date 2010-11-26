using Limaki.Widgets;
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;

namespace Limaki.Tests.Widget {
    public interface ISceneFactory:IGraphFactory<IWidget, IEdgeWidget> {
        Scene Scene { get; }
        void Populate ( Scene scene );
    }

}